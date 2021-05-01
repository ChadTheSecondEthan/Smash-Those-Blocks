using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class GameLevel : MonoBehaviour
{
    public UnityEvent OnLevelBegin;
    public UnityEvent OnLevelEnd;
    public UnityEvent OnCutSceneEnd;
    public LevelCompleter levelCompleter;
    public Blocks blocks;
    public Transform environment;
    public int number;
    //public bool allowFusions = true;
    //public int fusionsAllowed = 10;

    public BlockAttractor[] blockAttractors;

    void Start()
    {
        number = int.Parse("" + name[name.Length - 1]);
        /*if (allowFusions) GameUI.EnableFusionText();
        else GameUI.DisableFusionText();*/

        blockAttractors = environment.GetComponentsInChildren<BlockAttractor>();
        OnLevelBegin?.Invoke();
    }

    public void PlayCutScene() => StartCoroutine(MoveCamToLevelCompleter());

    IEnumerator MoveCamToLevelCompleter()
    {
        Camera.main.GetComponent<BlockFollower>().SnapToAverageBlockPosition();

        Vector3 initCamPos = Camera.main.transform.position;
        Vector3 levelCompleterPos = levelCompleter.transform.position;
        levelCompleterPos.y += 3f;
        levelCompleterPos.z = -10f;

        /*GameUI.AddGameOption(() => 
        {
            StopAllCoroutines();
            Camera.main.transform.position = initCamPos;
            OnCutSceneEnd?.Invoke();
            GameManager.instance.UnPause();
        }, "Skip", KeyCode.S);*/

        GameManager.instance.Pause();

        yield return new WaitForSeconds(1f);

        float distance = Vector2.Distance(initCamPos, levelCompleterPos);
        float time = distance / 10f;
        time = Mathf.Min(time, 3f);

        LeanTween.move(Camera.main.transform.gameObject,
            levelCompleterPos, time).setOnComplete(() => 
            Camera.main.transform.position = levelCompleterPos);
        
        yield return new WaitForSeconds(time + 1f);

        LeanTween.move(Camera.main.transform.gameObject,
            initCamPos, time).setOnComplete(() => 
            Camera.main.transform.position = initCamPos);
        yield return new WaitForSeconds(time);

        GameUI.RemoveGameOption(KeyCode.S);
        GameManager.instance.UnPause();
        OnCutSceneEnd?.Invoke();
    }

    void OnValidate()
    {
        if (!levelCompleter)
        {
            levelCompleter = transform.GetComponentInChildren<LevelCompleter>();
            if (!levelCompleter)
            {
                GameObject levelCompleter = new GameObject("LevelCompleter");
                levelCompleter.transform.parent = transform;
                SpriteRenderer sr = levelCompleter.AddComponent<SpriteRenderer>();
                sr.sprite = Resources.Load<Sprite>("WhitePixel");
                BoxCollider2D bc = levelCompleter.AddComponent<BoxCollider2D>();
                bc.isTrigger = true;
                levelCompleter.AddComponent<BoxCollider2D>();
                this.levelCompleter = levelCompleter.AddComponent<LevelCompleter>();
            }
        }
        if (!blocks)
        {
            blocks = transform.GetComponentInChildren<Blocks>();
            if (!blocks)
            {
                GameObject blocks = new GameObject("Blocks");
                blocks.transform.parent = transform;
                this.blocks = blocks.AddComponent<Blocks>();
            }
        }
        if (!environment)
        {
            if (transform.Find("Environment"))
                environment = transform.Find("Environment");

            else
            {
                GameObject env = new GameObject("Environment");
                env.transform.parent = transform;

                GameObject box = new GameObject("Box");
                SpriteRenderer sr = box.AddComponent<SpriteRenderer>();
                sr.sprite = Resources.Load<Sprite>("WhitePixel");

                box.AddComponent<BoxCollider2D>();
                box.transform.parent = env.transform;
                box.transform.localScale = new Vector2(3.5f, 3.5f);

                environment = env.transform;
            }
        }
    }
}