using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverHandler : MonoBehaviour
{

    public GameObject gameOverUI;
    public GameObject level;

    public void OnGameOver()
    {
        gameOverUI.SetActive(true);
    }

    public void OnRestart()
    {
        level.GetComponent<CreateLevel>().OnNewGame();
    }

}
