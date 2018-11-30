﻿#region usings

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#endregion

public class GameController : MonoBehaviour {

    HexCell currentCell;
    public HexGrid grid;

    [SerializeField]
    int numberOfPlayers = 2;
    List<Player> players;
    Player currentPlayer;

    GameUnit selectedUnit;

    int turn;

    [SerializeField]
    Text playerText;
    [SerializeField]
    Text turnText;

    public GameObject leftPanel;
    public GameObject rightPanel;
    public GameObject editCheckbox;

    public bool editMode;

    public void EndTurn() {
        Debug.Log("Pies");
        turn++;
        Debug.Log(turn);
        turnText.text = "Turn: " + turn;
        SetNextPlayer();
        playerText.text = "Player: " + (players.IndexOf(currentPlayer)+1);
        grid.Units.ForEach((x) => x.GameUnit.Refresh());
    }

    void SetNextPlayer() {
        if (players.IndexOf(currentPlayer) == players.Count-1) {
            currentPlayer = players[0];
        } else {
            currentPlayer = players[players.IndexOf(currentPlayer) + 1];
        }
    }

    public Player GetPlayer(int index) {
        Debug.Log(players[index]);
        return players[index];
    }

    public int GetPlayerIndex(Player player) {
        return players.IndexOf(player);
    }

    void Start() {
        players = new List<Player>();
        for (var i = 0; i < numberOfPlayers; i++) players.Add(new Player());
        Debug.Log(players.Count);
        turn = 1;
        currentPlayer = players[0];
        turnText.text = "Turn: " + turn;
        playerText.text = "Player: " + + (players.IndexOf(currentPlayer)+1);
        editMode = false;
    }

    public void SetEditMode(bool toggle) {
        enabled = !toggle;
        grid.ShowUI(!toggle);
        grid.ClearPath();
    }

    public void ToggleEditMode() {
        if (editMode) {
            editMode = false;
            leftPanel.SetActive(false);
            rightPanel.SetActive(false);
            editCheckbox.GetComponent<Toggle>().isOn = true;

        }
        else {
            editMode = true;
            leftPanel.SetActive(true);
            rightPanel.SetActive(true);
            editCheckbox.GetComponent<Toggle>().isOn = true;
        }
    }

    void Update() {
        if (!EventSystem.current.IsPointerOverGameObject()) {
            if (Input.GetMouseButtonDown(0)) {
                DoSelection();
            }

            if (selectedUnit) {
                Debug.Log(selectedUnit.Owner);
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
                selectedUnit.AttackOrder(currentCell.Unit.GameUnit, path);
            }
            grid.ClearPath();
        }
    }

    void DoSelection() {
        grid.ClearPath();
        UpdateCurrentCell();
        if (currentCell) {
            selectedUnit = currentCell.Unit.GameUnit;
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