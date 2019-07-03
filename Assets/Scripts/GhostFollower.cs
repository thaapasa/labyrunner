using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostFollower : MonoBehaviour
{

  public GameObject target;

  void FixedUpdate()
  {
    transform.position = Vector3.Lerp(transform.position, target.transform.position, Time.deltaTime);
    if (transform.position != target.transform.position)
    {
      transform.rotation = Quaternion.LookRotation(transform.position - target.transform.position, Vector3.up);
    }
  }
}
