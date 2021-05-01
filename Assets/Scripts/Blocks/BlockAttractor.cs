using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(SpriteRenderer), typeof(Collider2D))]
public class BlockAttractor : MonoBehaviour
{
    public bool linearForce = false;
    public float force = 10f;
    public float maxForce = 15f;
    public AttractionType attractionType;

    List<Block> allowedBlocks;
    List<Block> affectedBlocks;

    void Start()
    {
        switch (attractionType)
        {
            case AttractionType.Magnet:
            case AttractionType.Repulsion:
                AttractedBlock[] blocks = GameManager.instance.gameLevel.blocks.GetComponentsInChildren<AttractedBlock>();
                allowedBlocks = new List<Block>();
                foreach (AttractedBlock ab in blocks)
                {
                    if (attractionType == AttractionType.Magnet && ab.attract)
                        allowedBlocks.Add(ab);
                    else if (attractionType == AttractionType.Repulsion && !ab.attract)
                        allowedBlocks.Add(ab);
                }
                break;
            case AttractionType.Wind:
            default:
                allowedBlocks = new List<Block>(GameManager.instance.gameLevel.blocks.GetComponentsInChildren<LightBlock>());
                break;
        }

        affectedBlocks = new List<Block>();
    }
    
    void FixedUpdate()
    {
        Vector3 pos = transform.position;
        foreach (Block block in affectedBlocks)
        {
            AttractedBlock ab = block.FindChildOfType<AttractedBlock>();

            Vector3 direction;
            if (ab)
                direction = !ab.attract ? block.transform.position - pos : pos - block.transform.position;
            else
                direction = block.transform.position - pos;
            if (linearForce)
                block.rigidbody.AddForce(direction * force);
            else
            {
                float sqrDistance = direction.sqrMagnitude;
                float forceMagnitude = this.force / sqrDistance;
                if (forceMagnitude > maxForce) forceMagnitude = maxForce;
                Vector3 force = direction.normalized * forceMagnitude;
                block.rigidbody.AddForce(force);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Block b))
        {
            foreach (Block block in b.GetComponentsInChildren<Block>(true))
                if (allowedBlocks.Contains(block) && !affectedBlocks.Contains(b))
                {
                    affectedBlocks.Add(b);
                    return;
                }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Block b))
            affectedBlocks.Remove(b);
    }
}

[System.Serializable]
public enum AttractionType
{
    Magnet, Repulsion, Wind
}