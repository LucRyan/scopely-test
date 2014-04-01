using UnityEngine;
using System.Collections;

public class WarningGUIMgr : MonoBehaviour {
	
	UILabel _warningLbl;
	bool _isVisiable = false;
	bool _isWarning = false;
	
	// Use this for initialization
	void Start () {
		_warningLbl = this.transform.FindChild("WarningLbl").gameObject.GetComponent<UILabel>();
		NGUITools.SetActive(_warningLbl.gameObject, false);	
	}
	
	// Update is called once per frame
	void Update () {
		if(_isWarning)
		Blink(_warningLbl);
	}
	
	public void Warning()
	{
		_isWarning = true;
		NGUITools.SetActive(_warningLbl.gameObject, true);
		StartCoroutine(DisableWarning());
		Debug.Log ("Warning");
	}
	
	IEnumerator DisableWarning()
	{
		yield return new WaitForSeconds(3f);
		NGUITools.SetActive(_warningLbl.gameObject, false);
		_isWarning = false;
	}
	
	void Blink(UILabel lbl)
	{ 
		if(_isVisiable)
		{
			Debug.Log ("Blink");
			lbl.alpha -= 0.05f;
			if(lbl.alpha <= 0f)
			{
				_isVisiable = false;
			}
		}
		else 
		{
			lbl.alpha += 0.05f;
			if(lbl.alpha >= 1f)
			{
				_isVisiable = true;
			}
		}
	}
}
