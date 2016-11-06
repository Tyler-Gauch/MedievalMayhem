using UnityEngine;
using System.Collections;
using MedievalMayhem.Utilities;
using MedievalMayhem.Weapons;
using UnityStandardAssets.CrossPlatformInput;

namespace MedievalMayhem.Weapons {
	public class RangedWeapon : BaseWeapon {

		public GameObject projectile;
		public float shootingSpeed;
		public float fireRate; //seconds inbetween each shot
		public float reloadTime; //the time it takes to reload
		public int ammo;
		public int magSize;
		public int stockpile;


		protected bool _isReloading = false;
		protected float _lastShootTime = 0;
		protected float _lastReloadTime = 0;
		protected float _projectileSpeedMod = 1;
		protected float _projectileDamageMod = 1;

		protected override void Start () {
			base.Start ();
		}

		protected override void Update () {
			base.Update ();

			if (this._isBeingHeld) {
				bool shoot = CrossPlatformInputManager.GetButtonUp (GlobalUtilities.ATTACK_1);
				bool reload = CrossPlatformInputManager.GetButtonDown (GlobalUtilities.RELOAD);

				if (shoot) {
					Shoot ();
				}

				if (reload && this.NeedsReload()) {
					this._isReloading = true;
				}

				//check reload every frame 
				Reload ();
			}
		}

		protected virtual void Shoot() {
			if (ammo == 0) {
				GlobalUtilities.ShowInteractMessage ("Needs reload");
				return;
			} else if (this._lastShootTime < fireRate) {
				this._lastShootTime += Time.deltaTime;
				return;
			}

			this._lastShootTime = 0;
			ammo -= 1;


			// instantiate the projectile weapon
			GameObject child = (GameObject)Instantiate (
				this.projectile,
				transform.parent.position + (transform.parent.forward * 2),
				transform.parent.rotation
			);
			child.transform.localPosition = Camera.main.transform.position + (Camera.main.transform.forward * 3);
			child.transform.localRotation = Camera.main.transform.rotation * this.projectile.transform.rotation;
			child.transform.localScale = this.projectile.transform.localScale;
		

			ProjectileWeapon projectile = child.GetComponent<ProjectileWeapon> ();
			projectile.speed *= _projectileSpeedMod;
			projectile.Damage = projectile.Damage * _projectileDamageMod;
			projectile.Project (Camera.main.transform.forward);
		}

		protected virtual bool NeedsReload() {
			return (this.ammo < this.magSize);
		} 

		public virtual void Reload() {
			if (this._isReloading) {
				if (this._lastReloadTime < this.reloadTime) {
					this._lastReloadTime += Time.deltaTime;
				} else if (this.NeedsReload () && this.stockpile > 0) {
					this._isReloading = false;
					this._lastReloadTime = 0;
					if (this.stockpile < this.magSize) {
						this.ammo = this.stockpile;
						this.stockpile = 0;
					} else {
						this.ammo = this.magSize;
						this.stockpile -= this.magSize;
					}
				}
			}
		}
	}
}
