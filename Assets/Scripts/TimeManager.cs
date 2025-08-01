using System;
using System.IO;
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

        // 1秒ごとに UpdateTimeInfo を呼ぶ
        InvokeRepeating(nameof(UpdateTimeInfo), 1f, 1f);
    }

    void UpdateTimeInfo()
    {
        currentTime = DateTime.Now;
        uIController.Update_DayInfo(currentTime.Year, currentTime.Month, currentTime.Day, currentTime.DayOfWeek.ToString());
        uIController.Update_Weekday(isWeekDay());

        int firstIndex = -1, secondIndex = -1;

        // 次の時刻を検索(八王子方面)
        for (int i = 0; i < h_weekday.trainTimes.Length; i++)
        {
            TrainTime h;
            // 平日スケジュール
            if (isWeekDay())
            {
                h = h_weekday.trainTimes[i];
            }
            // 休日スケジュール
            else
            {
                h = h_weekend.trainTimes[i];
            }

            // hourは同じで分が過ぎてる
            if (currentTime.Hour == h.hour && currentTime.Minute < h.minute)
            {
                firstIndex = i;
                secondIndex = firstIndex + 1;
                break;
            }

            // hourが過ぎてる
            if (currentTime.Hour < h.hour)
            {
                firstIndex = i;
                secondIndex = firstIndex + 1;
                break;
            }
        }

        // 先発、次発ともにあり
        if (firstIndex != -1)
        {
            int first_hour = h_weekday.trainTimes[firstIndex].hour;
            int first_minute = h_weekday.trainTimes[firstIndex].minute;
            int second_hour = h_weekday.trainTimes[secondIndex].hour;
            int second_minute = h_weekday.trainTimes[secondIndex].minute;
            uIController.Update_h_time(first_hour, first_minute, second_hour, second_minute);
        }
        // 次発なし
        else if (secondIndex != -1)
        {
            int first_hour = h_weekday.trainTimes[firstIndex].hour;
            int first_minute = h_weekday.trainTimes[firstIndex].minute;
            uIController.Update_h_time(first_hour, first_minute, -1, -1);
        }
        // 先発、次発ともになし
        else
        {

        }

        firstIndex = -1;
        secondIndex = -1;

        // 次の時刻を検索(新宿方面)
        for (int i = 0; i < s_weekday.trainTimes.Length; i++)
        {
            TrainTime s;
            // 平日スケジュール
            if (isWeekDay())
            {
                s = s_weekday.trainTimes[i];
            }
            // 休日スケジュール
            else
            {
                s = s_weekend.trainTimes[i];
            }

            // hourは同じで分が過ぎてる
            if (currentTime.Hour == s.hour && currentTime.Minute < s.minute)
            {
                firstIndex = i;
                secondIndex = firstIndex + 1;
                break;
            }

            // hourが過ぎてる
            if (currentTime.Hour < s.hour)
            {
                firstIndex = i;
                secondIndex = firstIndex + 1;
                break;
            }
        }

        // 先発、次発ともにあり
        if (firstIndex != -1)
        {
            int first_hour = s_weekday.trainTimes[firstIndex].hour;
            int first_minute = s_weekday.trainTimes[firstIndex].minute;
            int second_hour = s_weekday.trainTimes[secondIndex].hour;
            int second_minute = s_weekday.trainTimes[secondIndex].minute;
            uIController.Update_s_time(first_hour, first_minute, second_hour, second_minute);
        }
        // 次発なし
        else if (secondIndex != -1)
        {
            int first_hour = s_weekday.trainTimes[firstIndex].hour;
            int first_minute = s_weekday.trainTimes[firstIndex].minute;
            uIController.Update_s_time(first_hour, first_minute, -1, -1);
        }
        // 先発、次発ともになし
        else
        {

        }
    }
    
    bool isWeekDay()
    {
        if (currentTime.DayOfWeek == DayOfWeek.Sunday || currentTime.DayOfWeek == DayOfWeek.Saturday)
        {
            return false;
        }
        return true;
    }
}
