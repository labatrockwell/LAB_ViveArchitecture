using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class mouseRayCast : MonoBehaviour {

	private GameObject sphereVert;
	private GameObject sphereHit;
	private bool firstPoint;
	private bool secondPoint;

	// Use this for initialization
	void Start () {
		firstPoint = false;
		secondPoint = false;
	}
	
	// Update is called once per frame
	void Update () {

		//left click
		if (Input.GetMouseButton (0)) {

			Destroy (sphereVert);

			sphereVert = GameObject.CreatePrimitive (PrimitiveType.Sphere);
			sphereVert.transform.localScale = new Vector3 (0.125f, 0.125f, 0.125f);
			Destroy (sphereVert.GetComponent<SphereCollider> ());
			sphereVert.GetComponent<MeshRenderer> ().material.color = new Color (1.0f, 0f, 0f);

			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;

			if (Physics.Raycast (ray, out hit)) {
				if (hit.collider != null) {
					sphereVert.transform.position = getClosestVertOnMesh(hit.collider.gameObject, hit.point);
					firstPoint = true;
				}
			}

			if (firstPoint && secondPoint) {
				createAlignedDimension (sphereVert.transform.position, sphereHit.transform.position);
				createLinearDimension (sphereVert.transform.position, sphereHit.transform.position);
				reset ();
			}
		}

		//right click
		if (Input.GetMouseButtonDown (1)) {

			Destroy (sphereHit);

			sphereHit = GameObject.CreatePrimitive (PrimitiveType.Sphere);
			sphereHit.transform.localScale = new Vector3 (0.125f, 0.125f, 0.125f);
			Destroy (sphereHit.GetComponent<SphereCollider> ());
			sphereHit.GetComponent<MeshRenderer> ().material.color = new Color (0f, 0f, 1.0f);


			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;

			if (Physics.Raycast (ray, out hit)) {
				if (hit.collider != null) {
					sphereVert.transform.position = getClosestVertOnMesh(hit.collider.gameObject, hit.point);
					secondPoint = true;
				}
			}

			if (firstPoint && secondPoint) {
				createAlignedDimension (sphereVert.transform.position, sphereHit.transform.position);
				createLinearDimension (sphereVert.transform.position, sphereHit.transform.position);
				reset ();
			}
		}
	}

	GameObject createLinearDimension(Vector3 _startPos, Vector3 _endPos){

		GameObject dim = new GameObject ("userCreatedLinearDim");
		LineRenderer dimLine = dim.AddComponent<LineRenderer> ();

		return dim;

	}

	GameObject createAlignedDimension(Vector3 _startPos, Vector3 _endPos){
		GameObject dim = new GameObject("userCreatedAlignDim");
		dim.AddComponent<LineRenderer> ();

		LineRenderer dimLine = dim.GetComponent<LineRenderer> ();
		dimLine.SetVertexCount (2);
		dimLine.SetPosition (0, _startPos);
		dimLine.SetPosition (1, _endPos);
		dimLine.SetWidth (0.05f, 0.05f);
		dimLine.material = new Material(Shader.Find("Standard"));
		dimLine.material.color = Color.black;

		//dimText
		GameObject dimText = new GameObject("dimText");
		Canvas dimTextCanvas = dimText.AddComponent<Canvas> ();
		CanvasScaler dimTextCanvasScaler = dimText.AddComponent<CanvasScaler> ();
		Text mText = dimText.AddComponent<Text> ();
		Font ArialFont = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");

		dimText.transform.parent = dim.transform;
		dimText.GetComponent<RectTransform> ().localScale = new Vector3 (0.1f,0.1f,0.1f);
		dimText.GetComponent<RectTransform> ().position = Vector3.Lerp(_startPos,_endPos,0.5f);
		dimText.GetComponent<RectTransform> ().eulerAngles = new Vector3(0f,0f,Vector3.Angle(_startPos,_endPos)/2);
		dimTextCanvas.renderMode = RenderMode.WorldSpace;
		dimTextCanvasScaler.dynamicPixelsPerUnit = 10;

		mText.font = ArialFont;
		mText.material = ArialFont.material;
		mText.text = Vector3.Distance(_startPos,_endPos).ToString();
		mText.alignment = TextAnchor.MiddleCenter;
		mText.fontSize = 2;

		return dim;
	}

	void reset(){
		Destroy (sphereVert);
		Destroy (sphereHit);
		firstPoint = false;
		secondPoint = false;
	}

	Vector3 getClosestVertOnMesh(GameObject _hitObject, Vector3 _hitPoint){

				GameObject hitObject = _hitObject;
				Vector3 hitCoords = _hitPoint;
				Vector3 localHitCoords = hitObject.transform.InverseTransformPoint (hitCoords);

				Mesh mesh = hitObject.GetComponent<MeshFilter> ().mesh;
				Vector3 nearestVert = Vector3.zero;
				float closestDist = Mathf.Infinity;

				//scan all the vertices
				int index = 0;
				foreach (Vector3 vertex in mesh.vertices) {
					float tempDistance = Vector3.Distance (vertex, localHitCoords);

					if (tempDistance < closestDist) {
						closestDist = tempDistance;
						nearestVert = hitObject.transform.TransformPoint (vertex);
					}

					index++;
				}


//				return nearestVert;
				return hitCoords;

	}

}
