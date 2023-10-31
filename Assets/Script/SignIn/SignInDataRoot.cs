using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignData
{
    public int signTimeMonth;  //本月累计签到次数
    public int signTimeYear;    //年度累计签到次数  
    public SignData()
    {
        signTimeMonth = 0;
        signTimeYear = 0;
    }
}
public class SignInDataRoot
{
    public SignInConfigRoot signInConfigRoot;
    public Dictionary<int, SignInData> signInDataDic = new Dictionary<int, SignInData>();

    public event Action<int> CheckSignStatus;

    public SignData m_signData;
    public SignInDataRoot()
    {
        signInConfigRoot = new SignInConfigRoot();
        TimeMgr._instance.onMonthRefersh += onMonthRefersh;
        TimeMgr._instance.onDayRefersh += onDayRefersh;
        Init();      
    }

    public void Init()
    {
        signInDataDic = signInConfigRoot.GetSignInDataDic();
        foreach (var signInData in signInDataDic)
        {
            CheckSignStatus += signInData.Value.CheckSignStatus;
            signInData.Value.OnSignStatusChange += OnSignStatusChange;

            UIMgr._instance.CreateSignInUI(signInData.Value);
        }

        List<SignData> signData = new List<SignData>();
        if(JsaonMgr.LoadFromPath<SignData>("/SignTimeData.txt",out signData))
        {
            m_signData = signData[0];
        }
        else
        {
            m_signData = new SignData();
        }

        //根据日期决定签到状态
        CheckSignStatus?.Invoke(TimeMgr._instance.returnLastLogTime().day);
    }

    public void OnSignStatusChange(SignInData signData)
    {
        if(signData.signStatus == SignDataStatus.CanResign|| signData.signStatus == SignDataStatus.CanSign)
        {
            CheckSignStatus -= signData.CheckSignStatus;
        }
        else if(signData.signStatus == SignDataStatus.Signed)
        {
            m_signData.signTimeMonth++;
            signData.OnSignStatusChange -= OnSignStatusChange;
        }
        SaveSignTimeData();
        //UI逻辑修改
        SaveSignInData();
    }
    private void onDayRefersh()
    {
        CheckSignStatus?.Invoke(TimeMgr._instance.returnLastLogTime().day);
        SaveSignTimeData();
    }
    private void onMonthRefersh()
    {
        //每月刷新
        foreach (var signInData in signInDataDic)
        {
            signInData.Value.Reset();

            //重新添加时，部分没有移除需要先移除
            CheckSignStatus -= signInData.Value.CheckSignStatus;
            signInData.Value.OnSignStatusChange -= OnSignStatusChange;

            CheckSignStatus += signInData.Value.CheckSignStatus;
            signInData.Value.OnSignStatusChange += OnSignStatusChange;
        }
        //每月刷新之后需要重新监听
        CheckSignStatus?.Invoke(TimeMgr._instance.returnLastLogTime().day);
        SaveSignInData();
        SaveSignTimeData();
    }

    public void SaveSignInData()
    {
        List<SignInData> signInDataList = new List<SignInData>();
        foreach (var item in signInDataDic)
        {
            signInDataList.Add(item.Value);
        }
        SaveMgr.SaveData<SignInData>("/SignInData.txt", signInDataList);
    }
    public void SaveSignTimeData()
    {
        SaveMgr.SaveData<SignData>("/SignTimeData.txt", new List<SignData>() { m_signData });
    }
    public void Save()
    {
        SaveSignInData();
        SaveSignTimeData();
    }
}
