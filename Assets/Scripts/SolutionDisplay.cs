using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SolutionDisplay : MonoBehaviour
{

    public Text solutionText;
    public RawImage character;
    public RawImage weapon;
    public RawImage room;

    // Start is called before the first frame update
    void Start()
    {
        character.texture = CharacterResourceManager.CardImageTexture(ClueData.Instance.Solution.Character);
        weapon.texture = CharacterResourceManager.CardImageTexture(ClueData.Instance.Solution.Weapon);
        room.texture = CharacterResourceManager.CardImageTexture(ClueData.Instance.Solution.Room);
        solutionText.text = "Player " + ClueData.Instance.PlayerAccusation + " Was " + (ClueData.Instance.Solution.Equals(ClueData.Instance.Guess) ? "Correct!" : "Incorrect!");
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Space))
        {
            SceneManager.LoadScene(0);
        }
    }
}
