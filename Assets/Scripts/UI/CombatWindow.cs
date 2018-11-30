using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatWindow : MonoBehaviour {

    public GameObject combatUI;
    public Text skillName;
    public Text skillDescription;
    public Text skillDamage;
    public Text hitTotal;
    public Text hitDetailsTxt;
    public Text critTotal;
    public Text critDetailsTxt;
    public GameObject hitDetails;
    public GameObject critDetails;
    

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void showHitDetails()
    {
        hitDetails.SetActive(true);
    }

    public void showCritDetails()
    {
        critDetails.SetActive(true);
    }

    public void closeHitDetails()
    {
        hitDetails.SetActive(false);
    }

    public void closeCritDetails()
    {
        critDetails.SetActive(false);
    }
}
