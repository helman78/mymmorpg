using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Managers;

public class UIMinimap : MonoBehaviour {
	public Collider miniMapBoudingBox;
	public Image miniMap;
	public Image arrow;
	public Text mapName;

	private Transform playerTransform;

	// Use this for initialization
	void Start () {
		UpdateMinimap();
	}
	
	// Update is called once per frame
	void Update () {
		if(this.playerTransform == null && User.Instance.CurrentCharacterObject != null)
        {
			this.playerTransform = User.Instance.CurrentCharacterObject.transform;
        }
		if (miniMapBoudingBox == null || playerTransform == null) return;
		float realWidth = miniMapBoudingBox.bounds.size.x;
		float realHeight = miniMapBoudingBox.bounds.size.z;

		float realX = playerTransform.position.x - miniMapBoudingBox.bounds.min.x;
		float realY = playerTransform.position.z - miniMapBoudingBox.bounds.min.z;

		float pivotX = realX / realWidth;
		float pivotY = realY / realHeight;

		this.miniMap.rectTransform.pivot = new Vector2(pivotX, pivotY);
		this.miniMap.rectTransform.localPosition = Vector2.zero;
		this.arrow.transform.eulerAngles = new Vector3(0, 0, -playerTransform.eulerAngles.y);
	}
	public void UpdateMinimap()
	{
		this.mapName.text = User.Instance.CurrentMapData.Name;
		this.miniMap.overrideSprite = MinimapManager.Instance.LoadCurMiniMap();
		
		this.miniMap.SetNativeSize();
		this.miniMap.transform.localPosition = Vector3.zero;
		this.miniMapBoudingBox = MinimapManager.Instance.MinimapBoundingBox;
		this.playerTransform = null;
		//this.playerTransform = User.Instance.CurrentCharacterObject.transform;
	}
}
