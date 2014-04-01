using UnityEngine;
using System.Collections;

public class AmmoPickup : PickupItem, IPickable{
	
	public AudioClip healthSFX;
	
	override public void Pickup()
	{
		RefillAmmo();
		GameGUIMgr.RefillAmmoGUI();

		AudioSource.PlayClipAtPoint(healthSFX, this.transform.position);
		
		Destroy(this.gameObject);
		Destroy(this);
	}
	
	void RefillAmmo()
	{
		AK47.Instance.UpdateAmmo(AK47.Instance.InitialAmmo);
		RocketLauncher.Instance.UpdateAmmo(RocketLauncher.Instance.InitialAmmo);
		GrenadeLauncher.Instance.UpdateAmmo(GrenadeLauncher.Instance.InitialAmmo);
		LandmineLauncher.Instance.UpdateAmmo(LandmineLauncher.Instance.InitialAmmo);
	}
}