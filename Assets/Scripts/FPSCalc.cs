using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSCalc : MonoBehaviour
{
  public Text fpsText;

  private Queue<float> frameTimes;
  private int frameCount = 30;

  void Start()
  {
    // Initialize the Queue with a capacity of frameCount
    frameTimes = new Queue<float>(frameCount);
    fpsText = GetComponent<Text>();
  }

  void Update()
  {
    // Store the time for this frame
    frameTimes.Enqueue(Time.deltaTime);

    // Remove the oldest frame time if we have more than frameCount frames stored
    if (frameTimes.Count > frameCount)
    {
      frameTimes.Dequeue();
    }

    float averageFrameTime = 0f;
    foreach (float frameTime in frameTimes)
    {
      averageFrameTime += frameTime;
    }
    averageFrameTime /= frameTimes.Count;

    // Calculate and display the average FPS
    float averageFPS = 1.0f / averageFrameTime;
    updateUI((int) Mathf.Round(averageFPS));
  }

  private void updateUI(int fps)
  {
    if (fpsText != null)
    {
      fpsText.text = fps.ToString();
    }
  }

}
