using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardShow : MonoBehaviour
{
    public RawImage cardImage;
    public Text text;

    public void Set(Texture2D cardTexture, int player)
    {
        text.text = "Player " + player + " Showed a Card";
        cardImage.texture = cardTexture;
    }

}
