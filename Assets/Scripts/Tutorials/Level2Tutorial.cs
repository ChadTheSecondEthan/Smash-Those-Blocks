using UnityEngine;

public class Level2Tutorial : Tutorial
{
    public GameObject flagPointer;
    public GameObject magnetPointer;
    public GameObject levelCompleterPointer;

    public Checkpoint passedMagnetCheckpoint;
    public Checkpoint beforeMagnetCheckpoint;

    void Start()
    {
        flagPointer.SetActive(false);
        magnetPointer.SetActive(false);
        levelCompleterPointer.SetActive(false);
    }

    void Update()
    {
        if (!playing) return;

        bool flagActive = false;
        bool magnetActive = false;
        bool levelCompleterActive = passedMagnetCheckpoint.reached;

        Block block = GameManager.instance.gameLevel.blocks.selectedBlock;
        if ((!block || !block.CanBreakApart()) && !beforeMagnetCheckpoint.reached)
        {
            flagActive = true;
            magnetActive = true;
        }

        flagPointer.SetActive(flagActive);
        magnetPointer.SetActive(magnetActive);
        levelCompleterPointer.SetActive(levelCompleterActive);
    }
}
