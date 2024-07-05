using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScore : MonoBehaviour
{

  public GameObject scoreText;

  public static int score = 0;

  public void Reset()
  {
    score = 0;
  }

  public int GetScore()
  {
    return score;
  }

  // Start is called before the first frame update
  void Start()
  {
    updateUI();
  }

  public void addScore(int points)
  {
    score += points;
    updateUI();
  }

  void updateUI()
  {
    if (scoreText != null)
    {
      scoreText.GetComponent<Text>().text = score.ToString();
    }
  }
}
