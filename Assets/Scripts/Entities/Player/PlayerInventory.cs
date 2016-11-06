using UnityEngine;
using System.Collections;
using MedievalMayhem.Weapons;

namespace MedievalMayhem.Entites.Player {
	public class PlayerInventory : MonoBehaviour
	{
		public int maxWeapons = 2;
		public BaseWeapon[] weapons;

		public GameObject weaponHold;
		public GameObject currentWeapon;
	}
}

