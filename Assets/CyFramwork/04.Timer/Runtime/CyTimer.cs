using System;
using System.Collections.Generic;

public class CyTimer
{
    private Action<string> logCallBack;
    private static readonly string obj = "lock";
    
    private int taskID;
    private HashSet<int> taskIDSet;
    
    private DateTime startUTCTime = new DateTime(1970, 1, 1, 0, 0, 0);
    private List<CyTimeTask> timeTaskList;
    private List<CyTimeTask> tempTimeTaskList;
    
    private int currentFrame;
    private List<CyFrameTask> frameTaskList;
    private List<CyFrameTask> tempFrameTaskList;
    
    public void Init(Action<string> logAction)
    {
        logCallBack = logAction;
        taskID = 0;
        taskIDSet = new HashSet<int>();
        
        timeTaskList = new List<CyTimeTask>();
        tempTimeTaskList = new List<CyTimeTask>();
        
        currentFrame = 0;
        frameTaskList = new List<CyFrameTask>();
        tempFrameTaskList = new List<CyFrameTask>();
        
        logCallBack?.Invoke("Cytimer Init Done.");
    }

    public void Tick()
    {
        CheckTimeTack();
        CheckFrameTack();
    }

    #region TimeTask

    public void CheckTimeTack()
    {
        for (int i = 0; i < tempTimeTaskList.Count; i++)
        {
            timeTaskList.Add(tempTimeTaskList[i]);
        }
        tempTimeTaskList.Clear();
        for (int i = 0; i < timeTaskList.Count; i++)
        {
            CyTimeTask timeTask = timeTaskList[i];
            if (GetNowUTCTime() < timeTask.FinishedTime)
                continue;
            Action action = timeTask.CallBack;
            action?.Invoke();
            if (timeTask.Count == 1)
            {
                timeTaskList.RemoveAt(i);
                taskIDSet.Remove(timeTask.TaskID);
                i--;
            }
            else
            {
                timeTask.FinishedTime += timeTask.Delay;
                if (timeTask.Count != 0)
                    timeTask.Count--;
            }
        }
    }

    public int AddTimeTask(Action callBack, float delay, CyTimerUnit timerUnit = CyTimerUnit.Millisecond,int count = 1)
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
                    logCallBack?.Invoke("CyTimerUnit类型错误，进制转换失败！");
                    break;
            }
        }

        int taskID = GetTaskID();
        double finishedTime = GetNowUTCTime() + delay;
        CyTimeTask timeTask = new CyTimeTask(taskID, callBack, finishedTime, delay, count >= 0 ? count : 1);
        tempTimeTaskList.Add(timeTask);
        taskIDSet.Add(taskID);
        return taskID;
    }

    public bool DeleteTimeTask(int taskID)
    {
        bool exist = false;
        if (taskIDSet.Contains(taskID))
        {
            taskIDSet.Remove(taskID);
            for (int i = 0; i < timeTaskList.Count; i++)
            {
                if (taskID == timeTaskList[i].TaskID)
                {
                    timeTaskList.Remove(timeTaskList[i]);
                    exist = true;
                    break;
                }
            }

            if (!exist)
            {
                for (int i = 0; i < tempTimeTaskList.Count; i++)
                {
                    if (taskID == tempTimeTaskList[i].TaskID)
                    {
                        tempTimeTaskList.Remove(tempTimeTaskList[i]);
                        exist = true;
                        break;
                    }
                }
            }
        }

        return exist;
    }

    #endregion

    #region FrameTask

    public void CheckFrameTack()
    {
        currentFrame++;
        for (int i = 0; i < tempFrameTaskList.Count; i++)
        {
            frameTaskList.Add(tempFrameTaskList[i]);
        }
        tempFrameTaskList.Clear();
        for (int i = 0; i < frameTaskList.Count; i++)
        {
            CyFrameTask frameTask = frameTaskList[i];
            if (currentFrame < frameTask.FinishedFrame)
                continue;
            Action action = frameTask.CallBack;
            action?.Invoke();
            if (frameTask.Count == 1)
            {
                frameTaskList.RemoveAt(i);
                taskIDSet.Remove(frameTask.TaskID);
                i--;
            }
            else
            {
                frameTask.FinishedFrame += frameTask.Delay;
                if (frameTask.Count != 0)
                    frameTask.Count--;
            }
        }
    }

    public int AddFrameTask(Action callBack, int delay,int count = 1)
    {
        int taskID = GetTaskID();
        CyFrameTask timeTask = new CyFrameTask(taskID, callBack, currentFrame + delay, delay, count >= 0 ? count : 1);
        tempFrameTaskList.Add(timeTask);
        taskIDSet.Add(taskID);
        return taskID;
    }

    public bool DeleteFrameTask(int taskID)
    {
        bool exist = false;
        if (taskIDSet.Contains(taskID))
        {
            taskIDSet.Remove(taskID);
            for (int i = 0; i < frameTaskList.Count; i++)
            {
                if (taskID == frameTaskList[i].TaskID)
                {
                    frameTaskList.Remove(frameTaskList[i]);
                    exist = true;
                    break;
                }
            }

            if (!exist)
            {
                for (int i = 0; i < tempFrameTaskList.Count; i++)
                {
                    if (taskID == tempFrameTaskList[i].TaskID)
                    {
                        tempFrameTaskList.Remove(tempFrameTaskList[i]);
                        exist = true;
                        break;
                    }
                }
            }
        }

        return exist;
    }

    #endregion
    

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

    private double GetNowUTCTime()
    {
        return (DateTime.Now - startUTCTime).TotalMilliseconds;
    }
}
