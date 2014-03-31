using UnityEngine;
using System.Collections;

public class Grenade : PlayerWeapon {
	
	public GameObject _go;
	

	#region Singleton
	private static Grenade _instance;
	public static Grenade Instance
	{
	  get 
	  {
	     if (_instance == null)
	     {
	        _instance = new Grenade();
	     }
	     return _instance;
	  }
	}
	#endregion 
	
	private Grenade() 
	{		
	}
	
}
