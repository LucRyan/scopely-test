using UnityEngine;
using System.Collections;

public class Zunny : Enemy, IDamageable
{
	//
	public const float BITE_COOLDOWN = 3f;
	public const float BITE_RADIUS = 10.0f; 
	//
	public GameObject fireExplosion;
	//
	private float _biteCooldown;
	private int _health = 10;
	public int Health  // read-write instance property
    {
        get{return _health;}
        set{_health = value;}
    }
	
	#region Unity Lifecycle
	override protected void Awake(){
		base.Awake();
	}
	override protected  void Start () {
		base.Start();

	}

	override protected void LateUpdate (){
		base.LateUpdate();
	}

	override protected void Update () {
		base.Update();
		_biteCooldown -= Time.deltaTime;
		DamagePlayer();
	}
	#endregion
	
	
	#region Actor Attributes
	public void DamageTaken(int amount, Vector3 impactDirection, Vector3 impactPosition)
	{
		Health -= amount;
		AnimateBulletImpact(impactDirection, impactPosition);
		if(Health <= 0)
		{
			_dead = true;
		}
	}
	#endregion
	
	#region Attack Style
	void DamagePlayer()
	{
		// Early return if there is no player
		if (_player == null)
		{
			return;
		}
		
		if (_biteCooldown < 0.0f) {
			_biteCooldown = BITE_COOLDOWN;
		} else {
			return;
		}
		
		float radius = (_player.transform.position - this.transform.position).magnitude;
		if (radius < BITE_RADIUS)
		{
			if(!_dead)
			{
				//Make the explosion impact at the bottom of Zunny.
				Vector3	impactPosition = this.transform.position;
				//Do physics
				//this.rigidbody.isKinematic = false;
				//Explode(impactPosition, 3000.0f, _explosionRadius);	
				//EffectCreator.Instance.Effect(impactPosition, fireExplosion);
				
				Vector3 hitDirection = _player.transform.position - this.transform.position;
				hitDirection.Normalize();
				_player.DamageTaken(5, hitDirection, _player.transform.position);
			}
		}
	}
	#endregion
}


