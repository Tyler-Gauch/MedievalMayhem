using UnityEngine;
using System.Collections;
using MedievalMayhem.Entites;

namespace MedievalMayhem.Items {
	public class HealthPickup : ItemPickup
	{
		public int healingCapacity = 10;

		protected override void Start() {
			this.SetPickupType (ItemPickup.POWERUP);
			base.Start ();
		}

		//means that our player object picked up our item
		public override GameObject PickUp(GameObject player) {
			HealthSystem health = player.GetComponent<HealthSystem>();

			if (health.Heal (this.healingCapacity)) {
				GameObject.Destroy (this.transform.gameObject);
				Debug.Log ("Player was healed");
			} else {
				Debug.Log ("Player is at full health");
			}

			//we don't want to return anything here
			return null;
		}

	}
}
