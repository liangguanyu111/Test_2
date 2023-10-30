using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMgr : MonoBehaviour
{
    public static UIMgr _instance;

    public Transform taskTransform;
    public Transform taskStageTransfrom;

    public List<TaskUI> taskUIList = new List<TaskUI>();
    public List<TaskStageUI> taskStageUIList = new List<TaskStageUI>();
    private void Awake()
    {
        if(_instance==null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void CreateTaskStageUI(TaskStageData taskStageData)
    {
        TaskStageUI newTaskStage = new(taskStageData);
        newTaskStage.uiObject.transform.SetParent(taskStageTransfrom);
        newTaskStage.uiObject.transform.localScale = new Vector3(1, 1, 1);
        taskStageUIList.Add(newTaskStage);
    }

    public void CreateTaskUI(TaskData taskData)
    {
        TaskUI newTask = new (taskData);
        newTask.uiObject.transform.SetParent(taskTransform);
        newTask.uiObject.transform.localScale = new Vector3(1, 1, 1);
        taskUIList.Add(newTask);
    }
}
