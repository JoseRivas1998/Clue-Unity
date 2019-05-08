using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HumanPlayerLoadCards : MonoBehaviour
{

    public GameObject[] rawImages;

    // Start is called before the first frame update
    void Start()
    {
        List<CharacterResourceManager.Cards> playerCards = ClueData.Instance.GetPlayerCards(0);
        for(int i = 0; i < rawImages.Length; i++)
        {
            Texture2D texture = CharacterResourceManager.CardImageTexture(playerCards[i]);
            rawImages[i].GetComponent<RawImage>().texture = texture;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HideAll()
    {
        for(int i = 0; i < rawImages.Length; i++)
        {
            rawImages[i].SetActive(false);
        }
    }

    public void ShowAll()
    {
        for(int i = 0; i < rawImages.Length; i++)
        {
            rawImages[i].SetActive(true);
        }
    }
}
