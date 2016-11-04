using UnityEngine;
using System.Collections;
using MedievalMayhem.Utilities;
using MedievalMayhem.Entites;

namespace MedievalMayhem.Weapons { 
	[RequireComponent(typeof(BoxCollider))]
	public class MeleeWeapon : BaseWeapon
	{
		private BoxCollider _hitZone;
		private bool _hitZoneOn = false;

		public bool HitZoneOn { 
			get { 
				return this._hitZoneOn; 
			}
			set { 
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
			this._weaponType = BaseWeapon.MELEE;
		}

		protected override void Update() {
			base.Update ();
			UpdateHitZone();
		}

		void OnTriggerEnter(Collider hit) {
			this.HandleAttackSuccess (hit);
		}

		void OnCollisionEnter(Collision hit) {
			this.HandleAttackSuccess (hit.collider);
		}

		//run when we had a successful hit on something
		protected override void HandleAttackSuccess (Collider hit) {
			base.HandleAttackSuccess (hit);
			//check if the object has a HealthSystem
			HealthSystem health = hit.gameObject.GetComponent<HealthSystem>();

			if (health != null) {
				health.Damage (this._damage);
			}
		}
	}
}

