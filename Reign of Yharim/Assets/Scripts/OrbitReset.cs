using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitReset : MonoBehaviour
{

    private void Update()
    {
        if (GameTime.internalHours == 4 && GameTime.internalTime == 30 && GameTime.amOrPm == 1) //if the time is 4:30 AM (1)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, -45f); //set rotation to -45
        }
        if (GameTime.internalHours == 7 && GameTime.internalTime == 30 && GameTime.amOrPm == 2) //if the time is 7:30 PM (2)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, -135f); //set rotation to -135
        }
        if (GameTime.internalHours == 12 && GameTime.internalTime == 0 && GameTime.amOrPm == 2) //if the time is 12:00 PM (2) (Noon)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, -90f); //set rotation to -90
        }
        if (GameTime.internalHours == 12 && GameTime.internalTime == 0 && GameTime.amOrPm == 1) //if the time is 12:00 AM (1) (Midnight)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, -180f); //set rotation to -180
        }
    }
}
