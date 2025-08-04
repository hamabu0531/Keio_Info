using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] Text first_h_time, second_h_time, first_s_time, second_s_time, dayInfo, timeInfo;
    [SerializeField] GameObject weekend_Weekday;
    [SerializeField] Sprite[] numSprites;
    [SerializeField] Image[] clockImages; // ���v��4���ɑΉ�

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Update_h_time(int first_hour, int first_minute, int second_hour, int second_minute)
    {
        first_h_time.text = first_hour + ":";
        if (first_minute < 10)
        {
            first_h_time.text += "0";
        }
        first_h_time.text += first_minute + " ��";
        second_h_time.text = second_hour + ":";
        if (second_minute < 10)
        {
            second_h_time.text += "0";
        }
        second_h_time.text += second_minute + " ��";
        if (first_hour == -1)
        {
            first_h_time.text = "";
        }
        if (second_hour == -1)
        {
            second_h_time.text = "";
        }
    }

    public void Update_s_time(int first_hour, int first_minute, int second_hour, int second_minute)
    {
        first_s_time.text = first_hour + ":";
        if (first_minute < 10)
        {
            first_s_time.text += "0";
        }
        first_s_time.text += first_minute + " ��";

        second_s_time.text = second_hour + ":";
        if (second_minute < 10)
        {
            second_s_time.text += "0";
        }
        second_s_time.text += second_minute + " ��";
        if (first_hour == -1)
        {
            first_s_time.text = "";
        }
        if (second_hour == -1)
        {
            second_s_time.text = "";
        }
    }

    public void Update_DayInfo(int year, int month, int day, string dayName)
    {
        string dayNameJp = Convert_Day_To_Jp(dayName);
        dayInfo.text = year + "�N" + month + "��" + day + "��(" + dayNameJp + ")";
    }

    public void Update_ClockTime(int hour, int minute)
    {
        int hour_first = hour / 10;
        int hour_second = hour % 10;
        int minute_first = minute / 10;
        int minute_second = minute % 10;

        clockImages[0].sprite = numSprites[hour_first];
        clockImages[1].sprite = numSprites[hour_second];
        clockImages[2].sprite = numSprites[minute_first];
        clockImages[3].sprite = numSprites[minute_second];
    }

    public void Update_Weekday(bool isWeekDay)
    {
        if (isWeekDay)
        {
            weekend_Weekday.transform.GetChild(0).gameObject.SetActive(true);
            weekend_Weekday.transform.GetChild(1).gameObject.SetActive(false);
            weekend_Weekday.GetComponent<Image>().color = Color.cyan;
        }
        else
        {
            weekend_Weekday.transform.GetChild(0).gameObject.SetActive(false);
            weekend_Weekday.transform.GetChild(1).gameObject.SetActive(true);
            weekend_Weekday.GetComponent<Image>().color = Color.magenta;
        }
    }

    string Convert_Day_To_Jp(string dayName)
    {
        switch (dayName)
        {
            case "Monday":
                return "��";
            case "Tuesday":
                return "��";
            case "Wednesday":
                return "��";
            case "Thursday":
                return "��";
            case "Friday":
                return "��";
            case "Saturday":
                return "�y";
            case "Sunday":
                return "��";
            default:
                return dayName;
        }
    }
}
