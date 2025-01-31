using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    A CharacterStatModifierSO example where we associate a bonus/malus for Character's Health
*/
[CreateAssetMenu]
public class CharacterStatHealthModifierSO : CharacterStatModifierSO
{
    public override void AffectCharacter(GameObject character, float val)
    {
        Health health = character.GetComponent<Health>();
        if (health != null)
            health.AddHealth((int)val);
    }
}