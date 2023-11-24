using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassthroughManager : MonoBehaviour
{
    public OVRPassthroughLayer passthrough;
    public bool isPassthrough;

    public Material skybox_Normal;
    public Material skybox_void = null;

    public Light sunLight;
    public Light voidLight = null;

    public GameObject map;

    // Start is called before the first frame update
    void Start()
    {
        passthrough.hidden = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PassthroughToggle(){
        isPassthrough = !isPassthrough;
        map.SetActive(!map.activeSelf);
        if(isPassthrough){
            RenderSettings.skybox = null;
            RenderSettings.sun = null;
        }else{
            RenderSettings.skybox = skybox_Normal;
            RenderSettings.sun = sunLight;
        }
        passthrough.hidden = !passthrough.hidden;
        Debug.Log(passthrough.hidden);
    }
}
