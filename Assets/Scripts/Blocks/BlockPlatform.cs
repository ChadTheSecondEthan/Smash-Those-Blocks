using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Collections;

[RequireComponent(typeof(Collider2D), typeof(SpriteRenderer))]
public class BlockPlatform : MonoBehaviour
{
    public UnityEvent OnBlocksPlaced;
    public SpriteRenderer image;
    public List<Block> allowedBlocks;
    public int requiredBlocks = 1;
    public bool destroyOnComplete = true;

    List<Block> enteredBlocks;
    bool checkingVelocities = false;

    protected virtual void Start()
    {
        enteredBlocks = new List<Block>();
        OnBlocksPlaced.AddListener(() => image.enabled = false);
    }

    protected virtual void Update()
    {
        if (enteredBlocks.Count < requiredBlocks) return;

        if (!checkingVelocities && !BlockIsMoving())
            StartCoroutine(WaitAndCheckVelocities());
    }

    IEnumerator WaitAndCheckVelocities()
    {
        checkingVelocities = true;
        yield return new WaitForSeconds(1f);

        if (enteredBlocks.Count >= requiredBlocks && !BlockIsMoving())
        {
            LeanTween.moveY(gameObject, transform.position.y - .2f, 1f);
            OnBlocksPlaced?.Invoke();
            if (destroyOnComplete)
                Destroy(this);
        }

        checkingVelocities = false;
    }

    bool BlockIsMoving()
    {
        foreach (Block block in enteredBlocks)
            if (block.IsMoving(0.0001f)) return true;
        return false;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Block b = collision.GetComponent<Block>();
        if (b && (allowedBlocks.Count == 0 || allowedBlocks.Contains(b))) 
            enteredBlocks.Add(b);
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        Block b = collision.GetComponent<Block>();
        if (b) enteredBlocks.Remove(b);
    }
}
