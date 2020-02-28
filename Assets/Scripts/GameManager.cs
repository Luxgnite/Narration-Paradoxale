using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager _instance = null;

    public Camera camera;
    public GameObject played;

    private void Awake()
    {
        //Singleton Pattern
        if (_instance == null)
            _instance = this;
        else if (_instance != this)
            Destroy(this.gameObject);

        DontDestroyOnLoad(gameObject);

        camera = GameObject.FindObjectOfType<Camera>();
        played = GameObject.FindGameObjectWithTag("Player");

        InitGame();
    }

    void InitGame()
    {
        //Initialize Game
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}
