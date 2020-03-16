using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    private Animator anim;
    public float timeRead = 20f;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        StartCoroutine(Prevention());
    }

    private void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StopCoroutine(Prevention());
            ChangeScene();
        }
    }

    IEnumerator Prevention()
    {
        yield return new WaitForSeconds(timeRead);
        ChangeScene();
    }

    void ChangeScene()
    {
        anim.SetTrigger("ChangeScene");
        StartCoroutine(NextScene());
    }

    IEnumerator NextScene()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(1);
    }
}
