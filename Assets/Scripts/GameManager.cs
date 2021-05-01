using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

public class GameManager : MonoBehaviour
{
    public GameLevel gameLevel;

    public static GameManager instance;
    public static bool playing = true;

    void Awake() => instance = this;

    void Start() => gameLevel.PlayCutScene();

    public void OnWin()
    {
        AudioManager.Play("Win", pauseTheme: true);
        gameLevel.OnLevelEnd?.Invoke();
        playing = false;
        GameUI.RemoveAllGameOptions();
        if (!Resources.Load($"Levels/Level{gameLevel.number + 1}"))
            LoadHomepage();
        else
            GameUI.OpenWinPage();
    }

    public void Pause()
    {
        GameUI.RemoveGameOption(KeyCode.R);
        playing = false;
        gameLevel.blocks.PauseBlocks();
    }

    public void UnPause()
    {
        GameUI.AddGameOption(RestartLevel, "Restart", KeyCode.R, false);
        playing = true;
        gameLevel.blocks.UnPauseBlocks();
    }

    public void RestartLevel() => LoadLevel(gameLevel.number, playCutScene: false);
    public void LoadNextLevel() => LoadLevel(gameLevel.number + 1);
    public void LoadLevel(int index, bool playCutScene = true)
    {
        GameUI.RemoveAllGameOptions();
        GameObject prevLevel = gameLevel.gameObject;
        GameLevel level = Resources.Load<GameLevel>($"Levels/Level{index}");
        if (!level)
            LoadHomepage();
        else
        {
            GameUI.AddGameOption(RestartLevel, "Restart", KeyCode.R, false);
            gameLevel = Instantiate(level);
            gameLevel.OnCutSceneEnd.AddListener(() => playing = true);
            if (playCutScene)
                gameLevel.PlayCutScene();
        }
        Destroy(prevLevel);
    }
    public void LoadHomepage() => SceneManager.LoadScene("Title");

    public void SelecteNextBlock() => instance.gameLevel.blocks.SelectNextBlock();
    public void SelectPreviousBlock() => instance.gameLevel.blocks.SelectPrevBlock();

    void OnValidate()
    {
        if (!gameLevel)
            gameLevel = FindObjectOfType<GameLevel>();
    }
}