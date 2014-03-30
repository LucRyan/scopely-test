using UnityEngine;
using System.Collections;

public class HelpBtn : MonoBehaviour {
		
	void OnClick()
	{
		StartCoroutine(AnimateMove());
	}
	
	IEnumerator AnimateMove()
	{		
		TweenPosition menuTp = TweenPosition.Begin<TweenPosition>(PauseMenuMgr.menuBG, 0.3f);
		menuTp.from = PauseMenuMgr.menuBG.transform.localPosition;
		menuTp.to = new Vector3(0f, 1000f, 0f);
		menuTp.delay = 0f;
		menuTp.Play();
		
		PauseMenuMgr.helpBG.SetActive(true);
		PauseMenuMgr.helpBG.transform.localPosition = new Vector3(1000f, 0f,0f);
		TweenPosition helpTp = TweenPosition.Begin<TweenPosition>(PauseMenuMgr.helpBG, 0.3f);
		helpTp.from = PauseMenuMgr.helpBG.transform.localPosition;
		helpTp.to = Vector3.zero;
		helpTp.delay = 0.3f;
		helpTp.Play();
		
		yield return new WaitForSeconds(0.3f);
		PauseMenuMgr.menuBG.SetActive(false);
	}
}
