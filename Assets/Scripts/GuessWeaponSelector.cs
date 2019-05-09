using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GuessWeaponSelector : MonoBehaviour, IPointerClickHandler
{

    public ClueGameManager cgm;
    public CharacterResourceManager.Cards weapon;

    public void OnPointerClick(PointerEventData eventData)
    {
        if(cgm.IsCurrentTurnHuman())
        {
            cgm.SelectGuessWeapon(weapon);
        }
    }
}
