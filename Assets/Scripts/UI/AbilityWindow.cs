using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityWindow : MonoBehaviour {


    //public variables
    public GameObject UIWindow;
    public Text skillName;
    public Text skillDescription;
    public Text skillDamageMax;
    public Text skillDamageMin;


    //Hides the window when it is awoken
    public void Awake()
    {
        UIWindow.SetActive(false);
    }
    //Function for when the Unit is selected.
    public void unitSelected(GameUnit selected) {
        UIWindow.SetActive(false);
        //Casts the first ability as a basic attack.
        BasicAttack uiAbilities = (BasicAttack)selected.abilities[0];
        //Sets the name and description to the proper text;
        skillName.text = uiAbilities.abilityName;
        skillDescription.text = uiAbilities.abilityDesc;
        //Sets the damage values to the proper texts
        skillDamageMax.text = uiAbilities.maxDamage.ToString();
        skillDamageMin.text = uiAbilities.minDamage.ToString();
        //Finally shows the UI window.
        UIWindow.SetActive(true);
    }




}
