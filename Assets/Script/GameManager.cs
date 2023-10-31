using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;

public class GameManager : MonoBehaviour
{
    public static GameManager _instance;

    public TaskDataRoot taskDataRoot;
    public RewardDataRoot rewardDataRoot;
    public SignInDataRoot signInDataRoot;
    public SaveMgr saveMgr;
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

    private void Start()
    {
        taskDataRoot = new TaskDataRoot();
        rewardDataRoot = new RewardDataRoot();
        signInDataRoot = new SignInDataRoot();
        saveMgr = new SaveMgr(this);

        saveMgr.OnSave += taskDataRoot.Save;
        saveMgr.OnSave += rewardDataRoot.Save;
        saveMgr.OnSave += signInDataRoot.Save;
        saveMgr.OnSave += TimeMgr._instance.Save;

        //向ID为2的任务添加1进度
        //随机抽一个奖励
        //rewardDataRoot.Draw((int availabledrawTime,int drawTime,bool buttonStatus) => { });
    }
    //退出时也保存一下
    private void OnApplicationQuit()
    {
        saveMgr.Save();
    }


}
