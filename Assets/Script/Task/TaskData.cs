using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using LitJson;
using Newtonsoft.Json;
using System.Threading.Tasks;

public class TaskData 
{
    private TaskConfig taskCfg; //任务配置
    [JsonIgnore]
    public TaskConfig TaskCfg { get => taskCfg; set => taskCfg = value; }

    public event Action<TaskData> OnTaskFinish;

    public int task_process; //任务进度
    public DataStatus taskStatus; // 奖励是否被领取,0为没有领取，1为任务完成，2为奖励领取
    public TaskData() { }
    public TaskData(TaskConfig taskCfg)
    { 
        this.TaskCfg = taskCfg;
        task_process = 0;
        taskStatus = DataStatus.NotFinish;
    }

    public bool IsFinished()
    {
        return taskStatus == DataStatus.Finished || taskStatus == DataStatus.Get;
    }

    public void ResetTaskData()
    {
        task_process = 0;
        taskStatus = 0;
    }
    public void CheckTaskStatus()
    {
        if(IsFinished())
        {
            OnTaskFinish?.Invoke(this);
        }
    }

    public void AddProcess(int processTimes, Action<int, DataStatus> callBack)
    {
        if(taskStatus!=DataStatus.NotFinish)
        {
            return;
        }
        task_process = task_process + processTimes <= taskCfg.target_amount ? task_process + processTimes: taskCfg.target_amount;
        if (task_process >= taskCfg.target_amount)
        {
            taskStatus = DataStatus.Finished;
            OnTaskFinish.Invoke(this);
            Debug.Log("任务id" + taskCfg.task_id + "完成");
        }
        callBack(task_process, taskStatus);
    }
    
    public void GetReward()
    {
        Debug.Log("获取任务奖励");
        taskStatus =  DataStatus.Get;
    }
}
