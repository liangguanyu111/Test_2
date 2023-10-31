using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using LitJson;

public enum SignDataStatus
{
    NotSign = 0,
    CanSign = 1,
    CanResign =2,
    Signed =3,
}
public class SignInData
{
    public SignInConfig signInCfg;

    public SignDataStatus signStatus; // 0表示未签到，1表示可以补签，2表示已经签到

    public event Action<SignInData> OnSignStatusChange;


    public SignInData() { }
    public SignInData(SignInConfig signInCfg)
    {
        this.signInCfg = signInCfg;
        signStatus = SignDataStatus.NotSign;
    }

    public bool CanSign()
    {
        return signStatus == SignDataStatus.CanSign || signStatus == SignDataStatus.CanResign;
    }

    public void CheckSignStatus(int signTimeMonth)
    {
        //累计登录日期和签到日期相同表示可以签到，超过签到日期表示可以补签
        if(signStatus== SignDataStatus.NotSign)
        {
            if (signTimeMonth == signInCfg.signDay-1)
            {
                signStatus = SignDataStatus.CanSign;
            }
            else if(signTimeMonth > signInCfg.signDay-1)
            {
                signStatus = SignDataStatus.CanResign;
            }
            OnSignStatusChange.Invoke(this);
        }
    }

    public void Reset()
    {
        signStatus = SignDataStatus.NotSign;
        OnSignStatusChange.Invoke(this);
    }
    //签到

    public void Sign()
    {
        if(CanSign())
        {
            if (signStatus== SignDataStatus.CanSign)
            {
                Debug.Log("签到!");
            }
            else if(signStatus == SignDataStatus.CanResign)
            {
                Debug.Log("补签!");
            }
            signStatus = SignDataStatus.Signed;
            OnSignStatusChange.Invoke(this);
        }  
    }
}
