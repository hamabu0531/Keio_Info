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

        // 1•b‚²‚Æ‚É UpdateTimeInfo ‚ğŒÄ‚Ô
        InvokeRepeating(nameof(UpdateTimeInfo), 1f, 1f);
    }

    void UpdateTimeInfo()
    {
         currentTime = DateTime.Now;
        // ƒfƒoƒbƒO—p
        //currentTime = new DateTime(2025, 8, 11, 18, 6, 54);

        uIController.Update_DayInfo(currentTime.Year, currentTime.Month, currentTime.Day, currentTime.DayOfWeek.ToString());
        uIController.Update_ClockTime(currentTime.Hour, currentTime.Minute);
        uIController.Update_Weekday(isWeekday());

        int firstIndex = -1, secondIndex = -1;

        TrainSchedule h, s;
        // •½“úƒXƒPƒWƒ…[ƒ‹
        if (isWeekday())
        {
            h = h_weekday;
            s = s_weekday;
        }
        // ‹x“úƒXƒPƒWƒ…[ƒ‹
        else
        {
            h = h_weekend;
            s = s_weekend;
        }

        // Ÿ‚Ì‚ğŒŸõ(”ª‰¤q•û–Ê)
        for (int i = 0; i < h.trainTimes.Length; i++)
        {
            // hour‚Í“¯‚¶‚Å•ª‚ª‰ß‚¬‚Ä‚é
            if (currentTime.Hour == h.trainTimes[i].hour && currentTime.Minute < h.trainTimes[i].minute)
            {
                firstIndex = i;
                secondIndex = firstIndex + 1;
                break;
            }

            // hour‚ª‰ß‚¬‚Ä‚é
            if (currentTime.Hour < h.trainTimes[i].hour)
            {
                firstIndex = i;
                secondIndex = firstIndex + 1;
                break;
            }
        }

        // æ”­AŸ”­‚Æ‚à‚É‚ ‚è
        if (firstIndex != -1)
        {
            int first_hour = h.trainTimes[firstIndex].hour;
            int first_minute = h.trainTimes[firstIndex].minute;
            int second_hour = h.trainTimes[secondIndex].hour;
            int second_minute = h.trainTimes[secondIndex].minute;
            uIController.Update_h_time(first_hour, first_minute, second_hour, second_minute);
        }
        // Ÿ”­‚È‚µ
        else if (secondIndex != -1)
        {
            int first_hour = h.trainTimes[firstIndex].hour;
            int first_minute = h.trainTimes[firstIndex].minute;
            uIController.Update_h_time(first_hour, first_minute, -1, -1);
        }
        // æ”­AŸ”­‚Æ‚à‚É‚È‚µ
        else
        {

        }

        firstIndex = -1;
        secondIndex = -1;

        // Ÿ‚Ì‚ğŒŸõ(Vh•û–Ê)
        for (int i = 0; i < s.trainTimes.Length; i++)
        {


            // hour‚Í“¯‚¶‚Å•ª‚ª‰ß‚¬‚Ä‚é
            if (currentTime.Hour == s.trainTimes[i].hour && currentTime.Minute < s.trainTimes[i].minute)
            {
                firstIndex = i;
                secondIndex = firstIndex + 1;
                break;
            }

            // hour‚ª‰ß‚¬‚Ä‚é
            if (currentTime.Hour < s.trainTimes[i].hour)
            {
                firstIndex = i;
                secondIndex = firstIndex + 1;
                break;
            }
        }

        // æ”­AŸ”­‚Æ‚à‚É‚ ‚è
        if (firstIndex != -1)
        {
            int first_hour = s.trainTimes[firstIndex].hour;
            int first_minute = s.trainTimes[firstIndex].minute;
            int second_hour = s.trainTimes[secondIndex].hour;
            int second_minute = s.trainTimes[secondIndex].minute;
            uIController.Update_s_time(first_hour, first_minute, second_hour, second_minute);
        }
        // Ÿ”­‚È‚µ
        else if (secondIndex != -1)
        {
            int first_hour = s.trainTimes[firstIndex].hour;
            int first_minute = s.trainTimes[firstIndex].minute;
            uIController.Update_s_time(first_hour, first_minute, -1, -1);
        }
        // æ”­AŸ”­‚Æ‚à‚É‚È‚µ
        else
        {

        }
    }
    
    bool isWeekday()
    {
        // “y“ú
        if (currentTime.DayOfWeek == DayOfWeek.Sunday || currentTime.DayOfWeek == DayOfWeek.Saturday)
        {
            return false;
        }

        // j“ú‚©‚Ç‚¤‚©
        return !isHoliday();
    }

    bool isHoliday()
    {
        // ŒÅ’è“ú‚Ìj“ú‚©‚Ç‚¤‚©
        if (isFixedHoliday(currentTime))
        {
            return true;
        }

        // ‘ænŒ—j“ú‚Ìj“ú‚©‚Ç‚¤‚©
        if ((currentTime.Month == 1 && isNthMonday(2)) ||
            (currentTime.Month == 7 && isNthMonday(3)) ||
            (currentTime.Month == 9 && isNthMonday(3)) ||
            (currentTime.Month == 10 && isNthMonday(2)))
        {
            return true;
        }

        // t•ª‚Ì“ú‚©‚Ç‚¤‚©
        if (isShunbunDay(currentTime))
        {
            return true;
        }

        // H•ª‚Ì“ú
        if (isShubunDay(currentTime))
        {
            return true;
        }

        // U‘Ö‹x“ú
        DateTime yesterday = currentTime.AddDays(-1);
        if (yesterday.DayOfWeek == DayOfWeek.Sunday &&
            (isFixedHoliday(yesterday) || isShubunDay(yesterday) || isShunbunDay(yesterday)))
        {
            return true;
        }

        return false;

        //================================
        // ƒ[ƒJƒ‹ŠÖ”
        //================================

        // ‘ænŒ—j“ú‚Ìj“ú
        bool isNthMonday(int n)
        {
            if (currentTime.DayOfWeek != DayOfWeek.Monday)
            {
                return false ;
            }
            int week = (currentTime.Day - 1) / 7 + 1;
            return week == n;
        }

        // ŒÅ’è“ú‚Ì‹x“ú
        bool isFixedHoliday(DateTime dt)
        {
            int month = dt.Month;
            int day = dt.Day;
            List<(int, int)> fixedHolidays = new List<(int, int)>()
            {
                (1, 1),    // Œ³“ú
                (2, 11),   // Œš‘‹L”O‚Ì“ú
                (2, 23),   // “Vc’a¶“ú
                (4, 29),   // º˜a‚Ì“ú
                (5, 3),    // Œ›–@‹L”O“ú
                (5, 4),    // ‚İ‚Ç‚è‚Ì“ú
                (5, 5),    // ‚±‚Ç‚à‚Ì“ú
                (8, 11),   // R‚Ì“ú
                (11, 3),   // •¶‰»‚Ì“ú
                (11, 23),  // ‹Î˜JŠ´Ó‚Ì“ú
            };
            if (fixedHolidays.Any(d => d.Item1 == month && d.Item2 == day))
            {
                return true;
            }
            return false;
        }

        // t•ª‚Ì“ú
        bool isShunbunDay(DateTime dt)
        {
            int year = dt.Year;
            int month = dt.Month;
            int day = dt.Day;
            int shunbunDay = (int)(20.8431 + 0.242194 * (year - 1980) - (int)((year - 1980) / 4));
            if (month == 3 && day == shunbunDay)
            {
                return true;
            }
            return false;
        }

        // H•ª‚Ì“ú
        bool isShubunDay(DateTime dt)
        {
            int year = dt.Year;
            int month = dt.Month;
            int day = dt.Day;
            int shubunDay = (int)(23.2488 + 0.242194 * (year - 1980) - (int)((year - 1980) / 4));
            if (month == 9 && day == shubunDay)
            {
                return true;
            }
            return false;
        }
    }
}
