using UnityEngine;
using System.Collections;

public class WeaponMgr : MonoBehaviour {
	
	private PlayerWeapon _weapon;
	private GameObject _weaponGo;
	
	#region Unity Lifecycle
	// Use this for initialization
	void Start () {
		Initial();
		_weapon.Initial();
	}
	
	// Update is called once per frame
	void Update () {
		_weapon.Tick();
		UpdateSelection();
	}
	#endregion
	
	void Initial()
	{
//		_weapon = WeaponFactory.Instance.CreateWeapon(WeaponFactory.WeaponType.AK47);
//		_weaponGo = WeaponFactory.Instance.GetWeaponGameObject(WeaponFactory.WeaponType.AK47);
		
		_weapon = WeaponFactory.Instance.CreateWeapon(WeaponFactory.WeaponType.Grenade);
		_weaponGo = WeaponFactory.Instance.GetWeaponGameObject(WeaponFactory.WeaponType.Grenade);
		
	}
	
	private void UpdateSelection(){
		if (Input.GetKeyDown (KeyCode.Alpha1)) {
			GameGUIMgr.MoveSelectionRect(1);
		}
		if (Input.GetKeyDown (KeyCode.Alpha2)) {
			GameGUIMgr.MoveSelectionRect(2);
		}
		if (Input.GetKeyDown (KeyCode.Alpha3)) {
			GameGUIMgr.MoveSelectionRect(3);
		}
		if (Input.GetKeyDown (KeyCode.Alpha4)) {
			GameGUIMgr.MoveSelectionRect(4);
		}
	}
}
