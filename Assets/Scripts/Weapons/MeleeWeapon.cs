using UnityEngine;
using System.Collections;

namespace MedievalMayhem.Weapons { 
	[RequireComponent(typeof(BoxCollider))]
	public class MeleeWeapon : Weapon
	{
		private BoxCollider _hitZone;
		private bool _hitZoneOn = false;

		public bool HitZoneOn { 
			get { 
				return this._hitZoneOn; 
			}
			set { 
				Debug.Log ("Setting _hitZoneOn for " + this + " to " + value);
				this._hitZoneOn = value;
				this.UpdateHitZone();
			}
		}

		private void UpdateHitZone() {
			this._hitZone.enabled = this._hitZoneOn;
		}

		protected override void Start() {
			base.Start ();
			this._hitZone = this.GetComponent<BoxCollider> ();
			this.UpdateHitZone ();
			this._weaponType = Weapon.MELEE;
		}

		protected override void Update() {
			base.Update ();
			Debug.Log ("HitZone for " + this + " is " + this._hitZone.enabled);
			UpdateHitZone();
		}

		void OnTriggerEnter(Collider hit) {
			Debug.Log ("HIT " + hit.tag);
		}
	}
}

