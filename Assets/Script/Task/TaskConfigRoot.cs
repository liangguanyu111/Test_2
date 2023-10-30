using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;
using System.Text;

//用来读取所有任务的配置
public class TaskConfigRoot
{
    public Dictionary<int, TaskConfig> taskConfigsDic = new Dictionary<int, TaskConfig>(); 
    public Dictionary<int, TaskStageConfig> taskStageConfigDic = new Dictionary<int, TaskStageConfig>();

    public TaskConfigRoot()
    {
        LoadTaskcfg();
        LoadTaskStagecfg();
    }
    public Dictionary<int,TaskData> GetTaskDataDic()
    {
        Dictionary<int, TaskData> taskDataDic = new Dictionary<int, TaskData>();
        List<TaskData> taskDataList = new List<TaskData>();
        if(JsaonMgr.LoadFromPath<TaskData>("/TaskData.txt",out taskDataList))
        {
            Debug.Log("读取TaskData数据");
            //有存储数据
            foreach (var taskData in taskDataList)
            {
                taskDataDic.Add(taskData.TaskCfg.task_id, taskData);
            }
        }
        else
        {
            //用配置初始化
            foreach (var taskConfig in taskConfigsDic)
            {
                taskDataDic.Add(taskConfig.Value.task_id,new TaskData(taskConfig.Value));
            }
        }
        return taskDataDic;
    }

    public Dictionary<int, TaskStageData> GetTaskStageDataDic()
    {
        Dictionary<int, TaskStageData> taskStageDataDic = new Dictionary<int, TaskStageData>();
        List<TaskStageData> taskStageDataList = new List<TaskStageData>();
        if (JsaonMgr.LoadFromPath<TaskStageData>("/TaskStageData.txt", out taskStageDataList))
        {
            Debug.Log("读取TaskStageData数据");
            //有存储数据
            foreach (var taskStageData in taskStageDataList)
            {
                taskStageDataDic.Add(taskStageData.TaskStageCfg.task_stageID, taskStageData);
            }
        }
        else
        {
            //用配置初始化
            foreach (var taskStageConfig in taskStageConfigDic)
            {
                taskStageDataDic.Add(taskStageConfig.Value.task_stageID,new TaskStageData(taskStageConfig.Value));
            }
        }
        return taskStageDataDic;
    }

    //读取任务阶段配置
    public void LoadTaskStagecfg()
    {
        List<TaskStageConfig> taskStageCfgs = new List<TaskStageConfig>();
        if(JsaonMgr.LoadFromPath<TaskStageConfig>("/taskStage_cfg.txt",out taskStageCfgs))
        {
            foreach (var taskStageCfg in taskStageCfgs)
            {
         
             taskStageConfigDic.Add(taskStageCfg.task_stageID, taskStageCfg);
            }
        }    
    }
  
    //读取任务配置
    public void LoadTaskcfg()
    {
        List<TaskConfig> taskCfgs = new List<TaskConfig>();
        if (JsaonMgr.LoadFromPath<TaskConfig>("/task_cfg.txt", out taskCfgs))
        {
            foreach (var taskCfg in taskCfgs)
            {
                 taskConfigsDic.Add(taskCfg.task_id, taskCfg);
            }
        }
    }
}
