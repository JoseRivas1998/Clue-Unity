using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{

    public Vector3 topLeft;
    public Vector3 bottomRight;

    public int numCols = 26;
    public int numRows = 20;

    public GameObject FreddiePrefab;
    public GameObject SherlockPrefab;
    public GameObject BlackWidowPrefab;
    public GameObject EchoPrefab;
    public GameObject MurderVPrefab;
    public GameObject ScoobyPrefab;

    private float tileWidth;
    private float tileHeight;

    private readonly int[,] startingPositions = new int[4, 2] { { 9, 9 }, { 9, 11 }, { 9, 13 }, { 9, 15 } };

    // Start is called before the first frame update
    void Start()
    {
        tileWidth = (bottomRight.x - topLeft.x) / numCols;
        tileHeight = (topLeft.z - bottomRight.z) / numRows;
        for(int i = 0; i < 4; i++)
        {
            PlaceCharacter(ClueData.Instance.GetPlayer(i), i);
        }
    }

    void PlaceCharacter(CharacterResourceManager.Cards card, int player)
    {
        Vector3 spawn = RowColToBoardLocation(startingPositions[player, 0], startingPositions[player, 1]);
        GameObject gm = null;
        switch(card)
        {
            case CharacterResourceManager.Cards.Freddie:
                gm = PlaceObject(FreddiePrefab, spawn);
                break;
            case CharacterResourceManager.Cards.Sherlock:
                gm = PlaceObject(SherlockPrefab, spawn);
                break;
            case CharacterResourceManager.Cards.Blackwidow:
                gm = PlaceObject(BlackWidowPrefab, spawn);
                break;
            case CharacterResourceManager.Cards.Echo:
                gm = PlaceObject(EchoPrefab, spawn);
                break;
            case CharacterResourceManager.Cards.MurderV:
                gm = PlaceObject(MurderVPrefab, spawn);
                break;
            case CharacterResourceManager.Cards.ScoobyDoo:
                gm = PlaceObject(ScoobyPrefab, spawn);
                break;
        }
        gm.transform.Rotate(0, 180, 0);
        ClueData.Instance.SetPlayerGameObject(gm, player);
    }

    private GameObject PlaceObject(GameObject gameObject, Vector3 pos)
    {
        return Instantiate(gameObject, pos, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        //MouseCoordinatesToGridCoordinates();
    }

    public Vector3 RowColToBoardLocation(int row, int col)
    {
        return new Vector3(topLeft.x + (col * tileWidth) + (tileWidth * 0.5f), 0, (topLeft.z - (row * tileHeight) - (tileHeight * 0.5f)));
    }

    public int[] MouseCoordinatesToGridCoordinates()
    {
        print(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        return null;
    }

    private void OnDrawGizmosSelected()
    {
        float sphereRadius = 0.05f;
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawSphere(topLeft, sphereRadius);
        Gizmos.DrawLine(new Vector3(topLeft.x, 0, 100), new Vector3(topLeft.x, 0, -100));
        Gizmos.DrawLine(new Vector3(100, 0, topLeft.z), new Vector3(-100, 0, topLeft.z));
        Gizmos.color = new Color(0, 1, 0, 0.5f);
        Gizmos.DrawSphere(bottomRight, sphereRadius);
        Gizmos.DrawLine(new Vector3(bottomRight.x, 0, 100), new Vector3(bottomRight.x, 0, -100));
        Gizmos.DrawLine(new Vector3(100, 0, bottomRight.z), new Vector3(-100, 0, bottomRight.z));
    }

}
