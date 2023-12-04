using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Linq;

public class Saver : MonoBehaviour
{
    [SerializeField] private ToggleGroup toggles;
    [SerializeField] private Transform SaveMenu;
    [SerializeField] private TMP_Text NameInput;
    public void Save()
    {

        int i = 1;
        GameObject SaveBack = null;
        for(;i<5; i++){
            SaveBack = SaveMenu.Find($"Save{i.ToString()}Back").gameObject;
            if(!SaveBack.activeSelf) break;
        }

        if(i>4)
        {
            Debug.LogError("No save found");
            return;
        }

        
        SaveMenu.Find($"NewGame{i.ToString()}").gameObject.SetActive(true);

        SaveBack.SetActive(true);
        
        SaveBack.transform.Find("DifficultyIcon").Find("Icon").GetComponent<Image>().sprite = toggles.ActiveToggles().FirstOrDefault().image.sprite;
        SaveBack.transform.Find("SaveName").GetComponent<TMP_Text>().text = NameInput.text;
    }
}
