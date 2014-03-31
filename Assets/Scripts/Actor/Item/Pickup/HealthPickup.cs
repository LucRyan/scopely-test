using UnityEngine;
using System.Collections;

public class HealthPickup : MonoBehaviour, IPickable{
	
	public AudioClip healthSFX;
	
	private Player _player;
	
	void OnTriggerEnter(Collider other) 
	{
		Debug.Log("TriggerEnter");
		//here you can set a tag so when your missile hits different stuff  
        if (other.gameObject.tag == "Player")
        {
			try{
				_player	= other.gameObject.GetComponent<Player>();
				Pickup();
			}catch{
				Debug.LogError("HealthPickup: OnCollisionEnter: not an Player");
				return;
			}

        }
	}
	
	void Update()
	{
		this.transform.Rotate(Vector3.up * 3);
	}
	
	public void Pickup()
	{
		_player.Health = 100;
		GameGUIMgr.RestoreHeart();
		AudioSource.PlayClipAtPoint(healthSFX, this.transform.position);
		
		Destroy(this.gameObject);
		Destroy(this);
	}
}
