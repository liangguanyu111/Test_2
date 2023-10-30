using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//������������ˢ�£�����ÿ��ˢ���¼���ÿ��ˢ���¼����ۼƵ�¼�¼�
//ȷ�е�ʱ����Ҫ�ӷ��������������ñ�������ģ��
public class myDate
{
    public int year;
    public int month;
    public int day;
    public int min;
    public myDate() { year = 0; month = 0;day = 0;min = 0;   }
    public myDate(int year,int month, int day,int min)
    {
        this.year = year;
        this.month = month;
        this.day = day;
        this.min = min;
    }
}

public class TimeMgr : MonoBehaviour
{
    public static TimeMgr _instance;

    public event Action onDayRefersh;   //ÿ��ˢ���¼�
    public event Action onMonthRefersh;  //ÿ��ˢ���¼�
    public event Action onYearRefersh;   //ÿ��ˢ���¼�

    private myDate lastLogTime;
    private myDate nowTime;
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
        List<myDate> timeList = new List<myDate>();
        if(JsaonMgr.LoadFromPath<myDate>("/time.txt",out timeList))
        {
            lastLogTime = timeList[0];
        }
        else
        {
            lastLogTime = new myDate();
            //��һ�ε�¼
        }

        nowTime = new myDate(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,DateTime.Now.Minute);
        //����һ�ε�¼���Ƚ�
        CheckTime(lastLogTime.year,lastLogTime.month,lastLogTime.day,lastLogTime.min);
        StartCoroutine(Check());
    }


    public void CheckTime(int year,int month,int day,int min)
    {

        if (year != DateTime.Now.Year)
        {
            onYearRefersh?.Invoke();
            onMonthRefersh?.Invoke();
            onDayRefersh?.Invoke();
            
        }
        else if (month != DateTime.Now.Month)
        {
            onMonthRefersh?.Invoke();
            onDayRefersh?.Invoke();
        }
        else if (day != DateTime.Now.Day)
        {
            onDayRefersh?.Invoke();
            
        }
        else if(min!=DateTime.Now.Minute)
        {
            Debug.Log("������һ����");
        }
        nowTime.year = DateTime.Now.Year;
        nowTime.month = DateTime.Now.Month;
        nowTime.day = DateTime.Now.Day;
        nowTime.min = DateTime.Now.Minute;
    }

    private void Refersh()
    {
        CheckTime(nowTime.year, nowTime.month, nowTime.day,nowTime.min);
    }
    //�����˳���ʱ��
    public void Save()
    {
        StopCoroutine(Check());
        JsaonMgr.WriteToPath<myDate>("/time.txt",new List<myDate>(){ new myDate(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day,DateTime.Now.Minute) });
    }

    IEnumerator Check()
    {
        while(true)
        {
            yield return new WaitForSeconds(60);
            Refersh();
        }
    }
}
