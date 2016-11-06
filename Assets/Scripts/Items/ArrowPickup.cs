using UnityEngine;
using System.Collections;
using MedievalMayhem.Weapons;
using MedievalMayhem.Entites.Player;

namespace MedievalMayhem.Items { 
	public class ArrowPickup : ItemPickup {
	
		public int arrowCount = 1;

		protected override void Start() {
			this.SetPickupType (ItemPickup.POWERUP);
			base.Start ();
		}

		//means that our player object picked up our item
		public override GameObject PickUp(GameObject player) {
			PlayerFighting playerFighting= player.GetComponent<PlayerFighting> ();
			if (playerFighting != null) {
				BowWeapon bow = playerFighting.GetLeftHandWeapon ().GetComponent<BowWeapon>();
				if (bow != null) {
					bow.stockpile += arrowCount;
					GameObject.Destroy (this.gameObject);
				}
			}

			//we don't want to return anything here
			return null;
		}


	}
}

