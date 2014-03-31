using UnityEngine;
using System.Collections;

[RequireComponent (typeof(CapsuleCollider))]
public class Rocket : MonoBehaviour {
	
	private bool _onHand = true;
	private bool _exploded = false;
	private CapsuleCollider _cpCollider;
	private GameObject _bloodSpray;
	private GameObject _explosionEffect;
	private const int WEAPON_POWER = 50;
	
	public bool OnHand{
		set{ _onHand = value; }	
	}
	
	#region Unity Lifecycle
	void Start()
	{
		_cpCollider = this.GetComponent<CapsuleCollider>();
		_bloodSpray = Resources.Load("Prefabs/Effect/BloodSpray") as GameObject;
		_explosionEffect = Resources.Load("Prefabs/Effect/FireExplosion") as GameObject;
	}
	#endregion
	
	IEnumerator Explode(){
		
		_exploded = true;
		Debug.Log ("Exploded");
		//Explode
		_cpCollider.radius *= 3f;
		_cpCollider.height *= 3f;
		this.rigidbody.isKinematic = true;
		EffectCreator.Instance.Effect(this.transform.position, _explosionEffect);

		yield return new WaitForSeconds(0.4f);
		Destroy(this.gameObject);
		Destroy(this);
	}
	
	void OnCollisionEnter(Collision collision) {
 
		if(!_onHand && !_exploded)
		{
	        ContactPoint contact  = collision.contacts[0];
			
			StartCoroutine(Explode());
			//here you can set a tag so when your missile hits different stuff  
	        if (collision.gameObject.tag == "Enemy")
	        {
				try{
					Creeper z = collision.gameObject.GetComponent<Creeper>();
					z.DamageTaken(WEAPON_POWER, -contact.normal, contact.point);
					EffectCreator.Instance.Effect(contact.point, _bloodSpray);
				}catch{
					Debug.LogError("PlayerWeapon: Shoot: not an enemy");
					return;
				}
	
	        }
		}
	}
}
