using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DrawingGrid : MonoBehaviour {

	public List<GameObject> gridPoints;

	public int rows;
	public int cols;
	public int jorbs; //nick made up this name

	public float spacing;
	private float lastSpacing;

	private float size = .005f;

	// Use this for initialization
	void Start () {
		//create a grid using a bunch of prefabs
		for(int i=0; i<rows; i++){
			for (int j = 0; j < cols; j++) {
				for (int k = 0; k < jorbs; k++) {
					GameObject gridPt = GameObject.CreatePrimitive (PrimitiveType.Cube);
					gridPt.GetComponent<Renderer> ().receiveShadows = false;
					gridPt.GetComponent<Renderer> ().useLightProbes = false;
					gridPt.GetComponent<Renderer> ().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
					Destroy ( gridPt.GetComponent<BoxCollider> ());
					gridPt.transform.parent = transform;
					gridPt.transform.localScale = new Vector3 (size, size, size);
					gridPt.transform.position = new Vector3 (i * spacing, k*spacing, j * spacing);
					gridPoints.Add (gridPt);
				}
			}
		}

		//move grid to be centered on origin
		transform.position = new Vector3((rows-1)*spacing/-2, 0.0f, (cols-1)*spacing/-2);
	
	}
	
	// Update is called once per frame
	void Update () {
		
		if (spacing != lastSpacing) {
			float difference = lastSpacing - spacing;
			transform.localScale += new Vector3 (difference, difference, difference);
			foreach (GameObject point in gridPoints) {
				point.transform.localScale = new Vector3 (size, size, size);
			}
		}

		lastSpacing = spacing;
	}
}
