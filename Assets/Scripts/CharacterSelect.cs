using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelect : MonoBehaviour
{

    private bool selected;

    private void Start()
    {
        selected = false;
    }

    public void SelectCharacter(CharacterResourceManager.Cards card)
    {
        if(CharacterResourceManager.Characters.Contains(card))
        {
            if(!selected)
            {
                selected = true;
                List<CharacterResourceManager.Cards> selectedCards = new List<CharacterResourceManager.Cards>();
                selectedCards.Add(card);
                ClueGameManager.Instance.SetPlayer(0, card);
                int numCards = CharacterResourceManager.Characters.Count;
                for(int i = 1; i < 4; i++)
                {
                    CharacterResourceManager.Cards aiCard;
                    do
                    {
                        aiCard = CharacterResourceManager.Characters[Random.Range(0, numCards - 1)];
                    } while (selectedCards.Contains(aiCard));
                    selectedCards.Add(aiCard);
                    ClueGameManager.Instance.SetPlayer(i, aiCard);
                }
                ClueGameManager.Instance.GenerateSolutionAndDistributeCards();
                SceneManager.LoadScene("DaGame");
            }
        }
        else
        {
            throw new System.Exception("Dude thats not a fucking character");
        }
    }
}
