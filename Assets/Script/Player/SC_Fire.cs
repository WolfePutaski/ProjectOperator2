using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]

public class SC_Fire : MonoBehaviour
{
    AudioSource audioSource;
    SC_Inventory inventory;
    //Transform crosshair;
    //Transform sway;
    //Transform recoil;
    //Transform aimPoint;
    public Transform[] crosshairArray;

    GameObject cameraHolder;
    GameObject cameraHolder2;

    //Sway
    Vector3 nextSwayPos;
    public float lerpPara = 0;

    Vector3 defaultAimPos;

    void Awake()
    {
        inventory = GetComponent<SC_Inventory>();
        //crosshair = FindObjectOfType<SC_Crosshair>().gameObject.transform;
        //recoil = crosshair.GetComponentInChildren<Transform>();

        //ch = new List<Transform>();
        crosshairArray = FindObjectOfType<SC_Crosshair>().GetComponentsInChildren<Transform>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }

        CrosshairUpdate();
    }

    void Shoot()
    {
        Debug.Log("FIRE!");
        GetComponent<AudioSource>().PlayOneShot(inventory.equippedWeapon.fireSound);
        Debug.DrawRay(transform.position, (crosshairArray[3].position - transform.position).normalized * 1000f, Color.green, 1f);
        Invoke("aimKick", 0.04f);
    }

    void CrosshairUpdate()
    {
        defaultAimPos = crosshairArray[0].position;

        //RETURNING FROM RECOIL
        //After the gun kicks up, it will return to it's initially is, which is the mouse cursor.
        if (crosshairArray[3].position != defaultAimPos)
        {
            crosshairArray[3].position = Vector3.Lerp(crosshairArray[3].position, defaultAimPos, inventory.equippedWeapon.recoilRecoverySpeed * Mathf.Sin(Time.deltaTime)); //I use Mathf.Sin to give the motion a curve feeling tp the motion instead of linear motion.
        }

        //SWAYING
        //For every seconds in swaySpeed will assign a new positiion randomly where the gun will sway to. The higher the sway speed, the more it will feel jumping around. lerpPara is the interval to assign newSwayPos.
        if (lerpPara < inventory.equippedWeapon.swaySpeed)
        {
            lerpPara += Time.deltaTime;
            crosshairArray[2].localPosition = Vector2.Lerp(crosshairArray[2].localPosition, nextSwayPos * inventory.equippedWeapon.swayRadius, Mathf.Sin(Time.deltaTime) / 12); //The SwayCrosshair will move to the nextSwayPos, which is assigned randomly times by swayRadius.
        }
        else
        {
            nextSwayPos = Random.insideUnitSphere;
            lerpPara = 0;

        }

    }


    //public IEnumerator aimKck() //gun Kicks up from a recoil when firing. This function is called from SC_PlayerShoot;
    //{
    //    yield return new WaitForSeconds(0.04f); //This gives a delay to recoil
    //    gunCrosshair.transform.position += new Vector3(0, playerProperties.currentGunInfo.recoilKick, 0)
    //        + Random.insideUnitSphere * playerProperties.currentGunInfo.recoilKick;

    //    //The gunCrosshair positon is added by a random recoil kick value on Y axis and a random angle to give a look that the gun spread everywhere while still ensuring that the gun will always kicks up and not "kicking down".
    //    // Kick up + Kick Angles
    //}
    void aimKick() //gun Kicks up from a recoil when firing. This function is called from SC_PlayerShoot;
    {
        crosshairArray[3].position += -GetComponent<SC_LookDir>().aimDir * inventory.equippedWeapon.recoilKick
            + (Random.insideUnitSphere * inventory.equippedWeapon.recoilKick);

        //The gunCrosshair positon is added by a random recoil kick value on Y axis and a random angle to give a look that the gun spread everywhere while still ensuring that the gun will always kicks up and not "kicking down".
        // Kick up + Kick Angles
    }
}
