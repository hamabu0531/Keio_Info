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

        // 1�b���Ƃ� UpdateTimeInfo ���Ă�
        InvokeRepeating(nameof(UpdateTimeInfo), 1f, 1f);
    }

    void UpdateTimeInfo()
    {
        currentTime = DateTime.Now;
        uIController.Update_DayInfo(currentTime.Year, currentTime.Month, currentTime.Day, currentTime.DayOfWeek.ToString());
        uIController.Update_Weekday(isWeekDay());

        int firstIndex = -1, secondIndex = -1;

        // ���̎���������(�����q����)
        for (int i = 0; i < h_weekday.trainTimes.Length; i++)
        {
            TrainTime h;
            // �����X�P�W���[��
            if (isWeekDay())
            {
                h = h_weekday.trainTimes[i];
            }
            // �x���X�P�W���[��
            else
            {
                h = h_weekend.trainTimes[i];
            }

            // hour�͓����ŕ����߂��Ă�
            if (currentTime.Hour == h.hour && currentTime.Minute < h.minute)
            {
                firstIndex = i;
                secondIndex = firstIndex + 1;
                break;
            }

            // hour���߂��Ă�
            if (currentTime.Hour < h.hour)
            {
                firstIndex = i;
                secondIndex = firstIndex + 1;
                break;
            }
        }

        // �攭�A�����Ƃ��ɂ���
        if (firstIndex != -1)
        {
            int first_hour = h_weekday.trainTimes[firstIndex].hour;
            int first_minute = h_weekday.trainTimes[firstIndex].minute;
            int second_hour = h_weekday.trainTimes[secondIndex].hour;
            int second_minute = h_weekday.trainTimes[secondIndex].minute;
            uIController.Update_h_time(first_hour, first_minute, second_hour, second_minute);
        }
        // �����Ȃ�
        else if (secondIndex != -1)
        {
            int first_hour = h_weekday.trainTimes[firstIndex].hour;
            int first_minute = h_weekday.trainTimes[firstIndex].minute;
            uIController.Update_h_time(first_hour, first_minute, -1, -1);
        }
        // �攭�A�����Ƃ��ɂȂ�
        else
        {

        }

        firstIndex = -1;
        secondIndex = -1;

        // ���̎���������(�V�h����)
        for (int i = 0; i < s_weekday.trainTimes.Length; i++)
        {
            TrainTime s;
            // �����X�P�W���[��
            if (isWeekDay())
            {
                s = s_weekday.trainTimes[i];
            }
            // �x���X�P�W���[��
            else
            {
                s = s_weekend.trainTimes[i];
            }

            // hour�͓����ŕ����߂��Ă�
            if (currentTime.Hour == s.hour && currentTime.Minute < s.minute)
            {
                firstIndex = i;
                secondIndex = firstIndex + 1;
                break;
            }

            // hour���߂��Ă�
            if (currentTime.Hour < s.hour)
            {
                firstIndex = i;
                secondIndex = firstIndex + 1;
                break;
            }
        }

        // �攭�A�����Ƃ��ɂ���
        if (firstIndex != -1)
        {
            int first_hour = s_weekday.trainTimes[firstIndex].hour;
            int first_minute = s_weekday.trainTimes[firstIndex].minute;
            int second_hour = s_weekday.trainTimes[secondIndex].hour;
            int second_minute = s_weekday.trainTimes[secondIndex].minute;
            uIController.Update_s_time(first_hour, first_minute, second_hour, second_minute);
        }
        // �����Ȃ�
        else if (secondIndex != -1)
        {
            int first_hour = s_weekday.trainTimes[firstIndex].hour;
            int first_minute = s_weekday.trainTimes[firstIndex].minute;
            uIController.Update_s_time(first_hour, first_minute, -1, -1);
        }
        // �攭�A�����Ƃ��ɂȂ�
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
