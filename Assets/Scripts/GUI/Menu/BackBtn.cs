using UnityEngine;
using System.Collections;

public class BackBtn : MonoBehaviour {

	void OnClick()
	{
		StartCoroutine(AnimateMove());
	}
	
	IEnumerator AnimateMove()
	{
		TweenPosition helpTp = TweenPosition.Begin<TweenPosition>(PauseMenuMgr.helpBG, 0.3f);
		helpTp.from = PauseMenuMgr.helpBG.transform.localPosition;
		helpTp.to = new Vector3(1000f, 0f, 0f);
		helpTp.delay = 0f;
		helpTp.Play();
		
		TweenPosition menuTp = TweenPosition.Begin<TweenPosition>(PauseMenuMgr.menuBG, 0.3f);
		menuTp.from = PauseMenuMgr.menuBG.transform.localPosition;
		menuTp.to = Vector3.zero;
		menuTp.delay = 0.3f;
		menuTp.Play();
		
		yield return new WaitForSeconds(0.3f);
		PauseMenuMgr.menuBG.SetActive(true);
		PauseMenuMgr.helpBG.SetActive(false);
	}

}
