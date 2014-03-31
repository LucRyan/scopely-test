using UnityEngine;
using System.Collections;

public class Grenade : PlayerWeapon {
	
	private const int INITIAL_AMMO = 10;
	
	public GameObject _go;
	public const float SHOOT_COOLDOWN = 1f;
	
	private AudioClip _fireInTheHole;
	
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
		//Initial ammo
		UpdateAmmo(INITIAL_AMMO);
		_go = _go = GameObject.Find("Grenade-Weapon");
		_fireInTheHole = Resources.Load("Sounds/Fire_in_the_Hole") as AudioClip; 
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
		GameObject clone = GameObject.Instantiate(_go, _go.transform.position, _go.transform.localRotation) as GameObject;
		clone.rigidbody.isKinematic = false;
		clone.rigidbody.useGravity = true;
		clone.rigidbody.AddForce(cam.transform.forward * 2000 + Vector3.up * 2000);
		GrenadeProperty gp = clone.GetComponent<GrenadeProperty>();
		gp.OnHand = false;
		AudioSource.PlayClipAtPoint(_fireInTheHole, cam.transform.position);
		//Ammo used
		UpdateAmmo(--ammo);
		

//		// Do Raycast and find collision point
//		RaycastHit hit = new RaycastHit ();
//		bool collided = Physics.Raycast (cam.transform.position, shootVector, out hit);
//		if (!collided) {
//			return;
//		}
//
//		// Damage the enemy
//		bool hitEnemy = false;
//		if (hit.collider.gameObject.tag == "Enemy") {
//			try{
//				Zunny z = hit.collider.gameObject.GetComponent<Zunny>();
//				z.DamageTaken(WEAPON_POWER,-hit.normal, hit.point);
//				hitEnemy = true;
//			}catch{
//				Debug.LogError("PlayerWeapon: Shoot: not an enemy");
//				return;
//			}
//		}
//
//		// Create bullet impact effect
//		Vector3 impactPoint = hit.point + hit.normal * IMPACT_OFFSET;
//		if (hitEnemy){
//			EffectCreator.Instance.Effect(impactPoint, bloodSpray);
//		}else{
//			EffectCreator.Instance.Effect(impactPoint, bulletImpact);
//		}
		

	}
	
}
