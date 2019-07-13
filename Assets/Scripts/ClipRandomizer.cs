using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClipRandomizer : MonoBehaviour
{

    public AudioClip[] clips;

    // Start is called before the first frame update
    void Start()
    {
        AudioSource source = GetComponent<AudioSource>();
        source.clip = clips[Random.Range(0, clips.Length)];
        source.Play();
    }
}
