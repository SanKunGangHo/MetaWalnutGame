using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class allSpecialtyDelete : MonoBehaviour
{
    public GameObject alreadySummonObject;
    public bool isAlreadySummon = false;

    private void Update() {
        if(!GameManagement.Instance.isStart){
            Destroy(alreadySummonObject);
            isAlreadySummon = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Specialty"))
        {
            if(!isAlreadySummon){
                isAlreadySummon = true;
                alreadySummonObject = collision.gameObject;
            }else if(isAlreadySummon && alreadySummonObject != null){
                Destroy(collision.gameObject);
            }
        }
    }

    private void OnCollisionExit(Collision collision){
        if(alreadySummonObject == collision.gameObject){
            alreadySummonObject = null;
            isAlreadySummon = false;
        }
    }
}
