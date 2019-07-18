using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingArea : MonoBehaviour
{

  private CreateLevel createLevel;

  void Start() {
    GameObject level = GameObject.Find("Level");
    createLevel = level.GetComponent<CreateLevel>();
  }

  private void OnTriggerEnter(Collider other)
  {
    if (other.gameObject.name == "Player")
    {
      Debug.Log("Player at ending area");
      createLevel.nextLevel();
    }
  }

}
