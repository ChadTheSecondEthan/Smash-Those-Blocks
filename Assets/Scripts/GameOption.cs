using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine;

public class GameOption : MonoBehaviour
{
    public Image image;
    public TMP_Text text;
    public Action OnComplete;
    [HideInInspector] public KeyCode keyCode;
    [HideInInspector] public bool destroyOnUse = true;

    void Start()
    {
        float x = image.sprite.rect.width;
        float y = image.sprite.rect.height;
        float total = x + y;
        float ratio = 2f / total;
        x *= ratio;
        y *= ratio;

        image.transform.localScale = new Vector2(x, y);
    }

    public void SetOnComplete(Action OnComplete) => this.OnComplete = OnComplete;

    public static Sprite SpriteFromKeyCode(KeyCode code)
    {
        return Resources.Load<Sprite>("Letters/" + code.ToString() + "Sprite");
    }
}
