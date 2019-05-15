using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Berries : MonoBehaviour
{
    private void OnTriggerEnter2D()
    {
        transform.parent.SendMessage("ye", SendMessageOptions.DontRequireReceiver);
        GameObject.Destroy(gameObject);
    }

}
