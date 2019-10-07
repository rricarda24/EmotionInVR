using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class paintable : MonoBehaviour {

    public GameObject Brush;
    public float BrushSize = 0.1f;
    public RenderTexture RTexture;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButton(0))
        {
            var Ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(Ray, out hit))
            {
                var go = Instantiate(Brush, hit.point + Vector3.up * 0.1f, Quaternion.identity, transform);
                go.transform.localScale = Vector3.one * BrushSize;
            }
        }	
	}
    public void Save()
    {
        StartCoroutine(CoSave());
    }

    private IEnumerator CoSave()
    {
        yield return new WaitForEndOfFrame();
        Debug.Log(Application.dataPath + "/savedImage.png");

        RenderTexture.active = RTexture;

        var texture2d = new Texture2D(RTexture.width, RTexture.height);
        texture2d.ReadPixels(new Rect(0, 0, RTexture.width, RTexture.height), 0, 0);
        texture2d.Apply();

        var data = texture2d.EncodeToPNG();
        File.WriteAllBytes(Application.dataPath + "/savedImage.png", data);
    }

   
}
