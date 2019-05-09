using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClueGameManager : MonoBehaviour
{

    public HumanPlayerLoadCards humanCardsDisplay;
    public TurnManager turnManager;
    public RollDice diceRoller;
    public CameraFollow camFollower;
    public BoardManager boardManager;
    public GameObject characterGuessParent;
    public GameObject weaponGuessParent;
    public GuessDisplay guessDisplay;
    public Texture2D cardBack;
    public CardShow cardShow;
    public AccusationOption accusationOption;

    public Text currentTurnText;

    public GameObject highlight;

    private Stack<GameObject> highlights;

    public static readonly ClueData.RowCol[] excludedLocations =
    {
        new ClueData.RowCol{col = 24, row = 15},
        new ClueData.RowCol{col = 24, row = 16},
        new ClueData.RowCol{col = 24, row = 17}
    };

    public static readonly ClueData.RowCol[] entrances =
    {
        new ClueData.RowCol{col = 0, row = 6},
        new ClueData.RowCol{col = 0, row = 7},
        new ClueData.RowCol{col = 0, row = 8},
        new ClueData.RowCol{col = 9, row = 0},
        new ClueData.RowCol{col = 25, row = 1},
        new ClueData.RowCol{col = 25, row = 2},
        new ClueData.RowCol{col = 25, row = 3},
        new ClueData.RowCol{col = 25, row = 13},
        new ClueData.RowCol{col = 25, row = 14},
        new ClueData.RowCol{col = 10, row = 19},
        new ClueData.RowCol{col = 11, row = 19},
        new ClueData.RowCol{col = 0, row = 18},
        new ClueData.RowCol{col = 24, row = 18},
    };

    public struct RoomEntrance
    {
        public CharacterResourceManager.Cards room;
        public ClueData.RowCol[] entrances;
    }

    public static readonly RoomEntrance[] roomEntracnes =
    {
        new RoomEntrance
        {
            room = CharacterResourceManager.Cards.HannibalsKitchen,
            entrances = new ClueData.RowCol[]
            {
                new ClueData.RowCol{row = 0, col = 9 },
            }
        },

        new RoomEntrance
        {
            room = CharacterResourceManager.Cards.PuppetRoom,
            entrances = new ClueData.RowCol[]
            {
                new ClueData.RowCol{row = 13, col = 25 },
                new ClueData.RowCol{row = 14, col = 25 },
            }
        },

        new RoomEntrance
        {
            room = CharacterResourceManager.Cards.Cemetary,
            entrances = new ClueData.RowCol[]
            {
                new ClueData.RowCol{row = 18, col = 0 },
            }
        },

        new RoomEntrance
        {
            room = CharacterResourceManager.Cards.ChuckysPlayroom,
            entrances = new ClueData.RowCol[]
            {
                new ClueData.RowCol{row = 1, col = 25 },
                new ClueData.RowCol{row = 2, col = 25 },
                new ClueData.RowCol{row = 3, col = 25 },
            }
        },

        new RoomEntrance
        {
            room = CharacterResourceManager.Cards.Nursery,
            entrances = new ClueData.RowCol[]
            {
                new ClueData.RowCol{row = 6, col = 0 },
                new ClueData.RowCol{row = 7, col = 0 },
                new ClueData.RowCol{row = 8, col = 0 },
            }
        },

        new RoomEntrance
        {
            room = CharacterResourceManager.Cards.Morge,
            entrances = new ClueData.RowCol[]
            {
                new ClueData.RowCol{row = 19, col = 10 },
                new ClueData.RowCol{row = 19, col = 11 },
            }
        },

        new RoomEntrance
        {
            room = CharacterResourceManager.Cards.Maze,
            entrances = new ClueData.RowCol[]
            {
                new ClueData.RowCol{row = 18, col = 24 }
            }
        }
    };

    public enum TurnState
    {
        PreTurn,
        RollingDice,
        Moving,
        ChoosingGuessCharacter,
        ChoosingGuessWeapon,
        ShowingGuess,
        FindingCards,
        ShowingCards,
        AccusationOption,
        MakingAccusation,
        PostTurn
    }

    private TurnState turnState;
    private bool isSelected;
    private bool isMoving;
    private Vector3 targetPosition;

    private bool makeAccusation;

    private CharacterResourceManager.Cards guessCharacter;
    private CharacterResourceManager.Cards guessWeapon;
    private CharacterResourceManager.Cards guessRoom;
    private Guess guess;

    private float aiTimer;

    // Start is called before the first frame update
    void Start()
    {
        turnState = TurnState.PreTurn;
        aiTimer = 0;
        highlights = new Stack<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        currentTurnText.text = "Player " + turnManager.CurrentTurn + "'s Turn";
        switch(turnState)
        {
            case TurnState.PreTurn: PreTurn(); break;
            case TurnState.RollingDice: RollingDice(); break;
            case TurnState.Moving: Moving(); break;
            case TurnState.ChoosingGuessCharacter: ChoosingGuessCharacter(); break;
            case TurnState.ChoosingGuessWeapon: ChoosingGuessWeapon(); break;
            case TurnState.ShowingGuess: ShowingGuess(); break;
            case TurnState.FindingCards: FindingCards(); break;
            case TurnState.ShowingCards: ShowingCards(); break;
            case TurnState.AccusationOption: AccusationOption(); break;
            case TurnState.MakingAccusation: MakingAccusation(); break;
            case TurnState.PostTurn: PostTurn(); break;
        }
    }

    private void PreTurn()
    {
        camFollower.Follow(ClueData.Instance.GetPlayerGameObject(turnManager.CurrentTurn - 1));
        if(camFollower.DistanceFromTarget() <= 0.01f)
        {
            aiTimer = 0;
            diceRoller.ActivateRoller();
            turnState = TurnState.RollingDice;
        }
    }

    private void RollingDice()
    {
        if(IsCurrentTurnHuman())
        {
            if(diceRoller.RollerState.Equals(RollDice.DiceRollerState.HoldingDice))
            {
                if (Input.GetKeyUp(KeyCode.Space))
                {
                    diceRoller.TossDice();
                }
            }
        }
        else
        {
            aiTimer += Time.deltaTime;
            if(aiTimer >= 1)
            {
                if (diceRoller.RollerState.Equals(RollDice.DiceRollerState.HoldingDice))
                {
                    diceRoller.TossDice();
                }
            }
        }
        if (diceRoller.RollerState.Equals(RollDice.DiceRollerState.Complete))
        {
            aiTimer = 0;
            diceRoller.DeactivateRoller();
            isMoving = false;
            isSelected = false;
            HighlightSpots();
            turnState = TurnState.Moving;
        };
    }

    public bool IsValid(int[] position)
    {
        return IsValid(ClueData.GetRowColl(position));
    }

    public bool IsEntrance(ClueData.RowCol rowCol)
    {
        bool found = false;
        for(int i = 0; i < entrances.Length && !found; i++)
        {
            found = (rowCol.row == entrances[i].row && rowCol.col == entrances[i].col);
        }
        return found;
    }

    public bool IsValid(ClueData.RowCol rowCol)
    {
        if(rowCol.row <= 0 || rowCol.row >= boardManager.numRows - 1 || rowCol.col <= 0 || rowCol.col >= boardManager.numCols - 1)
        {
            return IsEntrance(rowCol);
        }
        for(int i = 0; i < excludedLocations.Length; i++)
        {
            if(rowCol.row == excludedLocations[i].row && rowCol.col == excludedLocations[i].col)
            {
                return false;
            }
        }
        return true;
    }

    private void HighlightSpots()
    {
        ClueData.RowCol pos = ClueData.Instance.GetPlayerRowCol(turnManager.CurrentTurn - 1);
        List<int[]> positions = PossibleBoardLocations.GetBoardLocations(pos.row, pos.col, diceRoller.DiceValue, (i => IsValid(i)));
        for(int i = 0; i < positions.Count; i++)
        {
            Vector3 spawn = boardManager.RowColToBoardLocation(positions[i][0], positions[i][1], 5e-4f);
            GameObject tile = Instantiate(highlight, spawn, Quaternion.identity);
            tile.GetComponent<TileSelector>().cgm = this;
            tile.GetComponent<TileSelector>().location = ClueData.GetRowColl(positions[i]);
            tile.GetComponent<TileSelector>().clickable = IsCurrentTurnHuman();
            highlights.Push(tile);
        }
    }

    public void SelectTile(ClueData.RowCol pos)
    {
        if(!isSelected)
        {
            RemoveHighlights();
            targetPosition = boardManager.RowColToBoardLocation(pos.row, pos.col);
            ClueData.Instance.SetPlayerLocation(pos, turnManager.CurrentTurn - 1);
            isSelected = true;
            isMoving = true;
        }
    }

    private void RemoveHighlights()
    {
        while(highlights.Count > 0)
        {
            Destroy(highlights.Pop());
        }
    }

    private void Moving()
    {
        if(isSelected)
        {
            if(isMoving)
            {
                GameObject player = ClueData.Instance.GetPlayerGameObject(turnManager.CurrentTurn - 1);
                player.transform.position += (targetPosition - player.transform.position) / 25f;
                if((targetPosition - player.transform.position).magnitude < (boardManager.TileWidth / 30f))
                {
                    player.transform.position = targetPosition;
                    isMoving = false;
                }
            }
            else
            {
                if(IsEntrance(ClueData.Instance.GetPlayerRowCol(turnManager.CurrentTurn - 1)))
                {
                    humanCardsDisplay.HideAll();
                    characterGuessParent.SetActive(true);
                    isSelected = false;
                    isMoving = false;
                    CharacterResourceManager.Cards room = GetEntranceRoom(ClueData.Instance.GetPlayerRowCol(turnManager.CurrentTurn - 1));
                    guessRoom = room;
                    ClueData.Instance.GetPlayerGameObject(turnManager.CurrentTurn - 1).transform.position = CharacterResourceManager.RoomLocation(room);
                    turnState = TurnState.ChoosingGuessCharacter;
                }
                else
                {
                    turnState = TurnState.PostTurn;
                }
            }
        } 
        else if(!IsCurrentTurnHuman())
        {
            aiTimer += Time.deltaTime;
            if(aiTimer >= 1)
            {
                aiTimer = 0;
                SelectTile(AIChooseSpot());
            }
        }
        
    }

    private CharacterResourceManager.Cards GetEntranceRoom(ClueData.RowCol rowCol)
    {
        if (!IsEntrance(rowCol)) throw new Exception("Not an entrance");
        foreach(RoomEntrance roomEntrance in roomEntracnes)
        {
            foreach(ClueData.RowCol entrance in roomEntrance.entrances)
            {
                if(rowCol.row == entrance.row && rowCol.col == entrance.col)
                {
                    return roomEntrance.room;
                }
            }
        }
        throw new Exception("not found");
    }

    private ClueData.RowCol AIChooseSpot()
    {
        List<CharacterResourceManager.Cards> targetRooms = new List<CharacterResourceManager.Cards>();
        foreach(CharacterResourceManager.Cards room in CharacterResourceManager.Rooms)
        {
            if(!ClueData.Instance.HasPlayerSeenCard(room, turnManager.CurrentTurn - 1))
            {
                targetRooms.Add(room);
            }
        }
        List<ClueData.RowCol> tiles = new List<ClueData.RowCol>();
        foreach(GameObject tile in highlights)
        {
            ClueData.RowCol tilePos = tile.GetComponent<TileSelector>().location;
            if(IsEntrance(tilePos))
            {
                CharacterResourceManager.Cards room = GetEntranceRoom(tilePos);
                if(targetRooms.Contains(room))
                {
                    return tilePos;
                }
            }
            tiles.Add(tilePos);
        }
        return tiles[UnityEngine.Random.Range(0, tiles.Count - 1)];
    }

    public void SelectGuessCharacter(CharacterResourceManager.Cards character)
    {
        if(!isSelected)
        {
            isSelected = true;
            guessCharacter = character;
        }
    }

    public void ChoosingGuessCharacter()
    {
        if(isSelected)
        {
            characterGuessParent.SetActive(false);
            weaponGuessParent.SetActive(true);
            isSelected = false;
            turnState = TurnState.ChoosingGuessWeapon;
        } 
        else if(!IsCurrentTurnHuman())
        {
            aiTimer += Time.deltaTime;
            if(aiTimer > 1)
            {
                aiTimer = 0;
                AIChooseGuessCharacter();
            }
        }
    }

    private void AIChooseGuessCharacter()
    {
        if (UnityEngine.Random.Range(1, 100) < 50)
        {
            List<CharacterResourceManager.Cards> characters = new List<CharacterResourceManager.Cards>();
            foreach (CharacterResourceManager.Cards card in CharacterResourceManager.Characters)
            {
                if (!ClueData.Instance.HasPlayerSeenCard(card, turnManager.CurrentTurn - 1))
                {
                    characters.Add(card);
                }
            }
            SelectGuessCharacter(characters[UnityEngine.Random.Range(0, characters.Count - 1)]);
        }
        else
        {
            SelectGuessCharacter(CharacterResourceManager.Characters[UnityEngine.Random.Range(0, CharacterResourceManager.Characters.Count - 1)]);
        }
    }

    public void SelectGuessWeapon(CharacterResourceManager.Cards card)
    {
        if(!isSelected)
        {
            isSelected = true;
            guessWeapon = card;
        }
    }

    public void ChoosingGuessWeapon()
    {
        if(isSelected)
        {
            isSelected = false;
            weaponGuessParent.SetActive(false);
            guess = new Guess(guessCharacter, guessWeapon, guessRoom);
            guessDisplay.Set(guess, turnManager.CurrentTurn);
            guessDisplay.gameObject.SetActive(true);
            turnState = TurnState.ShowingGuess;
        }
        else if(!IsCurrentTurnHuman())
        {
            aiTimer += Time.deltaTime;
            if(aiTimer > 1)
            {
                aiTimer = 0;
                AIChooseGuessWeapon();
            }
        }
    }

    private void AIChooseGuessWeapon()
    {
        if(UnityEngine.Random.Range(1, 100) < 50)
        {
            List<CharacterResourceManager.Cards> weapons = new List<CharacterResourceManager.Cards>();
            foreach (CharacterResourceManager.Cards weapon in CharacterResourceManager.Weapons)
            {
                if (!ClueData.Instance.HasPlayerSeenCard(weapon, turnManager.CurrentTurn - 1))
                {
                    weapons.Add(weapon);
                }
            }
            SelectGuessWeapon(weapons[UnityEngine.Random.Range(0, weapons.Count - 1)]);
        }
        else
        {
            SelectGuessWeapon(CharacterResourceManager.Weapons[UnityEngine.Random.Range(0, CharacterResourceManager.Weapons.Count - 1)]);
        }
    }

    public void ShowingGuess()
    {
        aiTimer += Time.deltaTime;
        if(aiTimer > 5)
        {
            aiTimer = 0;
            guessDisplay.gameObject.SetActive(false);
            turnState = TurnState.FindingCards;
        }
    }

    public void FindingCards()
    {
        CharacterResourceManager.Cards cardToShow = CharacterResourceManager.Cards.Antifreeze;
        TurnState targetState = TurnState.PreTurn;
        int showedCardSource = -1; // sure
        turnManager.PushStack();

        int cT = turnManager.CurrentTurn;
        bool control = true; // yes, this is how lazy i am, no need to bother writing a real algorithm

        while(control)
        {
            turnManager.NextTurn();
            if (turnManager.CurrentTurn == cT)
            {
                targetState = TurnState.AccusationOption;
                control = false;
            }
            else
            {
                // Yes, this is going to be how it works, deal with it (in real clue there is a choice of what card to show)
                if (ClueData.Instance.HasPlayerCard(guess.Character, turnManager.CurrentTurn - 1))
                {
                    control = false;
                    targetState = TurnState.ShowingCards;
                    cardToShow = guess.Character;
                    showedCardSource = turnManager.CurrentTurn;
                }
                else if (ClueData.Instance.HasPlayerCard(guess.Weapon, turnManager.CurrentTurn - 1))
                {
                    control = false;
                    targetState = TurnState.ShowingCards;
                    cardToShow = guess.Weapon;
                    showedCardSource = turnManager.CurrentTurn;
                }
                else if (ClueData.Instance.HasPlayerCard(guess.Room, turnManager.CurrentTurn - 1))
                {
                    control = false;
                    targetState = TurnState.ShowingCards;
                    cardToShow = guess.Room;
                    showedCardSource = turnManager.CurrentTurn;
                }
            }
        }

        turnManager.PopStack();

        if(targetState.Equals(TurnState.ShowingCards))
        {
            ClueData.Instance.ShowPlayerCard(cardToShow, turnManager.CurrentTurn - 1);
            if(IsCurrentTurnHuman() || showedCardSource == 1)
            {
                cardShow.Set(CharacterResourceManager.CardImageTexture(cardToShow), showedCardSource);
            }
            else
            {
                cardShow.Set(cardBack, showedCardSource);
            }
            guessDisplay.gameObject.SetActive(false);
            humanCardsDisplay.ShowAll();
            cardShow.gameObject.SetActive(true);
            turnState = TurnState.ShowingCards;
        }
        else if(targetState.Equals(TurnState.AccusationOption))
        {
            guessDisplay.gameObject.SetActive(false);
            humanCardsDisplay.ShowAll();
            isSelected = false;
            aiTimer = 0;
            makeAccusation = false;
            accusationOption.Set(guess);
            accusationOption.gameObject.SetActive(true);
            turnState = TurnState.AccusationOption;
        }

    }

    public void ShowingCards()
    {
        if(Input.GetKeyUp(KeyCode.Space))
        {
            cardShow.gameObject.SetActive(false);
            turnState = TurnState.PostTurn;
        }
    }

    public void SelectAccusationOption(bool option)
    {
        if(!isSelected)
        {
            isSelected = true;
            makeAccusation = option;
        }
    }

    public void AccusationOption()
    {
        if(isSelected)
        {
            accusationOption.gameObject.SetActive(false);
            if(makeAccusation)
            {
                guessDisplay.Set(guess, turnManager.CurrentTurn, true);
                guessDisplay.gameObject.SetActive(true);
                turnState = TurnState.MakingAccusation;
            }
            else
            {
                turnState = TurnState.PostTurn;
            }
        }
        else if(!IsCurrentTurnHuman())
        {
            aiTimer += Time.deltaTime;
            if(aiTimer > 1)
            {
                aiTimer = 0;
                SelectAccusationOption(UnityEngine.Random.Range(1, 100) < 75);
            }
        }
    }

    public void MakingAccusation()
    {

    }

    public void PostTurn()
    {
        turnManager.NextTurn();
        turnState = TurnState.PreTurn;
    }

    public bool IsCurrentTurnHuman()
    {
        return turnManager.CurrentTurn == 1;
    }

}
