using System;

public class CyFrameTask
{
    public int TaskID { get; set; }
    public int FinishedFrame { get; set; }
    public Action CallBack;
    public int Delay { get; set; }
    public int Count { get; set; }

    public CyFrameTask(int taskID, Action callBack, int finishedFrame, int delay, int count)
    {
        TaskID = taskID;
        CallBack = callBack;
        FinishedFrame = finishedFrame;
        Delay = delay;
        Count = count;
    }
}