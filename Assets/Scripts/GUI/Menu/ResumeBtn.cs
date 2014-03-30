using UnityEngine;
using System.Collections;

public class ResumeBtn : MonoBehaviour {

	void OnClick()
	{
		if(PauseMenuMgr.resumeFlag == false)
		{
			PauseMenuMgr.resumeFlag = true;	
		}
	}
}
