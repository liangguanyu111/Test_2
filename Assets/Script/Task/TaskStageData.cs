using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;

public class TaskStageData 
{
    public DataStatus taskStageStatus; 
    private TaskStageConfig taskStageCfg;
    public TaskStageConfig TaskStageCfg { get => taskStageCfg; set => taskStageCfg = value; }
    public TaskStageData() { }
    public event Action<TaskStageData> OnTaskStageChange;
    //没有存储的进度数据时,用任务配置初始进度数据
    public TaskStageData(TaskStageConfig taskStageCfg)
    {
        TaskStageCfg = taskStageCfg;
        taskStageStatus = DataStatus.NotFinish;
    }
    //检查目标阶段是否被完成
    public void CheckStage(int finishedTaskNum)  
    {
        if(taskStageStatus==DataStatus.NotFinish && finishedTaskNum >= taskStageCfg.task_stageNum)
        {
            taskStageStatus = DataStatus.Finished;
            OnTaskStageChange?.Invoke(this);
        }
    }
    public void GetReward()
    {
        Debug.Log("获取任务阶段奖励");
        taskStageStatus = DataStatus.Get;
        OnTaskStageChange?.Invoke(this);
    }

    public void ResetTaskData()
    {
        taskStageStatus = DataStatus.NotFinish;
        OnTaskStageChange?.Invoke(this);
    }
}
