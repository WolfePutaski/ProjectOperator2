using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_CameraShake : MonoBehaviour
{
    private GameObject cameraHolder;
    private Animator animator;

    // Start is called before the first frame update
    void Awake()
    {
        cameraHolder = GameObject.FindGameObjectWithTag("CameraHolder");
        animator = cameraHolder.GetComponent<Animator>();
    }

    public void ShakeCamera(RecoilKickShake recoilKickShake)
    {
        switch (recoilKickShake)
        {
            case RecoilKickShake.LIGHT:
            animator.Play("Camera_RecoilLow", 0, 0f);
                break;
            case RecoilKickShake.MEDIUM:
                animator.Play("Camera_RecoilMedium", 0, 0f);
                break;
            case RecoilKickShake.HEAVY:
                break;

        }
    }
}
