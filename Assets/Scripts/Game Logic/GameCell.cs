using UnityEngine;

public class GameCell : MonoBehaviour {

    #region Variables and Properties

    public HexCell underlyingHexCell;

    public int Elevation {
        get { return underlyingHexCell.Elevation; }
        set {
            if (underlyingHexCell.Elevation == value) {
                return;
            }

            //using the property makes it that the refresh and update methods still run.
            underlyingHexCell.Elevation = value;
        }
    }
    public bool IsUnderwater {
        get { return underlyingHexCell.IsUnderwater; }
    }
    public bool HasRiver {
        get { return underlyingHexCell.HasRiver; }
    }
    public bool HasRoads {
        get { return underlyingHexCell.HasRoads; }
    }
    public HexCoordinates Coordinates {
        get {
            return underlyingHexCell.coordinates;
        }
    }

    #endregion

    // Use this for initialization
    void Start() {
        if (underlyingHexCell == null) {
            underlyingHexCell = gameObject.GetComponent<HexCell>();
        }
    }

    // Update is called once per frame
    void Update() { }


}