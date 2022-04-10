using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CyTimerTask
{
    public int TaskID { get; set; }
    public float FinishedTime { get; set; }
    public Action CallBack;
    public float Delay { get; set; }
    public int Count { get; set; }

    public CyTimerTask(int taskID,Action callBack, float finishedTime, float delay, int count)
    {
        TaskID = taskID;
        CallBack = callBack;
        FinishedTime = finishedTime;
        Delay = delay;
        Count = count;
    }
}
