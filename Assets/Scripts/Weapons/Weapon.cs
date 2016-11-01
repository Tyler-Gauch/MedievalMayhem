using UnityEngine;
using System.Collections;

namespace MedievalMayhem.Weapons {
	public class Weapon : MonoBehaviour {

		[SerializeField] private int _damage;
		[SerializeField] private GameObject _dropPrefab;

		private bool _droppable;

		protected virtual void Start() {
			if (this._dropPrefab != null) {
				this._droppable = true;
			} else {
				this._droppable = false;
			}
		}

		public bool IsDroppable() {
			return this._droppable;
		}

		public GameObject GetDropPrefab() {
			return this._dropPrefab;
		}
	}
}
