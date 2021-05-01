using UnityEngine;

public class Cursor : MonoBehaviour
{
    public Sprite cursorImage;

    void Awake()
    {
        Vector2 offset = new Vector2(cursorImage.rect.width / 2f, cursorImage.rect.height / 2f);
        UnityEngine.Cursor.SetCursor(cursorImage.texture, offset, CursorMode.Auto);
    }

    void Update()
    {
        if (!GameManager.playing) return;

        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(pos.x, pos.y, 0f);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!GameManager.playing) return;

        if (collision.TryGetComponent(out Block block))
            GameManager.instance.gameLevel.blocks.HighlightBlock(block);
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (!GameManager.playing) return;

        if (collision.TryGetComponent(out Block block) && GameManager.instance.gameLevel.blocks.highlightedBlock == block)
            GameManager.instance.gameLevel.blocks.UnHighlightBlock(block);
    }
}
