using UnityEngine;
using System.Collections;
using MedievalMayhem.Utilities;

namespace MedievalMayhem.Items {
	[RequireComponent(typeof(Rigidbody))]
	[RequireComponent(typeof(BoxCollider))] //used for collision
	[RequireComponent(typeof(SphereCollider))] //used for pickup trigger
	public class ItemPickup : BaseGameObject {

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
		protected override void Start () {
			if (this._pickupType == UNKNOWN) {
				throw new UnityException ("Pick up item created with no type");
			} else if (this._prefab == null && this._pickupType != UNKNOWN && this._pickupType != POWERUP) {
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

		void OnTriggerStay(Collider hit) {
			if (hit.CompareTag (GlobalUtilities.LOCAL_PLAYER_TAG)) {
				//if it requires player interaction let the player script
				//handle that
				if (this._requiresInteraction) {
					return;
				}

				this.PickUp (hit.gameObject);
			}
		}

		//means the player picked up the item
		public virtual GameObject PickUp(GameObject parent) {
			GameObject child = (GameObject)Instantiate (
				this._prefab,
				this._prefab.transform.position,
				this._prefab.transform.rotation
			);
			child.transform.parent = parent.transform;
			child.transform.localPosition = this._prefab.transform.position;
			child.transform.localRotation = this._prefab.transform.rotation;
			child.transform.localScale = this._prefab.transform.localScale;

			GameObject.Destroy (this.gameObject);

			return child;
		}
	}
}
