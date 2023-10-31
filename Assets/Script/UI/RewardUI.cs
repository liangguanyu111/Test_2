using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardUI
{
    public Text rewardText;

    public GameObject uiObj;
    public RewardData m_rewardData;
    public RewardUI(GameObject rewardObj,RewardData rewardData)
    {
        m_rewardData = rewardData;
        uiObj = rewardObj;
        rewardText = rewardObj.transform.GetChild(0).GetComponent<Text>();

        rewardData.onStatusChange += SetUI;
        SetUI(m_rewardData);
    }

    public void SetUI(RewardData rewardData)
    {
        switch(rewardData.rewardStatus)
        {
            case DataStatus.NotFinish:
                rewardText.text = "未抽取";
                break;
            case DataStatus.Get:
                rewardText.text = "已抽取";
                break;
        }
    }
}
