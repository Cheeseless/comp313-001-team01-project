using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public int index;
    public List<GameUnit> units;
    public bool lost = false;
    int unitNum;

    public Player()
    {
        units = new List<GameUnit>();
    }

    public void UpdateUnitCount()
    {
        for (int i = 0; i < units.Count; i++)
        {
            if (units[i] == null)
            {
                units.Remove(units[i]);
            }
        }
        if (unitNum != units.Count)
        {
            unitNum = units.Count;
            CheckLoss();
        }
    }

    void CheckLoss()
    {
        if (units.Count < 1)
        {
            lost = true;
            Debug.Log("PLayer" + index + " lost...");
            //TODO: Give some feedback indicating the player has lost
        }
        else
        {
            Debug.Log("Remaining Units: " + units.Count);
        }
    }
}
