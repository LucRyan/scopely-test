using UnityEngine;
using System.Collections;

public class Zombie : Enemy, IDamageable, ITickable
{
	//
	public const float BITE_COOLDOWN = 1.5f;
	public const float BITE_RADIUS = 3.0f;  
	public const int DEFAULT_HEALTH = 100;

	public GameObject healthPickup;
	public GameObject ammoPickup;
	public AudioClip walkingSFX;
	
	//
	private float _biteCooldown;
	private Vector3 _healthBarOriginScale;
	private const float WALKING_SOUND_COOLDOWN = 8.0f;
	
	private float _wsCooldown = 0.0f;
	private int _health;
	public int Health  // read-write instance property
    {
        get{return _health;}
        set{_health = value;}
    }
	
	#region Unity Lifecycle
	override protected  void Start () {
		Initial();
	
	}
	override protected void Update () {
		Tick();
	}
	#endregion
	
	
	#region Actor Attributes
	
	void Initial()
	{
		base.Start();
		_healthBarOriginScale = healthBar.transform.localScale;
		_health = DEFAULT_HEALTH;
		heightOffset = 0.55f;
		moveSpeed = 5.0f;
	}
	
	public void Tick()
	{
		base.Update();
		_biteCooldown -= Time.deltaTime;
		_wsCooldown -= Time.deltaTime;
		
		if(!dead)
		{
			transform.Rotate(Vector3.up * 90f);
		}
		
		DamagePlayer();
		PlayWalkingSound();
	}
	
	void PlayWalkingSound()
	{
		if(_wsCooldown < 0.0f)
		{
			_wsCooldown = WALKING_SOUND_COOLDOWN;
			AudioSource.PlayClipAtPoint(walkingSFX, this.transform.position);
		}	
	}
	
	public void DamageTaken(int amount, Vector3 impactDirection, Vector3 impactPosition)
	{
		if(!dead)
		{
			Health -= amount;
			UpdateHealthBar();
			AnimateBulletImpact(impactDirection, impactPosition);
		}

		if(Health <= 0)
		{
			if(!dead)
			{
				ScoreMgr.UpdateKills(ScoreMgr.EnemyType.Zombie, 1);
				DropPickup();
				//I LOVE PHYSICS!!
				animator.enabled = false;
			}
			Ragdoll();
			dead = true;
		}
	}
	
	void DropPickup()
	{
		float chance = Random.value;
		Debug.Log("Chance: " + chance);
		if(player.Health < 30)
		{
			if(chance < 0.5f)
			{
				GameObject clone = GameObject.Instantiate(healthPickup, this.transform.position, Quaternion.identity) as GameObject;
			}	
		}
		
		if(chance < 0.2f)
		{
			GameObject clone = GameObject.Instantiate(ammoPickup, this.transform.position, Quaternion.identity) as GameObject;
		}
	}
	
	void UpdateHealthBar()
	{
		float scale = (float)Health/ (float) DEFAULT_HEALTH;
		if(scale < 0f)
			scale = 0f;
		//Debug.Log(scale);
		healthBar.transform.localScale = new Vector3(_healthBarOriginScale.x *scale, 
													 _healthBarOriginScale.y, 
													 _healthBarOriginScale.z);
	}
	#endregion
	
	#region Attack Style
	void DamagePlayer()
	{
		// Early return if there is no player
		if (player == null)
		{
			return;
		}
		
		if (_biteCooldown < 0.0f) {
			_biteCooldown = BITE_COOLDOWN;
		} else {
			return;
		}
		
		float radius = (player.transform.position - this.transform.position).magnitude;
		if (radius < BITE_RADIUS)
		{
			if(!dead)
			{
				//Make the explosion impact at the bottom of Zunny.
				Vector3	impactPosition = this.transform.position;
				Vector3 hitDirection = player.transform.position - this.transform.position;
				hitDirection.Normalize();
				player.DamageTaken(5, hitDirection, player.transform.position);
			}
		}
	}
	#endregion
}


