using UnityEngine;
using System.Collections;

abstract public class PickupItem : MonoBehaviour, IPickable{
		
	protected Player _player;
	
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
				Debug.LogError("PickupItem: OnCollisionEnter: not an Player");
				return;
			}

        }
	}
	
	void Update()
	{
		this.transform.Rotate(Vector3.up * 3);
	}
	
	virtual public void Pickup(){}
}
