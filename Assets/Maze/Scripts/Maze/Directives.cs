using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Directives : MonoBehaviour
{
    public int berriesToFind;
    public Text berriesValueText;
    public Goal goalPrefab;
    public Berries berriesPrefab;

    Goal goal;

    int foundBerries;

    List<Vector3> berryPositions;

    private void Awake()
    {
        MazeGenerator.OnMazeReady += StartDirectives;
    }
    public void Start()
    {
        SetBerryValueText();
    }

    void StartDirectives()
    {
        goal = Instantiate(goalPrefab, MazeGenerator.instance.mazeGoalPosition, Quaternion.identity) as Goal;
        goal.transform.SetParent(transform);

        berryPositions = MazeGenerator.instance.GetRandomFloorPositions(berriesToFind);

        for(int i = 0; i < berryPositions.Count; i++)
        {
            Berries berries = Instantiate(berriesPrefab, berryPositions[i], Quaternion.identity) as Berries;
            berries.transform.SetParent(transform);

        }
    }

    public void OnGoalReached()
    {
        //allows for the game to finish
        if (foundBerries == berriesToFind)
        {
            berriesValueText.text = "You win!";
        }
    }

    public void ye()
    {
        foundBerries++;
        SetBerryValueText();

        if(foundBerries == berriesToFind)
        {
            GetComponentInChildren<Goal>().OpenGoal();
        }
    }

    void SetBerryValueText()
    {
        berriesValueText.text = foundBerries.ToString() + " of " + berriesToFind.ToString();
        if(foundBerries == berriesToFind)
        {
            berriesValueText.text = "Find Laparas!";
        }
    }
}
