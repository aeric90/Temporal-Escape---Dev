using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuUIController : MonoBehaviour
{
    public void OnPastClick()
    {
        TemporalEscapeController.instance.SwitchRoom("Past Room");
    }

    public void OnFutureClick()
    {
        TemporalEscapeController.instance.SwitchRoom("Future Room");
    }
}
