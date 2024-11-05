using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static int lives;
    public GameObject gameOverObject;

    // Start is called before the first frame update
    void Start()
    {
        lives = 2;
        gameOverObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(lives == 0)
        {
            Debug.Log("Game over!");
            gameOverObject.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void RestartGame()
    {
        lives = 2;
        Debug.Log("Loading scene...");
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
