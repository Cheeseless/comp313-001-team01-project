#region usings

using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

#endregion
[RequireComponent(typeof(HexUnit),typeof(Health))]
public class GameUnit : MonoBehaviour {

    [SerializeField]
    int attackPower;
    [SerializeField]
    int movementRange;

    HexUnit hexUnit; // graphics-wise unit

    bool hasMoved;
    bool hasAttacked;
    bool busy; //indicates if unit is currently moving or attacking

    [SerializeField]
    protected int owner;

    
    [SerializeField]
    Health health;

    public Animator animator;

    public HexUnit HexUnit {
        get { return hexUnit; }
        set { hexUnit = value; }
    }
    
    public int AttackPower {
        get { return attackPower; }
        set {
            attackPower = value;
        }
    }
    public int MovementRange {
        get { return movementRange; }
        set {
            movementRange = value;
            hexUnit.movementRange = value;
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
            Debug.Log(owner);
            UpdateOwnerColor();
        }
    }

    public void AttackOrder(GameUnit other, List<HexCell> path) {
        if (!HasAttacked) {
            HasAttacked = true;
            if (busy) {
                //TODO: busy voice clip
            }
            else {
                StopAllCoroutines();
                StartCoroutine(AttackMove(other, path));
            }
        }
    }

    public void MoveOrder(List<HexCell> path) {
        if (!HasMoved) {
            HasMoved = true;
            if (busy) {
                //TODO: busy voice clip
            }
            else {
                StopAllCoroutines();
                SetMoving(true);
                hexUnit.Travel(path);
            }
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
        other.Damage(AttackPower); //todo: proper damage calculation

        //todo: set this up with proper attack ability selection
        //todo: play animation
    }


    public void Damage(int amount) {
        health.Damage(amount);
    }

    void Awake() {
        HexUnit = GetComponent<HexUnit>();
    }

    void Start() {
        hexUnit.movementRange = MovementRange;
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
        writer.Write(attackPower);
        writer.Write(movementRange);
        health.Save(writer);
    }
    public void Load(BinaryReader reader) {
        Owner = reader.ReadInt32();
        AttackPower = reader.ReadInt32();
        MovementRange = reader.ReadInt32();
        health.Load(reader);

    }

    public void Refresh() {
        HasMoved = false;
        HasAttacked = false;
    }

    public void SetMoving(bool b) {
        animator.SetBool("Moving", b);
    }

}