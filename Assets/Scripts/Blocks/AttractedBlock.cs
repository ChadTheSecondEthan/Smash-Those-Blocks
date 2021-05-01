using UnityEngine;

public class AttractedBlock : Block
{
    [Tooltip("True => Magnet, False => Repulsion")]
    public bool attract = true;

    protected override bool Fusable(Block other)
    {
        return !other.ContainsBlockOfType<AttractedBlock>();
    }
}