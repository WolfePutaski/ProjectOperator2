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
    float lerpPeriodCount = 0;

    Vector3 defaultAimPos;

    string FireKey => "Fire1";
    WeaponItem _currentWeapon;
    public WeaponItem currentWeapon { get { return _currentWeapon; } }

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
            _currentWeapon = inventory.weaponItems[inventory.currentSlot];
            CrosshairUpdate();
        }

    }

    void ReloadUIUpdate()
    {
        reloadCircle.gameObject.SetActive(weaponToReload != null);

        reloadCircle.ReloadBar.fillAmount = 1- (ReloadTimeCount / _currentWeapon.equippedWeaponStats.reloadTime);
        reloadCircle.PerfectReloadBar.fillAmount = _currentWeapon.equippedWeaponStats.reloadPerfectWindow / _currentWeapon.equippedWeaponStats.reloadTime;
        reloadCircle.PerfectReloadBar.rectTransform.eulerAngles = new Vector3(0,0,360 - ( 360f* _currentWeapon.equippedWeaponStats.reloadPerfectStart / _currentWeapon.equippedWeaponStats.reloadTime));
    }
    void Shoot()
    {
        GetComponent<AudioSource>().PlayOneShot(_currentWeapon.equippedWeaponStats.fireSound);

        if (_currentWeapon.equippedWeaponStats.GetType() == typeof(SCO_Weapon_Class_Shotgun))
        {
            SCO_Weapon_Class_Shotgun sgClass = (SCO_Weapon_Class_Shotgun)_currentWeapon.equippedWeaponStats;
            for (int i = 0; i < sgClass.PelletCount; i++)
            {
                ShootRay(sgClass.damage / sgClass.PelletCount, crosshair.crosshairAim.transform.position, sgClass.Spread);
            }
        }
        else
            ShootRay(_currentWeapon.equippedWeaponStats.damage, crosshair.crosshairAim.transform.position);

        _currentWeapon.equippedWeaponProperties.isLoaded = false;
        aimKick();
        Invoke("CycleAmmo", _currentWeapon.equippedWeaponStats.cycleTime);

        void ShootRay(float Damage, Vector3 AimPoint, float spread = 0)
        {
            RaycastHit2D[] hit2D;
            var _spread = (Random.Range(-spread, spread) * new Vector3(Mathf.Sin(spread), Mathf.Cos(spread), 0));

            hit2D = Physics2D.RaycastAll(transform.position, 
                (AimPoint - transform.position).normalized + _spread, 1000f);
            Debug.DrawRay(transform.position, ((crosshair.crosshairAim.transform.position - transform.position).normalized + _spread) * 1000f, Color.red,1f);

            int hitCount = 0;

            for (int i = 0; i < hit2D.Length; i++)
            {
                CheckHit(hit2D[i].transform.gameObject);
                if (hitCount == 1) //PENETRATION
                    break;
            }


            void CheckHit(GameObject hitObject)
            {
                bool isSelf()
                {
                    if (hitObject == gameObject) return true;
                    else return false;
                }

                if (!isSelf())
                {
                    SC_Health healthReciever;
                    hitObject.TryGetComponent(out healthReciever);
                    if (healthReciever != null)
                        healthReciever.Damage(Damage);
                    
                    hitCount++;
                }

                //Debug.Log(hitObject);

                Vector3 lastHitpoint = hit2D.Length > 1 ? (Vector3)hit2D[1].point : ((crosshair.crosshairAim.transform.position - transform.position).normalized + _spread) * 20;

                var bulletTracer = SC_ObjectPooler.SharedInstance.GetPooledObject("BulletTracer");
                bulletTracer.SetActive(true);                        
                bulletTracer.GetComponent<LineRenderer>().SetPosition(1, lastHitpoint);
                SC_ObjectPooler.SharedInstance.DeactivePooledObject(bulletTracer, 0.04f);
            }
        }
    }
    void CycleAmmo()
    {
        if (_currentWeapon.equippedWeaponProperties.ammoInMag > 0)
        {
            _currentWeapon.equippedWeaponProperties.ammoInMag--;
            _currentWeapon.equippedWeaponProperties.isLoaded = true;
        }
    }
    void aimKick()
    {
        crosshair.crosshairAim.transform.position += (SC_LookDir.GetVectorFromAngle(transform.eulerAngles.z) * -_currentWeapon.equippedWeaponStats.recoilKick) 
            + Random.insideUnitSphere * _currentWeapon.equippedWeaponStats.recoilKick;
        
        GetComponent<SC_CameraShake>().ShakeCamera(_currentWeapon.equippedWeaponStats.recoilKickShake);
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

        void ReturnCrosshair()
        {
            AimPoint.position = Vector3.Lerp(AimPoint.position, defaultAimPos,
                Mathf.Pow(Time.deltaTime,.7f)* _currentWeapon.equippedWeaponStats.recoilRecoverySpeed);
        }

        float swayRadius = Vector2.Distance(transform.position, crosshair.transform.position) * Mathf.Tan(Mathf.Deg2Rad* _currentWeapon.equippedWeaponStats.swayOffset / 2f);
        if (lerpPeriodCount < _currentWeapon.equippedWeaponStats.swayPeriod)
        {
            MoveToSwayPosition();
        }
        else
        {
            AssignNewSway();
        }

        void AssignNewSway()
        {
            nextSwayPos = new Vector2(Random.Range(-1f, 1f),
                Random.Range(-1f, 1f)) * swayRadius;
            lerpPeriodCount = 0;
        }

        void MoveToSwayPosition()
        {
            lerpPeriodCount += Time.deltaTime;
            SwayPoint.localPosition = Vector2.Lerp(SwayPoint.localPosition, nextSwayPos , Time.deltaTime/_currentWeapon.equippedWeaponStats.swayPeriod);
        }

        Debug.DrawRay(transform.position, SC_LookDir.GetVectorFromAngle(transform.eulerAngles.z + _currentWeapon.equippedWeaponStats.swayOffset/2f) * 1000f, Color.cyan);
        Debug.DrawRay(transform.position, SC_LookDir.GetVectorFromAngle(transform.eulerAngles.z -_currentWeapon.equippedWeaponStats.swayOffset/2f) * 1000f, Color.cyan);
        
    }


    public void Input_EjectMag()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            _currentWeapon.equippedWeaponProperties.ammoInMag = 0;
        }
    }
    public void Input_RackSlide()
    {
        if (Input.GetMouseButton(2))
        {
        }
    }

    WeaponItem weaponToReload;
    bool PerfectReloadCanTry;
    public void Input_Reload()
    {
        if (Input.GetKeyDown(KeyCode.R) && weaponToReload == null)
        {
            weaponToReload = _currentWeapon;
            PerfectReloadCanTry = true;

            _currentWeapon.equippedWeaponProperties.ammoInMag = 0;
            ReloadTimeCount = _currentWeapon.equippedWeaponStats.reloadTime;
            if (!weaponToReload.equippedWeaponStats.isClosedBolt)
                _currentWeapon.equippedWeaponProperties.isLoaded = false;
        }

        if (weaponToReload == _currentWeapon)
        {
            if (ReloadTimeCount > 0)
            {
                float deltaTime = weaponToReload.equippedWeaponStats.reloadTime - ReloadTimeCount;
                ReloadTimeCount -= Time.deltaTime;

                if (Input.GetKeyDown(KeyCode.R) && deltaTime > .5f && PerfectReloadCanTry)
                {
                    if (deltaTime - _currentWeapon.equippedWeaponStats.reloadPerfectStart >= 0 &&
                        deltaTime - _currentWeapon.equippedWeaponStats.reloadPerfectStart <= _currentWeapon.equippedWeaponStats.reloadPerfectWindow)
                    {
                        ReloadTimeCount = 0;
                    }
                    PerfectReloadCanTry = false;
                }
            }
            else
            {
                StopReload();
                _currentWeapon.equippedWeaponProperties.ammoInMag = _currentWeapon.equippedWeaponStats.magCapacity;
                if (!_currentWeapon.equippedWeaponProperties.isLoaded)
                {
                    _currentWeapon.equippedWeaponProperties.ammoInMag--;
                    _currentWeapon.equippedWeaponProperties.isLoaded = true;
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
    public void Input_Fire()
    {

        if (_currentWeapon.equippedWeaponProperties.isLoaded)
        {
            if (_currentWeapon.equippedWeaponProperties.CurrentFiremode == FireMode.AUTO)
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
    public void Input_ChangeFireMode()
    {
        if (Input.GetButtonDown("ChangeFireMode"))
        {
            if (_currentWeapon.equippedWeaponStats.FiremodeToggle)

            {
                var _currentFireMode = _currentWeapon.equippedWeaponProperties.CurrentFiremode;

            if (_currentFireMode == _currentWeapon.equippedWeaponStats.fireMode)
                _currentFireMode = _currentWeapon.equippedWeaponStats.secondFireMode;
            else
                _currentFireMode = _currentWeapon.equippedWeaponStats.fireMode;

            _currentWeapon.equippedWeaponProperties.CurrentFiremode = _currentFireMode;
            }
        }
    }
}
