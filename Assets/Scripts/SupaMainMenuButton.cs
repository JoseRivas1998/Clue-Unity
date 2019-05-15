using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class SupaMainMenuButton : MonoBehaviour, IPointerClickHandler
{
    public string scene;

    public void OnPointerClick(PointerEventData eventData)
    {
        SceneManager.LoadScene(scene);
    }
}
