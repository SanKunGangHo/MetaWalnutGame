using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using Oculus.Interaction.HandGrab;
using Oculus.Interaction;
using System.Collections;
using System.Net;

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

    public GameObject alreadySummoned = null, alreadyNextSummoned = null;

    [Header("특산물 리스트 트렐로의 순서별로")]
    [Header("예) 1. 쌀, 2. 호두...")]
    [Tooltip("Prefabs/Specialty")]
    public List<GameObject> SpecialtyList = new List<GameObject>();
    public GameObject SpecialtyInformationTexts;
    public Text SpecialtyText_Name, SpecialtyText_Information;
    public Sprite Specialty_Image;
    public bool[] Specialty_bool;

    public Collider DeleteCol;

    [Header("게임 관련")]
    [Tooltip("게임 시작 했는지")]
    public bool isStart;
    public GameObject gameOver_Col;

    [Header("랭킹 시스템")]
    [SerializeField] private RankSystem rankSystem;

    [Header("소환 시스템")]
    [Tooltip("초기 소환할 곳의 empty 넣어주세요")]
    [SerializeField] private Transform spawnPoint;
    public bool oneSummon, oneNextSummon;
    public int index;
    public GameObject spawnedObject;
    private WaitForSeconds wait = new WaitForSeconds(2f);

    [Header("GrabInteractor 시스템")]
    public GameObject nowHolding;
    public HandGrabInteractor L_Hand, R_Hand;
    public GrabInteractor L_Con, R_Con;
    public bool isWait;

    Specialty HoldingObject;

    //합쳐질 시간을 주고 그 후에 물체 소환하면 될 듯?


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
        NewSpawnReady();

    }

    public static GameManagement Instance{//싱글톤 패턴 인스턴스화
        get{
            if(null == instance){
                return null;
            }
            return instance;
        }
    }

    public void Update()
    {
        GrabCheck();
    }

    public void GameStart(){//게임 시작할 때
        isStart = true;
        NewSpawnReady();
        NewSpawn();
    }

    public void GameEnd(){//게임 끝날 때
        isStart = false;
        endUIs.SetActive(true);
        //BGM
        //UI등장 밑 값 재설정
    }

    public void GrabCheck()
    {
        if(!isStart) return;
        if(L_Con.isGrabbed || R_Con.isGrabbed || L_Hand.isGrabbed || R_Hand.isGrabbed){
            GameOverColliderCheck_False();
        }else if(!L_Con.isGrabbed && !R_Con.isGrabbed && !L_Hand.isGrabbed && !R_Hand.isGrabbed)
        {
            Invoke("GameOverColliderCheck_True", 1f);
        }
    }

    public void AlreadySummonedCheck()
    {
        if(alreadySummoned == null) return;
        Specialty alSumo = alreadySummoned.GetComponent<Specialty>();
        if (alSumo.isAquarium)//이미 소환된 물체가 수조에 들어가 있을 때
        {
            if (!alSumo.isNextSummoned)//그런데 소환된 물체가 레벨업한 개체가 아닐 때
            {
                oneSummon = false;
                NewSpawn();
            }else if (alSumo.isNextSummoned)//레벨업한 개체일 때.
            {
                if (oneSummon) return; //이미 한 번 소환되면 막음
                oneSummon = true; //소환 bool값
                NewSpawn(); //소환
            }
        }
    }
    
    public void PointUp(int point){
        totalPoints += point;
        point_UI.text = totalPoints + "점";
    }

    public void PointReset(){ //나중에 다시시작 버튼에 넣기 위해서
        totalPoints = 0;
        point_UI.text = totalPoints+ "점";
    }

    public void NextSpecialtySpawn(Vector3 lerpVec, string nameSpe)
    {
        for(int i = 0; i < SpecialtyList.Count; i++){ //특산물 수만큼 반복
            Debug.Log(i);
            if(nameSpe == SpecialtyList[i].name){ //i번째 특산물 어레이와 지금 충돌한 오브젝트들의 이름이 같을 때
                GameObject nextSpe = Instantiate(SpecialtyList[i+1], lerpVec, Quaternion.identity); //i+1번째(다음) 특산물을 충돌한 지점에 소환
                nextSpe.GetComponent<Specialty>().isAquarium = true;
                alreadySummoned = nextSpe;
                Debug.Log(SpecialtyList[i+1] + "NextSpecialSpawn");

                if (!Specialty_bool[i])
                {
                    InformationOutput(nextSpe.GetComponent<Specialty>());
                    Specialty_bool[i] = true;
                }

                break;
            }else if(i == SpecialtyList.Count){//어레이 마지막에 도달했을 때, 아무 일도 일어나지 않음.
                break;
            }
        }

        alreadySummoned = null;
    }

    public void FloorFalledSpawn(GameObject nameSpe){
        nameSpe.transform.position = spawnPoint.position;
        nameSpe.GetComponent<Specialty>().isNextSummoned = false;
    }

    public void WallImpacted(SpecialtyData specialtyData){
        for(int i = 0; i< SpecialtyList.Count; i++){
            if(specialtyData.name == SpecialtyList[i].GetComponent<Specialty>().specialtyData.name){
                GameObject nextSpe = Instantiate(SpecialtyList[i], spawnPoint.position, Quaternion.identity);
                nextSpe.GetComponent<Specialty>().isNextSummoned = false;
                alreadySummoned = nextSpe;
            }
        }
    }

    public void NewSpawnReady(){
        if(isStart){
         index = Random.Range(0,5);
        }
    }

    public void NewSpawn()
    {
        GameObject nextSpe = Instantiate(SpecialtyList[index], spawnPoint.position, Quaternion.identity);
        nextSpe.GetComponent<Specialty>().isNextSummoned = false;
        Debug.Log("NewSpawned"+SpecialtyList[index]);
        alreadySummoned = nextSpe;

        if(!Specialty_bool[index]){//첫번째 소환
            InformationOutput(nextSpe.GetComponent<Specialty>());
            Specialty_bool[index] = true;
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
        Invoke("InformationQuit", 30f);    
    }

    public void InformationQuit(){
        SpecialtyInformationTexts.SetActive(false);
    }

    public void GameOverColliderCheck_True(){
        gameOver_Col.SetActive(true);
    }

    public void GameOverColliderCheck_False(){
        gameOver_Col.SetActive(false);
    }

}