using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public Sprite laprasMad;
    public Sprite laprasHappy;

    private bool isOpen;

    void Start()
    {
        isOpen = false;
        GetComponentInChildren<SpriteRenderer>().sprite = laprasMad;
    }

    public void OpenGoal()
    {
        isOpen = true;
        GetComponentInChildren<SpriteRenderer>().sprite = laprasHappy;
    }

    private void OnTriggerEnter2D()
    {
        if (isOpen)
        {
            transform.parent.SendMessage("OnGoalReached", SendMessageOptions.DontRequireReceiver);
            GameObject.Destroy(gameObject);
        }
    }


}
