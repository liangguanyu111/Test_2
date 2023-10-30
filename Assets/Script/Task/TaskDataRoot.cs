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
            taskData.Value.OnTaskFinish += TaskFinish;
            taskData.Value.CheckTaskStatus();
        }

        foreach (var taskStageData in taskStageDataDic)
        {
            OnfinishTask += taskStageData.Value.CheckStage;
            taskStageData.Value.OnTaskStageFinish += TaskStageFinish;
        }

    }

    public void AddTaskProcess(int task_id,int processTime, Action<int,DataStatus> callBack) //CallBack函数返回Task目前进度，以及是否完成控制领取奖励按钮
    {
        if(taskDataDic.ContainsKey(task_id)&& !taskDataDic[task_id].IsFinished())
        {
            taskDataDic[task_id].AddProcess(processTime, callBack);
        }
    }  
    //领取按钮获取奖励
    public void GetTaskReward(int task_id)
    {
        if(taskDataDic.ContainsKey(task_id))
        {
            taskDataDic[task_id].GetReward();
        }
    }
    public void GetTaskStageReward(int taskStage_id)
    {
        if (taskStageDataDic.ContainsKey(taskStage_id))
        {
            taskStageDataDic[taskStage_id].GetReward();
        }
    }
    //只有当完成任务时才能触发，避免重新上线后可以重复领取阶段性奖励
    private void TaskFinish(TaskData task)
    {
        finishTask++;
        OnfinishTask?.Invoke(finishTask);
        //移除已经完成的任务
        task.OnTaskFinish -= TaskFinish;
        //任务完成过后保存一下
        SaveTaskData();
    }

    private void TaskStageFinish(TaskStageData taskStageData)
    {
        OnfinishTask -= taskStageData.CheckStage;
        taskStageData.OnTaskStageFinish -= TaskStageFinish;
        SaveTaskStageData();
    }
    //状态重置
    public void RefershTask()
    {
        //刷新任务要把移除的监听全部添加回来
        foreach (var taskData in taskDataDic)
        {
            taskData.Value.ResetTaskData();
            taskData.Value.OnTaskFinish -= TaskFinish;
            taskData.Value.OnTaskFinish += TaskFinish;
        }
        foreach (var taskStageData in taskStageDataDic)
        {
            taskStageData.Value.ResetTaskData();

            taskStageData.Value.OnTaskStageFinish -= TaskStageFinish;
            taskStageData.Value.OnTaskStageFinish += TaskStageFinish;

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
