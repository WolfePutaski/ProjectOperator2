using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_LookDir : MonoBehaviour
{
    public Vector3 aimDir;
    public Transform target;

    // Update is called once per frame
    void Update()
    {
        {
            if (target)
            transform.eulerAngles = new Vector3(0, 0, LookAngle(target.transform.position));

        }

    }

    public float LookAngle(Vector3 targetPs)
    {
            aimDir = (targetPs - transform.position).normalized;
        return Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg;
    }

    public static Vector3 GetVectorFromAngle(float angle)
    {
        float angleRad = angle * (Mathf.PI / 180f);
        return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }
}


