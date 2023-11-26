using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class allSpecialtyDelete : MonoBehaviour
{
    public bool isAlreadySummon;

    void DestroyAll(Specialty spe)
    {
        if (!spe.isNextSummoned)
        {
            
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Specialty"))
        {
            DestroyAll(collision.gameObject.GetComponent<Specialty>());
        }
    }
}
