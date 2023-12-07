using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image barImage;
    [SerializeField] private RectTransform rectTransform;
    public int maxHealth;
    public int curHealth;
    [SerializeField][Range(0, 100)]private int barStep = 52;

    private void Start()
    {
        barImage = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
    }
    private void Update()
    {
        //HelthProcent * BarSpriteSize.x * ReferencePixelsPerUnit
        //(int)(x*barStep)/(float)(barStep) will divide healthBar in discrete sprites like it was before
        //expected that the limiter and the bar are the same width and height
        Sprite bar = barImage.sprite;
        rectTransform.localPosition = new Vector2(((int)((Mathf.Clamp01(curHealth/(float)maxHealth)-1)*barStep))/(float)(barStep) * bar.bounds.size.x * 100f , 0);
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
