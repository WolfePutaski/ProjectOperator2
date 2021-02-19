using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_CameraShake : MonoBehaviour
{
    public GameObject cameraHolder;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShakeCamera()
    {
        cameraHolder.GetComponent<Animator>().Play("CameraShake",0,0f);
    }
}
