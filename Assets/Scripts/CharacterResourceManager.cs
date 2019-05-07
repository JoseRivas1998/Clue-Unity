using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterResourceManager
{

    public enum CardType
    {
        Character, Weapon, Room
    }

    public enum Cards
    {
        Freddie,
        ScoobyDoo,
        MurderV,
        Sherlock,
        Blackwidow,
        Echo,
        Machete,
        Bazooka,
        Antifreeze,
        Chainsaw,
        Spoon,
        Syringe,
        HannibalsKitchen,
        PuppetRoom,
        Cemetary,
        ChuckysPlayroom,
        Nursery,
        Morge,
        Maze,
    }

    private static readonly Dictionary<Cards, CardType> _cardTypeMap = new Dictionary<Cards, CardType>()
    {
        { Cards.Freddie, CardType.Character },
        { Cards.ScoobyDoo, CardType.Character },
        { Cards.MurderV, CardType.Character },
        { Cards.Sherlock, CardType.Character },
        { Cards.Blackwidow, CardType.Character },
        { Cards.Echo, CardType.Character },
        { Cards.Machete, CardType.Weapon },
        { Cards.Bazooka, CardType.Weapon },
        { Cards.Antifreeze, CardType.Weapon },
        { Cards.Chainsaw, CardType.Weapon },
        { Cards.Spoon, CardType.Weapon },
        { Cards.Syringe, CardType.Weapon },
        { Cards.HannibalsKitchen, CardType.Room },
        { Cards.PuppetRoom, CardType.Room },
        { Cards.Cemetary, CardType.Room },
        { Cards.ChuckysPlayroom, CardType.Room },
        { Cards.Nursery, CardType.Room },
        { Cards.Morge, CardType.Room },
        { Cards.Maze, CardType.Room },
    };

    // TODO: Map Cards to Textures, Load Texture based on Given Card

    private static List<Cards> FilterCards(System.Predicate<Cards> filter)
    {
        List<Cards> cards = new List<Cards>();

        foreach (Cards card in (Cards[])System.Enum.GetValues(typeof(Cards)))
        {
            if (filter.Invoke(card))
            {
                cards.Add(card);
            }
        }

        return cards;
    }

    private static List<Cards> FilterCardsOfType(CardType type)
    {
        return FilterCards(c => _cardTypeMap[c].Equals(type));
    }

    public readonly static List<Cards> Characters = FilterCardsOfType(CardType.Character);
    public readonly static List<Cards> Weapons = FilterCardsOfType(CardType.Weapon);
    public readonly static List<Cards> Rooms = FilterCardsOfType(CardType.Room);

}
