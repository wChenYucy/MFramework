using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CyTimer : MonoBehaviour
{
    private List<CyTimerTask> timerTaskList;
    private List<CyTimerTask> tempTaskList;
    private int taskID;
    private HashSet<int> taskIDSet;
    private static readonly string obj = "lock";
    public void Init()
    {
        timerTaskList = new List<CyTimerTask>();
        tempTaskList = new List<CyTimerTask>();
        taskID = 0;
        taskIDSet = new HashSet<int>();
        Debug.Log("Cytimer Init Done.");
    }

    public void Tick()
    {
        for (int i = 0; i < tempTaskList.Count; i++)
        {
            timerTaskList.Add(tempTaskList[i]);
        }
        tempTaskList.Clear();
        for (int i = 0; i < timerTaskList.Count; i++)
        {
            CyTimerTask timerTask = timerTaskList[i];
            if (Time.realtimeSinceStartup * 1000 < timerTask.FinishedTime)
                continue;
            Action action = timerTask.CallBack;
            action?.Invoke();
            if (timerTask.Count == 1)
            {
                timerTaskList.RemoveAt(i);
                taskIDSet.Remove(timerTask.TaskID);
                i--;
            }
            else
            {
                timerTask.FinishedTime += timerTask.Delay;
                if (timerTask.Count != 0)
                    timerTask.Count--;
            }
        }
    }

    public int AddTimerTask(Action callBack, float delay, CyTimerUnit timerUnit = CyTimerUnit.Millisecond,int count = 1)
    {
        if (timerUnit != CyTimerUnit.Millisecond)
        {
            switch (timerUnit)
            {
                case CyTimerUnit.Second:
                    delay = delay * 1000;
                    break;
                case CyTimerUnit.Minute:
                    delay = delay * 1000 * 60;
                    break;
                case CyTimerUnit.Hour:
                    delay = delay * 1000 * 60 * 60;
                    break;
                case CyTimerUnit.Day:
                    delay = delay * 1000 * 60 * 60 * 24;
                    break;
                default:
                    Debug.LogError("CyTimerUnit类型错误，进制转换失败！");
                    break;
            }
        }

        int taskID = GetTaskID();
        float finishedTime = Time.realtimeSinceStartup * 1000 + delay;
        CyTimerTask timerTask = new CyTimerTask(taskID, callBack, finishedTime, delay, count >= 0 ? count : 1);
        tempTaskList.Add(timerTask);
        taskIDSet.Add(taskID);
        return taskID;
    }

    public bool DeleteTimerTask(int taskID)
    {
        bool exist = false;
        if (taskIDSet.Contains(taskID))
        {
            taskIDSet.Remove(taskID);
            for (int i = 0; i < timerTaskList.Count; i++)
            {
                if (taskID == timerTaskList[i].TaskID)
                {
                    timerTaskList.Remove(timerTaskList[i]);
                    exist = true;
                    break;
                }
            }

            if (!exist)
            {
                for (int i = 0; i < tempTaskList.Count; i++)
                {
                    if (taskID == tempTaskList[i].TaskID)
                    {
                        tempTaskList.Remove(tempTaskList[i]);
                        exist = true;
                        break;
                    }
                }
            }
        }

        return exist;
    }

    private int GetTaskID()
    {
        lock (obj)
        {
            taskID++;
            if (taskID == int.MaxValue)
                taskID = 0;
            while (taskIDSet.Contains(taskID))
            {
                taskID++;
            }
        }

        return taskID;
    }
}
