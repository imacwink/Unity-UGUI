using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UGUIGrid : MonoBehaviour 
{
	[SerializeField]private GameObject mCellPrefab = null; /*Cell预设*/

	[HideInInspector]public delegate void OnSetCell(UGUICell cell);  /*Cell填充回掉函数*/
	[HideInInspector]public OnSetCell onSetCell; 

	[HideInInspector]public delegate void OnSetCellFinish();  /*Cell填充结束回掉函数*/
	[HideInInspector]public OnSetCellFinish onSetCellFinish; 

	[HideInInspector]public delegate void OnCellClick(UGUICell cell); /*Cell 点击事件*/
	[HideInInspector]public OnCellClick onCellClick; 

	private GridLayoutGroup mGridLayoutGroup = null; /*Grid 容器*/
	private List<UGUICell> mCellList = new List<UGUICell>();  /*缓存Cell数据*/
	private int mCnt = 0;  /*需要显示元素的个数*/

	private float mCell_H = 0f; /*元素高度*/
	private float mCell_W = 0f; /*元素宽度*/
	private int mCol = 1; /*默认1*/

	void Awake () 
	{
		mGridLayoutGroup = GetComponentInChildren<GridLayoutGroup>();

		if(mGridLayoutGroup == null)
			Debug.Log("您需要在Content节点上挂载组件：GridLayoutGroup");

		mCell_W = mGridLayoutGroup.cellSize.x;
		mCell_H = mGridLayoutGroup.cellSize.x;

		if(mGridLayoutGroup.constraint == GridLayoutGroup.Constraint.FixedColumnCount)
			mCol = mGridLayoutGroup.constraintCount;
		else 
			mCol = 1; /*其它模式默认一列*/
	}

	void Start () 
	{
	}

	void OnEnable ()
	{
	}

	void OnDisable ()
	{
	}

	void OnDestroy()
	{
		onSetCell = null;
		onSetCellFinish = null;
		onCellClick = null;

		mCellPrefab = null;
		mGridLayoutGroup = null;
		for(int i = 0; i < mCellList.Count; i++)
			mCellList[i].onCellClick -= HandleCellClickEvent;
		mCellList.Clear();
		mCellList = null;
	}
		
	#region Public Fun
	public void SetCount(int count)
	{
		mCnt = count;

		// 首先清理数据;
		for(int i = 0; i < mGridLayoutGroup.transform.childCount; i++)
		{
			GameObject obj = mGridLayoutGroup.transform.GetChild(i).gameObject;
			if(obj != null)
			{
				obj.GetComponent<UGUICell>().onCellClick -= HandleCellClickEvent;
				GameObject.Destroy(obj);
			}
		}

		// 清理缓存;
		mCellList.Clear();

		// 填充数据;
		for(int i = 0; i < mCnt; i++)
		{
			UGUICell baseCell = null;
			GameObject baseCellObj = GameObject.Instantiate(mCellPrefab) as GameObject;
			if(baseCellObj != null)
			{
				baseCellObj.transform.parent = mGridLayoutGroup.transform;
				baseCellObj.transform.localPosition = Vector3.zero;
				baseCellObj.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

				baseCell = baseCellObj.GetComponent<UGUICell>();
				if(baseCell != null)
				{
					baseCell.onCellClick -= HandleCellClickEvent;
					baseCell.onCellClick += HandleCellClickEvent;
					baseCell.mIndex = i;
					mCellList.Add(baseCell);
				}
			}

			if(onSetCell != null)
				onSetCell(baseCell);
		}

		if(onSetCellFinish != null)
			onSetCellFinish();

		// 设置List可拖动范围;
		RectTransform rectTransform = mGridLayoutGroup.GetComponent<RectTransform>();
		int iRow = mCnt;
		if(mCol > 1)
		{
			iRow = mCnt / mCol;
			int iTemp = mCnt % mCol;
			if(iTemp > 0)
				iRow = iRow + 1;	
		}
		rectTransform.sizeDelta = new Vector2(0, iRow * mCell_H);
	}
	#endregion

	#region Callback Event
	private void HandleCellClickEvent(UGUICell cell)
	{
		if(onCellClick != null)
			onCellClick(cell);
	}
	#endregion
}
