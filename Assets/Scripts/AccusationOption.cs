using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AccusationOption : MonoBehaviour
{
    public RawImage character;
    public RawImage weapon;
    public RawImage room;

    public void Set(Guess guess)
    {
        character.texture = CharacterResourceManager.CardImageTexture(guess.Character);
        weapon.texture = CharacterResourceManager.CardImageTexture(guess.Weapon);
        room.texture = CharacterResourceManager.CardImageTexture(guess.Room);
    }

}
