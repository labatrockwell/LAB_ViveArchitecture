using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawToolPreview : MonoBehaviour {

    private LineRenderer mLine;
    private Vector2 uvOffset;

    public Material mLineMaterial;
    public GameObject mController;
    public Vector2 uvAnimRate;

	// Use this for initialization
	void Start () {
        mLine = gameObject.AddComponent<LineRenderer>();
        mLine.material = mLineMaterial;        
        mLine.SetPosition(0, mController.transform.position);
        mLine.SetPosition(1, transform.position);
        mLine.startWidth = 0.01f;
        mLine.endWidth = 0.01f;
        uvOffset = mLine.material.GetTextureOffset("_MainTex");
	}


	// Update is called once per frame
	void LateUpdate () {
        //update position of line renderer
        mLine.SetPosition(0, mController.transform.position);
        mLine.SetPosition(1, transform.position);

        //set UV scale
        float dist = Vector3.Distance(mController.transform.position, transform.position);

        mLine.material.mainTextureScale = new Vector2(dist*10, 1);

        //animate UVs of texture
        uvOffset += ( uvAnimRate * Time.deltaTime);
        mLine.material.SetTextureOffset("_MainTex", uvOffset);


    }


}
