using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SignInUI 
{
    public Text signDay;
    public Text signDes;

    public Button signButton;

    public SignInData m_signData;
    public GameObject uiObject;

    public  SignInUI(SignInData signInData)
    {
        m_signData = signInData;
        uiObject = GameObject.Instantiate(Resources.Load<GameObject>("SignIn/SignInSlot"));
        signButton = uiObject.GetComponent<Button>();
        signDay = uiObject.transform.GetChild(0).GetComponent<Text>();
        signDes = uiObject.transform.GetChild(1).GetComponent<Text>();

        signInData.OnSignStatusChange += SetUI;
        SetUI(signInData);
    }

    public void SetUI(SignInData signInData)
    {
        signDay.text = signInData.signInCfg.signDay.ToString();
        
        switch(signInData.signStatus)
        {
            case SignDataStatus.NotSign:
                signButton.onClick.RemoveAllListeners();
                signDes.text = "未签到";
                break;
            case SignDataStatus.CanSign:
                signButton.onClick.AddListener(signInData.Sign);
                signDes.text = "可签到";
                break;
            case SignDataStatus.CanResign:
                signButton.onClick.AddListener(signInData.Sign);
                signDes.text = "可补签";
                break;
            case SignDataStatus.Signed:
                signButton.onClick.RemoveAllListeners();
                signDes.text = "已签到";
                break;
        }
    }
}
