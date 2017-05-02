using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dimension : MonoBehaviour {

	public GameObject dimPtA;
	public GameObject dimPtB;
	public Material dimMaterial;
	public bool vertical;
	public bool overrideDim;
	public float offset;
	public string dimension;

	private GameObject dimExtensionLineA;
	private GameObject dimExtensionLineB;
	private GameObject dimArrowA;
	private GameObject dimArrowB;
	private GameObject annotation;
	private GameObject mainCamera;
	private LineRenderer mLineRenderer;
	private LineRenderer dimExtLineRendA;
	private LineRenderer dimExtLineRendB;
	private LineRenderer dimArrowLineRendA;
	private LineRenderer dimArrowLineRendB;
	private Vector3 arrowEndPtA;
	private Vector3 arrowEndPtB;
	private Vector3 perpendicularA;
	private Vector3 perpendicularB;
	private float arrowSize;

	void Awake () {
		//find dimPtA and dimPtB
		dimPtA = gameObject.transform.FindChild("DimPtA").gameObject;
		dimPtB = gameObject.transform.FindChild("DimPtB").gameObject;
		mLineRenderer = GetComponent<LineRenderer> ();
		annotation = gameObject.transform.FindChild ("Annotation").gameObject;
		annotation.GetComponent<CanvasScaler> ().dynamicPixelsPerUnit = 5;
		arrowSize = .0375f;
		mainCamera = GameObject.Find ("Main Camera");
		createArrows ();
		createExtensionLines ();
	}
	
	// Update is called once per frame
	void Update () {		
		updateArrows(); //Draw arrows at ends of dimensions
		updateExtensions (); //Draw dimension extensions
		updateAnnotation ();
	}

	string convertToFeetInches (float _dist){
		float feet, inches;
		float conversion = _dist*(3.28084f); //conversion factor from meters to feet
		feet = Mathf.Round( conversion );
		inches = Mathf.Round ((conversion * 12) % 12);
		if (inches == 12) 
			inches = 0;
		string result = feet.ToString() + "\'-" +inches.ToString()+"\"";
		return result;
	}

	void OnApplicationQuit(){
		DestroyImmediate (dimExtensionLineA);
		DestroyImmediate (dimExtensionLineB);
	}

	void createExtensionLines(){

		if (gameObject.transform.FindChild ("DimExtensionA")) {
//			Debug.LogWarning ("Found Extension A");
			DestroyImmediate (gameObject.transform.FindChild ("DimExtensionA").gameObject, true);
		}

		if (gameObject.transform.FindChild ("DimExtensionB")) {
//			Debug.LogWarning ("Found Extension B");
			DestroyImmediate (gameObject.transform.FindChild ("DimExtensionB").gameObject, true);
		}

		dimExtensionLineA = new GameObject ("DimExtensionA");
		dimExtensionLineB = new GameObject ("DimExtensionB");

		dimExtensionLineA.transform.parent = gameObject.transform;
		dimExtensionLineB.transform.parent = gameObject.transform;

		dimExtLineRendA = dimExtensionLineA.AddComponent<LineRenderer> ();
		dimExtLineRendB = dimExtensionLineB.AddComponent<LineRenderer> ();

		dimExtLineRendA.material = dimMaterial;
		dimExtLineRendB.material = dimMaterial;

		dimExtLineRendA.startWidth = mLineRenderer.startWidth / 4;
		dimExtLineRendA.endWidth = mLineRenderer.startWidth / 4;
		dimExtLineRendB.startWidth = mLineRenderer.startWidth / 4;
		dimExtLineRendB.endWidth = mLineRenderer.startWidth / 4;

	}
		
	void createArrows(){
	
		if (gameObject.transform.FindChild ("DimArrowA")) {
//			Debug.LogWarning ("Found Arrow A");
			DestroyImmediate (gameObject.transform.FindChild ("DimArrowA").gameObject, true);
		}

		if (gameObject.transform.FindChild ("DimArrowB")) {
//			Debug.LogWarning ("Found Arrow B");
			DestroyImmediate (gameObject.transform.FindChild ("DimArrowB").gameObject, true);
		}

		dimArrowA = new GameObject ("DimArrowA");
		dimArrowB = new GameObject ("DimArrowB");
		dimArrowA.transform.parent = gameObject.transform;
		dimArrowB.transform.parent = gameObject.transform;

		dimArrowLineRendA = dimArrowA.AddComponent<LineRenderer> ();
		dimArrowLineRendB = dimArrowB.AddComponent<LineRenderer> ();
		dimArrowLineRendA.material = dimMaterial;
		dimArrowLineRendB.material = dimMaterial;
		dimArrowLineRendA.startWidth = 0;
		dimArrowLineRendA.endWidth = mLineRenderer.startWidth*3;
		dimArrowLineRendB.startWidth = 0;
		dimArrowLineRendB.endWidth = mLineRenderer.startWidth*3;

		arrowEndPtA = perpendicularA - perpendicularB;
		arrowEndPtA = -Vector3.Normalize(arrowEndPtA) * arrowSize + perpendicularA;
		arrowEndPtB = perpendicularB - perpendicularA;
		arrowEndPtB = -Vector3.Normalize(arrowEndPtB) * arrowSize + perpendicularB;
	}

	void updateExtensions(){

		Vector3 originA = dimPtA.transform.position;
		Vector3 originB = dimPtB.transform.position;
		Vector3 vert = new Vector3 (0, 1, 0);

		if (vertical) {
			vert = new Vector3 (0, 0, 1);
		}

		Vector3 edgeA = originB - originA;
		perpendicularA = Vector3.Cross (edgeA, vert);
		perpendicularA = Vector3.Normalize(perpendicularA) * offset;
		perpendicularA += originA;
		dimExtLineRendA.SetPosition (0, originA);
		dimExtLineRendA.SetPosition (1, perpendicularA);

		Vector3 edgeB = originA - originB;
		perpendicularB = Vector3.Cross (vert, edgeB);
		perpendicularB = Vector3.Normalize(perpendicularB) * offset;
		perpendicularB += originB;
		dimExtLineRendB.SetPosition (0, originB);
		dimExtLineRendB.SetPosition (1, perpendicularB);

		mLineRenderer.SetPosition (0, arrowEndPtA);
		mLineRenderer.SetPosition (1, arrowEndPtB);

	}

	void updateArrows(){		
		arrowEndPtA = perpendicularA - perpendicularB;
		arrowEndPtA = -Vector3.Normalize(arrowEndPtA) * arrowSize + perpendicularA;
		dimArrowLineRendA.SetPosition (0, perpendicularA);
		dimArrowLineRendA.SetPosition (1, arrowEndPtA);

		arrowEndPtB = perpendicularB - perpendicularA;
		arrowEndPtB = -Vector3.Normalize(arrowEndPtB) * arrowSize + perpendicularB;
		dimArrowLineRendB.SetPosition (0, perpendicularB);
		dimArrowLineRendB.SetPosition (1, arrowEndPtB);	
	}

	void updateAnnotation(){
		RectTransform annotationText = annotation.GetComponent<RectTransform> ();
		Vector3 annotationOffset = new Vector3(0f,0.0625f,0f);

		if (Camera.main.transform.position.y > annotationText.position.y) {
			annotationText.position = ((perpendicularA + perpendicularB) / 2) + annotationOffset;
		} else {
			annotationText.position = ((perpendicularA + perpendicularB) / 2) - annotationOffset;
		}

		Vector3 lookTarget = Camera.main.transform.position;
		lookTarget.y = annotation.transform.position.y;
		annotationText.LookAt (lookTarget);
		float dist = Vector3.Distance (perpendicularA, perpendicularB);

		if (!overrideDim) {
			annotation.GetComponent<Text> ().text = convertToFeetInches (dist);
		} else {
			annotation.GetComponent<Text> ().text = dimension;
		}
	}

}
