using System;
using UnityEngine;

public abstract class Ability : ScriptableObject {

    GameUnit defender;

    public abstract void Execute(GameUnit other);

}



