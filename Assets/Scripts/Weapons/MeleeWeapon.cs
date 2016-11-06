using UnityEngine;
using System.Collections;
using MedievalMayhem.Utilities;
using MedievalMayhem.Entites;
using MedievalMayhem.Items;

namespace MedievalMayhem.Weapons { 
	public class MeleeWeapon : BaseWeapon
	{
		private MeleeWeaponHitZone _hitZone;

		protected override void Start() {
			base.Start ();
			this._hitZone = GetComponentInChildren<MeleeWeaponHitZone> ();

			if (this._hitZone == null) {
				throw new UnityException ("MeleeWeapon Created without a MeleeWeaponHitZone");
			}

			this._weaponType = BaseWeapon.MELEE;
		}

		protected override void Update() {
			base.Update ();
		}

		protected override void OnTriggerEnter(Collider hit) {
			base.OnTriggerEnter (hit);
			//throwing is handled on collision in the base class
			//but may sometimes get caught here
			if (!this._isBeingThrown) {
				this.HandleAttackSuccess (hit, this._damage);
			}
		}

		protected override void OnCollisionEnter(Collision hit) {
			base.OnCollisionEnter (hit);
		}

		//run when we had a successful hit on something
		protected override void HandleAttackSuccess (Collider hit, int damage) {
			//handles damaging
			base.HandleAttackSuccess (hit, damage);

		}

		public override GameObject Drop(bool withForce = true, Vector3 position = default(Vector3), Quaternion rotation = default(Quaternion)) {
			if (this.IsDroppable ()) {
				DisableHitZone ();
				return base.Drop (withForce, position, rotation);
			}
			return null;
		}

		public void EnableHitZone() {
			this._hitZone.HitZoneOn = true;
		}

		public void DisableHitZone () {
			this._hitZone.HitZoneOn = false;
		}
	}
}

