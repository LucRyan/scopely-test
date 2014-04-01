using UnityEngine;
using System.Collections;

public class RocketLauncher : PlayerWeapon {

	private const int INITIAL_AMMO = 15;
	
	public GameObject _projectile;
	public GameObject _go;
	public const float SHOOT_COOLDOWN = 1f;
	
	private AudioClip _missileLaunch;
	
	public int InitialAmmo
	{
		get{return INITIAL_AMMO;}
	}
	
	#region Singleton
	private static RocketLauncher _instance;
	public static RocketLauncher Instance
	{
	  get 
	  {
	     if (_instance == null)
	     {
	        _instance = new RocketLauncher();
	     }
	     return _instance;
	  }
	}
	#endregion 
	
	private RocketLauncher(){}
	
	override public void Initial()
	{
		base.Initial();
		//Initial ammo
		UpdateAmmo(INITIAL_AMMO);
		_go = GameObject.Find("RocketLauncher-Weapon");
		_projectile = Resources.Load("Prefabs/Weapon/Rocket") as GameObject;
		_missileLaunch = Resources.Load("Sounds/MissileLaunch") as AudioClip; 
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
		GameObject gunPoint = _go.transform.FindChild("GunPoint").gameObject;
		GameObject clone = GameObject.Instantiate(_projectile, gunPoint.transform.position, cam.transform.rotation) as GameObject;
		clone.transform.localScale *= 0.5f;
		clone.rigidbody.isKinematic = false;
		clone.rigidbody.useGravity = true;
		clone.rigidbody.AddForce(cam.transform.forward * 6000);
		Rocket gp = clone.GetComponent<Rocket>();
		gp.OnHand = false;
		AudioSource.PlayClipAtPoint(_missileLaunch, cam.transform.position);
		//Ammo used
		UpdateAmmo(--ammo);
		
	}	
}
