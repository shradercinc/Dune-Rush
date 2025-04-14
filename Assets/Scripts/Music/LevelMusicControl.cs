using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelMusicControl : MonoBehaviour
{
    [SerializeField] AK.Wwise.Event MusicLoop;
    [SerializeField] AK.Wwise.Event MusicStop;
    // Start is called before the first frame update
    void Start()
    {
        AkSoundEngine.SetSwitch("GameState", "GameOn", gameObject);
        MusicLoop.Post(gameObject);
    }

    public void onSceneExit()
    {
        MusicStop.Post(gameObject);
    }

    public void musicGameOver()
    {
        AkSoundEngine.SetSwitch("GameState", "GameOver", gameObject);
    }

}
