using Oculus.Interaction.HandGrab;
using UnityEngine;

public class Specialty : MonoBehaviour
{
    public SpecialtyData specialtyData;
    [SerializeField]private bool hasInteracted = false;
    [SerializeField]private bool isAquarium = false;
    [SerializeField]private bool isWall = false;

    public bool isNextSummoned = true;

    private void Update(){
        if(transform.position.y<-10){//떨어졌을 때
            GameManagement.Instance.FloorFalledSpawn(gameObject.GetComponent<Specialty>().specialtyData.SpecialtyName); //같은 물체로 소환
            if(isAquarium){//수조에 들어갔던 애면
                GameManagement.Instance.GameEnd();//게임 종료
            }
            Destroy(gameObject);//자신 삭제
        }
    }

    private void OnCollisionEnter(Collision other) { //충돌
        if(GameManagement.Instance.isStart && !hasInteracted && isAquarium){//게임이 시작되었을 때, 아직 합치지 않았을 때.
            Specialty otherSpecialty = other.gameObject.GetComponent<Specialty>();

            if(otherSpecialty != null && otherSpecialty != this && !otherSpecialty.hasInteracted){
                if(specialtyData.SpecialtyName == otherSpecialty.specialtyData.SpecialtyName){
                    hasInteracted = true;
                    otherSpecialty.hasInteracted = true;
                    GameManagement.Instance.PointUp(specialtyData.SpecialtyPoint);
                    Debug.Log(specialtyData.SpecialtyPoint + "and" + Vector3.Lerp(transform.position, other.transform.position, 0.5f) +"and"+ specialtyData.SpecialtyName);
                    GameManagement.Instance.NextSpecialtySpawn(Vector3.Lerp(transform.position, other.transform.position, 0.5f), specialtyData.SpecialtyName);
                    Destroy(other.gameObject);
                    Destroy(gameObject);
                }
            }
        }

        if(other.gameObject.CompareTag("Floor")){
            if(isAquarium){
                GameManagement.Instance.GameEnd(); //수조에 들어갔던 특산물은 바닥에 닿으면 게임이 끝남
            }else{
                GameManagement.Instance.FloorFalledSpawn(gameObject.GetComponent<Specialty>().specialtyData.SpecialtyName); //가져가다 떨어트린 특산물은 바닥에 닿으면 같은 특산물을 소환
                Destroy(gameObject);//자기 자신을 삭제
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.CompareTag("Aquarium")){//수조에 들어갔을 때
            Invoke("Aquarium", 1f);
            gameObject.GetComponent<HandGrabInteractable>().enabled =false;
            gameObject.GetComponent<Rigidbody>().isKinematic = false;
            if(!isNextSummoned){
                GameManagement.Instance.NewSpawn();
            }
        }

        if(other.gameObject.CompareTag("GameOver") && isAquarium){
            GameManagement.Instance.GameEnd();
        }

        if(other.gameObject.CompareTag("Wall") && !isAquarium && !isNextSummoned){
            GameManagement.Instance.FloorFalledSpawn(specialtyData.SpecialtyName);
            Destroy(gameObject);
        }
    }

    void Aquarium(){
        isAquarium = true;
    }
}
