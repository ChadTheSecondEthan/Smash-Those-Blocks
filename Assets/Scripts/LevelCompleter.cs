public class LevelCompleter : BlockPlatform
{
    protected override void Start()
    {
        base.Start();
        OnBlocksPlaced.AddListener(GameManager.instance.OnWin);
    }

    void OnValidate()
    {
        if (allowedBlocks.Count == 0)
            allowedBlocks.Add(FindObjectOfType<Blocks>().transform.Find("FlaggedBlock").GetComponent<Block>());
    }
}
