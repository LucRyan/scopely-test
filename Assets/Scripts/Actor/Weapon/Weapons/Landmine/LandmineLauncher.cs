using UnityEngine;
using System.Collections;

public class LandmineLauncher : PlayerWeapon {

	private const int INITIAL_AMMO = 10;
	
	public GameObject _projectile;
	public GameObject _go;
	public const float SHOOT_COOLDOWN = 1f;
	
	private AudioClip _fireInTheHole;
	
	#region Singleton
	private static LandmineLauncher _instance;
	public static LandmineLauncher Instance
	{
	  get 
	  {
	     if (_instance == null)
	     {
	        _instance = new LandmineLauncher();
	     }
	     return _instance;
	  }
	}
	#endregion 
	
	private LandmineLauncher(){}
	
	override public void Initial()
	{
		base.Initial();
		//Initial ammo
		UpdateAmmo(INITIAL_AMMO);
		_go = GameObject.Find("Landmine-Weapon");
		_projectile = Resources.Load("Prefabs/Weapon/Rocket") as GameObject;
		_fireInTheHole = Resources.Load("Sounds/MetalImpact") as AudioClip; 
	}
	
	override protected void Shoot(){
		
		// Early return if we are cooling down
		if (shootCooldown < 0.0f) {
			shootCooldown = SHOOT_COOLDOWN;
		} else {
			return;
		}
		
		if(ammo <= 0)
		{
			return;
		}
		
		// Calculate shoot direction
		GameObject clone = GameObject.Instantiate(_go, _go.transform.position, Quaternion.identity) as GameObject;
		clone.rigidbody.isKinematic = false;
		clone.rigidbody.useGravity = true;
		clone.rigidbody.AddForce(cam.transform.forward);
		Landmine lm = clone.GetComponent<Landmine>();
		lm.OnHand = false;
		AudioSource.PlayClipAtPoint(_fireInTheHole, cam.transform.position);
		//Ammo used
		UpdateAmmo(--ammo);
		
	}
}
