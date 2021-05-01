using UnityEngine;

public class BlockFollower : MonoBehaviour
{
    public float minYPos = -2f;
    public float minSize = 5f;

    Block selectedBlock;

    void FixedUpdate()
    {
        if (!GameManager.playing) return;

        if (selectedBlock || GameManager.instance.gameLevel.blocks.selectedBlock)
        {
            if (!selectedBlock || (GameManager.instance.gameLevel.blocks.selectedBlock
                && selectedBlock != GameManager.instance.gameLevel.blocks.selectedBlock))
                selectedBlock = GameManager.instance.gameLevel.blocks.selectedBlock;

            Vector3 pos = selectedBlock.transform.position;
            pos.z = -10f;
            if (pos.y < minYPos) pos.y = minYPos;
            transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime * 10f);
        }
        else
        {
            Block[] blocks = GameManager.instance.gameLevel.blocks.blocks;
            if (blocks.Length == 0) return;

            float totalX = 0f, totalY = 0f;
            foreach (Block block in blocks)
            {
                Vector2 blockPos = block.transform.position;
                totalX += blockPos.x;
                totalY += blockPos.y;
            }

            Vector3 pos = new Vector3(totalX / blocks.Length, totalY / blocks.Length, -10f);
            if (pos.y < minYPos) pos.y = minYPos;
            transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime * 10f);
        }
    }

    public void SnapToAverageBlockPosition()
    {
        Block[] blocks = GameManager.instance.gameLevel.blocks.blocks;
        if (blocks.Length == 0) return;

        float totalX = 0f, totalY = 0f;
        foreach (Block block in blocks)
        {
            Vector2 blockPos = block.transform.position;
            totalX += blockPos.x;
            totalY += blockPos.y;
        }

        Vector3 pos = new Vector3(totalX / blocks.Length, totalY / blocks.Length, -10f);
        if (pos.y < minYPos) pos.y = minYPos;
        transform.position = pos;
    }
}
