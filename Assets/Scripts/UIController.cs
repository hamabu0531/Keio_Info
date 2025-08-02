using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] Text first_h_time, second_h_time, first_s_time, second_s_time, dayInfo;
    [SerializeField] GameObject weekend_Weekday;
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
        first_h_time.text += first_minute + " î≠";
        second_h_time.text = second_hour + ":";
        if (second_minute < 10)
        {
            second_h_time.text += "0";
        }
        second_h_time.text += second_minute + " î≠";
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
        first_s_time.text += first_minute + " î≠";

        second_s_time.text = second_hour + ":";
        if (second_minute < 10)
        {
            second_s_time.text += "0";
        }
        second_s_time.text += second_minute + " î≠";
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
        dayInfo.text = year + "îN" + month + "åé" + day + "ì˙(" + dayNameJp + ")";
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
                return "åé";
            case "Tuesday":
                return "âŒ";
            case "Wednesday":
                return "êÖ";
            case "Thursday":
                return "ñÿ";
            case "Friday":
                return "ã‡";
            case "Saturday":
                return "ìy";
            case "Sunday":
                return "ì˙";
            default:
                return dayName;
        }
    }
}
