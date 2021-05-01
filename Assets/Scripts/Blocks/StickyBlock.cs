using UnityEngine;

public class StickyBlock : Block
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.TryGetComponent(out StickySurface ss))
        {
            print("Found sticky surface");
            rigidbody.isKinematic = true;
            rigidbody.velocity = Vector2.zero;
        }
    }

    protected override bool Fusable(Block other)
    {
        return !other.ContainsBlockOfType<StickyBlock>();
    }

    public override void Throw()
    {
        rigidbody.isKinematic = false;

        base.Throw();
    }
}
