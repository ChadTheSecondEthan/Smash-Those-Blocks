using UnityEngine;
using UnityEngine.EventSystems;

public class Blocks : MonoBehaviour
{
    [HideInInspector] public Block highlightedBlock;
    [HideInInspector] public Block selectedBlock;
    [HideInInspector] public int fusionsCompleted;
    [HideInInspector] public Block[] blocks;

    public Color selectedColor = new Color(220f, 220f, 0f);
    public Color highlightedColor = new Color(255f, 255f, 0f);
    public Color highlightedAndSelectedColor = new Color(180f, 180f, 0f);

    void Awake() => blocks = GetComponentsInChildren<Block>();
    void OnTransformChildrenChanged()
    {
        blocks = GetComponentsInChildren<Block>();
        if (blocks.Length == 0)
            GameManager.instance.RestartLevel();
    }
    void Update()
    {
        if (!GameManager.playing) return;

        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            if (highlightedBlock)
            {
                if (selectedBlock == highlightedBlock)
                {
                    highlightedBlock.spriteRenderer.color = highlightedColor;
                    selectedBlock = null;
                    GameUI.RemoveGameOption(KeyCode.Space);
                }
                else
                {
                    if (selectedBlock)
                        selectedBlock.spriteRenderer.color = Color.white;
                    selectedBlock = highlightedBlock;
                    selectedBlock.spriteRenderer.color = highlightedAndSelectedColor;

                    UpdateSelectedBlock();
                }
            }

            else if (!highlightedBlock && selectedBlock && !selectedBlock.IsMoving())
                selectedBlock.Throw();
        }
    }

    void UpdateSelectedBlock()
    {
        if (!selectedBlock) return;

        if (selectedBlock.CanBreakApart())
            GameUI.AddGameOption(selectedBlock.BreakApart, text: "Break Apart", KeyCode.Space);
        else
            GameUI.RemoveGameOption(KeyCode.Space);

        if (highlightedBlock && selectedBlock != highlightedBlock
            //&& fusionsCompleted <= GameManager.instance.gameLevel.fusionsAllowed
            && selectedBlock.CanFuseWith(highlightedBlock)
            && highlightedBlock.CanFuseWith(selectedBlock))
        {
            GameUI.AddGameOption(() =>
            {
                fusionsCompleted++;
                //GameUI.UpdateFusionCount();
                selectedBlock.FuseWith(highlightedBlock);
                UpdateSelectedBlock();
            }, text: "Fuse", KeyCode.E);
        }
        else
            GameUI.RemoveGameOption(KeyCode.E);
    }

    public void PauseBlocks()
    {
        foreach (Block block in blocks)
            block.rigidbody.isKinematic = true;
    }

    public void UnPauseBlocks()
    {
        foreach (Block block in blocks)
            block.rigidbody.isKinematic = false;
    }

    public void DeselectBlock()
    {
        if (highlightedBlock == selectedBlock)
            selectedBlock.spriteRenderer.color = highlightedColor;
        else
            selectedBlock.spriteRenderer.color = Color.white;
        selectedBlock = null;
    }

    public void HighlightBlock(Block block)
    {
        if (highlightedBlock && highlightedBlock != block) UnHighlightBlock(highlightedBlock);
        highlightedBlock = block;

        if (selectedBlock == highlightedBlock)
            selectedBlock.spriteRenderer.color = highlightedAndSelectedColor;
        else
        {
            highlightedBlock.spriteRenderer.color = highlightedColor;
            UpdateSelectedBlock();
        }
    }

    public void UnHighlightBlock(Block block)
    {
        if (selectedBlock && block == selectedBlock)
            selectedBlock.spriteRenderer.color = selectedColor;
        else
            block.spriteRenderer.color = Color.white;

        highlightedBlock = null;
        UpdateSelectedBlock();
    }

    public void SelectNextBlock()
    {
        Block closestBlock = null;

        float sx = selectedBlock ? selectedBlock.transform.position.x : Camera.main.transform.position.x;
        foreach (Block block in blocks)
        {
            float x = block.transform.position.x;
            if (x > sx && (!closestBlock || x < closestBlock.transform.position.x))
                closestBlock = block;
        }

        if (!closestBlock || closestBlock == selectedBlock) return;

        if (selectedBlock)
            selectedBlock.spriteRenderer.color = Color.white;
        selectedBlock = closestBlock;
        selectedBlock.spriteRenderer.color = selectedColor;
        UpdateSelectedBlock();
    }

    public void SelectPrevBlock()
    {
        Block closestBlock = null;

        float sx = selectedBlock ? selectedBlock.transform.position.x : Camera.main.transform.position.x;
        foreach (Block block in blocks)
        {
            float x = block.transform.position.x;
            if (x < sx && (!closestBlock || x > closestBlock.transform.position.x))
                closestBlock = block;
        }

        if (!closestBlock || closestBlock == selectedBlock) return;

        if (selectedBlock)
            selectedBlock.spriteRenderer.color = Color.white;
        selectedBlock = closestBlock;
        selectedBlock.spriteRenderer.color = selectedColor;
        UpdateSelectedBlock();
    }

    public void SelectClosestBlock()
    {
        Block closestBlock = null;

        float sx = selectedBlock ? selectedBlock.transform.position.x : Camera.main.transform.position.x;
        foreach (Block block in blocks)
        {
            if (block == selectedBlock) continue;

            float x = block.transform.position.x;
            if (closestBlock == null || Mathf.Abs(sx - x) < Mathf.Abs(sx - closestBlock.transform.position.x))
                closestBlock = block;
        }

        if (!closestBlock || closestBlock == selectedBlock) return;

        if (selectedBlock)
            selectedBlock.spriteRenderer.color = Color.white;
        selectedBlock = closestBlock;
        selectedBlock.spriteRenderer.color = selectedColor;
        UpdateSelectedBlock();
    }
}
