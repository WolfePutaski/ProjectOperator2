using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponItem
{

    public SCO_Weapon_Master equippedWeaponStats;
    public WeaponFunctionProperties equippedWeaponProperties;
    public List<SCO_WeaponMods> equippedMods;
    
    public WeaponItem(SCO_Weapon_Master weaponClass)
    {
        equippedWeaponStats = Object.Instantiate(weaponClass);
        equippedWeaponProperties = new WeaponFunctionProperties(weaponClass);

    }        

    public WeaponItem(SCO_Weapon_Master weaponClass, List<SCO_WeaponMods> newEquippedMods)
    {
        equippedWeaponStats = Object.Instantiate(weaponClass);
        equippedMods = new List<SCO_WeaponMods>();

        if (newEquippedMods.Count > 0)
        {
            foreach (SCO_WeaponMods _WeaponMods in newEquippedMods)
            {
                bool incompatibleMod()
                {
                    foreach (SCO_WeaponMods currentMod in equippedMods)
                    {
                        if (_WeaponMods != null)
                        {
                            bool sameType = _WeaponMods.modType == currentMod.modType;
                            if (equippedMods.Contains(_WeaponMods) || sameType)
                            {
                                return true;
                            }
                        }
                        else return true;

                    }

                    equippedMods.Add(_WeaponMods);
                    return false;
                }

                if (incompatibleMod())
                    break;

                float modByPercent(float a)
                {
                    return 1 + a;
                }

                equippedWeaponStats.cycleTime /= (1 + _WeaponMods.ModFirerate);
                equippedWeaponStats.reloadTime *= (1 + _WeaponMods.ModReloadTime);
                equippedWeaponStats.reloadPerfectStart *= (1 + _WeaponMods.ModReloadTime);

                if (_WeaponMods.ModForceOffPerfectLoad)
                    equippedWeaponStats.reloadPerfectWindow = 0;

                equippedWeaponStats.magCapacity += (int)(equippedWeaponStats.magCapacity * _WeaponMods.ModMag);

                equippedWeaponStats.recoilKick *= modByPercent(_WeaponMods.ModKick);
                equippedWeaponStats.swayOffset *= modByPercent(_WeaponMods.ModSway);
            }

            equippedWeaponStats.weaponName += "+";
        }

        equippedWeaponProperties = new WeaponFunctionProperties(equippedWeaponStats);
    }
}

[System.Serializable]
public class WeaponNMod
{
    [SerializeField] public SCO_Weapon_Master weaponClass;
    [SerializeField] public List<SCO_WeaponMods> equippedMods;
}

public class SC_Inventory : MonoBehaviour
{
    public List<WeaponItem> weaponItemList;
    public int currentSlot = 0;
    public bool NotReceiveInput;
    private IEnumerator _weaponPromptCoroutine;
    [SerializeField] private float newWeaponPromptTimer = 5;

    [Header("===Debug===")]
    public List<WeaponNMod> startWeaponList;

    void Start()
    {
        for (int i = 0; i < startWeaponList.Count; i++)
        {
            weaponItemList[i] = new WeaponItem(startWeaponList[i].weaponClass, startWeaponList[i].equippedMods);
        }
    }

    public void ChangeWeaponTo(int i)
    {
        currentSlot = weaponItemList[i].equippedWeaponStats? i : currentSlot ;
    }

    public void AskToSpawnWeapon(WeaponItem weaponToSpawn)
    {
        bool hasEmptySlot = false;
        foreach (WeaponItem wpn in weaponItemList)
        {
            hasEmptySlot = wpn.equippedWeaponStats == null;
        }

        if (_weaponPromptCoroutine != null)
            StopCoroutine(_weaponPromptCoroutine);

        var camera = FindObjectOfType<SC_CameraShake>();
        camera.ShakeCamera(RecoilKickShake.MEDIUM);

        if (!hasEmptySlot)
        {
            _weaponPromptCoroutine = ShowWeaponSpawnText(weaponToSpawn,true);
            StartCoroutine(_weaponPromptCoroutine);
        }
        else
        {
            ReplaceWeapon(weaponToSpawn);
            _weaponPromptCoroutine = ShowWeaponSpawnText(weaponToSpawn);
            StartCoroutine(_weaponPromptCoroutine);
        }
    }

    public void ReplaceWeapon(WeaponItem newWeapon)
    {
        int slotToReplace = currentSlot;

        for (int i = 0; i < 3; i++)
        {
            if (weaponItemList[i].equippedWeaponStats == null)
            {
                slotToReplace = i;
                break;
            }
        }

        weaponItemList[slotToReplace] = newWeapon;
    }

    IEnumerator ShowWeaponSpawnText(WeaponItem weaponToSpawn, bool askToSpawn = false)
    {
        var weaponUI = GetComponent<SC_PlayerUI>().weaponUI;

        weaponUI.SetWeaponPromptText(weaponToSpawn);
        weaponUI.SetActiveWeaponPromptText(true,askToSpawn);

        if (askToSpawn)
        {
            bool hasConfirmedPrompt = false;
            weaponUI.weaponPrompt.timeBar.fillAmount = 1;
            float countDownTime = 0;

            while (!hasConfirmedPrompt)
            {
                //CheckPressKey
                if (Input.GetKeyDown(KeyCode.E)) //Accept
                {
                    ReplaceWeapon(weaponToSpawn);
                    hasConfirmedPrompt = true;
                }
                else if (Input.GetKeyDown(KeyCode.Q)) //Decline
                {
                    hasConfirmedPrompt = true;
                }

                if (hasConfirmedPrompt)
                    break;
                
                //Countdown
                countDownTime += Time.deltaTime;
                weaponUI.weaponPrompt.timeBar.fillAmount = (newWeaponPromptTimer - countDownTime) / newWeaponPromptTimer;
                yield return new WaitForEndOfFrame();
                if (countDownTime >= newWeaponPromptTimer)
                    hasConfirmedPrompt = true;

                yield return null;
            }

            yield return new WaitUntil(() => hasConfirmedPrompt == true);
            var camera = FindObjectOfType<SC_CameraShake>();
            camera.ShakeCamera(RecoilKickShake.MEDIUM);
        }
        else
        {
            yield return new WaitForSeconds(3f);
        }
        weaponUI.SetActiveWeaponPromptText(false);
        yield break;

    }
}