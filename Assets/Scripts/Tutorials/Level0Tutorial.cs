using UnityEngine;

public class Level0Tutorial : Tutorial
{
    public GameObject blockPointer;
    public GameObject platformPointer;

    void Start()
    {
        blockPointer.SetActive(false);
        platformPointer.SetActive(false);
    }

    void Update()
    {
        if (!playing) return;

        blockPointer.SetActive(!GameManager.instance.gameLevel.blocks.selectedBlock);
        platformPointer.SetActive(GameManager.instance.gameLevel.blocks.selectedBlock);
    }
}
