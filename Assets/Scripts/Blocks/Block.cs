using UnityEngine;
using System;
using Utils;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D), typeof(SpriteRenderer))]
public class Block : MonoBehaviour
{
    public float throwPower = 5f;
    public event Action OnUpdate;
    public event Action OnFixedUpdate;

    [HideInInspector] public new Rigidbody2D rigidbody;
    [HideInInspector] public SpriteRenderer spriteRenderer;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected virtual void Update() 
    {
        if (GameManager.playing) OnUpdate?.Invoke(); 
    }
    protected virtual void FixedUpdate()
    {
        if (GameManager.playing) OnFixedUpdate?.Invoke();
    }

    public bool IsMoving() => IsMoving(0.01f);
    public bool IsMoving(float threshold) => rigidbody.velocity.sqrMagnitude > threshold;

    public bool CanFuseWith(Block other)
    {
        Block[] children = transform.GetComponentsInChildren<Block>();
        foreach (Block b in children)
            if (!b.Fusable(other)) return false;

        return Vector2.Distance(transform.position, other.transform.position) < 2.1f;
    }
    protected virtual bool Fusable(Block other) => true;
    public void FuseWith(Block other)
    {
        transform.localScale *= 1.414f;
        Vector3 pos = transform.position + other.transform.position;
        pos /= 2f;
        transform.position = pos;

        other.transform.parent = transform;
        other.gameObject.SetActive(false);
        other.OnBecomeFusedTo(this);
    }
    public virtual void OnBecomeFusedTo(Block other) { }

    public bool CanBreakApart() => GetFusedBlock() != null;
    public void BreakApart()
    {
        Block b = GetFusedBlock();
        b.transform.parent = GameManager.instance.gameLevel.blocks.transform;
 
        transform.localScale /= 1.414f;
        Vector3 move = new Vector2(0.75f * transform.localScale.x, 0f);
        b.transform.position = transform.position - move;
        transform.position += move;
        b.gameObject.SetActive(true);

        b.OnBreakApartFrom(this);

        GameManager.instance.gameLevel.blocks.DeselectBlock();
    }
    public virtual void OnBreakApartFrom(Block other) { }
    Block GetFusedBlock()
    {
        for (int i = 0; i < transform.childCount; i++)
            if (transform.GetChild(i).TryGetComponent(out Block b))
                return b;
        return null;
    }

    public T FindChildOfType<T>() where T : Block
    {
        if (this is T) return this as T;
        return GetComponentInChildren<T>(true);
    }

    public virtual void Throw()
    {
        AudioManager.Play("Whoosh");
        Vector2 force = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        rigidbody.AddForce(force.normalized * throwPower, ForceMode2D.Impulse);
    }

    public bool ContainsBlockOfType<T>() where T : Block
    {
        if (this is T) return true;
        return GetComponentInChildren<T>(true) != null;
    }

    public bool ContainsFlaggedBlock()
    {
        if (name.Equals("FlaggedBlock")) return true;
        foreach (Block block in GetComponentsInChildren<Block>(true))
            if (block.name.Equals("FlaggedBlock")) return true;
        return false;
    }

    void OnValidate()
    {
        if (GetComponent<SpriteRenderer>().sortingOrder == 0)
            GetComponent<SpriteRenderer>().sortingOrder = 1;
    }
}
