using UnityEngine;

public class BlockDestroyer : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Block b))
        {
            GameManager.instance.gameLevel.blocks.SelectClosestBlock();
            if (b.ContainsFlaggedBlock())
                GameManager.instance.RestartLevel();
            else
                Destroy(b.gameObject, 1f);
        }
    }
}