using UnityEngine;
using System.Collections;

namespace MedievalMayhem.Weapons {
	[RequireComponent(typeof(Rigidbody))]
	public class BaseWeapon : BaseGameObject {

		public const int DEFAULT = 0;
		public const int MELEE = 1;

		[SerializeField] protected int _damage;
		[SerializeField] protected GameObject _dropPrefab;

		protected bool _droppable;
		protected int _weaponType;
		protected Rigidbody _rigidBody;

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
		}

		public bool IsDroppable() {
			return this._droppable;
		}

		public GameObject GetDropPrefab() {
			return this._dropPrefab;
		}

		protected override string GetBaseEventTagName ()
		{
			return WEAPON_TAG;
		}

		protected virtual void HandleAttackSuccess(Collider hit) {
			Debug.Log ("We hit " + hit.tag);
		}
	}
}
