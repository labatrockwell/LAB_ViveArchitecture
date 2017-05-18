using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

public class DrawLineManager : MonoBehaviour {

	public GameObject mObj;
	private MeshLineRenderer currLine;
	private Vector3 prevPos = new Vector3(0,0,0);
	private int numClicks = 0;
	public bool drawingOn;
    public bool save = false, clearLines = false, recreateLines = false;
	public List<ListWrapper> mLines;


	// Use this for initialization
	void Start () {
		mLines = new List<ListWrapper>();
	}

	// Update is called once per frame
	void Update () {
		if (save) {
			Save ();
			save = false;
		}
		if (clearLines) {
			clearAllLines ();
			clearLines = false;
		}
		if (recreateLines) {
			recreateLines = false;
			loadData();
		}


		if (drawingOn) {
			if (mObj.GetComponent<DrawTool> ().isSelected) {
				if (mObj.GetComponent<DrawTool> ().clickDown) {
					GameObject go =  GameObject.Instantiate(Resources.Load("_prefabs/BlankDrawObject") as GameObject);
					go.AddComponent<MeshFilter> ();
					go.AddComponent<MeshRenderer> ();
					go.AddComponent<MeshCollider> ();
					currLine = go.AddComponent<MeshLineRenderer> ();
					go.AddComponent<Interaction> ();
					currLine.setWidth (.1f);
					numClicks = 0;
				} else if (mObj.GetComponent<DrawTool> ().clickMove) {
					currLine.AddPoint (mObj.transform.position);
					float dis = Vector3.Distance (prevPos, mObj.transform.position);
					prevPos = mObj.transform.position;
					currLine.setWidth (.2f - (dis / 10));
					numClicks++;
				}
			}
		}
			
	}

	void clearAllLines(){
		var lineObjects = new List<GameObject>(GameObject.FindGameObjectsWithTag("LineTag"));
		foreach (GameObject _go in lineObjects) {
			Destroy (_go);
		}
	}

	void recreateLine(){
		foreach (ListWrapper _line in mLines) {
			bool first = true;
			MeshLineRenderer _currLine = null;
			foreach (Vector3 _point in _line) {
				if (first) {
					GameObject go = GameObject.Instantiate (Resources.Load ("_prefabs/BlankDrawObject") as GameObject);
					go.AddComponent<MeshFilter> ();
					go.AddComponent<MeshRenderer> ();
					go.AddComponent<MeshCollider> ();
					_currLine = go.AddComponent<MeshLineRenderer> ();
					go.AddComponent<Interaction> ();
					_currLine.setWidth (.1f);
					first = false;
				} else {
					_currLine.AddPoint (_point);
					float dis = Vector3.Distance (prevPos, _point);
					prevPos = _point;
					_currLine.setWidth (.2f - (dis / 10));
				}
			}
		}
	}

	public void Save() {
		var lineObjects = new List<GameObject>(GameObject.FindGameObjectsWithTag("LineTag"));
		mLines.Clear ();
		foreach(GameObject _go in lineObjects)
		{
			mLines.Add (_go.GetComponent<MeshLineRenderer> ().mPoints);
		}
		saveData ();
	}

	void saveData(){
		BinaryFormatter binary = new BinaryFormatter ();
		SurrogateSelector surrogateSelector = new SurrogateSelector();
		Vector3SerializationSurrogate vector3SS = new Vector3SerializationSurrogate();
		surrogateSelector.AddSurrogate(typeof(Vector3),new StreamingContext(StreamingContextStates.All),vector3SS);
		binary.SurrogateSelector = surrogateSelector;

		FileStream fStream = File.Create (Application.persistentDataPath + "/saveFile.dat");
		SaveManager saver = new SaveManager ();
		saver.lines = mLines;
		binary.Serialize (fStream, saver);
		fStream.Close ();
	}

	void loadData(){
		if(File.Exists(Application.persistentDataPath + "/saveFile.dat")){
			BinaryFormatter binary = new BinaryFormatter();
			SurrogateSelector surrogateSelector = new SurrogateSelector();
			Vector3SerializationSurrogate vector3SS = new Vector3SerializationSurrogate();
			surrogateSelector.AddSurrogate(typeof(Vector3),new StreamingContext(StreamingContextStates.All),vector3SS);
			binary.SurrogateSelector = surrogateSelector;

			FileStream fStream = File.Open (Application.persistentDataPath + "/saveFile.dat", FileMode.Open);
			SaveManager saver = (SaveManager)binary.Deserialize(fStream);
			fStream.Close ();

			mLines = saver.lines;
			recreateLine ();
		}
	}
}

[System.Serializable]
class SaveManager{
	public List<ListWrapper> lines;

}

[System.Serializable]
public class ListWrapper : List<Vector3>{ }




public class Vector3SerializationSurrogate : ISerializationSurrogate
{

	// Method called to serialize a Vector3 object
	public void GetObjectData(System.Object obj,SerializationInfo info, StreamingContext context)
	{

		Vector3 v3 = (Vector3)obj;
		info.AddValue("x", v3.x);
		info.AddValue("y", v3.y);
		info.AddValue("z", v3.z);
	}

	// Method called to deserialize a Vector3 object
	public System.Object SetObjectData(System.Object obj,SerializationInfo info,
		StreamingContext context,ISurrogateSelector selector)
	{

		Vector3 v3 = (Vector3)obj;
		v3.x = (float)info.GetValue("x", typeof(float));
		v3.y = (float)info.GetValue("y", typeof(float));
		v3.z = (float)info.GetValue("z", typeof(float));
		obj = v3;
		return obj;
	}
}
