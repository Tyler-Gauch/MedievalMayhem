using UnityEngine;
using System.Collections;
using MedievalMayhem.Weapons;
using MedievalMayhem.Utilities;
using UnityStandardAssets.CrossPlatformInput;

namespace MedievalMayhem.Weapons {
	public class BowWeapon : RangedWeapon
	{

		public float maxHoldTime = 2;
		private float _heldTime = 0;

		protected override void Update () {
			base.Update ();

			bool holding = CrossPlatformInputManager.GetButton (GlobalUtilities.ATTACK_1);

			if (this._isBeingHeld) {
				if (ammo == 0) {
					GlobalUtilities.ShowInteractMessage ("Reloading... " + this._lastReloadTime + "/" + this.reloadTime);
					this._isReloading = true;
					this._heldTime = 0;
				} else {
					if (holding) {
						this._heldTime += Time.deltaTime;
						this._heldTime = Mathf.Min (this._heldTime, this.maxHoldTime);
					} else {
						this._heldTime = 0;
					}

					this._projectileSpeedMod = this._projectileDamageMod = this._heldTime / this.maxHoldTime;
				}
			}
		}
	}
}
