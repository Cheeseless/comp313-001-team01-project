using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

    private Slider slider;

    // Use this for initialization
    void Start () {
        slider = gameObject.GetComponentInChildren<Slider>();
        this.Damage(200, 400);
        Debug.Log(slider.value);
        this.Heal(100, 400);
        Debug.Log(slider.value);
    }

    public void FullHeal()
    {
        slider.value = 100;

    }

    public void Heal(float amount, float maxHealth)
    {
        float heal;
        heal = (100 * amount) / maxHealth;
        slider.value = slider.value + heal;

    }

    public void Damage(float amount, float maxHealth)
    {
        float damage;
        damage = (100 * amount) / maxHealth;
        slider.value = slider.value - damage;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
