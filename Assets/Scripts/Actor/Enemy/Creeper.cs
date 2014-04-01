using UnityEngine;
using System.Collections;

public class Creeper : Enemy, IDamageable, ITickable
{
	//
	public const float BITE_COOLDOWN = 3f;
	public const float BITE_RADIUS = 3.0f;  
	public const int DEFAULT_HEALTH = 10;
	//
	public GameObject fireExplosion;
	public GameObject healthPickup;
	//
	private Vector3 _healthBarOriginScale;
	private float _biteCooldown;
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
		heightOffset = 0.3f;
		moveSpeed = 3.0f;
	}
	
	public void Tick()
	{
		base.Update();
		_biteCooldown -= Time.deltaTime;
		DamagePlayer();
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
				ScoreMgr.UpdateKills(ScoreMgr.EnemyType.Creeper, 1);
				DropPickup();
				//I LOVE PHYSICS!!
				animator.enabled = false;
			}
//			Debug.Log("dead");
			Ragdoll();
			dead = true;
		}
	}
	
	void DropPickup()
	{
		if(player.Health < 30)
		{
			float chance = Random.value;
			//Debug.Log("Chance: " + chance);
			if(chance > 0.5f)
			{
				GameObject clone = GameObject.Instantiate(healthPickup, this.transform.position, Quaternion.identity) as GameObject;
			}	
		}
	}
	
	void UpdateHealthBar()
	{
		float scale = (float)Health/ (float) DEFAULT_HEALTH;
		if(scale < 0f)
			scale = 0f;
//		Debug.Log(scale);
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


