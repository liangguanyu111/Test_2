using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json;

//当设计状态变化的数据发生变化的时候主动存一下,例如领取了奖励，平常自动保存。
public class SaveMgr
{
    public event Action OnSave;

    public SaveMgr(MonoBehaviour mono)
    {
        //mono.StartCoroutine(SaveOnTime());
    }

    //固定一分钟保存一次数据
    IEnumerator SaveOnTime()
    {
        yield return new WaitForSecondsRealtime(300);
        Debug.Log("自动保存");
        OnSave?.Invoke();
    }

    //主动Call
    public void Save()
    {
        OnSave?.Invoke();
    }

    public static void SaveData<T>(string path,List<T> data)
    {
        string dataPath = Application.streamingAssetsPath + path;
        JsaonMgr.WriteToPath<T>(dataPath, data);
    }
}
