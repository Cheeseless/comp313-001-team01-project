using UnityEngine;

public class NewMapMenu : MonoBehaviour {

	public HexGrid hexGrid;

	public void Open () {
		gameObject.SetActive(true);
		HexMapCamera.Locked = true;
	}

	public void Close () {
		gameObject.SetActive(false);
		HexMapCamera.Locked = false;
	}

	public void CreateSmallMap () {
		CreateMap(5, 5);
	}

	public void CreateMediumMap () {
		CreateMap(10, 10);
	}

	public void CreateLargeMap () {
		CreateMap(20, 20);
	}

	void CreateMap (int x, int z) {
		hexGrid.CreateMap(x, z);
		HexMapCamera.ValidatePosition();
		Close();
	}
}