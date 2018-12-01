#region usings

using System.IO;
using UnityEngine;

#endregion

[RequireComponent(typeof(GameUnit))]
public class Health : MonoBehaviour {

    [SerializeField]
    int maxHealth;

    HealthBar healthBar;
    int currentHealth;
    GameUnit gameUnit;

    public int MaxHealth {
        get { return maxHealth; }
        set {
            if (value > 0) {
                maxHealth = value;
                UpdateUI();
            } else {
                Debug.Log("Max Health must be greater than zero");
            }
        }
    }
    public int CurrentHealth {
        get { return currentHealth; }
        set {
            currentHealth = value > maxHealth ? maxHealth : value; 
            UpdateUI();
        }
    }
    void Awake() {
        gameUnit = GetComponent<GameUnit>();
        healthBar = GetComponentInChildren<HealthBar>();
        Debug.Log(healthBar);
        healthBar.SetMaxValue(maxHealth);
        CurrentHealth = maxHealth;
        UpdateUI();
    }
    public void Damage(int amount) {
        CurrentHealth -= amount;
        if (currentHealth <= 0) {
            gameUnit.Die();
        }
    }
    public void Heal(int amount) {
        CurrentHealth += amount;
        if (CurrentHealth <= 0) {
            gameUnit.Die();
        }
    }
    void UpdateUI() {
        healthBar.MaxValue = maxHealth;
        healthBar.Value = currentHealth;
    }

    public void Save(BinaryWriter writer){
        writer.Write(maxHealth);
        writer.Write(currentHealth);
    }
    public void Load(BinaryReader reader){
        MaxHealth = reader.ReadInt32();
        CurrentHealth = reader.ReadInt32();
    }

}