using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockLockButtonController : MonoBehaviour
{
    public TMPro.TextMeshProUGUI button_text;
    public int button_hour = 12;
    public int button_min = 0;
    public bool isRoman = false;

    // Start is called before the first frame update
    void Start()
    {
        button_text = GetComponent<TMPro.TextMeshProUGUI>();
    }

    public void Increment_Button()
    {
        if (button_text != null)
        {
            button_min += 15;
            if (button_min >= 60)
            {
                button_hour++;
                button_min = 0;
            }
            if(button_hour >= 13)
            {
                button_hour = 1;
            }

            if (!isRoman)
            { button_text.text = button_hour.ToString() + ":" + button_min.ToString("D2"); }
            else
            {
                button_text.text = numToRoman(button_hour) + ":" + numToRoman(button_min);
            }
        }
    }

    private string numToRoman(int i)
    {
        switch(i)
        {
            case 1:
                return "I";
            case 2:
                return "II";
            case 3:
                return "III";
            case 4:
                return "IV";
            case 5:
                return "V";
            case 6:
                return "VI";
            case 7:
                return "VII";
            case 8:
                return "VIII";
            case 9:
                return "IX";
            case 10:
                return "X";
            case 11:
                return "XI";
            case 12:
                return "XII";
            case 15:
                return "XV";
            case 30:
                return "XXX";
            case 45:
                return "XLV";
            case 0:
                return "XII";
            default:
                return "";
        }
    }
}
