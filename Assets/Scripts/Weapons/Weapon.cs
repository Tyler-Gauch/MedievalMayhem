using UnityEngine;
using System.Collections;

namespace MedievalMayhem.Weapons {
	[RequireComponent(typeof(Rigidbody))]
	public class Weapon : MonoBehaviour {

		public const int DEFAULT = 0;
		public const int MELEE = 1;

		[SerializeField] private int _damage;
		[SerializeField] private GameObject _dropPrefab;

		protected bool _droppable;
		protected int _weaponType;
		protected Rigidbody _rigidBody;

		public int WeaponType {
			get { 
				return this._weaponType;
			}

			set { 
				this._weaponType = value;
			}
		}

		protected virtual void Start() {
			Debug.Log (this + " Drop Prefab: " + this._dropPrefab);

			if (this._dropPrefab != null) {
				this._droppable = true;
			} else {
				this._droppable = false;
			}

			this._rigidBody = GetComponent<Rigidbody> ();
			this._rigidBody.useGravity = false;
			this._rigidBody.isKinematic = true; 
		}

		protected virtual void Update(){
			//does nothing right now
		}

		public bool IsDroppable() {
			return this._droppable;
		}

		public GameObject GetDropPrefab() {
			return this._dropPrefab;
		}
	}
}
