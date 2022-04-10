using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRoot : MonoBehaviour
{
    // Start is called before the first frame update
    private CyTimer timer;
    private int taskID;
    void Start()
    {
        Debug.Log("Game Start...");
        timer = gameObject.GetComponent<CyTimer>();
        timer.Init();
    }

    private void Update()
    {
        timer.Tick();
    }

    private void FunA()
    {
        Debug.Log("FunA Called.");
    }

    public void OnAddButtonClick()
    {
        Debug.Log("Add Time Task.");
        taskID = timer.AddTimerTask(FunA, 1000, CyTimerUnit.Millisecond, 3);
    }

    public void OnCancelButtonClick()
    {
        bool result = timer.DeleteTimerTask(taskID);
        Debug.Log("Delete Time Task : " + result + " , taskID:" + taskID);
    }
}
