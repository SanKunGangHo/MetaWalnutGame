using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RankSystem : MonoBehaviour
{
    [Header("참가자")]
    [Tooltip("이름")]
    [SerializeField] private string[] rankerName = new string[8];
    [Tooltip("기록")]
    [SerializeField] private float[] rankRecorder = new float[8];
    //이미지 넣을지 얘기 해봐야

    [Header("랭킹 UI")]
    public GameObject rankUI;

    [Space(10f)]
    [Header("랭킹 프리팹")]
    [Tooltip("랭킹 칸")]
    public GameObject RankPrefab;
    public Sprite[] rankSprite;

    void Start()
    {
        RankingShow();
        RankInstantiate();
    }

    public void RankUpdate(string P_Name_Now, float P_Point_Now){
        for(int i = 0; i < 8; i++){
            float currentPoint = PlayerPrefs.GetFloat("P_Point_"+i.ToString(), 0);
            string currentName = PlayerPrefs.GetString("P_Name_"+i.ToString(), "NoName");
            if(currentPoint != 0.0f){
                if(P_Point_Now > currentPoint){
                    PlayerPrefs.SetFloat("P_Point_"+i.ToString(), P_Point_Now);
                    PlayerPrefs.SetString("P_Name_"+i.ToString(), P_Name_Now);
                    P_Name_Now = currentName;
                    P_Point_Now = currentPoint;
                }
            }else{
                PlayerPrefs.SetFloat("P_Point_"+i.ToString(), P_Point_Now);
                PlayerPrefs.SetString("P_Name_"+i.ToString(), P_Name_Now);
                break;
            }
        }
        RankingShow();
        RankInstantiate();
    }

    public void RankingShow(){
        for(int i = 1; i<=8; i++){
            rankRecorder[i-1] = PlayerPrefs.GetFloat("P_Point_"+i.ToString(), 00000);
            rankerName[i-1] = PlayerPrefs.GetString("P_Name_"+i.ToString(), "NoName");
        }
    }
    public void RankInstantiate(){
        if(rankUI.transform.childCount != 0){
            for(int i = 0; i < rankUI.transform.childCount; i++){
                Destroy(rankUI.transform.GetChild(i).gameObject);
            }
        }
        
        for(int i = 0; i<rankRecorder.Length; i++){
            var prefab = Instantiate(RankPrefab, rankUI.transform);
            if(i > 2){
                prefab.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = $"{i+1}";
                Destroy(prefab.transform.GetChild(0).GetChild(0).gameObject);
            }else if(i == 0){
                prefab.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = $"";
                prefab.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = rankSprite[i];
            }else if(i == 1){
                prefab.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = $"";
                prefab.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = rankSprite[i];
            }else if(i == 2){
                prefab.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = $"";
                prefab.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = rankSprite[i];
            }

            prefab.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = $"{PlayerPrefs.GetString("P_Name_"+i.ToString(), "NoName")}";
            prefab.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = $"{PlayerPrefs.GetFloat("P_Point_"+i.ToString(), 0)}";

            //UI에 정보값넣기
            //prefab.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = $"{min:00}:{sec:00.00}";
        }
    }

    public void AllResetPrefs(){
        PlayerPrefs.DeleteAll();
    }//초기화

    public void deleteRank(){
        if(rankUI.transform.childCount != 0){
            for(int i = 0; i < rankUI.transform.childCount; i++){
                Destroy(rankUI.transform.GetChild(i).gameObject);
            }
        }
    }
}
