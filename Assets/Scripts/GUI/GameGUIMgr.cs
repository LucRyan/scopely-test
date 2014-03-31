using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameGUIMgr : MonoBehaviour {
	
	static Stack<GameObject> _heartStack;
	static GameObject _weaponGUI;

	bool _isInitialized = false;
	
	//For check Heart Status
	static bool _isWholeExist = true;
	static bool _isHalfExist = true;
	
	//Remeber weapon status
	static string _currentWeapon;
	public static string CurrentWeapon
	{
		get{return _currentWeapon;}
	}
	
	#region Unity LifeCycle
	// Use this for initialization
	void Awake () {
		_heartStack = new Stack<GameObject>();
		InitialHearts();
		initalWeaponGUI();
		_isInitialized = true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	#endregion
	
	#region Heart Manager
	void InitialHearts()
	{
		for(int i = 1; i <= 10; i++)
		{		
			_heartStack.Push(GameObject.Find("Heart" + i));
		}
		DebugUtils.Assert(_heartStack.Count == 10, _heartStack.Count.ToString());
	}
	
	public static void HeartHit()
	{
		GameObject heart;
		
		if(_heartStack.Count !=0)
		{
			heart = _heartStack.Peek();
		}else
		{
			heart = null;
		}
		
		//Whole Disappear
		if(heart != null && _isWholeExist)
		{
			GameObject whole = heart.transform.FindChild("Whole").gameObject;
			DisappearAnimation(whole);
			_isWholeExist = false;
			return;
		}
		
		//Half Dissappear
		if(heart != null && _isHalfExist && !_isWholeExist)
		{
			GameObject half = heart.transform.FindChild("Half").gameObject;
			DisappearAnimation(half);
			_isHalfExist = false;
			_heartStack.Pop();
			
			//Reset Flags
			_isWholeExist = true;
			_isHalfExist = true;
			return;
		}
	}
	
	public static void RestoreHeart()
	{
		for(int i = 1; i <= 10; i++)
		{		
			if(!_heartStack.Contains(GameObject.Find("Heart" + i)))
			{
				if(_isHalfExist && !_isWholeExist)
				{
					AppearAnimation(_heartStack.Peek().transform.FindChild("Whole").gameObject);
				}
				GameObject heart = GameObject.Find("Heart" + i);
				DebugUtils.Assert(heart != null, "RestoreHeart: heart = null");
				AppearAnimation(heart.transform.FindChild("Whole").gameObject);
				AppearAnimation(heart.transform.FindChild("Half").gameObject);
				
				_heartStack.Push(heart);
			}
		}
		 _isWholeExist = true;
		 _isHalfExist = true;
		
	}
	
	private static void DisappearAnimation(GameObject go)
	{
		TweenScale heartTS = TweenScale.Begin<TweenScale>(go, 1f);;
		heartTS.from = Vector3.one;
		heartTS.to = new Vector3(1.5f, 1.5f, 1.5f);
		heartTS.delay = 0f;
		heartTS.Play();
		
		TweenAlpha heartAL = TweenAlpha.Begin<TweenAlpha>(go, 0.5f);
		heartAL.from = 1f;
		heartAL.to = 0f;
		heartAL.delay = 1f;
		heartAL.Play();
	}
	
	private static void AppearAnimation(GameObject go)
	{
		TweenScale heartTS = TweenScale.Begin<TweenScale>(go, 0.5f);;
		heartTS.from = new Vector3(1.5f, 1.5f, 1.5f);
		heartTS.to = Vector3.one;
		heartTS.delay = 1f;
		heartTS.Play();
		
		TweenAlpha heartAL = TweenAlpha.Begin<TweenAlpha>(go, 1f);
		heartAL.from = 0f;
		heartAL.to = 1f;
		heartAL.delay = 0f;
		heartAL.Play();
	}
	
	#endregion
	
	#region WeaponGUI Manager
	void initalWeaponGUI()
	{
		_weaponGUI = GameObject.Find("WeaponBase");
		_currentWeapon = "LandMine";
		UpdateAmmo(10);
		_currentWeapon = "Grenade";
		UpdateAmmo(15);
		_currentWeapon = "RocketLauncher";
		UpdateAmmo(15);
		_currentWeapon = "AK47";
		UpdateAmmo(60);
	}
	
	public static void MoveSelectionRect(int num)
	{
		GameObject selection = _weaponGUI.transform.FindChild("Selection").gameObject;
		TweenPosition selectionTP = TweenPosition.Begin<TweenPosition>(selection, 0.3f);
//		Debug.Log(selectionTP.duration);
		
		switch(num)
		{
			case 1:
				selectionTP.from = selection.transform.localPosition;
				selectionTP.to = new Vector3(-120f,0f,0f);
				_currentWeapon = "AK47";
				break;
			case 2:
				selectionTP.from = selection.transform.localPosition;
				selectionTP.to = new Vector3(-40f,0f,0f);
				_currentWeapon = "RocketLauncher";
				break;
			case 3:
				selectionTP.from = selection.transform.localPosition;
				selectionTP.to = new Vector3(40f,0f,0f);
				_currentWeapon = "Grenade";
				break;
			case 4:
				selectionTP.from = selection.transform.localPosition;
				selectionTP.to = new Vector3(120f,0f,0f);
				_currentWeapon = "LandMine";
				break;
			default:
				selectionTP.from = selection.transform.localPosition;
				selectionTP.to = selection.transform.localPosition;
				break;
		}
		selectionTP.Play();
	}
	
	public static void UpdateAmmo(int num)
	{
		Transform curWeaponTrans = _weaponGUI.transform.FindChild(_currentWeapon);
		UILabel ammoLbl = curWeaponTrans.FindChild("Ammo").gameObject.GetComponent<UILabel>();
		if(num > 99)
		{
			ammoLbl.text = "99";
		}
		else if(num < 10 && num >= 0)
		{
			ammoLbl.text = "0" + num.ToString();
		}
		else if(num < 0)
		{
			ammoLbl.text = "00";
		}
		else
		{
			ammoLbl.text = num.ToString();
		}
		
	}
	#endregion
	
	#region Interaction Helper

	#endregion
	
	
}
