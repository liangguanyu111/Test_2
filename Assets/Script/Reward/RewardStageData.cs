using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;

public class RewardStageData
{
    public DataStatus rewardStageStatus; //0代表未完成,1代表已经完成未领取，2代表已经领取
    public RewardStageConfig rewardStageCfg;

    public event Action<RewardStageData> onStageStatusChange;
    public RewardStageData() { }
   public RewardStageData(RewardStageConfig rewardStageConfig)
   {
        this.rewardStageCfg = rewardStageConfig;
        rewardStageStatus = 0;
   }

    //检测阶段奖励是否满足条件
    public void CheckStage(int drawTimeMonth)
    {
        if(drawTimeMonth >= rewardStageCfg.reward_stageNum)
        {
            rewardStageStatus = DataStatus.Finished;
            onStageStatusChange.Invoke(this);
        }
    }
    public void Reset()
    {
        rewardStageStatus = DataStatus.NotFinish;
        onStageStatusChange.Invoke(this);
    }

    public void GetReward()
    {
        rewardStageStatus = DataStatus.Get;
        onStageStatusChange.Invoke(this);
    }
}
