﻿#region usings

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#endregion

public class GameController : MonoBehaviour {

    HexCell currentCell;
    Player currentPlayer;
    public GameObject editCheckbox;

    public bool editMode;
    public HexGrid grid;

    public GameObject leftPanel;

    [SerializeField] int numberOfPlayers = 2;
    List<Player> players;

    [SerializeField] Text playerText;
    public GameObject rightPanel;

    public AbilityWindow selectedUI;

    GameUnit selectedUnit;
    //Behaviour halo;

    //int turn;

    public void EndTurn() {
        selectedUI.Awake();
        SetNextPlayer();
        turnText.text = "Turn: " + currentPlayer.Turn;
        playerText.text = "Player: " + (players.IndexOf(currentPlayer)+1);
        int lostPlayers = 0;
        Player wonPlayer = null;
        foreach (Player player in players) {
            player.units.ForEach((unit) => unit.Refresh());
            player.UpdateUnitCount();
            if (player.Lost) {
                lostPlayers++;
            }
            else {
                wonPlayer = player;
            }
        }
        Debug.Log("Lost: " + lostPlayers);
        if (lostPlayers == players.Count - 1) {
            Debug.Log("Player " + wonPlayer.index + " has won.");
            //TODO: Send the game back to the main menu/restart the level
        }
    }

    void SetNextPlayer() {
        if (players.IndexOf(currentPlayer) == players.Count - 1) {
            currentPlayer = players[0];
        }
        else {
            currentPlayer = players[players.IndexOf(currentPlayer) + 1];
        }
    }

    public Player GetPlayer(int index) {
        //Debug.Log(players[index]);
        return players[index];
    }

    public int GetPlayerIndex(Player player) {
        return players.IndexOf(player);
    }

    void Start() {
        players = new List<Player>();
        for (int i = 0; i < numberOfPlayers; i++) {
            players.Add(new Player());
        }
        Debug.Log(players.Count);
        //turn = 1;
        currentPlayer = players[0];
        turnText.text = "Turn: " + currentPlayer.Turn;
        playerText.text = "Player: " + (players.IndexOf(currentPlayer)+1);
        editMode = false;
        for (int j = 0; j < players.Count; j++) {
            players[j].index = j;
            if (players[j].units.Count < 1) {
                var gameObjs = GameObject.FindGameObjectsWithTag("Unit1");
                foreach (GameObject unitObj in gameObjs) {
                    GameUnit unit = unitObj.GetComponent<GameUnit>();
                    Debug.Log(unit.Owner);
                    if (unit.Owner == j) {
                        players[j].units.Add(unit);
                        Debug.Log("Player " + j + " unit added.");
                    }
                }
                players[j].UpdateUnitCount();
            }
        }
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
            editCheckbox.GetComponent<Toggle>().isOn = false;
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
                //Debug.Log(selectedUnit.Owner);
            }
            if (selectedUnit && GetPlayer(selectedUnit.Owner) == currentPlayer) {
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
                if (currentCell.Unit != selectedUnit) {
                    DoAttack();
                }
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
            if (path.Count >=  2)
                if (currentCell) {
                    Debug.Log("Attack Order");
                    selectedUnit.AttackOrder(currentCell.Unit.GameUnit, path);
                }

            grid.ClearPath();
        }
    }

    void DoSelection() {
        grid.ClearPath();
        grid.Units.ForEach((HexUnit obj) => {
            if(obj.GameUnit.Selected) {
                obj.GameUnit.ToggleSelection();
            }
        });
        UpdateCurrentCell();
        if (currentCell) {
            if (currentCell.Unit) {
                selectedUnit = currentCell.Unit.GameUnit;

                if (selectedUnit.Owner == players.IndexOf(currentPlayer)) {
                    selectedUI.UnitSelected(selectedUnit);
                }
            }
            else {
                selectedUnit = null;
                selectedUI.HideAll();
            }
            selectedUnit.ToggleSelection();
        }
    }

    void DoPathfinding() {
        if (UpdateCurrentCell()) {
            if (currentCell && selectedUnit.HexUnit.IsNotUnderwater(currentCell)) {
                grid.FindPath(selectedUnit.HexUnit.Location, currentCell, selectedUnit.MovementRange);
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