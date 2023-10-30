using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class RewardData
{
    private RewardConfig rewardCfg;
    public RewardConfig RewardCfg { get => rewardCfg; set => rewardCfg = value; }

    public DataStatus rewardStatus; //0��ʾ��û�б���ȡ,1��ʾ����ȡ��
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
        //�齱�Ľ������������ŵ� ������û����ȡ
        rewardStatus = DataStatus.Get;
    }

}
