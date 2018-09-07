using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class HexUnit : MonoBehaviour {

	const float rotationSpeed = 180f;
	const float travelSpeed = 4f;

    protected bool traveled;

	public static HexUnit unitPrefab;
    [SerializeField] protected int owner;

	public HexCell Location {
		get {
			return location;
		}
		set {
			if (location) {
				location.Unit = null;
			}
			location = value;
			value.Unit = (GameUnit)this;
			transform.localPosition = value.Position;
		}
	}

	HexCell location;

	public float Orientation {
		get {
			return orientation;
		}
		set {
			orientation = value;
			transform.localRotation = Quaternion.Euler(0f, value, 0f);
		}
	}

	float orientation;
    [SerializeField] protected int movementRange;

	List<HexCell> pathToTravel;

	public void ValidateLocation () {
		transform.localPosition = location.Position;
	}

	public bool IsNotUnderwater (HexCell cell) {
		return !cell.IsUnderwater;
	}

	public void Travel (List<HexCell> path) {
		
		pathToTravel = path;
        //limit movement to allowance this turn
	    int travelAllowance = movementRange;
	    for (int i = 1; i < pathToTravel.Count; i++) {
	        travelAllowance -= pathToTravel[i].Distance;
	        if (travelAllowance <0) {
	            pathToTravel = pathToTravel.GetRange(0, i+1);
	            break;
	        }
	    }
	    Location = pathToTravel[pathToTravel.Count - 1];
		StartCoroutine(TravelPath());
	}
    public void TravelMinusOne (List<HexCell> path) {
        if (path.Count <=2) {
            traveled = true;
            //face target
            StartCoroutine(LookAt(path.Last().Position));
            return;
            
        }
        pathToTravel = path.GetRange(0, path.Count - 1);
        Location = pathToTravel[pathToTravel.Count - 1];
        StartCoroutine(TravelPath());
    }

	IEnumerator TravelPath () {
	    traveled = false;
		Vector3 a, b, c = pathToTravel[0].Position;
		transform.localPosition = c;
		yield return LookAt(pathToTravel[1].Position);

		float t = Time.deltaTime * travelSpeed;
		for (int i = 1; i < pathToTravel.Count; i++) {
			a = c;
			b = pathToTravel[i - 1].Position;
			c = (b + pathToTravel[i].Position) * 0.5f;
			for (; t < 1f; t += Time.deltaTime * travelSpeed) {
				transform.localPosition = Bezier.GetPoint(a, b, c, t);
				Vector3 d = Bezier.GetDerivative(a, b, c, t);
				d.y = 0f;
				transform.localRotation = Quaternion.LookRotation(d);
				yield return null;
			}
			t -= 1f;
		}

		a = c;
		b = pathToTravel[pathToTravel.Count - 1].Position;
		c = b;
		for (; t < 1f; t += Time.deltaTime * travelSpeed) {
			transform.localPosition = Bezier.GetPoint(a, b, c, t);
			Vector3 d = Bezier.GetDerivative(a, b, c, t);
			d.y = 0f;
			transform.localRotation = Quaternion.LookRotation(d);
			yield return null;
		}

		transform.localPosition = location.Position;
		orientation = transform.localRotation.eulerAngles.y;
		ListPool<HexCell>.Add(pathToTravel);
		pathToTravel = null;
	    traveled = true;
	}

	IEnumerator LookAt (Vector3 point) {
		point.y = transform.localPosition.y;
		Quaternion fromRotation = transform.localRotation;
		Quaternion toRotation =
			Quaternion.LookRotation(point - transform.localPosition);
		float speed = rotationSpeed / Quaternion.Angle(fromRotation, toRotation);

		for (
			float t = Time.deltaTime * speed;
			t < 1f;
			t += Time.deltaTime * speed
		) {
			transform.localRotation =
				Quaternion.Slerp(fromRotation, toRotation, t);
			yield return null;
		}

		transform.LookAt(point);
		orientation = transform.localRotation.eulerAngles.y;
	}

	public void Die () {
		location.Unit = null;
		Destroy(gameObject);
	}

	public void Save (BinaryWriter writer) {
		location.coordinates.Save(writer);
		writer.Write(orientation);
        writer.Write(owner);
	}

	public static void Load (BinaryReader reader, HexGrid grid) {
		HexCoordinates coordinates = HexCoordinates.Load(reader);
		float orientation = reader.ReadSingle();
	    int owner = reader.ReadInt32();
	    HexUnit unit = Instantiate(unitPrefab);
	    unit.owner = owner;
		grid.AddUnit(
		    unit, grid.GetCell(coordinates), orientation
		);
	}

	void OnEnable () {
		if (location) {
			transform.localPosition = location.Position;
		}
	}

//	void OnDrawGizmos () {
//		if (pathToTravel == null || pathToTravel.Count == 0) {
//			return;
//		}
//
//		Vector3 a, b, c = pathToTravel[0].Position;
//
//		for (int i = 1; i < pathToTravel.Count; i++) {
//			a = c;
//			b = pathToTravel[i - 1].Position;
//			c = (b + pathToTravel[i].Position) * 0.5f;
//			for (float t = 0f; t < 1f; t += 0.1f) {
//				Gizmos.DrawSphere(Bezier.GetPoint(a, b, c, t), 2f);
//			}
//		}
//
//		a = c;
//		b = pathToTravel[pathToTravel.Count - 1].Position;
//		c = b;
//		for (float t = 0f; t < 1f; t += 0.1f) {
//			Gizmos.DrawSphere(Bezier.GetPoint(a, b, c, t), 2f);
//		}
//	}
}