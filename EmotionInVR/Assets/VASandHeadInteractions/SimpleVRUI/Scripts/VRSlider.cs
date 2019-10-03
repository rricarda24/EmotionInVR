using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRStandardAssets.Utils;

public class VRSlider : MonoBehaviour {


	//[SerializeField] private SelectionRadial selectionRadial;

	//private SelectionRadial selectionRadial;

	//public GameObject VRInteractiveCamera;
	private Scrollbar vScale;
	private VRInteractiveItem m_InteractiveItem; 
	private SelectionRadial m_SelectionRadial;

	private Transform reticlePosition;
	public GameObject VRInteractiveCamera;
	private GameObject sliderHandle;

	public float gazeTimeForSelection;
	private float momentOnView;

	private bool lastScrollerStatus;



	// Use this for initialization
	void Start () {

		if (gazeTimeForSelection == 0) gazeTimeForSelection = 1;
		
		if (this.GetComponent<VRInteractiveItem> () != null) m_InteractiveItem = this.GetComponent<VRInteractiveItem> ();

		else {
			Debug.Log ("Attaching VR Interactive Script to this GameObject, it's required");
			this.gameObject.AddComponent (typeof(VRInteractiveItem));
			m_InteractiveItem = this.GetComponent<VRInteractiveItem> ();
		}

		if(this.GetComponent<BoxCollider>() == null) {
			this.gameObject.AddComponent (typeof(BoxCollider));
			GetComponent<BoxCollider> ().size = new Vector3(this.GetComponent<RectTransform> ().rect.width, this.GetComponent<RectTransform> ().rect.height, 1);
			Debug.Log ("Attaching Box collider to this GameObject, it's required");
		}

		if (this.GetComponent<Scrollbar> () != null) vScale = this.GetComponent<Scrollbar> ();
		else Debug.Log ("No Scrollbar component attached to this GameObject, it's required");

		if (this.GetComponent<Scrollbar>().handleRect.gameObject != null) sliderHandle =  this.GetComponent<Scrollbar>().handleRect.gameObject;
		else Debug.Log ("No child Handle (GameObject) attached to this GameObject, it's required");

		if(VRInteractiveCamera.GetComponent<SelectionRadial>() != null) m_SelectionRadial = VRInteractiveCamera.GetComponent<SelectionRadial> ();
		else Debug.Log ("No SelectionRadial Script attached to the VR Interactive Camera, it's required");

		if(VRInteractiveCamera.GetComponent<Reticle>() != null) reticlePosition = VRInteractiveCamera.GetComponent<Reticle>().ReticleTransform;
		else Debug.Log ("No Reticle Script attached to the VR Interactive Camera, it's required with it's references");

		sliderHandle.SetActive(false);
	
	}

	// Update is called once per frame
	void Update () {


		if (m_InteractiveItem.IsOver == true) {
			m_SelectionRadial.Show ();

			if (lastScrollerStatus == false)
				momentOnView = Time.realtimeSinceStartup;

			OnScrolling ();
		} 


		if (m_InteractiveItem.IsOver == false && lastScrollerStatus == true)
			m_SelectionRadial.Hide ();



		lastScrollerStatus = m_InteractiveItem.IsOver;


	}

	void OnScrolling () {

		float canvasScale = GetComponentInParent<Canvas>().transform.localScale.x;
		float scrollBarSize = GetComponent<RectTransform> ().rect.width; 
		float elapsedTime = Time.realtimeSinceStartup - momentOnView;

		float mappedPosition =(reticlePosition.transform.position.x/(scrollBarSize*canvasScale))+0.5f; //where 250 is the size in X of the scrollbar, 0.25 the scaling of the canvas and 0.5 to go between 0 and 1 instead of -0.5 to 0.5

		if (elapsedTime >= gazeTimeForSelection) 
		{
			sliderHandle.SetActive(true);
			vScale.value = mappedPosition;
			momentOnView = Time.realtimeSinceStartup; //resets to actual time so that the elapsed time goes back to 0

		}
	}
}
