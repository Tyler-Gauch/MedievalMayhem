using UnityEngine;
using System.Collections;

namespace MedievalMayhem.Items {
	[RequireComponent(typeof(GameObject))]
	public class ItemPickup : MonoBehaviour {

		public const int UNKNOWN			= -1; //this is incase we forget to change the type
		public const int RIGHT_HAND_WEAPON 	= 0;
		public const int LEFT_HAND_WEAPON  	= 1;
		public const int RIGHT_HAND_SHIELD 	= 2;
		public const int LEFT_HAND_SHIELD  	= 3;
		public const int POWERUP			= 4;


		[SerializeField] private GameObject _prefab 	= null;
		[SerializeField] private int _pickupType		= UNKNOWN;

		// Use this for initialization
		void Start () {
			if (this._pickupType == UNKNOWN) {
				Debug.LogError ("Pick up item created with no type");
			}
		}

		public int GetPickupType() {
			return this._pickupType;
		}

		public void SetPickupType(int type) {
			this._pickupType = type;
		}

		public GameObject GetPrefab() {
			return this._prefab;
		}

		public void SetPrefab(GameObject prefab) {
			this._prefab = prefab;
		}
	}
}
