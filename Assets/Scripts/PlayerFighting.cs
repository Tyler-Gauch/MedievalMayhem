using UnityEngine;
using System.Collections;
using MedievalMayhem.Utilities;
using MedievalMayhem.Items;
using UnityStandardAssets.CrossPlatformInput;

namespace MedievalMayhem.Player {
	public class PlayerFighting : MonoBehaviour {

		/**
		 *  _ variables denote private variables
		 */

		[SerializeField] private Animator _playerAnimator;
		[SerializeField] private GameObject _right_hand_weapon;
		[SerializeField] private GameObject _left_hand_weapon;
		[SerializeField] private GameObject _right_hand_weapon_hold;
		[SerializeField] private GameObject _left_hand_weapon_hold;

		private bool _hasWeapon;

		public void Start() {

			this._hasWeapon = (this._right_hand_weapon != null || this._left_hand_weapon != null);

			if (this._hasWeapon) {
				this._playerAnimator.Play (GlobalUtilities.IDLE_WITH_WEAPON);
			} else {
				this._playerAnimator.Play (GlobalUtilities.IDLE_NO_WEAPON);
			}
		}

		public void Update() {

			// Check if we are trying to attack
			bool attack1 = CrossPlatformInputManager.GetButtonDown(GlobalUtilities.ATTACK_1);
			bool attack2 = CrossPlatformInputManager.GetButtonDown (GlobalUtilities.ATTACK_2);

			if (attack1) {
				if (this._hasWeapon) {
					this._playerAnimator.SetTrigger (GlobalUtilities.ATTACK_1_WEAPON);
				} else {
					this._playerAnimator.SetTrigger (GlobalUtilities.ATTACK_1_NO_WEAPON);
				}
			} else if (attack2) {
				if (this._hasWeapon) {
					this._playerAnimator.SetTrigger (GlobalUtilities.ATTACK_2_WEAPON);
				} else {
					this._playerAnimator.SetTrigger (GlobalUtilities.ATTACK_2_NO_WEAPON);
				}
			}

			// handle moving animations here
			// Read input
			float horizontal = CrossPlatformInputManager.GetAxis(GlobalUtilities.INPUT_LEFT_RIGHT);
			float vertical = CrossPlatformInputManager.GetAxis(GlobalUtilities.INPUT_FORWARD_BACKWARD);

			bool isMoving = (horizontal != 0 || vertical != 0);

			bool isWalking = !CrossPlatformInputManager.GetButton(GlobalUtilities.INPUT_RUN);

			this._playerAnimator.SetBool (GlobalUtilities.WALK, (isWalking && isMoving));
			this._playerAnimator.SetBool (GlobalUtilities.RUN, (!isWalking && isMoving));
			this._playerAnimator.SetBool (GlobalUtilities.IDLE_NO_WEAPON, (!isMoving && !this._hasWeapon));
			this._playerAnimator.SetBool (GlobalUtilities.IDLE_WITH_WEAPON, (!isMoving && this._hasWeapon));

			if (this._playerAnimator.GetBool (GlobalUtilities.RUN)) {
				this._playerAnimator.Play (GlobalUtilities.RUN);
			} else {
				if (this._playerAnimator.GetBool (GlobalUtilities.IDLE_NO_WEAPON)) {
					this._playerAnimator.CrossFade (GlobalUtilities.IDLE_NO_WEAPON, 0.25f);
				} else {
					this._playerAnimator.CrossFade (GlobalUtilities.IDLE_WITH_WEAPON, 0.25f);
				}
			}

		}

		public void OnTriggerEnter(Collider hit) {
			// we hit something

			//was it a pickup item?
			if (hit.tag == GlobalUtilities.PICKUP_TAG) {
				//we want to first check the type
				ItemPickup pickUp = hit.GetComponent<ItemPickup>();
				switch (pickUp.GetPickupType ()) {
				case ItemPickup.RIGHT_HAND_WEAPON:
					//we want a right handed weapon so instantiate the prefab
					//on the Right Hand Hold Spot bone of the hands
					this._right_hand_weapon = (GameObject)Instantiate (
						pickUp.GetPrefab (), 
						Vector3.zero,
						Quaternion.identity
					);
					this.AddGear (this._right_hand_weapon, this._right_hand_weapon_hold);
					this._hasWeapon = true;
					break;
				case ItemPickup.RIGHT_HAND_SHIELD:
					break;
				case ItemPickup.LEFT_HAND_WEAPON:
					//we want a left handed weapon so instantiate the prefab
					//on the left Hand Hold Spot bone of the hands
					this._left_hand_weapon = (GameObject)Instantiate (
						pickUp.GetPrefab (), 
						Vector3.zero,
						Quaternion.identity
					);
					this.AddGear (this._left_hand_weapon, this._left_hand_weapon_hold);
					this._hasWeapon = true;
					break;
				case ItemPickup.LEFT_HAND_SHIELD:
					break;
				case ItemPickup.POWERUP:
					break;
				case ItemPickup.UNKNOWN:
					break;
				default:
					break;
				}
			}
		}

		private void AddGear(GameObject child, GameObject parent) {
			child.transform.parent = parent.transform;
			child.transform.localPosition = Vector3.zero;
			child.transform.localRotation = Quaternion.identity;
			child.transform.localScale = new Vector3 (1, 1, 1);
		}

	}
}
