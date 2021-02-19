using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]

public class SC_WeaponFunction : MonoBehaviour
{
    AudioSource audioSource;
    SC_Inventory inventory;

    GameObject cameraHolder;
    GameObject cameraHolder2;
    SC_Crosshair crosshair;

    SC_ReloadCircle reloadCircle;
    float ReloadTimeCount;

    SC_WeaponUI weaponUI;

    Vector3 nextSwayPos;
    float lerpPara = 0;

    Vector3 defaultAimPos;

    string FireKey => "Fire1";
    WeaponItem currentWeapon;

    void Awake()
    {
        TryGetComponent(out inventory);
        TryGetComponent(out audioSource);
        crosshair = FindObjectOfType<SC_Crosshair>();
        reloadCircle = FindObjectOfType<SC_ReloadCircle>();
        weaponUI = FindObjectOfType<SC_WeaponUI>();

    }
    void Update()
    {
        if (inventory.weaponItems[inventory.currentSlot] != null)
        {
            currentWeapon = inventory.weaponItems[inventory.currentSlot];

            Input_Fire();
            Input_ChangeFireMode();
            Input_Reload();
            Input_RackSlide();


            CrosshairUpdate();
            weaponUI.SetWeaponUIText(currentWeapon);
        }

    }

    void Input_EjectMag()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            currentWeapon.equippedWeaponProperties.ammoInMag = 0;
        }
    }

    void Input_RackSlide()
    {
        if (Input.GetMouseButton(2))
        {
            print("HEY!");
        }
    }

    WeaponItem weaponToReload;
    bool PerfectReloadCanTry;
    void Input_Reload()
    {
        if (Input.GetKeyDown(KeyCode.R) && weaponToReload == null)
        {
            weaponToReload = currentWeapon;
            PerfectReloadCanTry = true;

            currentWeapon.equippedWeaponProperties.ammoInMag = 0;
            ReloadTimeCount = currentWeapon.equippedWeaponStats.reloadTime;
            if (!weaponToReload.equippedWeaponStats.isClosedBolt)
                currentWeapon.equippedWeaponProperties.isLoaded = false;
        }

        if (weaponToReload == currentWeapon)
        {
            if (ReloadTimeCount > 0)
            {
                float deltaTime = weaponToReload.equippedWeaponStats.reloadTime - ReloadTimeCount;
                ReloadTimeCount -= Time.deltaTime;

                if (Input.GetKeyDown(KeyCode.R) && deltaTime > .5f && PerfectReloadCanTry)
                {
                    if (deltaTime - currentWeapon.equippedWeaponStats.reloadPerfectStart >= 0 &&
                        deltaTime - currentWeapon.equippedWeaponStats.reloadPerfectStart <= currentWeapon.equippedWeaponStats.reloadPerfectWindow)
                    {
                        ReloadTimeCount = 0;
                        print("Success");
                    }
                    else
                    {
                        print("Fail");
                    }
                    PerfectReloadCanTry = false;
                }
            }
            else
            {
                StopReload();
                currentWeapon.equippedWeaponProperties.ammoInMag = currentWeapon.equippedWeaponStats.magCapacity;
                if (!currentWeapon.equippedWeaponProperties.isLoaded)
                {
                    currentWeapon.equippedWeaponProperties.ammoInMag--;
                    currentWeapon.equippedWeaponProperties.isLoaded = true;
                }


            }
        }
        else
        {
            ReloadTimeCount = 0;
            weaponToReload = null;
        }

        void StopReload()
        {
            PerfectReloadCanTry = false;
            weaponToReload = null;
        }

        ReloadUIUpdate();
    }

    void ReloadUIUpdate()
    {
        reloadCircle.gameObject.SetActive(weaponToReload != null);

        reloadCircle.ReloadBar.fillAmount = 1- (ReloadTimeCount / currentWeapon.equippedWeaponStats.reloadTime);
        reloadCircle.PerfectReloadBar.fillAmount = currentWeapon.equippedWeaponStats.reloadPerfectWindow / currentWeapon.equippedWeaponStats.reloadTime;
        reloadCircle.PerfectReloadBar.rectTransform.eulerAngles = new Vector3(0,0,360 - ( 360f* currentWeapon.equippedWeaponStats.reloadPerfectStart / currentWeapon.equippedWeaponStats.reloadTime));
    }

    void Input_Fire()
    {

        if (currentWeapon.equippedWeaponProperties.isLoaded)
        {
            if (currentWeapon.equippedWeaponProperties.CurrentFiremode == FireMode.AUTO)
            {
                if (Input.GetButton(FireKey))
                {
                    Shoot();
                }

            }
            else
            {
                if (Input.GetButtonDown(FireKey))
                {
                    Shoot();
                }
            }
        }

    }

    void Input_ChangeFireMode()
    {
        if (Input.GetButtonDown("ChangeFireMode"))
        {
            if (currentWeapon.equippedWeaponStats.FiremodeToggle)

            {
                var _currentFireMode = currentWeapon.equippedWeaponProperties.CurrentFiremode;

            if (_currentFireMode == currentWeapon.equippedWeaponStats.fireMode)
                _currentFireMode = currentWeapon.equippedWeaponStats.secondFireMode;
            else
                _currentFireMode = currentWeapon.equippedWeaponStats.fireMode;

            currentWeapon.equippedWeaponProperties.CurrentFiremode = _currentFireMode;
            }
        }
    }


    void Shoot()
    {
        GetComponent<AudioSource>().PlayOneShot(currentWeapon.equippedWeaponStats.fireSound);
        //Debug.DrawRay(transform.position, (crosshair.crosshairAim.transform.position - transform.position).normalized * 1000f, Color.green, 1f);

        if (currentWeapon.equippedWeaponStats.GetType() == typeof(SCO_Weapon_Class_Shotgun))
        {
            SCO_Weapon_Class_Shotgun sgClass = (SCO_Weapon_Class_Shotgun)currentWeapon.equippedWeaponStats;
            for (int i = 0; i < sgClass.PelletCount; i++)
            {
                ShootRay(sgClass.damage / sgClass.PelletCount, crosshair.crosshairAim.transform.position, sgClass.Spread);
                print("Hi");

            }
        }
        else
            ShootRay(currentWeapon.equippedWeaponStats.damage, crosshair.crosshairAim.transform.position, 0);

        //RaycastHit2D[] hit2D;
        //hit2D = Physics2D.RaycastAll(transform.position,(crosshair.crosshairAim.transform.position - transform.position).normalized , 1000f);

        //if (hit2D.Length > 1 && hit2D[1].transform.CompareTag("Enemy"))
        //{
        //    hit2D[1].transform.GetComponent<SC_Health>().Damage(currentWeapon.equippedWeaponStats.damage);
        //    print("HIT!");
        //}

        currentWeapon.equippedWeaponProperties.isLoaded = false;
        aimKick();
        Invoke("CycleGun", currentWeapon.equippedWeaponStats.cycleTime);

        void ShootRay(float Damage, Vector3 AimPoint, float spread)
        {
            RaycastHit2D[] hit2D;
            hit2D = Physics2D.RaycastAll(transform.position, (AimPoint - transform.position).normalized + (Random.Range(-spread, spread) * new Vector3(Mathf.Sin(spread), Mathf.Cos(spread), 0)), 1000f);
            Debug.DrawRay(transform.position, ((crosshair.crosshairAim.transform.position - transform.position).normalized + (Random.Range(-spread, spread) * new Vector3(Mathf.Sin(spread),Mathf.Cos(spread),0))) * 1000f, Color.red, 1f);

            if (hit2D.Length > 1 && hit2D[1].transform.CompareTag("Enemy"))
            {
                hit2D[1].transform.GetComponent<SC_Health>().Damage(Damage);
            }
        }
    }

    void CrosshairUpdate()
    {
        defaultAimPos = crosshair.transform.position;

        Transform AimPoint = crosshair.crosshairAim.transform;
        Transform SwayPoint = crosshair.crosshairSway.transform;

        if (AimPoint.position != defaultAimPos)
        {
            ReturnCrosshair();
        }

        if (lerpPara < currentWeapon.equippedWeaponStats.swayPeriod)
        {
            MoveToSwayPosition();
        }
        else
        {
            AssignNewSway();

        }

        void ReturnCrosshair()
        {
            AimPoint.position = Vector3.Lerp(AimPoint.position, defaultAimPos, currentWeapon.equippedWeaponStats.recoilRecoverySpeed * Mathf.Sin(Time.deltaTime));
        }

        void AssignNewSway()
        {
            nextSwayPos = Random.insideUnitSphere;
            lerpPara = 0;
        }

        void MoveToSwayPosition()
        {

            lerpPara += Time.deltaTime;
            SwayPoint.localPosition = Vector2.Lerp(SwayPoint.localPosition, nextSwayPos * currentWeapon.equippedWeaponStats.swayRadius, Mathf.Sin(Time.deltaTime) / 12);
        }


    }

    void aimKick()
    {
        crosshair.crosshairAim.transform.position += -GetComponent<SC_LookDir>().aimDir * currentWeapon.equippedWeaponStats.recoilKick
            + (Random.insideUnitSphere * currentWeapon.equippedWeaponStats.recoilKick);

        GetComponent<SC_CameraShake>().ShakeCamera(currentWeapon.equippedWeaponStats.recoilKickShake);

        
    }

    void CycleGun()
    {
        if (currentWeapon.equippedWeaponProperties.ammoInMag > 0)
        {
            currentWeapon.equippedWeaponProperties.ammoInMag--;
            currentWeapon.equippedWeaponProperties.isLoaded = true;
        }
    }


}
