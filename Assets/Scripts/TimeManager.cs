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

        TrainSchedule h, s;
        // �����X�P�W���[��
        if (isWeekDay())
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
    
    bool isWeekDay()
    {
        if (currentTime.DayOfWeek == DayOfWeek.Sunday || currentTime.DayOfWeek == DayOfWeek.Saturday)
        {
            return false;
        }
        return true;
    }
}
