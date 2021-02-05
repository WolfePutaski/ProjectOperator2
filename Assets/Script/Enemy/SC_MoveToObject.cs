using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_MoveToObject : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform target;
    public float speed;

    // Update is called once per frame
    void Update()
    {
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target.position, step);
    }
}
