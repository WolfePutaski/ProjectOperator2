using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SC_GetMousePosition))]
public class SC_LookDir : MonoBehaviour
{
    public Vector3 aimDir;
    float angle;
    public bool followControl;
    SC_GetMousePosition mousePosition;
    public Transform target;
    // Start is called before the first frame update
    void Start()
    {
        mousePosition = GetComponent<SC_GetMousePosition>();
        //transform.rotation.
    }
    // Update is called once per frame
    void Update()
    {

        if (followControl)
        {
            transform.eulerAngles = new Vector3(0, 0, LookAngle());
        }

        else
        {
            if (target)
            transform.eulerAngles = new Vector3(0, 0, LookAngle());

        }

    }

    public float LookAngle()
    {
        Vector3 targetPos;

        if (followControl)
            targetPos = mousePosition.GetMousePos();
        else
            targetPos = target.position;

            aimDir = (targetPos - transform.position).normalized;
        return Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg;
    }
}
