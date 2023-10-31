using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Security.Cryptography;

//用来管理日期刷新，包括每日刷新事件，每月刷新事件，累计登录事件
//确切的时间需要从服务器请求，这里用本地日期模拟
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

    public event Action onDayRefersh;   //每日刷新事件
    public event Action onMonthRefersh;  //每月刷新事件
    public event Action onYearRefersh;   //每年刷新事件

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

        List<myDate> timeList = new List<myDate>();
        if (JsaonMgr.LoadFromPath<myDate>("/time.txt", out timeList))
        {
            lastLogTime = timeList[0];
        }
        else
        {
            lastLogTime = new myDate();
            //第一次登录
        }
        onDayRefersh += DayRefersh;
        onMonthRefersh += DayRefersh;
    }

    private void Start()
    {
        nowTime = new myDate(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,DateTime.Now.Minute);
        //和上一次登录作比较
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
            Debug.Log("又是下一分钟");
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

    //更新上次登录时间
    public void DayRefersh()
    {
        lastLogTime.year = DateTime.Now.Year;
        lastLogTime.month = DateTime.Now.Month;
        lastLogTime.day = DateTime.Now.Day;
        lastLogTime.min = DateTime.Now.Minute;
    }

    public void DayRefershButton()
    {
        onDayRefersh?.Invoke();
    }
    public void MonthRefershButton()
    {
        onDayRefersh?.Invoke(); 
        onMonthRefersh?.Invoke();
    }

    //保存退出的时间
    public void Save()
    {
        StopCoroutine(Check());
        JsaonMgr.WriteToPath<myDate>("/time.txt",new List<myDate>(){ new myDate(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day,DateTime.Now.Minute) });
    }


    public myDate returnLastLogTime()
    {
        return lastLogTime;
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
