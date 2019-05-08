using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    private List<CharacterResourceManager.Cards> cards;

    private List<CharacterResourceManager.Cards> cardsSeen;

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
        cardsSeen = new List<CharacterResourceManager.Cards>();
    }

    public void GiveCard(CharacterResourceManager.Cards card)
    {
        cards.Add(card);
        ShowCard(card);
    }

    public void ShowCard(CharacterResourceManager.Cards card)
    {
        cardsSeen.Add(card);
    }

    public bool HasCard(CharacterResourceManager.Cards card)
    {
        return cards.Contains(card);
    }

    public bool HasSeenCard(CharacterResourceManager.Cards card)
    {
        return cardsSeen.Contains(card);
    }

}
