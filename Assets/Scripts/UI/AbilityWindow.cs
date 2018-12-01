using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AbilityWindow : MonoBehaviour {


    //public variables
    public GameObject windowUI; //Should be set to
    public GameObject buttonUI;
    public Text skillName;
    public Text skillDescription;
    public Text skillDamageMax;
    public Text skillDamageMin;


    //Hides the window when it is awoken
    public void Awake()
    {
        windowUI.SetActive(false);
        buttonUI.SetActive(false);
    }

    public void HideAll() {
        windowUI.SetActive(false);
        buttonUI.SetActive(false);
    }
    //Function for when the Unit is selected.
    public void UnitSelected(GameUnit selected) {
        //Resets the UI when you select a Unit.
        windowUI.SetActive(false);
        //Opens the Buttons when you select a Unit
        buttonUI.SetActive(true);
        //Casts the first ability as a basic attack.
        BasicAttack uiAbilities = (BasicAttack)selected.abilities[0];
        //Sets the name and description to the proper text;
        skillName.text = uiAbilities.abilityName;
        skillDescription.text = uiAbilities.abilityDesc;
        //Sets the damage values to the proper texts
        skillDamageMax.text = uiAbilities.maxDamage.ToString();
        skillDamageMin.text = uiAbilities.minDamage.ToString();
        
        
    }
    //Function for when you press a button.
    public void ShowAbility() {
        windowUI.SetActive(true);
    }




}
