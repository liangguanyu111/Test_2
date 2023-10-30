using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;

public class TaskDataRoot 
{
    public TaskConfigRoot taskConfigRoot;
    public Dictionary<int, TaskData> taskDataDic = new Dictionary<int, TaskData>();
    public Dictionary<int, TaskStageData> taskStageDataDic = new Dictionary<int, TaskStageData>();

    public int finishTask = 0; //完成任务的数量
    public event Action<int> OnfinishTask;
    public TaskDataRoot()
    {
        taskConfigRoot = new TaskConfigRoot();

        //监听每日刷新事件
        TimeMgr._instance.onDayRefersh += RefershTask;
        Init();

    }
    public void Init()
    {
        taskDataDic = taskConfigRoot.GetTaskDataDic();
        taskStageDataDic = taskConfigRoot.GetTaskStageDataDic();

        foreach (var taskData in taskDataDic)
        {
            taskData.Value.OnTaskStatusChange += TaskStatusChange;
            taskData.Value.CheckTaskStatus(ref finishTask);

            UIMgr._instance.CreateTaskUI(taskData.Value);
        }

        foreach (var taskStageData in taskStageDataDic)
        {
            OnfinishTask += taskStageData.Value.CheckStage;
            taskStageData.Value.OnTaskStageChange += TaskStageChange;

            UIMgr._instance.CreateTaskStageUI(taskStageData.Value);
        }

        OnfinishTask.Invoke(finishTask);
    }

    public void AddTaskProcess(int task_id, int processTime, Action<int> callback) //CallBack函数返回Task目前进度，以及是否完成控制领取奖励按钮
    {
        if(taskDataDic.ContainsKey(task_id)&& !taskDataDic[task_id].IsFinished())
        {
            taskDataDic[task_id].AddProcess(processTime,callback);
        }
    }  

    public void GetTaskStageReward(int taskStage_id)
    {
        if (taskStageDataDic.ContainsKey(taskStage_id))
        {
            taskStageDataDic[taskStage_id].GetReward();
        }
    }

    public void TaskStatusChange(TaskData task)
    {
        if(task.taskStatus==DataStatus.Finished)
        {
            finishTask++;
            OnfinishTask?.Invoke(finishTask);
        }
        else if(task.taskStatus == DataStatus.Get)
        {
            task.OnTaskStatusChange -= TaskStatusChange;
        }
        SaveTaskData();
    }

    private void TaskStageChange(TaskStageData taskStageData)
    {
        if(taskStageData.taskStageStatus==DataStatus.Finished)
        {
            OnfinishTask -= taskStageData.CheckStage;
        }
        else if(taskStageData.taskStageStatus == DataStatus.Get)
        {
            taskStageData.OnTaskStageChange -= TaskStageChange;
        }
        SaveTaskStageData();
    }
    //状态重置
    public void RefershTask()
    {
        //刷新任务要把移除的监听全部添加回来
        foreach (var taskData in taskDataDic)
        {
            taskData.Value.ResetTaskData();
            taskData.Value.OnTaskStatusChange -= TaskStatusChange;
            taskData.Value.OnTaskStatusChange += TaskStatusChange;
        }
        foreach (var taskStageData in taskStageDataDic)
        {
            taskStageData.Value.ResetTaskData();

            taskStageData.Value.OnTaskStageChange -= TaskStageChange;
            taskStageData.Value.OnTaskStageChange += TaskStageChange;

            OnfinishTask -= taskStageData.Value.CheckStage;
            OnfinishTask += taskStageData.Value.CheckStage;
        }

        finishTask = 0;

        SaveTaskData();
        SaveTaskStageData();
    }

    public void SaveTaskData()
    {
        List<TaskData> taskDataList = new List<TaskData>();
        foreach (var item in taskDataDic)
        {
            taskDataList.Add(item.Value);
        }
        SaveMgr.SaveData<TaskData>("/TaskData.txt", taskDataList);
    }

    public void SaveTaskStageData()
    {
        List<TaskStageData> taskDataStageList = new List<TaskStageData>();
        foreach (var item in taskStageDataDic)
        {
            taskDataStageList.Add(item.Value);
        }
        SaveMgr.SaveData<TaskStageData>("/TaskStageData.txt", taskDataStageList);
    }
    public void Save()
    {
        SaveTaskData();
        SaveTaskStageData();
    }
}
