using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverHandler : MonoBehaviour
{

    public GameObject gameOverUI;
    public GameObject levelHandler;
    public GameObject endScoreText;
    public GameObject endLevelText;

    public void OnGameOver(int level, int score)
    {
        gameOverUI.SetActive(true);
        endScoreText.GetComponent<Text>().text = score.ToString();
        endLevelText.GetComponent<Text>().text = level.ToString();
    }

    public void OnRestart()
    {
        levelHandler.GetComponent<CreateLevel>().OnNewGame();
    }

}
