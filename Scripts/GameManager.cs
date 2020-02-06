using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public Camera camera;
    public GameObject player;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        camera = GameObject.FindObjectOfType<Camera>();
        player = GameObject.FindGameObjectWithTag("Player");

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
