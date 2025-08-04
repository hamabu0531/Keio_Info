using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;

public class TimeManager : MonoBehaviour
{
    DateTime currentTime;
    TrainSchedule h_weekday, h_weekend, s_weekday, s_weekend;
    public GameObject uIObject;
    UIController uIController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        uIController = uIObject.GetComponent<UIController>();

        TextAsset h_d = Resources.Load<TextAsset>("Jsons/h_times_weekday");
        TextAsset h_e = Resources.Load<TextAsset>("Jsons/h_times_weekend");
        TextAsset s_d = Resources.Load<TextAsset>("Jsons/s_times_weekday");
        TextAsset s_e = Resources.Load<TextAsset>("Jsons/s_times_weekend");
        h_weekday = JsonUtility.FromJson<TrainSchedule>(h_d.text);
        h_weekend = JsonUtility.FromJson<TrainSchedule>(h_e.text);
        s_weekday = JsonUtility.FromJson<TrainSchedule>(s_d.text);
        s_weekend = JsonUtility.FromJson<TrainSchedule>(s_e.text);

        UpdateTimeInfo();

        // 1�b���Ƃ� UpdateTimeInfo ���Ă�
        InvokeRepeating(nameof(UpdateTimeInfo), 1f, 1f);
    }

    void UpdateTimeInfo()
    {
        currentTime = DateTime.Now;
        // �f�o�b�O�p
        //currentTime = new DateTime(2025, 11, 24, 18, 6, 54);

        uIController.Update_DayInfo(currentTime.Year, currentTime.Month, currentTime.Day, currentTime.DayOfWeek.ToString());
        uIController.Update_ClockTime(currentTime.Hour, currentTime.Minute);
        uIController.Update_Weekday(isWeekday(currentTime));

        int firstIndex = -1, secondIndex = -1;

        TrainSchedule h, s;
        // �����X�P�W���[��
        if (isWeekday(currentTime))
        {
            h = h_weekday;
            s = s_weekday;
        }
        // �x���X�P�W���[��
        else
        {
            h = h_weekend;
            s = s_weekend;
        }

        // ���̎���������(�����q����)
        for (int i = 0; i < h.trainTimes.Length; i++)
        {
            // hour�͓����ŕ����߂��Ă�
            if (currentTime.Hour == h.trainTimes[i].hour && currentTime.Minute < h.trainTimes[i].minute)
            {
                firstIndex = i;
                secondIndex = firstIndex + 1;
                break;
            }

            // hour���߂��Ă�
            if (currentTime.Hour < h.trainTimes[i].hour)
            {
                firstIndex = i;
                secondIndex = firstIndex + 1;
                break;
            }
        }

        // �攭�A�����Ƃ��ɂ���
        if (firstIndex != -1)
        {
            int first_hour = h.trainTimes[firstIndex].hour;
            int first_minute = h.trainTimes[firstIndex].minute;
            int second_hour = h.trainTimes[secondIndex].hour;
            int second_minute = h.trainTimes[secondIndex].minute;
            uIController.Update_h_time(first_hour, first_minute, second_hour, second_minute);
        }
        // �����Ȃ�
        else if (secondIndex != -1)
        {
            int first_hour = h.trainTimes[firstIndex].hour;
            int first_minute = h.trainTimes[firstIndex].minute;
            uIController.Update_h_time(first_hour, first_minute, -1, -1);
        }
        // �攭�A�����Ƃ��ɂȂ�
        else
        {

        }

        firstIndex = -1;
        secondIndex = -1;

        // ���̎���������(�V�h����)
        for (int i = 0; i < s.trainTimes.Length; i++)
        {


            // hour�͓����ŕ����߂��Ă�
            if (currentTime.Hour == s.trainTimes[i].hour && currentTime.Minute < s.trainTimes[i].minute)
            {
                firstIndex = i;
                secondIndex = firstIndex + 1;
                break;
            }

            // hour���߂��Ă�
            if (currentTime.Hour < s.trainTimes[i].hour)
            {
                firstIndex = i;
                secondIndex = firstIndex + 1;
                break;
            }
        }

        // �攭�A�����Ƃ��ɂ���
        if (firstIndex != -1)
        {
            int first_hour = s.trainTimes[firstIndex].hour;
            int first_minute = s.trainTimes[firstIndex].minute;
            int second_hour = s.trainTimes[secondIndex].hour;
            int second_minute = s.trainTimes[secondIndex].minute;
            uIController.Update_s_time(first_hour, first_minute, second_hour, second_minute);
        }
        // �����Ȃ�
        else if (secondIndex != -1)
        {
            int first_hour = s.trainTimes[firstIndex].hour;
            int first_minute = s.trainTimes[firstIndex].minute;
            uIController.Update_s_time(first_hour, first_minute, -1, -1);
        }
        // �攭�A�����Ƃ��ɂȂ�
        else
        {

        }
    }
    
    bool isWeekday(DateTime dt)
    {
        // �y��
        if (dt.DayOfWeek == DayOfWeek.Sunday || dt.DayOfWeek == DayOfWeek.Saturday)
        {
            return false;
        }

        // �j�����ǂ���
        return !isHoliday(dt);
    }

    bool isHoliday(DateTime dt)
    {
        // �Œ���̏j�����ǂ���
        if (isFixedHoliday(dt))
        {
            return true;
        }

        // ��n���j���̏j�����ǂ���
        if ((dt.Month == 1 && isNthMonday(dt, 2)) ||
            (dt.Month == 7 && isNthMonday(dt, 3)) ||
            (dt.Month == 9 && isNthMonday(dt, 3)) ||
            (dt.Month == 10 && isNthMonday(dt, 2)))
        {
            return true;
        }

        // �t���̓����ǂ���
        if (isShunbunDay(dt))
        {
            return true;
        }

        // �H���̓�
        if (isShubunDay(dt))
        {
            return true;
        }

        // �U�֋x��
        DateTime yesterday = dt.AddDays(-1);
        if (yesterday.DayOfWeek == DayOfWeek.Sunday &&
            (isFixedHoliday(yesterday) || isShubunDay(yesterday) || isShunbunDay(yesterday)))
        {
            return true;
        }

        return false;

        //================================
        // ���[�J���֐�
        //================================

        // ��n���j���̏j��
        bool isNthMonday(DateTime dt_f, int n)
        {
            if (dt_f.DayOfWeek != DayOfWeek.Monday)
            {
                return false ;
            }
            int week = (dt_f.Day - 1) / 7 + 1;
            return week == n;
        }

        // �Œ���̋x��
        bool isFixedHoliday(DateTime dt_f)
        {
            int month = dt_f.Month;
            int day = dt_f.Day;
            List<(int, int)> fixedHolidays = new List<(int, int)>()
            {
                (1, 1),    // ����
                (2, 11),   // �����L�O�̓�
                (2, 23),   // �V�c�a����
                (4, 29),   // ���a�̓�
                (5, 3),    // ���@�L�O��
                (5, 4),    // �݂ǂ�̓�
                (5, 5),    // ���ǂ��̓�
                (8, 11),   // �R�̓�
                (11, 3),   // �����̓�
                (11, 23),  // �ΘJ���ӂ̓�
            };
            if (fixedHolidays.Any(d => d.Item1 == month && d.Item2 == day))
            {
                return true;
            }
            return false;
        }

        // �t���̓�
        bool isShunbunDay(DateTime dt_f)
        {
            int year = dt_f.Year;
            int month = dt_f.Month;
            int day = dt_f.Day;
            int shunbunDay = (int)(20.8431 + 0.242194 * (year - 1980) - (int)((year - 1980) / 4));
            if (month == 3 && day == shunbunDay)
            {
                return true;
            }
            return false;
        }

        // �H���̓�
        bool isShubunDay(DateTime dt_f)
        {
            int year = dt_f.Year;
            int month = dt_f.Month;
            int day = dt_f.Day;
            int shubunDay = (int)(23.2488 + 0.242194 * (year - 1980) - (int)((year - 1980) / 4));
            if (month == 9 && day == shubunDay)
            {
                return true;
            }
            return false;
        }
    }
}
