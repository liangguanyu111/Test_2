using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;

//用来管理Jason的读写
public class JsaonMgr 
{
    public static bool LoadFromPath<T>(string path,out List<T> cfgItemList)
    {
        string filePath = Application.streamingAssetsPath + path;
        List<T> cfgList = new List<T>();
        if (System.IO.File.Exists(filePath))
        {
            StreamReader streamReader = new StreamReader(filePath);
            JsonReader js = new JsonReader(streamReader);
            var jd = JsonMapper.ToObject<JsonData>(js);
            if(jd!=null)
            {
                for (int i = 0; i < jd.Count; i++)
                {
                    var itemJD = jd[i] as JsonData;

                    T cfgItem = JsonMapper.ToObject<T>(itemJD.ToJson());
                    cfgList.Add(cfgItem);
                }
            }
            streamReader.Close();
            js.Close();
            cfgItemList = cfgList;
            return true;
        }
        cfgItemList = cfgList;
        return false;
    }

    //清空后重新写入
    public static void WriteToPath<T>(string filePath,List<T> configList)
    {
        if (!System.IO.File.Exists(filePath))
        {
            FileInfo file = new FileInfo(filePath);
            file.CreateText().Dispose();
        }
        StreamWriter sw = new StreamWriter(filePath);

        string json = JsonMapper.ToJson(configList);
        sw.WriteLine(json);
        sw.Close();
        sw.Dispose();
    }
}
