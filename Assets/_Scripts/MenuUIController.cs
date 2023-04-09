using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuUIController : MonoBehaviour
{
    public TMPro.TextMeshProUGUI statusText;
    public TMPro.TextMeshProUGUI buttonText;
    private bool roomClicked = false;

    private void Update()
    {
        statusText.text = TemporalEscapeController.instance.GetStatusText();
    }

    public void OnPastClick()
    {
        if (roomClicked == false)
        {
            TemporalEscapeController.instance.SwitchRoom("Past Room");
            buttonText.text = "Cancel";
            roomClicked = true;
        } else
        {
            TemporalEscapeController.instance.CancelConnect();
            buttonText.text = "Enter Game";
            roomClicked = false;
        }
    }

    public void OnFutureClick()
    {
        if (roomClicked == false)
        {
            TemporalEscapeController.instance.SwitchRoom("Future Room");
            buttonText.text = "Cancel";
            roomClicked = true;
        }
        else
        {
            TemporalEscapeController.instance.CancelConnect();
            buttonText.text = "Enter Game";
            roomClicked = false;
        }
    }

    public void OnQuitClick()
    {
        Application.Quit();
    }


}
