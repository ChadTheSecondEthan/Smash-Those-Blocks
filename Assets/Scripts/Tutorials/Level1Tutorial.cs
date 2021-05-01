using UnityEngine;

public class Level1Tutorial : Tutorial
{
    public GameObject heavyBlockPointer;
    public GameObject flagPointer;
    public GameObject platformPointer;
    public GameObject levelCompleterPointer;
    public Door door;

    void Start()
    {
        heavyBlockPointer.SetActive(false);
        flagPointer.SetActive(false);
        platformPointer.SetActive(false);
        levelCompleterPointer.SetActive(false);
    }

    void Update()
    {
        if (!playing) return;

        bool heavyBlockActive = false;
        bool flagActive = false;
        bool platformActive = false;
        bool levelCompleterActive = false;

        Block block = GameManager.instance.gameLevel.blocks.selectedBlock;
        if (block)
        {
            if (!door.open)
            {
                if (!(block is WeightBlock))
                    heavyBlockActive = true;
                else
                    platformActive = true;
            }
            else
            {
                if (block.name.Equals("FlaggedBlock"))
                    levelCompleterActive = true;
                else
                    flagActive = true;
            }
        }
        else
        {
            if (door.open)
                flagActive = true;
            else
                heavyBlockActive = true;
        }

        heavyBlockPointer.SetActive(heavyBlockActive);
        flagPointer.SetActive(flagActive);
        platformPointer.SetActive(platformActive);
        levelCompleterPointer.SetActive(levelCompleterActive);
    }

    public override void StopTutorial()
    {
        base.StopTutorial();

        heavyBlockPointer.SetActive(false);
        flagPointer.SetActive(false);
        platformPointer.SetActive(false);
        levelCompleterPointer.SetActive(false);
    }
}
