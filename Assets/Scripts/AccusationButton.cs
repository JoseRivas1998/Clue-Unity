using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AccusationButton : MonoBehaviour, IPointerClickHandler
{
    public ClueGameManager cgm;
    public bool isYesButton;

    public void OnPointerClick(PointerEventData eventData)
    {
        if(cgm.IsCurrentTurnHuman())
        {
            cgm.SelectAccusationOption(isYesButton);
        }
    }
}
