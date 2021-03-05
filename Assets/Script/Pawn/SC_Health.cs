using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_Health : MonoBehaviour, IKillable, IDamagable<float>
{
    public float HealthStart;
    public float HealthCurrent;

    // Start is called before the first frame update
    void Start()
    {
        HealthCurrent = HealthStart;
    }

    public void Kill()
    {
        Destroy(gameObject);
    }

    public void Damage(float damage)
    {
        HealthCurrent -= damage;
    }
}
