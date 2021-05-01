using UnityEngine;

public abstract class Tutorial : MonoBehaviour
{
    protected bool playing = false;

    void Start() => GameManager.instance.gameLevel.OnLevelEnd.AddListener(() => Destroy(gameObject));

    public virtual void PlayTutorial() => playing = true;
    public virtual void StopTutorial() => playing = false;
}