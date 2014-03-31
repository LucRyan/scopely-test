using UnityEngine;
using System.Collections;

public class WeaponMgr : MonoBehaviour {
	
	private PlayerWeapon _weapon;
	private GameObject _weaponGo;
	
	#region Unity Lifecycle
	// Use this for initialization	
	void Start () {
		Initial();
		PostInitial();
	}
	
	// Update is called once per frame
	void Update () {
		_weapon.Tick();
		UpdateSelection();
	}
	#endregion
	
	void Initial()
	{
		_weapon = WeaponFactory.Instance.CreateWeapon(WeaponFactory.WeaponType.AK47);
		_weapon.Initial();
		
		WeaponFactory.Instance.CreateWeapon(WeaponFactory.WeaponType.RocketLauncher).Initial();
		WeaponFactory.Instance.CreateWeapon(WeaponFactory.WeaponType.Grenade).Initial();
		WeaponFactory.Instance.CreateWeapon(WeaponFactory.WeaponType.LandMine).Initial();

	}
	
	void PostInitial()
	{
		_weaponGo = WeaponFactory.Instance.GetWeaponGameObject(WeaponFactory.WeaponType.AK47);
		_weaponGo.SetActive(true);
		
		WeaponFactory.Instance.GetWeaponGameObject(WeaponFactory.WeaponType.RocketLauncher).SetActive(false);
		WeaponFactory.Instance.GetWeaponGameObject(WeaponFactory.WeaponType.Grenade).SetActive(false);
		WeaponFactory.Instance.GetWeaponGameObject(WeaponFactory.WeaponType.LandMine).SetActive(false);
	}
	
	private void UpdateSelection(){
		if (Input.GetKeyDown (KeyCode.Alpha1)) {
			GameGUIMgr.MoveSelectionRect(1);
			SelectWeapon(WeaponFactory.WeaponType.AK47);	
		}
		if (Input.GetKeyDown (KeyCode.Alpha2)) {
			GameGUIMgr.MoveSelectionRect(2);
			SelectWeapon(WeaponFactory.WeaponType.RocketLauncher);
		}
		if (Input.GetKeyDown (KeyCode.Alpha3)) {
			GameGUIMgr.MoveSelectionRect(3);
			SelectWeapon(WeaponFactory.WeaponType.Grenade);
		}
		if (Input.GetKeyDown (KeyCode.Alpha4)) {
			GameGUIMgr.MoveSelectionRect(4);
			SelectWeapon(WeaponFactory.WeaponType.LandMine);
		}
	}
	
	
	private void SelectWeapon(WeaponFactory.WeaponType weaponTp)
	{
		_weaponGo.SetActive(false);
		_weapon = WeaponFactory.Instance.CreateWeapon(weaponTp);
		_weaponGo = WeaponFactory.Instance.GetWeaponGameObject(weaponTp);
		_weaponGo.SetActive(true);	
	}
}
