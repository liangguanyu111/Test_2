using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TaskUI 
{
    public Text taskID;
    public Text taskDescription;
    public Text taskProcess;
    public Button taskClickButton;
    public Text buttonText;

    //添加进度
    public Button addProcessButton;

    public TaskData m_taskData;
    public GameObject uiObject;

    public TaskUI(TaskData taskData)
    {
        this.m_taskData = taskData;
        uiObject = GameObject.Instantiate(Resources.Load<GameObject>("Task/TaskObj"));
        taskID = uiObject.transform.GetChild(0).GetComponent<Text>();
        taskDescription = uiObject.transform.GetChild(1).GetComponent<Text>();
        taskProcess = uiObject.transform.GetChild(2).GetComponent<Text>();
        taskClickButton = uiObject.transform.GetChild(3).GetComponent<Button>();
        buttonText = uiObject.transform.GetChild(3).GetChild(0).GetComponent<Text>();

        addProcessButton = uiObject.transform.GetChild(4).GetComponent<Button>();
        addProcessButton.onClick.AddListener(() => { taskData.AddProcess(1, (int a) => { taskProcess.text = a.ToString() + "/" + taskData.TaskCfg.target_amount.ToString(); });}); 
        SetUI(m_taskData);

        taskData.OnTaskStatusChange += SetUI;
    }

    public void SetUI(TaskData taskData)
    {
        taskID.text = taskData.TaskCfg.task_id.ToString();
        taskDescription.text = taskData.TaskCfg.desc.ToString();
        taskProcess.text = taskData.task_process.ToString() + "/" + taskData.TaskCfg.target_amount.ToString();

        switch (taskData.taskStatus)
        {
            case DataStatus.NotFinish:
                buttonText.text = "进行中";
                taskClickButton.onClick.RemoveAllListeners();
                break;
            case DataStatus.Finished:
                buttonText.text = "领取奖励";
                taskClickButton.onClick.AddListener(()=> { taskData.GetReward(); });
                break;
            case DataStatus.Get:
                buttonText.text = "完成";
                taskClickButton.onClick.RemoveAllListeners();
                break;
        }
    }
}
