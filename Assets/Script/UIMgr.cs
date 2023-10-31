using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.AI;
using UnityEngine.UI;

public class UIMgr : MonoBehaviour
{
    public static UIMgr _instance;

    [Header("Panel Transfrom")]
    public Transform taskTransform;
    public Transform taskStageTransfrom;
    public Transform rewardListTransform;
    public Transform rewardStateListTransform;
    public Transform signInDataListTrainsform;

    public Button drawClickBtn;
    public Button adClickBtn;

    [HideInInspector]
    public List<TaskUI> taskUIList = new List<TaskUI>();
    public List<TaskStageUI> taskStageUIList = new List<TaskStageUI>();
    public List<RewardUI> rewardUI = new List<RewardUI>();
    public List<RewardStageUI> rewardStateUIList = new List<RewardStageUI>();
    public List<SignInUI> signInDataUIList = new List<SignInUI>();

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
        
    }

    public void CreateSignInUI(SignInData signInData)
    {
        SignInUI newSignInUI = new(signInData);
        newSignInUI.uiObject.transform.SetParent(signInDataListTrainsform);
        newSignInUI.uiObject.transform.localScale = new Vector3(1, 1, 1);
        signInDataUIList.Add(newSignInUI);
    }
    public void CreateRewardStateUI(RewardStageData rewardStageData)
    {
        RewardStageUI newRewardStage = new(rewardStageData);
        newRewardStage.uiObject.transform.SetParent(rewardStateListTransform);
        newRewardStage.uiObject.transform.localScale = new Vector3(1, 1, 1);
        rewardStateUIList.Add(newRewardStage);
    }

    public void InitRewardList(Dictionary<int,RewardData> rewardDataDic)
    {
        drawClickBtn.onClick.AddListener(() => { GameManager._instance.rewardDataRoot.Draw(); });
        adClickBtn.onClick.AddListener(() => { GameManager._instance.rewardDataRoot.AddDrawTime(1); });
        for (int i = 0; i < rewardListTransform.childCount; i++)
        {
            RewardUI newRewardUI = new RewardUI(rewardListTransform.GetChild(i).gameObject, rewardDataDic[i+1]);
            newRewardUI.uiObj.transform.localScale = new Vector3(1, 1, 1);

            rewardUI.Add(newRewardUI);
        }

    }

    public void CreateTaskStageUI(TaskStageData taskStageData)
    {
        TaskStageUI newTaskStage = new(taskStageData);
        newTaskStage.uiObject.transform.SetParent(taskStageTransfrom);
        newTaskStage.uiObject.transform.localScale = new Vector3(1, 1, 1);
        taskStageUIList.Add(newTaskStage);
    }

    public void CreateTaskUI(TaskData taskData)
    {
        TaskUI newTask = new (taskData);
        newTask.uiObject.transform.SetParent(taskTransform);
        newTask.uiObject.transform.localScale = new Vector3(1, 1, 1);
        taskUIList.Add(newTask);
    }
}
