using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SolutionDisplay : MonoBehaviour
{

    public Text solutionText;

    public CardReveal character;
    public CardReveal weapon;
    public CardReveal room;

    public GameObject spaceText;

    private int currentActive;

    // Start is called before the first frame update
    void Start()
    {
        character.target = CharacterResourceManager.CardImageTexture(ClueData.Instance.Solution.Character);
        weapon.target = CharacterResourceManager.CardImageTexture(ClueData.Instance.Solution.Weapon);
        room.target = CharacterResourceManager.CardImageTexture(ClueData.Instance.Solution.Room);
        currentActive = 0;
        character.Activate();
    }

    // Update is called once per frame
    void Update()
    {
        if(currentActive == 0 && character.IsDone())
        {
            currentActive++;
            weapon.Activate();
        }

        if(currentActive == 1 && weapon.IsDone())
        {
            currentActive++;
            room.Activate();
        }

        if(currentActive == 2 && room.IsDone())
        {
            currentActive++;
            spaceText.SetActive(true);
            solutionText.text = "Player " + ClueData.Instance.PlayerAccusation + " Was " + (ClueData.Instance.Solution.Equals(ClueData.Instance.Guess) ? "Correct!" : "Incorrect!");
        }

        if(Input.GetKeyUp(KeyCode.Space) && room.IsDone())
        {
            SceneManager.LoadScene(0);
        }
    }
}
