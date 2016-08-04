using UnityEngine;
using System.Collections;

public class PlaneRenderer : MonoBehaviour {
	public Material mMat;
	private Mesh mMesh;
	public Shader mShader;

	// Use this for initialization
	public Vector3  mCorner;

	void Start () {
		mMesh = GetComponent<MeshFilter>().mesh;
		mMat = new Material (Shader.Find("Standard"));
		mMat.color = new Color (.2f, .5f, .4f);
	 	GetComponent<MeshRenderer>().material = mMat;


	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public void createQuad(Vector3 _nextCorner){
			Debug.Log ("TRYING TO CREATE QUAD");
			Debug.Log (mCorner);
			Debug.Log (_nextCorner);

			Vector3 normal = Vector3.Cross(mCorner, _nextCorner);
			Vector3 l = Vector3.Cross(normal, _nextCorner - mCorner);

			l.Normalize();
			Vector3[] quad = new Vector3[4];

			quad[0] = transform.InverseTransformPoint(mCorner + l * 1f);
			quad[1] = transform.InverseTransformPoint(mCorner + l * -1f);
			quad[2] = transform.InverseTransformPoint(mCorner + l * 1f);
			quad[3] = transform.InverseTransformPoint(mCorner + l * -1f);

			int vl = mMesh.vertices.Length;

			Vector3[] vs = mMesh.vertices;
			vs = resizeVertices(vs, 2 * quad.Length);

			for(int i = 0; i < 2*quad.Length; i += 2) {
				vs[vl + i] = quad[i / 2];
				vs[vl + i + 1] = quad[i / 2];
			}

			Vector2[] uvs = mMesh.uv;
			uvs = resizeUVs(uvs, 2 * quad.Length);
			uvs[vl] = Vector2.zero;
			uvs[vl + 1] = Vector2.zero;
			uvs[vl + 2] = Vector2.right;
			uvs[vl + 3] = Vector2.right;
			uvs[vl + 4] = Vector2.up;
			uvs[vl + 5] = Vector2.up;
			uvs[vl + 6] = Vector2.one;
			uvs[vl + 7] = Vector2.one;

			int tl = mMesh.triangles.Length;

			int[] ts = mMesh.triangles;
			ts = resizeTriangles(ts, 12);


			// front-facing quad
			ts[tl] = vl;
			ts[tl + 1] = vl + 2;
			ts[tl + 2] = vl + 4;

			ts[tl + 3] = vl + 2;
			ts[tl + 4] = vl + 6;
			ts[tl + 5] = vl + 4;

			// back-facing quad
			ts[tl + 6] = vl + 5;
			ts[tl + 7] = vl + 3;
			ts[tl + 8] = vl + 1;

			ts[tl + 9] = vl + 5;
			ts[tl + 10] = vl + 7;
			ts[tl + 11] = vl + 3;

			mMesh.vertices = vs;
			mMesh.uv = uvs;
			mMesh.triangles = ts;
			mMesh.RecalculateBounds();
			mMesh.RecalculateNormals();
			Debug.Log (mMesh.bounds);

//			gameObject.GetComponent<MeshCollider> ().sharedMesh = null;
//			gameObject.GetComponent<MeshCollider> ().sharedMesh = mMesh;
	}

	Vector3[] resizeVertices(Vector3[] ovs, int ns) {
		Vector3[] nvs = new Vector3[ovs.Length + ns];
		for(int i = 0; i < ovs.Length; i++) {
			nvs[i] = ovs[i];
		}

		return nvs;
	}

	Vector2[] resizeUVs(Vector2[] uvs, int ns) {
		Vector2[] nvs = new Vector2[uvs.Length + ns];
		for(int i = 0; i < uvs.Length; i++) {
			nvs[i] = uvs[i];
		}

		return nvs;
	}

	int[] resizeTriangles(int[] ovs, int ns) {
		int[] nvs = new int[ovs.Length + ns];
		for(int i = 0; i < ovs.Length; i++) {
			nvs[i] = ovs[i];
		}

		return nvs;
	}
}
