using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class JengaManager : MonoBehaviour
{
    //Jenga Stuff
    public GameObject table;
    public GameObject jengaPiece;
    public Vector3 spawnPoint;
    private float pieceOffsetZ = 0.25f;
    private float pieceOffsetY = 0.15f;
    public int layers = 9;
    private int currentLayer;
    private float spawnDelay = 0.1f;
    public string jengaPieceTag = "JengaPiece";

    [HideInInspector]
    public bool pieceSelected;
    public bool isPaused;
    public bool canMove;

    public int numOfPlayers;
    private bool gameInProgress;

    public GameObject gameOverText;
    public Button resetButton;

    protected JengaManager() { }
    
    private void Start()
    {
        //settingsButton.onClick.AddListener(OpenSettings);
        table.GetComponent<TableTouching>().piecesTouching = 0;

        currentLayer = 0;
        pieceSelected = false;
        isPaused = false;
        gameInProgress = false;

        SpawnJengaPieces();


        resetButton.onClick.AddListener(() =>
        {
            currentLayer = 0;
            pieceSelected = false;
            gameInProgress = false;
            isPaused = false;
            table.GetComponent<TableTouching>().piecesTouching = 0;
            gameOverText.SetActive(false);
            Camera.main.GetComponent<FlyCamera>().enabled = true;
            resetButton.gameObject.SetActive(false);
            canMove = true;
            ResetPieces();
        });

    }

    private void Update()
    {

        if (!isPaused)
        {

            if (Input.GetKeyUp("c"))
            {
                Camera.main.GetComponent<FlyCamera>().enabled = !Camera.main.GetComponent<FlyCamera>().enabled;
            }

            if (table.GetComponent<TableTouching>().piecesTouching >= 5)
            {
                GameOver();
            }
        }
    }

    public void SpawnJengaPieces()
    {
        if (currentLayer < layers)
        {
            if (currentLayer % 2 == 0)
            {
                SpawnHorizontalLayer(currentLayer);
            }
            else
            {
                SpawnVerticalLayer(currentLayer);
            }
            currentLayer++;
            Invoke("SpawnJengaPieces", spawnDelay);
        }

        gameInProgress = true;
    }
    private void SpawnHorizontalLayer(int layer)
    {
        Vector3 center = new Vector3(spawnPoint.x, spawnPoint.y + pieceOffsetY * layer, spawnPoint.z);
        Quaternion rotation = new Quaternion();
        GameObject piece1 = Instantiate(jengaPiece, center, rotation);
        GameObject piece2 = Instantiate(jengaPiece, new Vector3(center.x, center.y, center.z + pieceOffsetZ), rotation);
        GameObject piece3 = Instantiate(jengaPiece, new Vector3(center.x, center.y, center.z - pieceOffsetZ), rotation);
        piece1.GetComponent<MovePiece>().jengaManager = this;
        piece2.GetComponent<MovePiece>().jengaManager = this;
        piece3.GetComponent<MovePiece>().jengaManager = this;
    }
    private void SpawnVerticalLayer(int layer)
    {
        Vector3 center = new Vector3(spawnPoint.x, spawnPoint.y + pieceOffsetY * layer, spawnPoint.z);
        Quaternion rotation = Quaternion.Euler(0, 90, 0);
        GameObject piece1 = Instantiate(jengaPiece, center, rotation);
        GameObject piece2 = Instantiate(jengaPiece, new Vector3(center.x + pieceOffsetZ, center.y, center.z), rotation);
        GameObject piece3 = Instantiate(jengaPiece, new Vector3(center.x - pieceOffsetZ, center.y, center.z), rotation);
        piece1.GetComponent<MovePiece>().jengaManager = this;
        piece2.GetComponent<MovePiece>().jengaManager = this;
        piece3.GetComponent<MovePiece>().jengaManager = this;
    }

    public void ResetPieces()
    {
        ClearPieces();
        table.GetComponent<TableTouching>().piecesTouching = 0;
        currentLayer = 0;
        SpawnJengaPieces();
    }
    private void ClearPieces()
    {
        GameObject[] pieces = GameObject.FindGameObjectsWithTag(jengaPieceTag);
        foreach (GameObject piece in pieces)
        {
            Destroy(piece);
        }
    }

    private void GameOver()
    {
        canMove = false;
        gameInProgress = false;
        isPaused = true;
        Camera.main.GetComponent<FlyCamera>().enabled = false;
        gameOverText.SetActive(true);
        resetButton.gameObject.SetActive(true);
    }

}
