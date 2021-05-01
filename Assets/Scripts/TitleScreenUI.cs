using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Utils;

public class TitleScreenUI : MonoBehaviour
{
    public Button playButton;
    public Button audioButton;

    void Awake()
    {
        playButton.SetOnClick(() => SceneManager.LoadScene("Game"));
        audioButton.SetOnClick(() => 
        {
            if (AudioListener.volume == 0f)
            {
                audioButton.image.sprite = Resources.Load<Sprite>("VolumeOn");
                AudioListener.volume = 1f;
            }
            else
            {
                audioButton.image.sprite = Resources.Load<Sprite>("VolumeOff");
                AudioListener.volume = 0f;
            }
        });
    }
}