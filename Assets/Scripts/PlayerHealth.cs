using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

  public int maxHealth = 6;
  public int startHealth = 3;

  public GameObject ui;
  public GameObject healthIcon;

  private List<GameObject> healthIcons = new List<GameObject>();

  private static int health = 0;

  // Start is called before the first frame update
  void Start()
  {
    if (health == 0) {
      health = startHealth;
    }
    updateUIIcons();
  }

  public void damagePlayer()
  {
    if (health > 0)
    {
      health--;
      updateUIIcons();
    }
  }

  public bool giveHealth()
  {
    if (health < maxHealth)
    {
      health++;
      updateUIIcons();
      return true;
    }
    else {
      return false;
    }
  }

  void updateUIIcons()
  {
    while (healthIcons.Count < health)
    {
      GameObject icon = Instantiate(healthIcon);
      icon.transform.SetParent(ui.transform, false);
      healthIcons.Add(icon);
      icon.transform.position = new Vector3(120 * healthIcons.Count, 100, 0);
    }
    while (healthIcons.Count > health)
    {
      GameObject o = healthIcons[healthIcons.Count - 1];
      healthIcons.Remove(o);
      Destroy(o);
    }
  }
}
