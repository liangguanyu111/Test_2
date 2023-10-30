using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class RewardData
{
    private RewardConfig rewardCfg;
    public RewardConfig RewardCfg { get => rewardCfg; set => rewardCfg = value; }

    public DataStatus rewardStatus; //0表示还没有被抽取,1表示被抽取了
    public RewardData() { }
    public RewardData(RewardConfig rewardCfg)
    {
        this.rewardCfg = rewardCfg;
        rewardStatus = DataStatus.NotFinish;
    }

    public void Reset()
    {
        rewardStatus = DataStatus.NotFinish;
    }
    public void Draw()
    {
        //抽奖的奖励是立即发放的 不存在没有领取
        rewardStatus = DataStatus.Get;
    }

}
