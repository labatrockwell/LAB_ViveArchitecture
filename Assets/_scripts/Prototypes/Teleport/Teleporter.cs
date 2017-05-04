using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour {

	public GameObject teleportee;
	public GameObject originObject;
	private Color color = new Color(0f,164f,255f,0.5f);

	private GameObject line;
	private LineRenderer lineRenderer;
	private GameObject target;

	public int numberOfPoints = 100;
	public int scale = 1;
	public float velocityMagnitude = 0.3f;
	public float accelerationMagnitude = 0.03f;
	private List<Vector3> trajectoryPoints = new List<Vector3>();

	// Use this for initialization
	void Awake () {
		
		line = new GameObject();
		line.transform.position = Vector3.zero;
		lineRenderer = line.AddComponent<LineRenderer>();
		lineRenderer.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
		lineRenderer.startWidth = 0.01f;
		lineRenderer.endWidth = 0.01f;
		lineRenderer.SetColors(color, color);
		lineRenderer.SetPosition(0, Vector3.zero);
		lineRenderer.SetPosition(1, Vector3.zero);
		lineRenderer.material.color = color;

		target = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		Destroy (target.GetComponent<SphereCollider> ());
		target.transform.position = Vector3.zero;
		target.transform.localScale = new Vector3 (0.5f, 0.5f, 0.5f);
		target.GetComponent<Renderer>().material.color = color;

	}
	
	// Update is called once per frame
	void Update () {

		// JUMP!
		if (Input.GetKeyDown (KeyCode.J)) {
			teleportee.transform.position = target.transform.position + Vector3.up;
		}

		Vector3 origin = originObject.transform.position;
		Vector3 direction = originObject.transform.forward;

		// draw line
		line.transform.position = origin;

		trajectoryPoints.Clear ();
		lineRenderer.SetVertexCount(numberOfPoints);

		int i = 0;
		bool found = false;
		Vector3 dLast = Vector3.zero;
		for (i = 0; i < numberOfPoints && !found; i++) {

			Vector3 d = kinematic (i * scale, origin, direction);
			trajectoryPoints.Add ( d );
			lineRenderer.SetPosition( i, d );

			if ( i>0 && !found) {
				Vector3 r = d - dLast;
				RaycastHit hitInfo;
				Ray ray = new Ray (dLast, d - dLast);
				if ( Physics.Raycast (ray, out hitInfo, (d - dLast).magnitude) ) {
					target.transform.position = hitInfo.point;
					found = true;
				}
			}

			dLast = d;

		}

		lineRenderer.SetVertexCount(i);

	}

	Vector3 kinematic(float t, Vector3 origin, Vector3 velocity){
		return origin + t * velocityMagnitude * velocity + 0.5f * accelerationMagnitude * t * t * Vector3.down;
	}

}
