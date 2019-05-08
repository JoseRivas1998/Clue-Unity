using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guess
{

    public CharacterResourceManager.Cards Character { get; }
    public CharacterResourceManager.Cards Weapon { get; }
    public CharacterResourceManager.Cards Room { get; }

    public Guess(CharacterResourceManager.Cards character, CharacterResourceManager.Cards weapon, CharacterResourceManager.Cards room)
    {
        this.Character = character;
        this.Weapon = weapon;
        this.Room = room;
    }

    public override bool Equals(object obj)
    {
        bool result;
        if ((obj == null) || !this.GetType().Equals(obj.GetType()))
        {
            result = false;
        }
        else
        {
            Guess g = (Guess)obj;
            result = this.Character.Equals(g.Character) && this.Weapon.Equals(g.Weapon) && this.Room.Equals(g.Room);
        }
        return result;
    }

    public override string ToString()
    {
        return "{" + Character + ", " + Weapon + ", " + Room + "}";
    }

}
