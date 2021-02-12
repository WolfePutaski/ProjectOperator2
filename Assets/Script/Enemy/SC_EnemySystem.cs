using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_EnemySystem : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var PlayerTarget = GameObject.FindGameObjectWithTag("Player").transform;
        GetComponent<SC_MoveToObject>().target = PlayerTarget;
        GetComponent<SC_LookDir>().target = PlayerTarget;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
