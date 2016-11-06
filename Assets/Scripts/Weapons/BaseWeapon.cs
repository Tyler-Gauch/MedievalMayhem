using UnityEngine;
using System.Collections;
using MedievalMayhem.Utilities;
using MedievalMayhem.Entites;

namespace MedievalMayhem.Weapons {
	[RequireComponent(typeof(Rigidbody))]
	public class BaseWeapon : BaseGameObject {

		public const int DEFAULT = 0;
		public const int MELEE = 1;
		public const int RANGED = 2;

		public bool isTwoHanded = false;
		[SerializeField] protected float _damage = 10;
		public float Damage {
			get { 
				return this._damage;
			}

			set { 
				this._damage = value;
			}
		}
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
		[SerializeField] protected GameObject _throwPrefab;


		protected bool _droppable;
		protected int _weaponType;
		protected Rigidbody _rigidBody;
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

		protected override void Awake() {
			base.Awake ();

			this._rigidBody = GetComponent<Rigidbody> ();
			this._rigidBody.useGravity = false;
			this._rigidBody.isKinematic = true; 

			this._mainCollisionCollider = GetComponent<BoxCollider> ();
		}

		protected override void Start() {
			base.Start ();

			if (this._dropPrefab != null) {
				this._droppable = true;
			} else {
				this._droppable = false;
			}

			if (this._isThrowable && this._throwPrefab == null) {
				throw new UnityException ("Throwable weapon created without throwable prefab (" + this.gameObject.name + ", " + this.gameObjectName + ")");
			} else if (this._isThrowable && this._throwPrefab.GetComponent<ProjectileWeapon> () == null) {
				throw new UnityException ("Throwable weapon's prefab does not contain ProjectileWeapon Script (" + this.gameObject.name + ", " + this.gameObjectName + ")");
			}
		}

		protected override void Update() {
			base.Update ();

			//we don't want this enabled if we are holding the weapon
			if (this._mainCollisionCollider != null) { 
				this._mainCollisionCollider.enabled = !this._isBeingHeld;
			}
		}

		protected virtual void FixedUpdate() {
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
		}

		protected virtual void HandleAttackSuccess(Collider hit, float damage) {
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
			this._isBeingHeld = false;

			// instantiate the throwable
			GameObject prefab = (GameObject)Instantiate (
				this._throwPrefab,
				transform.parent.position + (transform.parent.forward * 2),
				transform.parent.rotation
			);
			prefab.transform.localPosition = Camera.main.transform.position + (Camera.main.transform.forward * 3);
			prefab.transform.localRotation = Camera.main.transform.rotation * this._throwPrefab.transform.rotation;
			prefab.transform.localScale = this._throwPrefab.transform.localScale;

			ProjectileWeapon projectile = prefab.GetComponent<ProjectileWeapon> ();
			projectile.Project (location.forward);
			GameObject.Destroy (this.gameObject);
		}
	}
}
