using UnityEngine;
using System.Collections;
using MedievalMayhem.Utilities;

namespace MedievalMayhem.Items {
	[RequireComponent(typeof(Rigidbody))]
	[RequireComponent(typeof(BoxCollider))] //used for collision
	[RequireComponent(typeof(SphereCollider))] //used for pickup trigger
	public class ItemPickup : MonoBehaviour {

		public const int UNKNOWN			= -1; //this is incase we forget to change the type
		public const int RIGHT_HAND_WEAPON 	= 0;
		public const int LEFT_HAND_WEAPON  	= 1;
		public const int RIGHT_HAND_SHIELD 	= 2;
		public const int LEFT_HAND_SHIELD  	= 3;
		public const int POWERUP			= 4;

		[SerializeField] private GameObject _prefab 	= null;
		[SerializeField] private int _pickupType		= UNKNOWN;
		[SerializeField] private bool _requiresInteraction;
		[SerializeField] private string _identifier;

		// Use this for initialization
		void Start () {
			if (this._pickupType == UNKNOWN) {
				throw new UnityException ("Pick up item created with no type");
			} else if (this._prefab == null) {
				throw new UnityException ("Pick up item created with no prefab");
			} else {
				tag = GlobalUtilities.PICKUP_TAG;
				var rigidBody = GetComponent<Rigidbody> ();
				rigidBody.mass = 2;
				rigidBody.useGravity = true;
				rigidBody.collisionDetectionMode = CollisionDetectionMode.Continuous;

				GetComponent<SphereCollider> ().isTrigger = true;
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

		public bool RequiresInteraction() {
			return this._requiresInteraction;
		}

		public void RequiresInteraction(bool requiresInteraction) {
			this._requiresInteraction = requiresInteraction;
		}

		public string GetIdentifier() {
			return this._identifier;
		}
	}
}
