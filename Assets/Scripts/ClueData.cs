﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClueData : Singleton<ClueData>
{

    public struct RowCol
    {
        public int row;
        public int col;
    }

    private CharacterResourceManager.Cards[] players = new CharacterResourceManager.Cards[4];
    private PlayerData[] playerData = new PlayerData[4];
    private GameObject[] playerGameObjects = new GameObject[4];
    private RowCol[] playerLocations = new RowCol[4];
    public Guess Solution { get; private set; }
    public Guess Guess;
    public int PlayerAccusation;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static RowCol GetRowColl(int[] position)
    {
        return new RowCol { row = position[0], col = position[1] };
    }

    public void SetPlayer(int player, CharacterResourceManager.Cards card)
    {
        players[player] = card;
    }

    public CharacterResourceManager.Cards GetPlayer(int i)
    {
        return players[i];
    }

    private void Swap<T>(List<T> list, int i, int j)
    {
        T temp = list[i];
        list[i] = list[j];
        list[j] = temp;
    }
    
    private List<T> CopyAndShuffle<T>(List<T> list)
    {
        List<T> result = new List<T>();
        for(int i = 0; i < list.Count; i++)
        {
            result.Add(list[i]);
        }
        for(int i = 0; i < result.Count; i++)
        {
            Swap(result, i, UnityEngine.Random.Range(0, result.Count - 1));
        }
        return result;
    } 

    private List<T> union<T>(List<T> list1, List<T> list2)
    {
        List<T> result = new List<T>();
        for (int i = 0; i < list1.Count; i++)
        {
            result.Add(list1[i]);
        }
        for (int i = 0; i < list2.Count; i++)
        {
            result.Add(list2[i]);
        }
        return result;
    }

    public void GenerateSolutionAndDistributeCards()
    {
        List<CharacterResourceManager.Cards> characters = CopyAndShuffle(CharacterResourceManager.Characters);
        List<CharacterResourceManager.Cards> weapons = CopyAndShuffle(CharacterResourceManager.Weapons);
        List<CharacterResourceManager.Cards> rooms = CopyAndShuffle(CharacterResourceManager.Rooms);
        this.Solution = new Guess(characters[characters.Count - 1], weapons[weapons.Count - 1], rooms[rooms.Count - 1]);
        print(this.Solution);
        characters.RemoveAt(characters.Count - 1);
        weapons.RemoveAt(weapons.Count - 1);
        rooms.RemoveAt(rooms.Count - 1);
        for(int i = 0; i < 4; i++)
        {
            playerData[i] = new PlayerData();
        }
        List<CharacterResourceManager.Cards> rest = CopyAndShuffle(union(union(characters, weapons), rooms));
        for(int i = 0; i < rest.Count; i++)
        {
            playerData[i % 4].GiveCard(rest[i]);
        }
    }

    public List<CharacterResourceManager.Cards> GetPlayerCards(int player)
    {
        return playerData[player].Cards;
    }

    public void SetPlayerGameObject(GameObject go, int player)
    {
        this.playerGameObjects[player] = go;
    }

    public GameObject GetPlayerGameObject(int player)
    {
        return playerGameObjects[player];
    }

    public void SetPlayerLocation(int[] position, int player)
    {
        playerLocations[player] = GetRowColl(position);
    }

    public void SetPlayerLocation(RowCol location, int player)
    {
        playerLocations[player] = location;
    }

    public RowCol GetPlayerRowCol(int player)
    {
        return playerLocations[player];
    }

    public int[] GetPlayerLocation(int player)
    {
        return new int[] { playerLocations[player].row, playerLocations[player].col };
    }

    public bool HasPlayerSeenCard(CharacterResourceManager.Cards card, int player)
    {
        return playerData[player].HasSeenCard(card);
    }

    public void ShowPlayerCard(CharacterResourceManager.Cards card, int player)
    {
        playerData[player].ShowCard(card);
    }

    public bool HasPlayerCard(CharacterResourceManager.Cards card, int player)
    {
        return playerData[player].HasCard(card);
    }

}
