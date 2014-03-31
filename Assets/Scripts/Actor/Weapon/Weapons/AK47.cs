using System.Collections;
using UnityEngine;

public class AK47 : PlayerWeapon
{
	private const int INITIAL_AMMO = 60;
	public GameObject _go;
	
	private static AK47 _instance;
	private AudioClip _reloadSound;
	private GameObject _gunFire;
	private GameObject _gunPoint;
	
	
	public static AK47 Instance
	{
	  get 
	  {
	     if (_instance == null)
	     {
	        _instance = new AK47();
	     }
	     return _instance;
	  }
	}
	private AK47() {}
	
	override public void Initial()
	{
		base.Initial();

		//Load assets
		shootSound = Resources.Load("Sounds/CarbineShoot") as AudioClip;
		bulletImpact = Resources.Load("Prefabs/Effect/BulletImpact") as GameObject;
		bloodSpray = Resources.Load("Prefabs/Effect/BloodSpray") as GameObject;

		_reloadSound = Resources.Load("Sounds/Reload") as AudioClip;
		_gunFire = Resources.Load("Prefabs/Effect/GunFire") as GameObject;
		_go = GameObject.Find("AK47-Weapon");
		_gunPoint = _go.transform.FindChild("GunPoint").gameObject;
		
		//Initial ammo
		UpdateAmmo(INITIAL_AMMO);
		
		//Check if loaded.
		if (shootSound == null || bulletImpact == null|| bloodSpray == null)
			Debug.Log("F*******ck!");
	}
	
	override public void Tick () {
		base.Tick();
		Reload();
	}
	

	void Reload()
	{
		if(Input.GetKeyDown(KeyCode.R))
		{
			_go.animation.Play("Reload");
			shootCooldown = 3f;
			UpdateAmmo(60);
			AudioSource.PlayClipAtPoint(_reloadSound, cam.transform.position);

		}
	}
	
	void AnimateGunFire(){
		// Early return if we are cooling down
		if (shootCooldown < 0.0f) {
			shootCooldown = SHOOT_COOLDOWN;
		} else {
			return;
		}
		EffectCreator.Instance.Effect(_gunPoint.transform.position, _gunFire);
	}
}


