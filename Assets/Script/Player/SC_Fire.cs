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
    SC_Crosshair crosshair;

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
        crosshair = FindObjectOfType<SC_Crosshair>();
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
        Debug.DrawRay(transform.position, (crosshair.crosshairAim.transform.position - transform.position).normalized * 1000f, Color.green, 1f);

        RaycastHit2D[] hit2D;
        hit2D = Physics2D.RaycastAll(transform.position, (crosshair.crosshairAim.transform.position - transform.position).normalized, 1000f);

        if (hit2D[1].transform.CompareTag("Enemy"))
        {
            hit2D[1].transform.GetComponent<SC_Health>().Damage(inventory.equippedWeapon.damage);
        }


        Invoke("aimKick", 0.04f);
    }

    void CrosshairUpdate()
    {
        defaultAimPos = crosshair.transform.position;

        Transform AimPoint = crosshair.crosshairAim.transform;
        Transform SwayPoint = crosshair.crosshairSway.transform;


        //RETURNING FROM RECOIL
        //After the gun kicks up, it will return to it's initially is, which is the mouse cursor.
        if (AimPoint.position != defaultAimPos)
        {
            AimPoint.position = Vector3.Lerp(AimPoint.position, defaultAimPos, inventory.equippedWeapon.recoilRecoverySpeed * Mathf.Sin(Time.deltaTime)); //I use Mathf.Sin to give the motion a curve feeling tp the motion instead of linear motion.
        }

        //SWAYING
        //For every seconds in swaySpeed will assign a new positiion randomly where the gun will sway to. The higher the sway speed, the more it will feel jumping around. lerpPara is the interval to assign newSwayPos.
        if (lerpPara < inventory.equippedWeapon.swayPeriod)
        {
            lerpPara += Time.deltaTime;
            SwayPoint.localPosition = Vector2.Lerp(SwayPoint.localPosition, nextSwayPos * inventory.equippedWeapon.swayRadius, Mathf.Sin(Time.deltaTime) / 12); //The SwayCrosshair will move to the nextSwayPos, which is assigned randomly times by swayRadius.
        }
        else
        {
            nextSwayPos = Random.insideUnitSphere;
            lerpPara = 0;

        }

    }

    void aimKick() //gun Kicks up from a recoil when firing. This function is called from SC_PlayerShoot;
    {
        crosshair.crosshairAim.transform.position += -GetComponent<SC_LookDir>().aimDir * inventory.equippedWeapon.recoilKick
            + (Random.insideUnitSphere * inventory.equippedWeapon.recoilKick);

        //The gunCrosshair positon is added by a random recoil kick value on Y axis and a random angle to give a look that the gun spread everywhere while still ensuring that the gun will always kicks up and not "kicking down".
        // Kick up + Kick Angles
    }
}
