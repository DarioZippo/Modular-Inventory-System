using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    An abstract class to associate a logic to be triggered to the effect of a modifier.
    A child class should then be created, the corresponding Asset created and connected to the modifier, in which the desired value can be set
*/
public abstract class CharacterStatModifierSO : ScriptableObject
{
    public abstract void AffectCharacter(GameObject character, float val);
}
