using System.Collections;
using UnityEngine;

public class WeaponFactory
{
	public enum WeaponType
	{
		AK47,
		RocketLauncher,
		Grenade,
		LandMine
	}
	
	#region Singleton
	private static WeaponFactory instance;
	private WeaponFactory() {}
	
	public static WeaponFactory Instance
	{
	  get 
	  {
	     if (instance == null)
	     {
	        instance = new WeaponFactory();
	     }
	     return instance;
	  }
	}
	#endregion
	
	#region Weapon Factory Helper
	public PlayerWeapon CreateWeapon(WeaponType wpType)
	{
		switch(wpType)
		{
		case WeaponType.AK47:
			return AK47.Instance;
		case WeaponType.RocketLauncher:
			return RocketLauncher.Instance;
		case WeaponType.Grenade:
			return GrenadeLauncher.Instance;
		case WeaponType.LandMine:
			return LandmineLauncher.Instance;
		default:
			return null;
		}
	}
	
	public GameObject GetWeaponGameObject(WeaponType wpType)
	{
		switch(wpType)
		{
		case WeaponType.AK47:
			return AK47.Instance._go;
		case WeaponType.RocketLauncher:
			return RocketLauncher.Instance._go;
		case WeaponType.Grenade:
			return GrenadeLauncher.Instance._go;
		case WeaponType.LandMine:
			return LandmineLauncher.Instance._go;
		default:
			return null;
		}
	}
	#endregion
	
}


