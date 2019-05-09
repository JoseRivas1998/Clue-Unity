using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GuessCharacterSelector : MonoBehaviour, IPointerClickHandler
{

    public ClueGameManager cgm;
    public CharacterResourceManager.Cards character;

    public void OnPointerClick(PointerEventData eventData)
    {
        if(cgm.IsCurrentTurnHuman())
        {
            cgm.SelectGuessCharacter(character);
        }
    }
}
