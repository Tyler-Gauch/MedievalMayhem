using UnityEngine;
using System.Collections;
using MedievalMayhem.Utilities;
using MedievalMayhem.Entites;

namespace MedievalMayhem.Weapons {
	[RequireComponent(typeof(Rigidbody))]
	public class BaseWeapon : BaseGameObject {

		public const int DEFAULT = 0;
		public const int MELEE = 1;

		public Vector3 throwRotation = new Vector3(0,-400,0);
		[SerializeField] protected int _damage = 10;
		[SerializeField] protected int _throwingDamage = 0;
		[SerializeField] protected GameObject _dropPrefab;
		[SerializeField] protected bool _isThrowable = false;
		public bool IsThrowable {
			get { 
				return this._isThrowable;
			}

			set { 
				this._isThrowable = value;
			}
		}

		protected bool _droppable;
		protected int _weaponType;
		protected Rigidbody _rigidBody;
		protected bool _isBeingThrown = false;  //whether or not the weapon is currently being thrown
		protected bool _handleThrowStop = false;
		protected Collision _throwHit;
		protected BoxCollider _mainCollisionCollider;
		protected bool _isBeingHeld = false;
		public bool IsBeingHeld {
			get{ 
				return this._isBeingHeld;
			}

			set{ 
				this._isBeingHeld = value;
			}
		}

		private const string WEAPON_TAG = "Weapon";

		public int WeaponType {
			get { 
				return this._weaponType;
			}

			set { 
				this._weaponType = value;
			}
		}

		protected override void Start() {
			base.Start ();

			if (this._dropPrefab != null) {
				this._droppable = true;
			} else {
				this._droppable = false;
			}

			this._rigidBody = GetComponent<Rigidbody> ();
			this._rigidBody.useGravity = false;
			this._rigidBody.isKinematic = true; 

			this._mainCollisionCollider = GetComponent<BoxCollider> ();
		}

		protected override void Update() {
			base.Update ();

			//we don't want this enabled if we are holding the weapon
			if (this._mainCollisionCollider != null) { 
				this._mainCollisionCollider.enabled = !this._isBeingHeld;
			}

			if (this._handleThrowStop && this._rigidBody.IsSleeping ()) {
				HandleThrowHit (this._throwHit, transform.position);
				this._throwHit = null;
				this._handleThrowStop = false;
			}
		}

		protected virtual void FixedUpdate() {
			//if the weapon was thrown make it spin
			if (this._isBeingThrown) {
				Quaternion deltaRotation = Quaternion.Euler (this.throwRotation * Time.deltaTime);
				this._rigidBody.MoveRotation (this._rigidBody.rotation * deltaRotation);
			}
		}

		public bool IsDroppable() {
			return this._droppable;
		}

		public GameObject GetDropPrefab() {
			return this._dropPrefab;
		}

		public override string GetGameObjectName () {
			return WEAPON_TAG;
		}

		protected virtual void OnTriggerEnter(Collider hit) {
			Debug.Log ("Hit " + hit.tag);
		}

		protected virtual void OnCollisionEnter(Collision hit) {
			if (this._isBeingThrown && hit.collider.tag != GlobalUtilities.LOCAL_PLAYER_TAG) {
				this._isBeingThrown = false;
				if (hit.collider.CompareTag (GlobalUtilities.ENEMY_TAG)) {
					HandleThrowHit (hit, hit.contacts [0].point);
				} else {
					this._handleThrowStop = true;
					this._throwHit = hit;
					this._rigidBody.drag = 0;
					this._rigidBody.angularDrag = 1;
				}				
			}
		}

		protected virtual void HandleThrowHit(Collision hit, Vector3 endPosition) {
			GameObject dropped = this.Drop (false, endPosition, transform.rotation);
			dropped.transform.parent = hit.gameObject.transform;
			HandleAttackSuccess (hit.collider, this._throwingDamage);
		}

		protected virtual void HandleAttackSuccess(Collider hit, int damage) {
			//check if the object has a HealthSystem
			HealthSystem health = hit.gameObject.GetComponent<HealthSystem>();

			if (health != null) {
				health.Damage (damage);
			}
		}

		public virtual GameObject Drop(bool withForce = true, Vector3 position = default(Vector3), Quaternion rotation = default(Quaternion)) {
			GameObject dropped = (GameObject)Instantiate (
				this.GetDropPrefab (), 
				(position != default(Vector3) ? position : transform.position + transform.forward + transform.up),
				(rotation != default(Quaternion) ? rotation : Quaternion.identity)
			);
			GameObject.Destroy (this.gameObject);
			if (withForce) {
				dropped.GetComponent<Rigidbody> ().AddForce (transform.forward, ForceMode.Impulse);
			} else {
				Rigidbody rb = dropped.GetComponent<Rigidbody> ();
				rb.useGravity = false;
				rb.isKinematic = true;
			}

			this._isBeingHeld = false;
			return dropped;
		}

		//when we throw the object we first want to center it in the players view
		public virtual void Throw(Transform location) {
			//if we can't throw this weapon just return
			if (!this._isThrowable) {
				return;
			} 

			this._isBeingThrown = true;
			this._isBeingHeld = false;

			Vector3 force = location.forward * 20;
			transform.parent = null;
			this.gameObject.layer = GlobalUtilities.DEFAULT_LAYER;
			transform.position = location.position + location.forward;
			if (this._rigidBody == null) {
				this._rigidBody = GetComponent<Rigidbody> ();
			}
			this._rigidBody.collisionDetectionMode = CollisionDetectionMode.Continuous;
			this._rigidBody.useGravity = true;
			this._rigidBody.isKinematic = false;
			this._mainCollisionCollider.isTrigger = false;
			this._rigidBody.AddForce (force, ForceMode.Impulse);
			//this._rigidBody.angularVelocity = new Vector3 (2, 0, 0);

		}
	}
}
