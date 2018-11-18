﻿#region usings

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#endregion

public class GameManager : MonoBehaviour {

    HexCell currentCell;
    public HexGrid grid;

    [SerializeField]
    int numberOfPlayers;
    List<Player> players;
    Player currentPlayer;

    GameUnit selectedUnit;

    int turn;

    [SerializeField]
    Text playerText;
    [SerializeField]
    Text turnText;

    public void EndTurn() {
        turn += 1;
        turnText.text = "Turn: " + turn;
        playerText.text = "Player: " + (players.IndexOf(currentPlayer) + 1);
        SetNextPlayer();
    }

    void SetNextPlayer() {
        if (players.IndexOf(currentPlayer) == players.Count) {
            currentPlayer = players[0];
        } else {
            currentPlayer = players[players.IndexOf(currentPlayer) + 1];
        }
    }

    public Player GetPlayer(int index) {
        return players[index];
    }

    public int GetPlayerIndex(Player player) {
        return players.IndexOf(player);
    }

    void Start() {
        players = new List<Player>();
        for (int i = 0; i < numberOfPlayers; i++) players.Add(new Player());
        turn = 0;
        turnText.text = "Turn: " + (turn + 1);
        playerText.text = "Player: " + (turn % numberOfPlayers + 1);
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

            if (selectedUnit && GetPlayer(selectedUnit.Owner) == currentPlayer) {
                if (Input.GetMouseButtonDown(1)) {
                    Debug.Log("Right click");
                    HandleOrder();
                } else {
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
            } else {
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
            selectedUnit = currentCell.Unit;
        }
    }

    void DoPathfinding() {
        if (UpdateCurrentCell()) {
            if (currentCell && selectedUnit.HexUnit.IsNotUnderwater(currentCell)) {
                grid.FindPath(selectedUnit.HexUnit.Location, currentCell, selectedUnit.MovementRange);
            } else {
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