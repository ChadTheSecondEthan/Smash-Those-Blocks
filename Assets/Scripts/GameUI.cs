using UnityEngine;
using System.Collections.Generic;
using System;
using TMPro;

public class GameUI : MonoBehaviour
{
    public GameOption gameOptionPrefab;
    public Animator winPageAnimator;

    static GameUI instance;

    List<GameOption> gameOptions;

    void Awake()
    {
        instance = this;

        gameOptions = new List<GameOption>();
    }

    //void Start() => UpdateFusionCount();

    void Update()
    {
        for (int i = 0; i < gameOptions.Count; i++)
        {
            GameOption go = gameOptions[i];
            if (Input.GetKeyDown(go.keyCode))
            {
                go.OnComplete();
                if (go.destroyOnUse)
                {
                    gameOptions.Remove(go);
                    Destroy(go.gameObject);
                    UpdateGameOptions();
                    i--;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
            GameManager.instance.gameLevel.blocks.SelectPrevBlock();
        if (Input.GetKeyDown(KeyCode.RightArrow))
            GameManager.instance.gameLevel.blocks.SelectNextBlock();
    }

    void UpdateGameOptions()
    {
        for (int i = 0; i < gameOptions.Count; i++)
            gameOptions[i].transform.position = new Vector2(100f, 500f - 75f * i);
    }

    public static void AddGameOption(Action OnComplete, string text = null, 
        KeyCode code = KeyCode.None, bool destroyOnUse = true)
    {
        foreach (GameOption option in instance.gameOptions)
            if (option.keyCode == code)
                return;

        GameOption go = Instantiate(instance.gameOptionPrefab, instance.transform);

        if (code != KeyCode.None)
            go.image.sprite = GameOption.SpriteFromKeyCode(code);
        go.OnComplete = OnComplete;
        go.text.text = text;
        go.destroyOnUse = destroyOnUse;
        go.keyCode = code;

        instance.gameOptions.Add(go);
        instance.UpdateGameOptions();
    }

    public static void RemoveGameOption(KeyCode keyCode)
    {
        for (int i = 0; i < instance.gameOptions.Count; i++)
            if (instance.gameOptions[i].keyCode == keyCode)
            {
                Destroy(instance.gameOptions[i].gameObject);
                instance.gameOptions.RemoveAt(i);
                instance.UpdateGameOptions();
                return;
            }
    }
    public static void RemoveAllGameOptions()
    {
        for (int i = instance.gameOptions.Count - 1; i >= 0; i--)
        {
            Destroy(instance.gameOptions[i].gameObject);
            instance.gameOptions.RemoveAt(i);
        }
    }

    /*public static void EnableFusionText() => instance.fusionText.transform.parent.gameObject.SetActive(true);
    public static void DisableFusionText() => instance.fusionText.transform.parent.gameObject.SetActive(false);
    public static void UpdateFusionCount()
    {
        instance.fusionText.text = $"{GameManager.instance.gameLevel.fusionsAllowed - GameManager.instance.gameLevel.blocks.fusionsCompleted}";
    }*/

    public static void OpenWinPage() => instance.winPageAnimator.Play("In");
    public static void CloseWinPage() => instance.winPageAnimator.Play("Out");
}
