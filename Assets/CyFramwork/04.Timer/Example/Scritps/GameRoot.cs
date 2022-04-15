using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRoot : MonoBehaviour
{
    // Start is called before the first frame update
    private CyTimer _timer;
    private int taskID;
    void Start()
    {
        Debug.Log("Game Start...");
        _timer = new CyTimer();
        _timer.Init(info => { Debug.Log("Timer:" + info); });
    }

    private void Update()
    {
        _timer.Tick();
    }

    private void FunA()
    {
        Debug.Log("FunA Called.");
    }

    public void OnAddButtonClick()
    {
        Debug.Log("Add Time Task.");
        taskID = _timer.AddTimeTask(FunA, 1000, CyTimerUnit.Millisecond, 3);
    }

    public void OnCancelButtonClick()
    {
        bool result = _timer.DeleteTimeTask(taskID);
        Debug.Log("Delete Time Task : " + result + " , taskID:" + taskID);
    }
    
    public void OnFrameAddButtonClick()
    {
        Debug.Log("Add Time Task.");
        taskID = _timer.AddFrameTask(FunB, 60, 0);
    }

    public void OnFrameCancelButtonClick()
    {
        bool result = _timer.DeleteFrameTask(taskID);
        Debug.Log("Delete Time Task : " + result + " , taskID:" + taskID);
    }
    
    private void FunB()
    {
        Debug.Log("FunB Called.");
    }
}
