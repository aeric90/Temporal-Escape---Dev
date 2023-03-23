using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockLockButtonController : MonoBehaviour
{
    public TMPro.TextMeshProUGUI button_text;
    public int button_hour = 12;
    public int button_min = 0;

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

            button_text.text = button_hour.ToString() + ":" + button_min.ToString("D2");
        }
    }
}
