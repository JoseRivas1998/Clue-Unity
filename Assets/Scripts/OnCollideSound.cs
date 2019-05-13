using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollideSound : MonoBehaviour
{
    public AudioSource sound;

    private void OnCollisionEnter(Collision collision)
    {
        sound.pitch = Random.Range(0.9f, 1.1f);
        sound.Play();
    }
}
