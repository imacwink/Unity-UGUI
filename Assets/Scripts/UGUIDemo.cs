using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UGUIDemo : MonoBehaviour {

	public UGUIGrid mGrid;

	// Use this for initialization
	void Start () {

		mGrid.onSetCell += HandleSetCellEvent;
		mGrid.SetCount(10);
		
	}

	void HandleSetCellEvent(UGUICell cell)
	{
		Debug.Log("HandleSetCellEvent :" + cell.mIndex);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
