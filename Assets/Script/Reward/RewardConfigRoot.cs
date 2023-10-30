using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardConfigRoot
{
    public Dictionary<int, RewardConfig> rewardConfigsDic = new Dictionary<int, RewardConfig>();
    public Dictionary<int, RewardStageConfig> rewardStageConfigDic = new Dictionary<int, RewardStageConfig>();

    public RewardConfigRoot()
    {
        LoadRewardCfg();
        LoadRewardStagecfg();
    }


    public Dictionary<int, RewardData> GetRewardDataDic()
    {
        Dictionary<int, RewardData> rewardDataDic = new Dictionary<int, RewardData>();
        List<RewardData> rewardDataList = new List<RewardData>();
        if (JsaonMgr.LoadFromPath<RewardData>("/RewardData.txt", out rewardDataList))
        {
            Debug.Log("读取RewardData数据");
            //有存储数据
            foreach (var rewardData in rewardDataList)
            {
                rewardDataDic.Add(rewardData.RewardCfg.reward_id, rewardData);
            }
        }
        else
        {
            //用配置初始化
            foreach (var rewardConfig in rewardConfigsDic)
            {
                rewardDataDic.Add(rewardConfig.Value.reward_id, new RewardData(rewardConfig.Value));
            }
        }
        return rewardDataDic;
    }

    public Dictionary<int, RewardStageData> GetRewardStageDataDic()
    {
        Dictionary<int, RewardStageData> rewardStageDataDic = new Dictionary<int, RewardStageData>();
        List<RewardStageData> rewardStageDataList = new List<RewardStageData>();
        if (JsaonMgr.LoadFromPath<RewardStageData>("/RewardStageData.txt", out rewardStageDataList))
        {
            Debug.Log("读取RewardStageData数据");
            //有存储数据
            foreach (var rewardStageData in rewardStageDataList)
            {
                rewardStageDataDic.Add(rewardStageData.rewardStageCfg.reward_stageID, rewardStageData);
            }
        }
        else
        {
            //用配置初始化
            foreach (var rewardStageConfig in rewardStageConfigDic)
            {
                rewardStageDataDic.Add(rewardStageConfig.Value.reward_stageID, new RewardStageData(rewardStageConfig.Value));
            }
        }
        return rewardStageDataDic;
    }
    public void LoadRewardStagecfg()
    {
        List<RewardStageConfig> rewardStageCfgs = new List<RewardStageConfig>();
        if (JsaonMgr.LoadFromPath<RewardStageConfig>("/rewardStage_cfg.txt", out rewardStageCfgs))
        {
            foreach (var rewardStageCfg in rewardStageCfgs)
            {
              rewardStageConfigDic.Add(rewardStageCfg.reward_stageID, rewardStageCfg);
            }
        }
    }

    //读取奖励配置
    public void LoadRewardCfg()
    {
        List<RewardConfig> rewardCfgs = new List<RewardConfig>();
        if (JsaonMgr.LoadFromPath<RewardConfig>("/reward_cfg.txt", out rewardCfgs))
        {
            foreach (var rewardCfg in rewardCfgs)
            {
              rewardConfigsDic.Add(rewardCfg.reward_id, rewardCfg);
            }
        }
    }

}
