using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClueGameManager : MonoBehaviour
{

    public HumanPlayerLoadCards humanCardsDisplay;
    public TurnManager turnManager;
    public RollDice diceRoller;
    public CameraFollow camFollower;

    public enum TurnState
    {
        PreTurn,
        RollingDice,
        Moving,
        ChoosingGuessCharacter,
        ChoosingGuessWeapon,
        FindingCards,
        ShowingCards,
        AccusationOption,
        MakingAccusation,
        PostTurn
    }

    private TurnState turnState;

    private float aiTimer;

    // Start is called before the first frame update
    void Start()
    {
        turnState = TurnState.PreTurn;
        aiTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        switch(turnState)
        {
            case TurnState.PreTurn: PreTurn(); break;
            case TurnState.RollingDice: RollingDice(); break;
            case TurnState.Moving: Moving(); break;
            case TurnState.ChoosingGuessCharacter: ChoosingGuessCharacter(); break;
            case TurnState.ChoosingGuessWeapon: ChoosingGuessWeapon(); break;
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
        StartCoroutine(WaitUntilCameraReaches());
    }

    private IEnumerator WaitUntilCameraReaches()
    {
        print(camFollower.DistanceFromTarget());
        if (camFollower.DistanceFromTarget() > 0.01f)
        {
            yield return null;
        }
        else
        {
            aiTimer = 0;
            diceRoller.ActivateRoller();
            turnState = TurnState.RollingDice;
        }
    }

    private void RollingDice()
    {
        if(isCurrentTurnHuman())
        {
            StartCoroutine(WaitUntilPlayerRollsDice());
        }
        else
        {
            aiTimer += Time.deltaTime;
            StartCoroutine(WaitUntilAiRollsDice());
        }
    }

    private IEnumerator WaitUntilPlayerRollsDice()
    {
        if (!Input.GetKeyUp(KeyCode.Space) && diceRoller.RollerState.Equals(RollDice.DiceRollerState.HoldingDice))
        {
            yield return null;
        }
        else
        {
            if(diceRoller.RollerState.Equals(RollDice.DiceRollerState.HoldingDice)) diceRoller.TossDice();
            yield return WaitUntilDiceAreDoneRolling();
        }
    }

    private IEnumerator WaitUntilAiRollsDice()
    {
        if(aiTimer < 1)
        {
            yield return null;
        } else
        {
            if (diceRoller.RollerState.Equals(RollDice.DiceRollerState.HoldingDice)) diceRoller.TossDice();
            yield return WaitUntilDiceAreDoneRolling();
        }
    }

    private IEnumerator WaitUntilDiceAreDoneRolling()
    {
        if(diceRoller.RollerState.Equals(RollDice.DiceRollerState.DiceRolling))
        {
            yield return null;
        }
        else
        {
            aiTimer = 0;
            diceRoller.DeactivateRoller();
            turnState = TurnState.Moving;
        }
    }

    private void Moving()
    {
        turnManager.NextTurn();
        turnState = TurnState.PreTurn;
    }

    public void ChoosingGuessCharacter()
    {

    }

    public void ChoosingGuessWeapon()
    {

    }

    public void FindingCards()
    {

    }

    public void ShowingCards()
    {

    }

    public void AccusationOption()
    {

    }

    public void MakingAccusation()
    {

    }

    public void PostTurn()
    {

    }

    private bool isCurrentTurnHuman()
    {
        return turnManager.CurrentTurn == 1;
    }

}
