using UnityEngine;
using System.Collections;

public class PlayerWeapon{

	public const float IMPACT_OFFSET = 0.01f;
	public const float SHOOT_COOLDOWN = 0.15f;
	public const float ACCURACY_DELTA = 2.0f;
	public const int WEAPON_POWER = 5;

	public AudioClip shootSound;
	public GameObject bulletImpact;
	public GameObject bloodSpray;
	
	protected int ammo;
	protected Camera cam;
	protected float shootCooldown;
	
	#region public helper
	public virtual void Initial () {		
		// Get reference to Camera
		cam = Camera.main;
		if (cam == null){
			Debug.LogError("PlayerWeapon: Start: could not find required Camera component");
			return;
		}
	}
	public virtual void Tick () {
		
		// Press spacebar to shoot
		if (Input.GetMouseButton(0) ){
			Shoot ();
		}

		shootCooldown -= Time.deltaTime;
	}
	#endregion
	

	#region Gun Helpers
	private Quaternion GetOffsetQuaternion(float radius, float angleInRadians){
		Quaternion xQuaternion = Quaternion.AngleAxis(Mathf.Sin (angleInRadians) * radius, Vector3.up);
		Quaternion yQuaternion = Quaternion.AngleAxis(Mathf.Cos (angleInRadians) * radius, Vector3.left);
		return xQuaternion * yQuaternion;
	}
	protected virtual void Shoot(){
		
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
		
		//Crosshair Animation
		CrosshairMgr.ChangeCrosshairAccuracy("less");
		
		// Calculate shoot direction
		Quaternion shootRotation = cam.transform.rotation;
		float angle = Random.value * Mathf.PI * 2.0f;
		float radius = Mathf.Sqrt(Random.value) * ACCURACY_DELTA; 
		Vector3 shootVector = (shootRotation * GetOffsetQuaternion(radius, angle)) * Vector3.forward;

		// Play gunshot sound
		AudioSource.PlayClipAtPoint(shootSound, cam.transform.position);
		
		//Ammo used
		UpdateAmmo(--ammo);
		
		// Do Raycast and find collision point
		RaycastHit hit = new RaycastHit ();
		bool collided = Physics.Raycast (cam.transform.position, shootVector, out hit);
		if (!collided) {
			return;
		}

		// Damage the enemy
		bool hitEnemy = false;
		if (hit.collider.gameObject.tag == "Enemy") {
			try{
				Creeper z = hit.collider.gameObject.GetComponent<Creeper>();
				z.DamageTaken(WEAPON_POWER,-hit.normal, hit.point);
				hitEnemy = true;
			}catch{
				Debug.LogError("PlayerWeapon: Shoot: not an enemy");
				return;
			}
		}

		// Create bullet impact effect
		Vector3 impactPoint = hit.point + hit.normal * IMPACT_OFFSET;
		if (hitEnemy){
			EffectCreator.Instance.Effect(impactPoint, bloodSpray);
		}else{
			EffectCreator.Instance.Effect(impactPoint, bulletImpact);
		}
		

	}
	
	protected void UpdateAmmo(int num)
	{
		ammo = num;
		GameGUIMgr.UpdateAmmo(num);
	}
	
	#endregion
}
