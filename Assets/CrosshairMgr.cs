using UnityEngine;
using System.Collections;

public class CrosshairMgr : MonoBehaviour {
	
	private static bool _needLessAccurate = false;
	private static string _lastStatus = "more";
	private static bool _needUpdate = false;
	
	private bool _crosshairChanged = true;
	
	private GameObject _left;
	private GameObject _right;
	private GameObject _top;
	private GameObject _bottom;
	#region Unity Lifecycle
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
		if(_needUpdate)
		{
			UpdateCrosshairAccuracy();
		}
	
	}
	#endregion
	
	#region Crosshair Helper
	void UpdateCrosshairAccuracy()
	{
		if(_crosshairChanged)
		{
			_left = this.transform.FindChild(GameGUIMgr.CurrentWeapon).transform.FindChild("Left").gameObject;
			_right = this.transform.FindChild(GameGUIMgr.CurrentWeapon).transform.FindChild("Right").gameObject;
			_top = this.transform.FindChild(GameGUIMgr.CurrentWeapon).transform.FindChild("Top").gameObject;
			_bottom = this.transform.FindChild(GameGUIMgr.CurrentWeapon).transform.FindChild("Bottom").gameObject;
			_crosshairChanged = false;
		}

		
		if(_needLessAccurate)
		{
			AnimateCrosshairMove(_left, _left.transform.localPosition, new Vector3(-150,0,0));
			AnimateCrosshairMove(_right, _right.transform.localPosition, new Vector3(150,0,0));
			AnimateCrosshairMove(_top, _top.transform.localPosition, new Vector3(0,120,0));
			AnimateCrosshairMove(_bottom, _bottom.transform.localPosition, new Vector3(0,-120,0));
			//Debug.Log("Less and Animated");
		}
		else
		{
			AnimateCrosshairMove(_left, _left.transform.localPosition, new Vector3(-75,0,0));
			AnimateCrosshairMove(_right, _right.transform.localPosition, new Vector3(75,0,0));
			AnimateCrosshairMove(_top, _top.transform.localPosition, new Vector3(0,70,0));
			AnimateCrosshairMove(_bottom, _bottom.transform.localPosition, new Vector3(0,-70,0));	
			//Debug.Log("More and Animated");
		}
		
		_needUpdate = false;
	}
	
	void AnimateCrosshairMove(GameObject go, Vector3 fromPos, Vector3 toPos)
	{
		TweenPosition tp = TweenPosition.Begin<TweenPosition>(go, 0.2f);
		tp.from = fromPos;
		tp.to = toPos;
		tp.Play();
	}
	
	/**
	 * Change Crosshair Accuracy
	 * @param status - more, less;
	 */
	public static void ChangeCrosshairAccuracy(string status)	
	{
		if(status.Equals("more") && !status.Equals(_lastStatus))
		{
			_needLessAccurate = false;
			_lastStatus = status;
			_needUpdate = true;
			//Debug.Log("More");
		}
		else if(status.Equals("less") && !status.Equals(_lastStatus))
		{
			_needLessAccurate = true;
			_lastStatus = status;
			_needUpdate = true;
			//Debug.Log("Less");
		}
		
	}
	#endregion	
	
}
