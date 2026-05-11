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
      CreateLevel.Instance.NextLevel();
    }
  }

}
