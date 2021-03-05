using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_PlayerHealth : SC_Health
{
    public void Update()
    {
        if (HealthCurrent <= 0)
        {
            Kill();
        }
    }
    public new void Kill()
    {
        gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.gray;
        gameObject.GetComponentInChildren<SC_WeaponFunction>().enabled = false;
        gameObject.GetComponentInChildren<SC_LookDir>().enabled = false;
        gameObject.GetComponentInChildren<SC_Inventory>().NotReceiveInput = true;

        Debug.Log("PlayerDied");
    }
}
