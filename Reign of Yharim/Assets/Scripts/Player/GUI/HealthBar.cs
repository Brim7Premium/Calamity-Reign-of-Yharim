using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image barImage;
    [SerializeField] private Sprite[] barSprites = new Sprite[53];
    public int maxHealth;
    public int curHealth;

    private void Start() //on start
    {
        barImage = GetComponent<Image>(); //the image variable is equal to the attached image component
    }
    private void Update() //every frame
    {
        barImage.sprite = barSprites[(int)(Mathf.Clamp01(curHealth/(float)maxHealth)*(barSprites.Length-1)+0.5f)]; 
    }
    public void SetMaxHealth(int health)
    {
        maxHealth = health;
        curHealth = health;
    }

    public void SetHealth(int health)
    {
        curHealth = health;
    }
}
