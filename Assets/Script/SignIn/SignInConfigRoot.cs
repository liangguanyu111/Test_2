using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignInConfigRoot 
{
    public Dictionary<int, SignInConfig> signInConfigsDic = new Dictionary<int, SignInConfig>();


    public SignInConfigRoot()
    {
        LoadSignInCfg();
    }

    public Dictionary<int, SignInData> GetSignInDataDic()
    {
        Dictionary<int, SignInData> signDataDic = new Dictionary<int, SignInData>();
        List<SignInData> signDataList = new List<SignInData>();
        if (JsaonMgr.LoadFromPath<SignInData>("/SignInData.txt", out signDataList))
        {
            Debug.Log("读取SignInData数据");
            //有存储数据
            foreach (var signData in signDataList)
            {
                signDataDic.Add(signData.signInCfg.signDay, signData);
            }
        }
        else
        {
            //用配置初始化
            foreach (var signConfig in signInConfigsDic)
            {
                signDataDic.Add(signConfig.Value.signDay,new SignInData(signConfig.Value));
            }
        }
        return signDataDic;
    }



    public void LoadSignInCfg()
    {
        List<SignInConfig> SignInCfgs = new List<SignInConfig>();
        if (JsaonMgr.LoadFromPath<SignInConfig>("/SignIn_cfg.txt", out SignInCfgs))
        {
            foreach (var SignInCfg in SignInCfgs)
            {
                signInConfigsDic.Add(SignInCfg.signDay, SignInCfg);
            }
        }
    }
}
