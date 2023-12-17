using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class GameManagement : MonoBehaviour
{
    private static GameManagement instance = null;

    [Header("포인트")]
    public float totalPoints;
    [Header("Point_UI_TMPro 넣어줘")]
    public TextMeshProUGUI point_UI;

    [Header("이름")]
    [SerializeField] private string nowPlayerName;
    public InputField nameInput;
    public GameObject endUIs;

    public float newSpawnTime;

    public GameObject alreadySummoned;

    [Header("특산물 리스트 트렐로의 순서별로")]
    [Header("예) 1. 쌀, 2. 호두...")]
    [Tooltip("Prefabs/Specialty")]
    public List<GameObject> SpecialtyList = new List<GameObject>();
    public GameObject SpecialtyInformationTexts;
    public Text SpecialtyText_Name, SpecialtyText_Information;
    public Sprite Specialty_Image;
    public bool[] Specialty_bool;

    [Header("게임 관련")]
    [Tooltip("게임 시작 했는지")]
    public bool isStart;

    [Header("랭킹 시스템")]
    [SerializeField] private RankSystem rankSystem;

    [Header("소환 시스템")]
    [Tooltip("초기 소환할 곳의 empty 넣어주세요")]
    [SerializeField] private Transform spawnPoint;

    private void Awake() {
        if(null == instance){
            instance = this;
        }else{
            Destroy(this.gameObject);
        }

        rankSystem = FindObjectOfType<RankSystem>();
        PointReset();
        totalPoints = 0;
        SpecialtyInformationTexts.SetActive(false);
        Specialty_bool = new bool[SpecialtyList.Count];
        //시작할 때 UI 초기화
    }

    public static GameManagement Instance{//싱글톤 패턴 인스턴스화
        get{
            if(null == instance){
                return null;
            }
            return instance;
        }
    }

    public void Update(){
        newSpawnTime += Time.deltaTime;
        if(newSpawnTime > 2){
            newSpawnTime = 2;
        }
    }

    public void GameStart(){//게임 시작할 때
        isStart = true;
        NewSpawn();
    }

    public void GameEnd(){//게임 끝날 때
        isStart = false;
        endUIs.SetActive(true);
        //BGM
        //UI등장 밑 값 재설정
    }

    public void PointUp(int point){
        totalPoints += point;
        point_UI.text = totalPoints.ToString() + "점";
    }

    public void PointReset(){ //나중에 다시시작 버튼에 넣기 위해서
        totalPoints = 0;
        point_UI.text = totalPoints.ToString() + "점";
    }

    public void NextSpecialtySpawn(Vector3 lerpVec, string nameSpe){
        SpecialtyInformationTexts.SetActive(false);
        for(int i = 0; i < SpecialtyList.Count; i++){ //특산물 수만큼 반복
            Debug.Log(i);
            if(nameSpe == SpecialtyList[i].name){ //i번째 특산물 어레이와 지금 충돌한 오브젝트들의 이름이 같을 때
                GameObject NextSpe = Instantiate(SpecialtyList[i+1], lerpVec, Quaternion.identity); //i+1번째(다음) 특산물을 충돌한 지점에 소환
                Debug.Log(SpecialtyList[i+1] + "NextSpecialSpawn");

                if(!Specialty_bool[i]){
                    InformationOutput(NextSpe.GetComponent<Specialty>());
                    Specialty_bool[i] = true;
                }

                break;
            }else if(i == SpecialtyList.Count){//어레이 마지막에 도달했을 때, 아무 일도 일어나지 않음.
                break;
            }
        }
    }

    public void FloorFalledSpawn(string nameSpe){
        SpecialtyInformationTexts.SetActive(false);
        for(int i = 0; i<SpecialtyList.Count; i++){
            if(nameSpe == SpecialtyList[i].name){
                GameObject NextSpe = Instantiate(SpecialtyList[i], spawnPoint.position, Quaternion.identity);
                NextSpe.GetComponent<Specialty>().isNextSummoned = false;
                break;
            }
        }
    }

    public void NewSpawn(){
        SpecialtyInformationTexts.SetActive(false);
        if(isStart && newSpawnTime > 0.5f){
            int index = Random.Range(0,5);
            GameObject NextSpe = Instantiate(SpecialtyList[index], spawnPoint.position, Quaternion.identity);
            NextSpe.GetComponent<Specialty>().isNextSummoned = false;
            newSpawnTime = 0f;
            Debug.Log("NewSpawned"+SpecialtyList[index]);

            if(!Specialty_bool[index]){//첫번째 소환
                InformationOutput(NextSpe.GetComponent<Specialty>());
                Specialty_bool[index] = true;
            }
        }
    }

    public void InputName(){
        if(nameInput != null){
            nowPlayerName = nameInput.text;
        }else{
            nowPlayerName = $"「   」";
        }
        rankSystem.RankUpdate(nowPlayerName, totalPoints);
    }

    public void InformationOutput(Specialty specialty_Now){
        SpecialtyInformationTexts.SetActive(true);
        SpecialtyData data = specialty_Now.specialtyData;
        SpecialtyText_Name.text = data.SpecialtyName_Korean;
        SpecialtyText_Information.text = data.SpecialtyExplain;        
    }

}