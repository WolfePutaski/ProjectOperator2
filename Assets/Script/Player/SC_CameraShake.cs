using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_CameraShake : MonoBehaviour
{
    public GameObject cameraHolder;
    public Animator animator;

    // Start is called before the first frame update
    void Awake()
    {
        animator = cameraHolder.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShakeCamera(RecoilKickShake recoilKickShake)
    {
        if (recoilKickShake == RecoilKickShake.LIGHT)
            animator.Play("Camera_RecoilLow", 0, 0f);
        if(recoilKickShake == RecoilKickShake.MEDIUM)
            animator.Play("Camera_RecoilMedium", 0, 0f);
    }
}
