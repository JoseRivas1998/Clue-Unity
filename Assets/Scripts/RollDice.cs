using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollDice : MonoBehaviour
{

    public GameObject dicePrefab;
    public Camera worldCamera;
    [Range(1,100)]
    public int numDice = 2;
    public float diceDistance = 1.0f;
    public float tossForce = 200f;
    public float minForceOffset = -100.0f;
    public float maxForceOffset = 100.0f;
    public float minTorque = -400.0f;
    public float maxTorque = 400.0f;
    

    public enum DiceRollerState
    {
        Inactive, HoldingDice, DiceRolling, Complete
    }

    private DiceRollerState currentState;
    public DiceRollerState RollerState => currentState; // nice

    private int diceValue;
    public int DiceValue => diceValue;

    private GameObject[] dice;

    // Start is called before the first frame update
    void Start()
    {
        currentState = DiceRollerState.Inactive;
        dice = new GameObject[numDice];
        ActivateRoller();
    }

    // Update is called once per frame
    void Update()
    {
        switch(currentState)
        {
            case DiceRollerState.HoldingDice:
                UpdateHoldingDice();
                break;
            case DiceRollerState.DiceRolling:
                StartCoroutine(UpdateDiceRolling());
                break;
            case DiceRollerState.Complete:
                UpdateComplete();
                break;
            case DiceRollerState.Inactive:
            default:
                // Don't do anything
                break;
        }
    }

    private void UpdateHoldingDice()
    {
        if(Input.GetKeyUp(KeyCode.Space))
        {
            foreach(var die in dice)
            {
                Rigidbody dieBody = die.GetComponent<Rigidbody>();
                Vector3 force = Vector3.Normalize(worldCamera.transform.forward) * tossForce;
                Vector3 forceOffset = Vector3.up * Random.Range(minForceOffset, maxForceOffset);
                Vector3 torque = new Vector3(
                    Random.Range(minTorque, maxTorque),
                    Random.Range(minTorque, maxTorque),
                    Random.Range(minTorque, maxTorque)
                );
                dieBody.AddForce(force + forceOffset);
                dieBody.AddTorque(torque);
                dieBody.useGravity = true;
            }
            currentState = DiceRollerState.DiceRolling;
        }
    }

    /*
     * For Following two methods:
     * https://stackoverflow.com/questions/51644503/detect-when-all-dice-have-stopped-moving
     */
    IEnumerator UpdateDiceRolling()
    {
        yield return CheckIfDiceAreMoving();

        diceValue = 0;
        foreach(var die in dice)
        {
            DiceDirection diceDrection = die.GetComponent<DiceDirection>();
            diceValue += diceDrection.getNumber();
        }

        currentState = DiceRollerState.Complete;

    }

    IEnumerator CheckIfDiceAreMoving()
    {
        foreach (var die in dice)
        {
            var dieRigidbody = die.GetComponent<Rigidbody>();
            //Wait until all dice stops moving
            while (!dieRigidbody.IsSleeping())
            {
                yield return null;
            }
        }
    }

    private void UpdateComplete()
    {
        if(Input.GetKeyUp(KeyCode.Space))
        {
            ActivateRoller();
        }
        print(diceValue);
    }

    private void CreateDice()
    {
        Transform camTransform = worldCamera.transform;
        int mid = numDice / 2;
        Vector3 middle = camTransform.position + camTransform.forward * diceDistance;
        for(int i = 0; i < numDice; i++)
        {
            int distToMid = i - mid;
            Vector3 spawn = new Vector3(middle.x, middle.y, middle.z);
            dice[i] = Instantiate(dicePrefab, spawn, camTransform.rotation);
            dice[i].GetComponent<Rigidbody>().useGravity = false;
            dice[i].transform.position += Vector3.right * (distToMid * 0.5f);
        }
    }

    private void DestroyDice()
    {
        for(int i = 0; i < numDice; i++)
        {
            Destroy(dice[i]);
        }
    }

    public void ActivateRoller()
    {
        if(currentState == DiceRollerState.Inactive)
        {
            CreateDice();
            currentState = DiceRollerState.HoldingDice;
        } 
        else if(currentState == DiceRollerState.Complete)
        {
            DestroyDice();
            currentState = DiceRollerState.Inactive;
            ActivateRoller();
        }
    }

}
