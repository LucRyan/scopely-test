using UnityEngine;
using System.Collections;

public class Landmine : MonoBehaviour {

	private bool _onHand = true;
	private bool _calculatedBC = false;
	private bool _exploded = false;
	private BoxCollider _bxCollider;
	private GameObject _bloodSpray;
	private GameObject _explosionEffect;
	private const int WEAPON_POWER = 20;
	
	public bool OnHand{
		set{ _onHand = value; }	
	}
	
	#region Unity Lifecycle
	void Start()
	{
		_bloodSpray = Resources.Load("Prefabs/Effect/BloodSpray") as GameObject;
		_explosionEffect = Resources.Load("Prefabs/Effect/FireExplosion") as GameObject;
	}
	#endregion
	
	void Update()
	{
		if(!_onHand && !_calculatedBC)
		{
			_bxCollider = this.gameObject.AddComponent<BoxCollider>();
			_bxCollider.center = Vector3.zero;
			_bxCollider.size = new Vector3(2f,4f,2f);
			_calculatedBC = true;
		}
	}
	
	IEnumerator Explode(){
		
		_exploded = true;
		Debug.Log ("Exploded");
		//Explode
		_bxCollider.size *= 3f;
		EffectCreator.Instance.Effect(this.transform.position, _explosionEffect);

		yield return new WaitForSeconds(0.1f);
		Destroy(this.gameObject);
		Destroy(this);
	}
	
	void OnCollisionEnter(Collision collision) {
 
		if(!_onHand && !_exploded)
		{
	        ContactPoint contact  = collision.contacts[0];
			
			this.rigidbody.isKinematic = true;
			//here you can set a tag so when your missile hits different stuff  
	        if (collision.gameObject.tag == "Enemy")
	        {
				try{
					DetermineEnemy(collision, contact);
					StartCoroutine(Explode());
					EffectCreator.Instance.Effect(contact.point, _bloodSpray);
				}catch{
					Debug.LogError("PlayerWeapon: Shoot: not an enemy");
					return;
				}
	
	        }
		}
	}
	
	
	protected void DetermineEnemy(Collision collision, ContactPoint contact)
	{
		IDamageable z;
		if(collision.gameObject.name == "Creeper(Clone)")
		{
			z = collision.gameObject.GetComponent<Creeper>();
		}
		else
		{
			z = collision.gameObject.GetComponent<Zombie>();
		}
		
		z.DamageTaken(WEAPON_POWER,-contact.normal, contact.point);
	}
}
