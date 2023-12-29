using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameComponents : MonoBehaviour
{
    public MusicPlayer musicPlayer;
    public LevelLoader levelLoader;
    public AudioListener audioListener;
    public GameData gameData;

    private static GameComponents _instance;
    public static GameComponents Instance
    {
        get
        {
            return _instance;
        }
    }

    private void Awake()
    {
        if(_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            //  _instance can still be changed after it is assigned due to it being a refrence
            _instance = this;
            DontDestroyOnLoad(gameObject);

            Init();
        }
    }

    private void Init()
    {
        musicPlayer = GetComponentInChildren<MusicPlayer>();
        levelLoader = GetComponentInChildren<LevelLoader>();
        audioListener = GetComponentInChildren<AudioListener>();
        gameData = GetComponentInChildren<GameData>();
    }
}
