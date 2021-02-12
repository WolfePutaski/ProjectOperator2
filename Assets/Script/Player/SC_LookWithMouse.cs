using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SC_GetMousePosition))]
public class SC_LookWithMouse : SC_LookDir
{
    SC_GetMousePosition mousePosition;

    // Start is called before the first frame update
    void Awake()
    {
        mousePosition = GetComponent<SC_GetMousePosition>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles = new Vector3(0, 0, LookAngle(mousePosition.GetMousePos()));
    }
}
