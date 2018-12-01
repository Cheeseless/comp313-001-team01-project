using UnityEngine;

//Water can be passed only using abilities (i.e. Jump)
//Mountain cannot be passed by any units/abilities
public enum TileType { NoCost, Field, Forest, Swamp, Water = 99, Mountain };

public class GameCellProperties : ScriptableObject
{
    [SerializeField] TileType movementCost = TileType.Field;
    bool isOnFire;

    public TileType MovementCost
    {
        get
        {
            return movementCost;
        }

        set
        {
            movementCost = value;
        }
    }

    public bool IsOnFire
    {
        get
        {
            return isOnFire;
        }

        set
        {
            isOnFire = value;
            if(isOnFire)
            {
                //TODO: Add fire to tile
            }
            else 
            {
                //TODO: Remove fire from tile
            }
        }
    }

    //Stat Modifier is applied to unit's stats at the start of the turn
    [SerializeField] GameUnit containedUnit;
    [SerializeField] StatModifier statModifier;

    //Can be used for misc. objects contained within the cell
    [SerializeField] GameObject[] containedObjects;
}
