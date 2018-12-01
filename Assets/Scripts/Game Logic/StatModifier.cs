using UnityEngine;

//New fields can be added if GameUnit changes
//Could be refactored into single class on both GameUnit and GameCellProperties?
public class StatModifier: ScriptableObject
{
    public int healthBonus;
    public int damageBonus;
    public int moveBonus;
}
