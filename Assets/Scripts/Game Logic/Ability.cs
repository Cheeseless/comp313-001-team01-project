using System;
using UnityEngine;

public abstract class Ability : ScriptableObject {

    public string abilityName;

    public string abilityDesc;

    public abstract void Execute(GameUnit other);

}



