using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardClickSelectPlayer : MonoBehaviour, IPointerClickHandler
{
    public CharacterResourceManager.Cards card;
    public CharacterSelect sceneManager;

    public void OnPointerClick(PointerEventData eventData)
    {
        sceneManager.SelectCharacter(card);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
