using UnityEngine;
using System.Collections;
using MedievalMayhem.Utilities;
using MedievalMayhem.Weapons;
using MedievalMayhem.Entites;

namespace MedievalMayhem.Weapons {
	[RequireComponent(typeof(BoxCollider))]
	public class ProjectileWeapon : BaseWeapon {

		public float speed = 100;
		public Vector3 flyingRotation;

		protected bool _handleStop = false;
		protected Collision _landingSpot = null;
		protected bool _isProjecting = false;

		protected override void Start () {
			base.Start ();
		}

		protected override void Update() {
			base.Update ();

			if (this._handleStop && this._rigidBody.IsSleeping ()) {
				HandleHit (this._landingSpot, transform.position);
				this._landingSpot = null;
				this._handleStop = false;
			}
		}

		protected override void FixedUpdate() {
			//if the weapon was thrown make it spin
			if (this._isProjecting) {
				Quaternion deltaRotation = Quaternion.Euler (this.flyingRotation * Time.deltaTime);
				this._rigidBody.MoveRotation (this._rigidBody.rotation * deltaRotation);
			}
		}

		protected override void OnCollisionEnter(Collision hit) {
			//we don't want to do the base trigger enter
			HealthSystem health = hit.gameObject.GetComponent<HealthSystem>();

			//if we hit something with health we want to hurt it and destroy the projectile
			if (health != null) {
				health.Damage (this._damage);
			}

			if (hit.collider.tag != GlobalUtilities.LOCAL_PLAYER_TAG) {
				this._isProjecting = false;
				if (hit.collider.CompareTag (GlobalUtilities.ENEMY_TAG)) {
					HandleHit (hit, hit.contacts [0].point);
				} else {
					this._handleStop = true;
					this._landingSpot = hit;
					this._rigidBody.drag = 0;
					this._rigidBody.angularDrag = 1;
				}				
			}
		}

		//project the item in that direction
		public void Project(Vector3 direction) {
			Rigidbody rb = GetComponent<Rigidbody> ();
			rb.useGravity = true;
			rb.isKinematic = false;
			rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
			rb.AddForce (direction * speed, ForceMode.Impulse);
			this._isProjecting = true;
		}

		protected virtual void HandleHit(Collision hit, Vector3 endPosition) {
			if (this.IsDroppable()) {
				GameObject dropped = this.Drop (false, endPosition, transform.rotation);
				dropped.transform.parent = hit.gameObject.transform;
				BoxCollider collider = dropped.GetComponent<BoxCollider> ();
				if(collider != null) {
					collider.enabled = false;
				}
			}

			HandleAttackSuccess (hit.collider, this._damage);
		}
	}
}