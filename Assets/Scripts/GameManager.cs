using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{

    [SerializeField]
    public GameObject _DefaultPrefab;

    private GameObject _prefabX;

    private GameObject _prefabO;

    private static GameManager _instance;

    public static GameManager Instance
    {
        get { return _instance; }
    }


    /// <summary>
    /// indicates if the game is paused
    /// </summary>
    private bool _paused = false;



    public static Action<Mark> OnPlayerWins = delegate { };

    public static Action<GameObject, GameObject> OnStartGame = delegate { };
    

    protected GameManager() { }

    /// <summary>
    /// 
    /// </summary>
    private void Awake()
    {
        //Checks to make sure its the real one.. if not destroy itself

        if (Instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }

        if (Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            SceneManager.sceneLoaded += HandleSceneLoaded;
        }

       
        
    }


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public static void ToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public static void StartGame(GameObject prefabX, GameObject prefabO)
    {
        Instance._prefabX = prefabX;
        Instance._prefabO = prefabO;
        SceneManager.LoadScene(1);
        
    }


    public static void PlayerWins(Mark player)
    {
        if(GameManager.OnPlayerWins != null)
        {
            GameManager.OnPlayerWins(player);
        }
    }

    private void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.buildIndex == 1)
        {
            OnStartGame(_prefabX != null ? _prefabX : _DefaultPrefab,
                _prefabO != null ? _prefabO : _DefaultPrefab);
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= HandleSceneLoaded;
    }
}
