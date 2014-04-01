using UnityEngine;
using System.Collections;

public class GrenadeLauncher : PlayerWeapon {
	
	private const int INITIAL_AMMO = 15;
	
	public GameObject _go;
	public const float SHOOT_COOLDOWN = 2f;
	
	private AudioClip _fireInTheHole;
	
	public int InitialAmmo
	{
		get{return INITIAL_AMMO;}
	}
	
	#region Singleton
	private static GrenadeLauncher _instance;
	public static GrenadeLauncher Instance
	{
	  get 
	  {
	     if (_instance == null)
	     {
	        _instance = new GrenadeLauncher();
	     }
	     return _instance;
	  }
	}
	#endregion 
	
	private GrenadeLauncher(){}
	
	
	override public void Initial()
	{
		base.Initial();
		_go = _go = GameObject.Find("Grenade-Weapon");
		_fireInTheHole = Resources.Load("Sounds/Fire_in_the_Hole") as AudioClip; 
		
		//Initial ammo
		UpdateAmmo(INITIAL_AMMO);
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
		GameObject clone = GameObject.Instantiate(_go, _go.transform.position, cam.transform.rotation) as GameObject;
		clone.rigidbody.isKinematic = false;
		clone.rigidbody.useGravity = true;
		clone.rigidbody.AddForce(cam.transform.forward * 2000 + Vector3.up * 2000);
		Grenade gp = clone.GetComponent<Grenade>();
		gp.OnHand = false;
		AudioSource.PlayClipAtPoint(_fireInTheHole, cam.transform.position);
		//Ammo used
		UpdateAmmo(--ammo);
		
	}
	
}
