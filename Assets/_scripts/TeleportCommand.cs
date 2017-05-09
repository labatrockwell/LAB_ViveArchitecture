using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportCommand : Command {

	public GameObject teleportee;
	public GameObject originObject;
	private Color color = new Color(0f,164f,255f,0.5f);

    private float prevTouchPos;

	private GameObject line;
	private LineRenderer lineRenderer;
	private GameObject target;

	public int numberOfPoints = 100;
	public int scale = 1;
	public float velocityMagnitude = 0.3f;
	public float accelerationMagnitude = 0.03f;
	private List<Vector3> trajectoryPoints = new List<Vector3>();

    private SteamVR_TrackedObject trackedObject;
    private SteamVR_Controller.Device device { get { return SteamVR_Controller.Input((int)trackedObject.index); } }

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
		target.transform.localScale = new Vector3 (0.125f, 0.125f, 0.125f);
		target.GetComponent<Renderer>().material.color = color;

        trackedObject = originObject.GetComponent<SteamVR_TrackedObject>();

        line.GetComponent<LineRenderer>().enabled = false;
        target.GetComponent<MeshRenderer>().enabled = false;

        prevTouchPos = 0.0f;

    }
	
	// Update is called once per frame
	void Update () {

        if (commandActive && !paused)
        {
            // JUMP!

            line.GetComponent<LineRenderer>().enabled = true;
            target.GetComponent<MeshRenderer>().enabled = true;

            //if right trigger is pulled
            if (device.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
            {
                teleportee.transform.position = target.transform.position;
                //commandActive = false;
                line.GetComponent<LineRenderer>().enabled = false;
                target.GetComponent<MeshRenderer>().enabled = false;
                //recenter VR
                UnityEngine.VR.InputTracking.Recenter();
            }

            if (device.GetTouch(SteamVR_Controller.ButtonMask.Touchpad))
            {
                float touchPos = device.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad).y;
                float extension = (touchPos - prevTouchPos)/5.0f;
                velocityMagnitude += extension;
                prevTouchPos = touchPos;
            }

            //this should be the controller
            Vector3 origin = originObject.transform.position;
            Vector3 direction = originObject.transform.forward;

            // draw line
            line.transform.position = origin;

            trajectoryPoints.Clear();
            lineRenderer.SetVertexCount(numberOfPoints);

            int i = 0;
            bool found = false;
            Vector3 dLast = Vector3.zero;
            for (i = 0; i < numberOfPoints && !found; i++)
            {

                Vector3 d = kinematic(i * scale, origin, direction);
                trajectoryPoints.Add(d);
                lineRenderer.SetPosition(i, d);

                if (i > 0 && !found)
                {
                    Vector3 r = d - dLast;
                    RaycastHit hitInfo;
                    Ray ray = new Ray(dLast, d - dLast);
                    if (Physics.Raycast(ray, out hitInfo, (d - dLast).magnitude))
                    {
                        target.transform.position = hitInfo.point;
                        found = true;
                    }
                }

                dLast = d;
            }
            lineRenderer.SetVertexCount(i);
        }
        else {
            velocityMagnitude = 0.3f;
        }

        if (paused) {
            line.GetComponent<LineRenderer>().enabled = false;
            target.GetComponent<MeshRenderer>().enabled = false;
        }

	}

	Vector3 kinematic(float t, Vector3 origin, Vector3 velocity){
		return origin + t * velocityMagnitude * velocity + 0.5f * accelerationMagnitude * t * t * Vector3.down;
	}

}
