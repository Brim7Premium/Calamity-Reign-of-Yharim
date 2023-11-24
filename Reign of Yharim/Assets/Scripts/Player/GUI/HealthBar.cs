using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image barImage;
    [SerializeField] private RectTransform rectTransform;
    public int maxHealth;
    public int curHealth;

    private void Start()
    {
        barImage = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
    }
    private void Update()
    {
        //HelthProcent * BarSpriteSize.x * ReferencePixelsPerUnit * BarWidth/BarNativeWidth
        //*52)/52f will divide healthBar in 52 sprites like it was before
        Sprite bar = barImage.sprite;
        rectTransform.localPosition = new Vector2(((int)((Mathf.Clamp01(curHealth/(float)maxHealth)-1)*52))/52f * bar.bounds.size.x * 100f * rectTransform.rect.width/433.3333f , 0);
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
