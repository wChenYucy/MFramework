using System;

public class CyTimeTask
{
    public int TaskID { get; set; }
    public double FinishedTime { get; set; }
    public Action CallBack;
    public double Delay { get; set; }
    public int Count { get; set; }

    public CyTimeTask(int taskID,Action callBack, double finishedTime, double delay, int count)
    {
        TaskID = taskID;
        CallBack = callBack;
        FinishedTime = finishedTime;
        Delay = delay;
        Count = count;
    }
}

public enum CyTimerUnit
{
    Millisecond,
    Second,
    Minute,
    Hour,
    Day
}
