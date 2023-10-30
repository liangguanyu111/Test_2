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

    public event Action<SignInData> OnSignIn;


    public SignInData() { }
    public SignInData(SignInConfig signInCfg)
    {
        this.signInCfg = signInCfg;
        signStatus = SignDataStatus.NotSign;
    }
    public void CheckSignStatus(int signTimeMonth)
    {
        //累计登录日期和签到日期相同表示可以签到，超过签到日期表示可以补签
        if(signStatus== SignDataStatus.NotSign)
        {
            if (signTimeMonth == signInCfg.signDay)
            {
                signStatus = SignDataStatus.CanSign;
            }
            else if(signTimeMonth > signInCfg.signDay)
            {
                signStatus = SignDataStatus.CanResign;
            }
        }
    }

    public void Reset()
    {
        signStatus = SignDataStatus.NotSign;
    }
    //签到

    public void Sign(Action<SignDataStatus> callback)
    {
        signStatus = SignDataStatus.Signed;
        OnSignIn.Invoke(this);
        callback(signStatus);     
    }
}
