using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    private List<CharacterResourceManager.Cards> cards;

    public List<CharacterResourceManager.Cards> Cards
    {
        get
        {
            return new List<CharacterResourceManager.Cards>(cards);
        }
    }

    public PlayerData()
    {
        cards = new List<CharacterResourceManager.Cards>();
    }

    public void GiveCard(CharacterResourceManager.Cards card)
    {
        cards.Add(card);
    }

    public bool HasCard(CharacterResourceManager.Cards card)
    {
        return cards.Contains(card);
    }

}
