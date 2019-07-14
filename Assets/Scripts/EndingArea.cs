using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingArea : MonoBehaviour
{
  private void OnTriggerEnter(Collider other)
  {
    if (other.gameObject.name == "Player")
    {
      Debug.Log("Player at ending area");
      nextLevel();
    }
  }

  void nextLevel() {
    CreateLevel.level = CreateLevel.level + 1;
    SceneManager.LoadScene("Game");
  }

}
