using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UGUICell : MonoBehaviour
{
	[HideInInspector]public int mIndex = 0; /*检索号*/

	public delegate void OnCellClick(UGUICell cell);  // 当前元素被按下;
	public OnCellClick onCellClick;

	private Button mCellButton = null; /*触发Cell点击事件*/

	void Awake () 
	{
		mCellButton = GetComponent<Button>();

		if(mCellButton == null)
			Debug.Log("您配置的Cell没有挂载Button组建，您将无法实现事件处理！");	
	}

	void Start () 
	{
	}

	void OnEnable ()
	{
		mCellButton.onClick.AddListener(HandleCellClickEvent);
	}

	void OnDisable ()
	{
		mCellButton.onClick.RemoveListener(HandleCellClickEvent);
	}

	void OnDestroy()
	{
		onCellClick = null;
		mCellButton = null;
	}

	#region UI Event;
	private void HandleCellClickEvent()
	{
		if(onCellClick != null)
			onCellClick(this);
	}
	#endregion
}
