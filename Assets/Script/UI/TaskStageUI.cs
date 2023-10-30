using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TaskStageUI
{
    public Text stageAmountText;
    public Text stageText;

    public Button clickButton;

    public TaskStageData m_taskStageData;
    public GameObject uiObject;

    public TaskStageUI(TaskStageData taskStageData)
    {
        this.m_taskStageData = taskStageData;
        uiObject = GameObject.Instantiate(Resources.Load<GameObject>("Task/TaskStageObj"));

        clickButton = uiObject.GetComponent<Button>();
        stageAmountText = uiObject.transform.GetChild(0).GetComponent<Text>();
        stageText = uiObject.transform.GetChild(1).GetComponent<Text>();
        SetUI(m_taskStageData);

        //可以考虑都用事件不用回调比较统一
        taskStageData.OnTaskStageChange += SetUI;
    }

    public void SetUI(TaskStageData taskStageData)
    {
        stageAmountText.text = taskStageData.TaskStageCfg.task_stageNum.ToString();

        switch(taskStageData.taskStageStatus)
        {
            case DataStatus.NotFinish:
                stageText.text = "未完成";
                clickButton.onClick.RemoveAllListeners();
                break;
            case DataStatus.Finished:
                stageText.text = "可领取";
                clickButton.onClick.AddListener(()=> { taskStageData.GetReward(); });
                break;
            case DataStatus.Get:
                stageText.text = "已领取";
                clickButton.onClick.RemoveAllListeners();
                break;

        }
    }
}
