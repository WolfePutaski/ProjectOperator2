using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_CameraFunctions : MonoBehaviour
{
    public static SC_CameraFunctions sharedInstance;
    AudioSource audioSource;
    Animator animator;

    [SerializeField] private AudioClip damageSound;

    void Start()
    {
        sharedInstance = this;
        TryGetComponent(out audioSource);
        TryGetComponent(out animator);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShakeDamage()
    {
        audioSource.PlayOneShot(damageSound);
        animator.Play("Camera_ShakeDamage", 0, 0f);

    }
}
