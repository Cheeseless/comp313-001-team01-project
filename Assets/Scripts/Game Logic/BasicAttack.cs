using System;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "New Attack", menuName = "Basic Attack")]
[RequireComponent(typeof(GameUnit))]
internal class BasicAttack : Ability {

    //PublicVariables
    //Minimum Damage;
    public int minDamage;
    //Maximum Damage;
    public int maxDamage;
    //Damage Modifer;
    int damageMod;
    
    


    GameUnit defender;


    public void Select(GameUnit source) {
        defender = source.Target;
    }

    public void Refresh(GameUnit source) { }

    public void Execute(GameUnit source) {
        //Sets the damage Modifier to the attack power of hosted game unit.
        damageMod = source.AttackPower;
        //Calculates the damage Causes the selected target to take damage.
        defender.Damage(DamageCalc());

    }

    //Calculation of Damage
    int DamageCalc() {
        //Damage is done via a min and max random function, with the damage mod being a stagnant modifier to the total damage.
        var damage = Random.Range(minDamage, maxDamage) + damageMod;
        return damage;
    }

}