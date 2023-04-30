using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GUIController : MonoBehaviour
{
    public TMP_Text guiText;

    void Update()
    {
        guiText.text = GameTime.displayTime;
    }
}
