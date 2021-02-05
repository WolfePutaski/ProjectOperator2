using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_GetMousePosition : MonoBehaviour
{
    public Vector3 GetMousePos()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition); ;
    }
}
