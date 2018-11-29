using System;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;


[RequireComponent(typeof(GameUnit))]
internal class BasicAttack : Ability {

    //PublicVariables
    //Minimum Damage;
    public int minDamage;
    //Maximum Damage;
    public int maxDamage;
    //Damage Modifer;
    public int damageMod;
    //FinalDamage;s
    int damage;

    
    GameUnit defender;


    public void Select(GameUnit source) {
        defender = source.Target;
    }

    public void Refresh(GameUnit source) {
    }

    public void Execute(GameUnit source) {
        //Calculates Damage
        DamageCalc();
        //Causes the selected target to take damage.
        defender.Damage(damage);

    }
    //Calculation of Damage
    void DamageCalc() {
        //Damage is done via a min and max random function, with the damage mod being a stagnant modifier to the total damage.
        damage = Random.Range(minDamage, maxDamage) + damageMod;
}