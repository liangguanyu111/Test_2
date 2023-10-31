using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RewardStageUI 
{
    public Text rewardStageNum;
    public Text rewardDes;

    public Button clickBtn;

    public GameObject uiObject;
    RewardStageData m_rewardStageData;

    public RewardStageUI(RewardStageData data)
    {
        m_rewardStageData = data;

        uiObject = GameObject.Instantiate(Resources.Load<GameObject>("Reward/RewardStage"));
        rewardStageNum = uiObject.transform.GetChild(0).GetComponent<Text>();
        rewardDes = uiObject.transform.GetChild(1).GetComponent<Text>();
        clickBtn = uiObject.transform.GetComponent<Button>();

        data.onStageStatusChange += SetUI;
        SetUI(m_rewardStageData);
    }
    public void SetUI(RewardStageData rewardStageData)
    {
        rewardStageNum.text = rewardStageData.rewardStageCfg.reward_stageNum.ToString();

        switch(rewardStageData.rewardStageStatus)
        {
            case DataStatus.NotFinish:
                
                rewardDes.text = "未完成";
                clickBtn.onClick.RemoveAllListeners();
                break;
            case DataStatus.Finished:
                clickBtn.onClick.AddListener(rewardStageData.GetReward);
                rewardDes.text = "可领取";
                break;
            case DataStatus.Get:
                clickBtn.onClick.RemoveAllListeners();
                rewardDes.text = "已领取";
                break;
        }
    }
}
