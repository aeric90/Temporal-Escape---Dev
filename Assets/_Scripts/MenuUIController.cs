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
        if(statusText != null) statusText.text = TemporalEscapeController.instance.GetStatusText();

        if (Input.GetKeyDown(KeyCode.A)) OnPastClick();
        if (Input.GetKeyDown(KeyCode.S)) OnFutureClick();
    }

    public void OnPastClick()
    {
        if (roomClicked == false)
        {
            TemporalEscapeController.instance.SwitchRoom();
            if (buttonText != null) buttonText.text = "Cancel";
            roomClicked = true;
        } else
        {
            TemporalEscapeController.instance.CancelConnect();
            if (buttonText != null) buttonText.text = "Enter Game";
            roomClicked = false;
        }
    }

    public void OnFutureClick()
    {
        if (roomClicked == false)
        {
            TemporalEscapeController.instance.SwitchRoom();
            if (buttonText != null) buttonText.text = "Cancel";
            roomClicked = true;
        }
        else
        {
            TemporalEscapeController.instance.CancelConnect();
            if (buttonText != null) buttonText.text = "Enter Game";
            roomClicked = false;
        }
    }

    public void OnMenuClick()
    {
        TemporalEscapeController.instance.SwitchRoom("Menu Room");
        NetworkController.instance.LeaveRoom();
    }

    public void OnQuitClick()
    {
        Application.Quit();
    }


}
