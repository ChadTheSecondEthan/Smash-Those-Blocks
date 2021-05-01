using UnityEngine;
using Utils;

public class Door : MonoBehaviour
{
    public float dy = 3f;
    [HideInInspector] public bool open = false;

    public void Open()
    {
        AudioManager.Play("DoorOpen");
        open = true;
        LeanTween.moveY(gameObject, transform.position.y + dy, 3f);
    }

    public void Close()
    {
        open = false;
        LeanTween.moveY(gameObject, transform.position.y - dy, 3f);
    }
}