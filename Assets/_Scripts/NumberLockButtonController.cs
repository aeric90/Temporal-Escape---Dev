using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberLockButtonController : MonoBehaviour
{
    public TMPro.TextMeshProUGUI button_text ;
    // Start is called before the first frame update
    void Start()
    {
        button_text = GetComponent<TMPro.TextMeshProUGUI>();
    }

    public void Increment_Button()
    {
        if (button_text != null)
        {
            int button_num = int.Parse(button_text.text);
            button_num++;
            if (button_num > 6) button_num = 1;
            button_text.text = button_num.ToString();
        }
    }
}
