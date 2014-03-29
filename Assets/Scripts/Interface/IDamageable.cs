using System;
using UnityEngine;

interface IDamageable
{
	//
	int Health {get; set;}
	
	void DamageTaken(int amount, Vector3 damageDirection, Vector3 damagePosition);
}

