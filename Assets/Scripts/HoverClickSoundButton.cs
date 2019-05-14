using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverClickSoundButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler
{

    public AudioSource cursorSource;

    public AudioClip cursor;
    public AudioClip choice;

    public void OnPointerClick(PointerEventData eventData)
    {
        cursorSource.clip = choice;
        cursorSource.Play();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        cursorSource.clip = cursor;
        cursorSource.Play();
    }
}
