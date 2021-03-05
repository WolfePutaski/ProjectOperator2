using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_RecordScore : MonoBehaviour
{
    public static SC_RecordScore scoreRecorder;
    public int killCount;

    public void Start()
    {
        scoreRecorder = this;
    }
    void Update()
    {
        
    }

    public void addKillCount()
    {
        killCount++;
    }

    public void ResetKillCount()
    {
        killCount = 0;
    }

    public int GetKillCount()
    {
        return killCount;
    }
}
