#region usings

using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

#endregion
[RequireComponent(typeof(HexUnit))]
public class GameUnit : MonoBehaviour {

    [SerializeField]
    int maxHealth;
    int health;
    [SerializeField]
    int damage;
    [SerializeField]
    int movementRange;

    HexUnit hexUnit; // graphics-wise unit

    bool hasMoved;
    bool hasAttacked;
    bool busy; //indicates if unit is currently moving or attacking

    [SerializeField]
    protected int owner;

    //TODO: 
    [SerializeField]
    Text hpText;
    [SerializeField]
    Text dmgText;
    [SerializeField]
    Text moveText;

    public HexUnit HexUnit {
        get { return hexUnit; }
        set { hexUnit = value; }
    }

    public int Health {
        get { return health; }
        set {
            health = value;
            if (health > maxHealth) {
                health = maxHealth;
            }
            UpdateUI();
            if (health <= 0) {
                Die();
            }
        }
    }
    public int Damage {
        get { return damage; }
        set {
            damage = value;
            UpdateUI();
        }
    }
    public int MovementRange {
        get { return movementRange; }
        set {
            movementRange = value;
            hexUnit.movementRange = value;
            UpdateUI();
        }
    }
    public bool HasMoved {
        get { return hasMoved; }
        set { hasMoved = value; }
    }
    public bool HasAttacked {
        get { return hasAttacked; }
        set {
            hasAttacked = value;
            if (hasAttacked) {
                //design?
                hasMoved = true;
            }
        }
    }
    public int Owner {
        get { return owner; }
        set {
            owner = value;
            UpdateOwnerColor();
        }
    }

    public void AttackOrder(GameUnit other, List<HexCell> path) {
        if (busy) {
            //TODO: busy voice clip
        } else {
            StopAllCoroutines();
            StartCoroutine(AttackMove(other, path));
        }
    }

    public void MoveOrder(List<HexCell> path) {
        if (busy) {
            //TODO: busy voice clip
        } else {
            StopAllCoroutines();
            hexUnit.Travel(path);
        }
    }

    public void Die () {
        //todo: death animation and sound
        hexUnit.Die();
    }

    IEnumerator AttackMove(GameUnit other, List<HexCell> path) {
        hexUnit.TravelMinusOne(path);
        yield return new WaitUntil(() => hexUnit.traveled);
        Attack(other);
        busy = false;
    }

    void Attack(GameUnit other) {
        other.Health -= Damage;
        //todo: play animation
    }

    void UpdateUI() {
        hpText.text = "HP: " + Health;
        dmgText.text = "DMG: " + Damage;
        moveText.text = "MA: " + MovementRange;
    }

    void Awake() {
        HexUnit = GetComponent<HexUnit>();
        health = 999;
    }

    void Start() {
        hexUnit.movementRange = MovementRange;
        health = maxHealth;
        UpdateUI();
    }

    public void UpdateOwnerColor() {
        switch (owner) {
            case 0:
                GetComponentInChildren<Renderer>().material.color = Color.blue;
                break;
            case 1:
                GetComponentInChildren<Renderer>().material.color = Color.red;
                break;
            case 2:
                GetComponentInChildren<Renderer>().material.color = Color.green;
                break;
            case 3:
                GetComponentInChildren<Renderer>().material.color = Color.black;
                break;
        }
    }

    public void Save(BinaryWriter writer) {
        writer.Write(owner);
        writer.Write(maxHealth);
        writer.Write(damage);
        writer.Write(movementRange);
    }
    public void Load(BinaryReader reader) {
        Owner = reader.ReadInt32();
        maxHealth = reader.ReadInt32();
        Debug.Log(maxHealth);
        Health = maxHealth;
        Damage = reader.ReadInt32();
        MovementRange = reader.ReadInt32();
    }

}