using UnityEngine;
using System.Collections;


[RequireComponent (typeof(CapsuleCollider))]
public class GrenadeProperty : MonoBehaviour
{
	private bool _onHand = true;
	private bool _exploded = false;
	private bool _exploding = false;
	private CapsuleCollider _cpCollider;
	private GameObject _bloodSpray;
	private GameObject _explosionEffect;
	private const int WEAPON_POWER = 10;
	
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
	
	void Update(){
		if(!_onHand && !_exploding)
		{
			StartCoroutine(Explode());
		}
	}
	#endregion
	
	IEnumerator Explode(){
		_exploding = true;
		
		yield return new WaitForSeconds(2f);
		Debug.Log ("Exploded");
		//Explode
		_cpCollider.radius *= 5f;
		_cpCollider.height *= 5f;
		EffectCreator.Instance.Effect(this.transform.position, _explosionEffect);
		_exploded = true;
		
		yield return new WaitForSeconds(1f);
		Destroy(this.gameObject);
		Destroy(this);
	}
	
	void OnCollisionEnter(Collision collision) {
 
		if(!_onHand && _exploded)
		{
	        ContactPoint contact  = collision.contacts[0];
			
	
			//here you can set a tag so when your missile hits different stuff  
	        if (collision.gameObject.tag == "Enemy")
	        {
				try{
					Zunny z = collision.gameObject.GetComponent<Zunny>();
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


