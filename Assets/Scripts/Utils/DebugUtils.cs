#define DEBUG
 
using System;
using System.Diagnostics;
using UnityEngine;
 
public class DebugUtils : MonoBehaviour
{
	#region Unity Lifecycle
	[Conditional("DEBUG")]
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.T)) {
			//GameGUIMgr.HeartHit();
			GameGUIMgr.RestoreHeart();
		}	
		UpdateCrosshair();
		//UpdateSelection();
		UpdateWeaponAmmo();
		
	}
	#endregion
	
	#region Debug Helper
    [Conditional("DEBUG")]
    public static void Assert(bool condition, String messages)
    {
        if (!condition) throw new Exception(messages);
    }
	
	[Conditional("DEBUG")]
	private static void TestHit()
	{
		GameGUIMgr.HeartHit();
	}
	
	[Conditional("DEBUG")]
	private void UpdateSelection(){
		if (Input.GetKeyDown (KeyCode.Alpha1)) {
			GameGUIMgr.MoveSelectionRect(1);
		}
		if (Input.GetKeyDown (KeyCode.Alpha2)) {
			GameGUIMgr.MoveSelectionRect(2);
		}
		if (Input.GetKeyDown (KeyCode.Alpha3)) {
			GameGUIMgr.MoveSelectionRect(3);
		}
		if (Input.GetKeyDown (KeyCode.Alpha4)) {
			GameGUIMgr.MoveSelectionRect(4);
		}
	}
	
	[Conditional("DEBUG")]
	private void UpdateWeaponAmmo(){
		if (Input.GetKeyDown (KeyCode.Y) ){
			GameGUIMgr.UpdateAmmo(99);
		}
	}
	
	[Conditional("DEBUG")]
	private void UpdateCrosshair(){
		if (Input.GetKeyDown (KeyCode.M) ){
			UnityEngine.Debug.Log ("Press M");
			CrosshairMgr.ChangeCrosshairAccuracy("more");
			
		}
		if (Input.GetKeyDown (KeyCode.L) ){
			UnityEngine.Debug.Log ("Press L");
			CrosshairMgr.ChangeCrosshairAccuracy("less");
			
		}
	}
	#endregion
	

}

