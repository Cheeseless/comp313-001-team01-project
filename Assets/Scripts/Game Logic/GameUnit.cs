#region usings

using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

#endregion
[RequireComponent(typeof(HexUnit),typeof(Health),typeof(BasicAttack))]
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
    public Animator animator;

    
    [SerializeField]
    Health health;

    [SerializeField]
    public Ability[] abilities;

    
    

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
    public GameUnit Target { get; set; }

    public void AttackOrder(GameUnit other, List<HexCell> path) {
        if (!HasAttacked) {
            Target = other;
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
                StartCoroutine(Move(path));
            }
        }
    }

    public void Die () {
        //todo: death animation and sound
        hexUnit.Die();
    }

    IEnumerator Move(List<HexCell> path) {
        SetMoving(true);
        yield return null;
        hexUnit.Travel(path);
        yield return new WaitUntil(() => hexUnit.traveled);
        SetMoving(false);
        yield return null;
    }
    IEnumerator AttackMove(GameUnit other, List<HexCell> path) {
        SetMoving(true);
        yield return null;
        hexUnit.TravelMinusOne(path);
        yield return new WaitUntil(() => hexUnit.traveled);
        SetMoving(false);
        yield return null;
        Attack(other);
        busy = false;
    }

    void Attack(GameUnit other) {
        var attack = abilities[0];
        attack.Execute(this);
        

    }
    public void SetMoving(bool b) {
        animator.SetBool("Moving", b);
    }

    public void SetAttacking(bool b) {
        animator.SetBool("Attacking", b);
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

    

}