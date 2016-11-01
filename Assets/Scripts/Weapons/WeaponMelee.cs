using UnityEngine;
using System.Collections;

namespace MedievalMayhem.Weapons { 
	[RequireComponent(typeof(BoxCollider))]
	public class WeaponMelee : Weapon
	{
		public BoxCollider hitZone;

		void OnTriggerEnter(Collider hit) {
			Debug.Log ("HIT " + hit.tag);
			hitZone = this.GetComponent<BoxCollider> ();
			if (hitZone == null) {
				Debug.LogError ("NO BOX COLLIDER");
			}
		}
	}
}

