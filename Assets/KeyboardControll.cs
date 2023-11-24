using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardControll : MonoBehaviour
{

    private OVRVirtualKeyboard keyboard;

    // Start is called before the first frame update
    public void ShowKeyboard(){
        keyboard.gameObject.SetActive(true);
    }
}
