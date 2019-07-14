using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScore : MonoBehaviour
{

  public GameObject scoreText;

  public static int score = 0;

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
    scoreText.GetComponent<Text>().text = score.ToString();
  }
}
