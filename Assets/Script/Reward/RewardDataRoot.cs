using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DrawData
{
    public int aviliableDrawTime;  //可抽取次数
    public int dailyDrawTime;   //每天可以看广告抽的次数小于等于十次
    public int drawTimeMonth;       //每个月已经抽取的次数
    public int joinTimeMonth;      //本月参与次数
    public DrawData()
    {
        aviliableDrawTime = 0;
        dailyDrawTime = 0;
        drawTimeMonth = 0;
        joinTimeMonth = 0;
    }
}
public class RewardDataRoot 
{
    public RewardConfigRoot rewardConfigRoot;
    public Dictionary<int, RewardData> rewardDataDic = new Dictionary<int, RewardData>();
    public Dictionary<int, RewardStageData> rewardStageDataDic = new Dictionary<int, RewardStageData>();

    DrawData m_DrawData;

    public event Action<int> onDraw;//发生抽取时检查是否满足阶段奖励
    public RewardDataRoot()
    {
        rewardConfigRoot = new RewardConfigRoot();
        TimeMgr._instance.onDayRefersh += RefershDrawDaliy;
        TimeMgr._instance.onMonthRefersh += RefershDrawMonthily;

        Init();
    }

    public void Init()
    {
        LoadDrawData();
        rewardDataDic = rewardConfigRoot.GetRewardDataDic();
        rewardStageDataDic = rewardConfigRoot.GetRewardStageDataDic();

        //初始化TaskStageData
        foreach (var rewardStageData in rewardStageDataDic)
        {
            //没有被领取的才会检测
            if (rewardStageData.Value.rewardStageStatus == 0)
            {
                onDraw += rewardStageData.Value.CheckStage;
                rewardStageData.Value.onStageFinish += OnRewardStageFinish; 
            }
        }
        onDraw?.Invoke(m_DrawData.drawTimeMonth);
    }
    
    //读取已经保存的抽取数据
    public void LoadDrawData()
    {
        List<DrawData> drawData = new List<DrawData>();
        if(JsaonMgr.LoadFromPath<DrawData>("/DrawData.txt",out drawData))
        {
            m_DrawData = drawData[0];
        }
        else
        {
            m_DrawData = new DrawData();
        }
    }

    //用来显示抽取进度  抽取次数/可抽取次数/状态
    public void Draw(Action<int,int,bool> CallBack)
    {
        RewardData randomGet;
        if (CheckDrawCondition(CallBack,out randomGet))
        {
            randomGet.Draw();
            onDraw?.Invoke(m_DrawData.drawTimeMonth);//检查阶段奖励


            SaveDrawData();
            SaveRewardData();
        }
    }
    public bool CheckDrawCondition(Action<int,int,bool> CallBack,out RewardData randomReward)
    {
        randomReward = null;
        //抽取次数到上限了，无法抽取
        if (m_DrawData.dailyDrawTime >= 10||m_DrawData.joinTimeMonth >= 10)
        {
            CallBack(m_DrawData.aviliableDrawTime, m_DrawData.dailyDrawTime,false);
            return false;
        }
        else if(m_DrawData.aviliableDrawTime <= 0 && m_DrawData.dailyDrawTime < 10)
        {
            //还可以看广告增加次数
            CallBack(m_DrawData.aviliableDrawTime, m_DrawData.dailyDrawTime, true);
            return false;
        }

        List<RewardData> rewardNotGetList = new List<RewardData>();
        foreach (var reward in rewardDataDic)
        {
            if (reward.Value.rewardStatus == DataStatus.NotFinish)
            {
                rewardNotGetList.Add(reward.Value);
            }
        }
        //全部抽取了也无法抽取了
        if(rewardNotGetList.Count<=0)
        {
            CallBack(m_DrawData.aviliableDrawTime, m_DrawData.dailyDrawTime, false);
            return false;
        }

        randomReward = GetRandomDraw(rewardNotGetList);
        return true;
    }

    public RewardData GetRandomDraw(List<RewardData> rewardNotGetList)
    {
        m_DrawData.dailyDrawTime++;
        m_DrawData.drawTimeMonth++;
        m_DrawData.aviliableDrawTime--;
        //在还没有被抽取的奖励中随机返回一个              
        int index = UnityEngine.Random.Range(0, rewardNotGetList.Count);
        return rewardNotGetList[index];  
    }

    public void OnRewardStageFinish(RewardStageData rewaredStage)
    {
        //领取奖励
        rewaredStage.onStageFinish -= OnRewardStageFinish;
        onDraw -= rewaredStage.CheckStage;
    }

    //广告增加抽奖次数
    public void AddDrawTime(int drawTimes)
    {
        m_DrawData.aviliableDrawTime += drawTimes;
    }
    //新的一天抽奖次数为1
    private void RefershDrawDaliy()
    {
        Debug.Log("重置每天抽奖次数和看广告次数");
        m_DrawData.aviliableDrawTime = 1;
        m_DrawData.dailyDrawTime = 0;
        m_DrawData.joinTimeMonth++;
        
        foreach (var rewardData in rewardDataDic)
        {
            rewardData.Value.Reset();
        }

        SaveDrawData();
        SaveRewardData();
    }

    private void RefershDrawMonthily()
    {
        m_DrawData.drawTimeMonth = 0;

        //刷新每月阶段奖励状态
        foreach (var rewardStageData in rewardStageDataDic)
        {
            rewardStageData.Value.Reset();

            rewardStageData.Value.onStageFinish -= OnRewardStageFinish;
            onDraw -= rewardStageData.Value.CheckStage;

            rewardStageData.Value.onStageFinish += OnRewardStageFinish;
            onDraw += rewardStageData.Value.CheckStage;
        }
    }

    public void SaveDrawData()
    {
        SaveMgr.SaveData<DrawData>("/DrawData.txt", new List<DrawData>() { m_DrawData });
    }

    public void SaveRewardData()
    {
        List<RewardData> rewardDataList = new List<RewardData>();
        foreach (var item in rewardDataDic)
        {
            rewardDataList.Add(item.Value);
        }
        SaveMgr.SaveData<RewardData>("/RewardData.txt", rewardDataList);
    }

    public void SaveRewardStageData()
    {
        List<RewardStageData> rewardDataStageList = new List<RewardStageData>();
        foreach (var item in rewardStageDataDic)
        {
            rewardDataStageList.Add(item.Value);
        }
        SaveMgr.SaveData<RewardStageData>("/RewardStageData.txt", rewardDataStageList);
    }
    public void Save()
    {
        SaveDrawData();
        SaveRewardData();
        SaveRewardStageData();
    }
}
