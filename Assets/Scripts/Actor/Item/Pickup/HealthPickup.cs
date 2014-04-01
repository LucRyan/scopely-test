using UnityEngine;
using System.Collections;

public class HealthPickup : PickupItem, IPickable{
	
	public AudioClip healthSFX;
		
	override public void Pickup()
	{
		_player.Health = 100;
		GameGUIMgr.RestoreHeart();
		AudioSource.PlayClipAtPoint(healthSFX, this.transform.position);
		
		Destroy(this.gameObject);
		Destroy(this);
	}
}
