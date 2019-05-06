using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class TurnManager : MonoBehaviour
{

    public int numberOfPlayers = 2;
    public SequenceMode sequenceMode = SequenceMode.ByPlayerNumber;
    public CycleMode cycleMode = CycleMode.Loop;

    public enum SequenceMode
    {
        ByPlayerNumber, ByPlayerNumberReversed,
        Random, RollDice
    }

    public enum CycleMode
    {
        Loop, PingPong
    }

    private int[] turnSequence;
    public int[] TurnSequence { get { return (int[])turnSequence.Clone(); } }

    private int turnCursor;
    public int CurrentTurn { get { return turnSequence[turnCursor]; } }

    private bool goBackwards;

    void Start()
    {
        turnSequence = new int[numberOfPlayers];
        turnCursor = 0;
        goBackwards = false;
        switch (sequenceMode)
        {
            case SequenceMode.ByPlayerNumber:
                PlayerNumberSequence();
                break;
            case SequenceMode.ByPlayerNumberReversed:
                PlayerNumberReversedSequence();
                break;
            case SequenceMode.Random:
                RandomSequence();
                break;
            case SequenceMode.RollDice:
                RollDiceSequence();
                break;
        }
        PrintTurnSequence();
    }

    private void PlayerNumberSequence()
    {
        for (int i = 0; i < numberOfPlayers; i++)
        {
            turnSequence[i] = i + 1;
        }
    }

    private void PlayerNumberReversedSequence()
    {
        for (int i = 0; i < numberOfPlayers; i++)
        {
            turnSequence[i] = numberOfPlayers - i;
        }
    }

    private void RandomSequence()
    {
        PlayerNumberSequence();
        for (int i = 0; i < numberOfPlayers; i++)
        {
            int j = Random.Range(0, numberOfPlayers - 1);
            Swap(i, j);
        }
    }

    private void RollDiceSequence()
    {
        int[] diceRolls = new int[numberOfPlayers];
        for (int i = 0; i < numberOfPlayers; i++)
        {
            diceRolls[i] = Random.Range(1, 6);
        }
        for(int i = 0; i < numberOfPlayers; i++)
        {
            int player = GetMaxIndex(diceRolls);
            turnSequence[i] = player + 1;
            diceRolls[player] = -1;
        }
    }

    private int GetMaxIndex(int[] arr)
    {
        int max = arr[0];
        int maxIndex = 0;
        for (int i = 1; i < numberOfPlayers; i++)
        {
            if (arr[i] > max)
            {
                max = arr[i];
                maxIndex = i;
            }
        }
        return maxIndex;
    }

    private void Swap(int i, int j)
    {
        int temp = turnSequence[i];
        turnSequence[i] = turnSequence[j];
        turnSequence[j] = temp;
    }

    public void PrintTurnSequence()
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("[");
        for (int i = 0; i < numberOfPlayers; i++)
        {
            stringBuilder.Append(turnSequence[i]);
            if (i < numberOfPlayers - 1)
            {
                stringBuilder.Append(", ");
            }
        }
        stringBuilder.Append("]");
        print(stringBuilder.ToString());
    }

    public void NextTurn()
    {
        if (goBackwards)
        {
            turnCursor--;
        }
        else
        {
            turnCursor++;
        }
        if(turnCursor >= numberOfPlayers)
        {
            switch(cycleMode)
            {
                case CycleMode.Loop:
                    turnCursor = 0;
                    break;
                case CycleMode.PingPong:
                    turnCursor = numberOfPlayers - 2;
                    goBackwards = true;
                    break;
            }
        }
        if(turnCursor < 0)
        {
            switch(cycleMode)
            {
                case CycleMode.Loop:
                    turnCursor = numberOfPlayers - 1;
                    break;
                case CycleMode.PingPong:
                    turnCursor = 1;
                    goBackwards = false;
                    break;
            }
        }
    }

}
