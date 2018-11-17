using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameUnit : HexUnit {
    [SerializeField]
    int health;
    [SerializeField]
    int damage;
    
    bool hasMoved;
    bool hasAttacked;

    

    [SerializeField] Text hpText;
    [SerializeField] Text dmgText;
    [SerializeField] Text moveText;

    public int Health {
        get { return health; }
        set {
            health = value;
            UpdateUI();
            if (health<=0) {
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
    public Player Owner {
        get { return owner; }
        set {
            owner = value; 
            UpdateOwnerColor();
        }
    }

    public void AttackOrder(GameUnit other,List<HexCell> path) {
        StopAllCoroutines();
        StartCoroutine(AttackMove(other,path));
    }
    public void MoveOrder(List<HexCell> path) {
        StopAllCoroutines();
        Travel(path);
    }

    IEnumerator AttackMove(GameUnit other,List<HexCell> path) {
        
        TravelMinusOne(path);
        yield return new WaitUntil(() => traveled);
        Attack(other);

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

    void Start() {
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

}