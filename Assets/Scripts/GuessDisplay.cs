using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuessDisplay : MonoBehaviour
{
    public RawImage character;
    public RawImage weapon;
    public RawImage room;
    public Text text;


    public void Set(Guess guess, int player, bool isAccusation = false)
    {
        text.text = "Player " + player + (isAccusation ? "'s Accusation" : "'s Guess");
        character.texture = CharacterResourceManager.CardImageTexture(guess.Character);
        weapon.texture = CharacterResourceManager.CardImageTexture(guess.Weapon);
        room.texture = CharacterResourceManager.CardImageTexture(guess.Room);
    }
}
