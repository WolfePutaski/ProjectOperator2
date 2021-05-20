using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]

public class SC_WeaponFunction : MonoBehaviour
{
    AudioSource audioSource;
    SC_Inventory inventory;
    SC_Crosshair crosshair;
    SC_CameraShake cameraShake;

    SC_ReloadCircle reloadCircle;
    float ReloadTimeCount;

    float cycleDelay;

    Vector3 nextSwayPos;
    float lerpPeriodCount = 0;

    Vector3 defaultAimPos;

    string FireKey => "Fire1";
    WeaponItem _currentWeapon;
    public WeaponItem currentWeapon => _currentWeapon;
    [SerializeField] Transform _muzzle;

    SC_ObjectPooler objectPooler;

    [Header("===Audio===")]
    [SerializeField] AudioClip boltLockSound;
    [SerializeField] AudioClip UnjamSound;



    void Awake()
    {
        TryGetComponent(out inventory);
        TryGetComponent(out audioSource);
        crosshair = FindObjectOfType<SC_Crosshair>();
        reloadCircle = FindObjectOfType<SC_ReloadCircle>();
        objectPooler = FindObjectOfType<SC_ObjectPooler>();
        cameraShake = FindObjectOfType<SC_CameraShake>();

    }
    void Update()
    {
        if (inventory.weaponItemList[inventory.currentSlot] != null)
        {
            _currentWeapon = inventory.weaponItemList[inventory.currentSlot];
            CrosshairUpdate();
        }

    }

    void FixedUpdate()
    {
        if (_currentWeapon != null)
        {
            if (_currentWeapon.equippedWeaponProperties.chamberCondition == ChamberCondition.EMPTY)
            {
                if (cycleDelay >= 0)
                    cycleDelay -= Time.fixedDeltaTime;
                else
                    CycleAmmo();
            }

        }
    }

    void ReloadUIUpdate()
    {
        switch (currentWeapon.equippedWeaponProperties.chamberCondition)
        {
            case ChamberCondition.JAMMED:
                UnjammingUI();
                break;
            default:
                ReloadingUI();
                break;
        }

        void UnjammingUI()
        {
            reloadCircle.gameObject.SetActive(true);

            reloadCircle.ReloadBar.color = reloadCircle.JamColor;
            reloadCircle.ReloadBar.fillAmount = (float)_currentWeapon.equippedWeaponProperties.UnJamAmount/ (float)_currentWeapon.equippedWeaponProperties.MaxUnJamAmount; //unjam amount



        }

        void ReloadingUI()
        {
            reloadCircle.gameObject.SetActive(weaponToReload != null);

            reloadCircle.ReloadBar.color = reloadCircle.ReloadColor;
            reloadCircle.ReloadBar.fillAmount = 1 - (ReloadTimeCount / _currentWeapon.equippedWeaponStats.reloadTime);

            reloadCircle.PerfectReloadBar.fillAmount = PerfectReloadCanTry? _currentWeapon.equippedWeaponStats.reloadPerfectWindow / _currentWeapon.equippedWeaponStats.reloadTime : 0f;
            reloadCircle.PerfectReloadBar.rectTransform.eulerAngles = new Vector3(0,0,360 - ( 360f* _currentWeapon.equippedWeaponStats.reloadPerfectStart / _currentWeapon.equippedWeaponStats.reloadTime));
        }
        
    }
    void Shoot()
    {
        audioSource.PlayOneShot(_currentWeapon.equippedWeaponStats.fireSound);

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
        
        aimKick();

        DecreaseCondition();

        void DecreaseCondition()
        {
            _currentWeapon.equippedWeaponProperties.currentCondition = Mathf.Max(0, _currentWeapon.equippedWeaponProperties.currentCondition - 1);
            float conditionPercent = Mathf.Clamp01(_currentWeapon.equippedWeaponProperties.currentCondition / _currentWeapon.equippedWeaponStats.maxCondition);
            float conditionPercentDegraded = Mathf.Clamp01(1f - conditionPercent);

            float conditionFactor = Mathf.Max(0,(Mathf.Max(conditionPercentDegraded, 0f) - 1f)/0.8f + 1f);
            float jamChance = _currentWeapon.equippedWeaponStats.minJamChance + (Mathf.Pow(conditionFactor,2f)*(_currentWeapon.equippedWeaponStats.maxJamChance - _currentWeapon.equippedWeaponStats.minJamChance));
            float jamCheckNum = Random.Range(0f, 1f);
            Debug.Log("JamChance: " + jamChance + " JamCheck: " + jamCheckNum);


            if (jamCheckNum < jamChance)
                JamWeapon();
            else
            {
                _currentWeapon.equippedWeaponProperties.chamberCondition = ChamberCondition.EMPTY;
                if (_currentWeapon.equippedWeaponProperties.ammoInMag > 0)
                    cycleDelay = _currentWeapon.equippedWeaponStats.cycleTime;
                else
                    audioSource.PlayOneShot(boltLockSound);
            }

        }

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
                    {
                        //Check Headshot
                        foreach (RaycastHit2D col in hit2D)
                        {
                            //Debug.Log(col.collider.name);
                            if(col.collider.name == "Head" && col.collider.transform.IsChildOf(hitObject.transform))
                            {
                                var headCollider = col.collider;

                                if (headCollider.OverlapPoint(crosshair.crosshairAim.transform.position))
                                {
                                    Damage += Damage * _currentWeapon.equippedWeaponStats.headShotMultiplier;
                                }
                                break;

                            }

                        }
                        healthReciever.Damage(Damage);

                    }
                    
                    hitCount++;

                    SC_WeaponPool weaponPool;
                    TryGetComponent(out weaponPool);
                    weaponPool.AddComboScore(weaponPool.hitScore, false);
                }

                //Debug.Log(hitObject);

                Vector3 lastHitpoint = hit2D.Length > 1 ? (Vector3)hit2D[1].point : ((crosshair.crosshairAim.transform.position - transform.position).normalized + _spread) * 20;

                var bulletTracer = objectPooler.GetPooledObject("BulletTracer");
                bulletTracer.SetActive(true);
                bulletTracer.GetComponent<LineRenderer>().SetPosition(0, _muzzle.position);
                bulletTracer.GetComponent<LineRenderer>().SetPosition(1, lastHitpoint);
                objectPooler.DeactivePooledObject(bulletTracer, 0.04f);
            }
        }
        
    }

    void JamWeapon()
    {
        _currentWeapon.equippedWeaponProperties.chamberCondition = ChamberCondition.JAMMED;

        int UnjamAmountSet = Mathf.Max(1, (int)
            (Random.Range(0, 5) * (0.8f - (_currentWeapon.equippedWeaponProperties.currentCondition / _currentWeapon.equippedWeaponStats.maxCondition)) + 1));

        _currentWeapon.equippedWeaponProperties.UnJamAmount = UnjamAmountSet;
        _currentWeapon.equippedWeaponProperties.MaxUnJamAmount = UnjamAmountSet;

        cameraShake.ShakeCamera(RecoilKickShake.MEDIUM);
        audioSource.PlayOneShot(boltLockSound);
        
    }
    void CycleAmmo()
    {
        if (_currentWeapon.equippedWeaponProperties.ammoInMag > 0)
        {
            _currentWeapon.equippedWeaponProperties.ammoInMag--;
            _currentWeapon.equippedWeaponProperties.chamberCondition = ChamberCondition.LOADED;
        }
        else
        {
            _currentWeapon.equippedWeaponProperties.chamberCondition = ChamberCondition.EMPTY;
        }
    }
    void aimKick()
    {
        crosshair.crosshairAim.transform.position += (SC_LookDir.GetVectorFromAngle(transform.eulerAngles.z) * -_currentWeapon.equippedWeaponStats.recoilKick) 
            + Random.insideUnitSphere * _currentWeapon.equippedWeaponStats.recoilKick;

        //crosshair.RecoilOffset += (SC_LookDir.GetVectorFromAngle(transform.eulerAngles.z) * -_currentWeapon.equippedWeaponStats.recoilKick)
        //    + Random.insideUnitSphere * _currentWeapon.equippedWeaponStats.recoilKick;

        GetComponent<SC_CameraShake>().ShakeCamera(_currentWeapon.equippedWeaponStats.recoilKickShake);
    }
    void CrosshairUpdate() //Will use Vector3 instead of transform
    {
        defaultAimPos = crosshair.transform.position;

        Transform AimPoint = crosshair.crosshairAim.transform;
        Transform SwayPoint = crosshair.crosshairSway.transform;

        //Vector3 swayOffset = crosshair.SwayOffset;
        //Vector3 recoilOffset = crosshair.RecoilOffset;

        //if (recoilOffset != Vector3.zero)
        //{
        //    recoilOffset = Vector3.Lerp(recoilOffset, Vector3.zero,
        //    Mathf.Pow(Time.deltaTime, .7f) * _currentWeapon.equippedWeaponStats.recoilRecoverySpeed);
        //}


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
            //swayOffset = Vector2.Lerp(swayOffset, nextSwayPos, Time.deltaTime / _currentWeapon.equippedWeaponStats.swayPeriod);
        }

        Debug.DrawRay(transform.position, SC_LookDir.GetVectorFromAngle(transform.eulerAngles.z + _currentWeapon.equippedWeaponStats.swayOffset/2f) * 1000f, Color.cyan);
        Debug.DrawRay(transform.position, SC_LookDir.GetVectorFromAngle(transform.eulerAngles.z -_currentWeapon.equippedWeaponStats.swayOffset/2f) * 1000f, Color.cyan);


        //crosshair.RecoilOffset = recoilOffset;
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
        switch(_currentWeapon.equippedWeaponProperties.chamberCondition)
        {
            case ChamberCondition.JAMMED:
                UnJam();
                break;
            default:
                Reload();
                break;
        }

        void Reload()
        {
            if (Input.GetKeyDown(KeyCode.R) && weaponToReload == null)
            {
                weaponToReload = _currentWeapon;
                PerfectReloadCanTry = true;

                _currentWeapon.equippedWeaponProperties.ammoInMag = 0;
                ReloadTimeCount = _currentWeapon.equippedWeaponStats.reloadTime;
                if (!weaponToReload.equippedWeaponStats.isClosedBolt)
                    _currentWeapon.equippedWeaponProperties.chamberCondition = ChamberCondition.EMPTY;
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
                    if (_currentWeapon.equippedWeaponProperties.chamberCondition == ChamberCondition.EMPTY)
                    {
                        _currentWeapon.equippedWeaponProperties.ammoInMag--;
                        _currentWeapon.equippedWeaponProperties.chamberCondition = ChamberCondition.LOADED;
                    }


                }
            }
            else
            {
                ReloadTimeCount = 0;
                weaponToReload = null;
                }
        }

        void StopReload()
        {
            PerfectReloadCanTry = false;
            weaponToReload = null;
        }

        void UnJam()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                _currentWeapon.equippedWeaponProperties.UnJamAmount--;
                audioSource.PlayOneShot(UnjamSound);
                cameraShake.ShakeCamera(RecoilKickShake.LIGHT);
                if (_currentWeapon.equippedWeaponProperties.UnJamAmount <= 0)
                {
                    audioSource.PlayOneShot(boltLockSound);
                    CycleAmmo();
                    Debug.Log("Unjammed!");
                }
            }
        }

        ReloadUIUpdate();
    }
    public void Input_Fire()
    {

        if (_currentWeapon.equippedWeaponProperties.chamberCondition == ChamberCondition.LOADED)
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
