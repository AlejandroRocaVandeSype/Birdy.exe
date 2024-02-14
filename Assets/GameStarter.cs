using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStarter : MonoBehaviour
{

    public Animator cinematic;
    public float cinematicTime = 2f;
    public Animator fade;


    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            startGame();
        }
    }

    public void startGame()
    {
        StartCoroutine(loadGame());
    }

    IEnumerator loadGame()
    {
        // play animation
        cinematic.SetTrigger("Start");
        fade.SetTrigger("Start");

        // wait
        yield return new WaitForSeconds(cinematicTime);

        // load scene
        SceneManager.LoadScene(0);
    }
}
