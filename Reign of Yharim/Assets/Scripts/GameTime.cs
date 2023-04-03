using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTime : MonoBehaviour
{
    private int count = 0;
    private int internalTime;
    private int internalHours;
    private int amOrPm;
    public static string displayTime;
    public string displayAmOrPm;

    void Start()
    {
        count = 0;
        internalTime = 30;
        internalHours = 4;
        amOrPm = 1;
    }
    private void Update()
    {
        if (count > 60)
        {
            internalTime++;
            if (internalTime == 60)
            {
                internalTime = 0;
                internalHours++;
                if (internalHours == 13)
                {
                    internalHours = 1;
                    amOrPm++;
                    if(amOrPm == 3)
                    {
                        amOrPm = 1;
                    }
                }
            }
            count = 1;
        }
        if (amOrPm == 1)
        {
            displayAmOrPm = "AM";
        }
        else
        {
            displayAmOrPm = "PM";
        }
        if (internalTime < 10)
        {
            displayTime = internalHours + ":0" + internalTime + " " + displayAmOrPm;
        }
        else
        {
            displayTime = internalHours + ":" + internalTime + " " + displayAmOrPm;
        }
        count++;
    }
}
