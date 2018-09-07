using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    #region Variables and Properties

    HexCell currentCell;

    public HexGrid grid;

    [SerializeField] int numberOfPlayers;

    GameUnit selectedUnit;

    int turn;

    
    [SerializeField]
    Text playerText;
    [SerializeField]
    Text turnText;

    #endregion

    public void EndTurn() {
        turn +=1;
        turnText.text = "Turn: " + Mathf.FloorToInt(turn /numberOfPlayers) +1;
        playerText.text = "Player: " + ((turn % numberOfPlayers)+1);
    }

    void Start() {
        turn = 0;
        turnText.text = "Turn: " + (turn +1);
        playerText.text = "Player: " + ((turn % numberOfPlayers)+1);
    }

    public void SetEditMode(bool toggle) {
        enabled = !toggle;
        grid.ShowUI(!toggle);
        grid.ClearPath();
    }

    void Update() {
        if (!EventSystem.current.IsPointerOverGameObject()) {
            if (Input.GetMouseButtonDown(0)) {
                DoSelection();
            }

            if (selectedUnit && selectedUnit.Owner == turn % numberOfPlayers) {
                if (Input.GetMouseButtonDown(1)) {
                    Debug.Log("Right click");
                    HandleOrder();
                }
                else {
                    DoPathfinding();
                }
            }
        }
    }

    void HandleOrder() {
        UpdateCurrentCell();
            Debug.Log("CurrentCell" + currentCell.Position);
            if (grid.HasPath) {
                if (currentCell.Unit) {
                    DoAttack();
                }
                else {
                    Debug.Log("Move Order");
                    DoMove();
                }
            }
    }

    void DoAttack() {
        if (grid.HasPath) {
            List<HexCell> path = grid.GetPath();
            if (currentCell) {
                Debug.Log("Attack Order");
                selectedUnit.AttackOrder(currentCell.Unit, path);
            }
            grid.ClearPath();
        }
    }

    void DoSelection() {
        grid.ClearPath();
        UpdateCurrentCell();
        if (currentCell) {
            selectedUnit =  currentCell.Unit;
        }
    }

    void DoPathfinding() {
        if (UpdateCurrentCell()) {
            if (currentCell && selectedUnit.IsNotUnderwater(currentCell)) {
                grid.FindPath(selectedUnit.Location, currentCell, selectedUnit.MovementRange);
            }
            else {
                grid.ClearPath();
            }
        }
    }

    void DoMove() {
        if (grid.HasPath) {
            List<HexCell> path = grid.GetPath();
            selectedUnit.MoveOrder(path);
            grid.ClearPath();
        }
    }

    bool UpdateCurrentCell() {
        HexCell cell =
            grid.GetCell(Camera.main.ScreenPointToRay(Input.mousePosition));
        if (cell != currentCell) {
            currentCell = cell;
            return true;
        }

        return false;
    }

}