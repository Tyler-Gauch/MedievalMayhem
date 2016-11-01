using UnityEngine;
using System.Collections;
using MedievalMayhem.Utilities;
using MedievalMayhem.Items;
using MedievalMayhem.Weapons;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.UI;

namespace MedievalMayhem.Player {
	public class PlayerFighting : MonoBehaviour {

		/**
		 *  _ variables denote private variables
		 */

		[SerializeField] private Animator _playerAnimator;
		[SerializeField] private GameObject _rightHandWeapon;
		[SerializeField] private GameObject _leftHandWeapon;
		[SerializeField] private GameObject _rightHandWeaponHold;
		[SerializeField] private GameObject _leftHandWeaponHold;
		[SerializeField] private Text _interactText;

		private bool _hasWeapon;
		private bool _isInteracting = false;
		private Collider _currentInteractionObject = null; // this makes it so that we can only interact with one object at a time

		public void Start() {

			if (this._rightHandWeapon != null) {
				this._hasWeapon = true;

				if (this._rightHandWeaponHold.transform.childCount == 0) {
					this._rightHandWeapon = this.AddGear (this._rightHandWeapon, this._rightHandWeaponHold);
				}
			}

			if (this._leftHandWeapon != null) {
				this._hasWeapon = true;

				if (this._leftHandWeaponHold.transform.childCount == 0) {
					this._leftHandWeapon = this.AddGear (this._leftHandWeapon, this._leftHandWeaponHold);
				}
			}


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
			bool dropWeapon = CrossPlatformInputManager.GetButtonDown (GlobalUtilities.DROP_WEAPON);

			if (attack1) {
				this.HandleAttack1 ();
			} else if (attack2) {
				this.HandleAttack2 ();
			}

			HandleMeleeWeaponAttack (this._rightHandWeapon);
			HandleMeleeWeaponAttack (this._leftHandWeapon);

			if (dropWeapon && this._hasWeapon) {
				this.HandleDropWeapon ();
			}

			// this is for moving animations
			this.HandleMoving ();
		}

		private void HandleAttack1() {
			if (this._hasWeapon) {
				this._playerAnimator.SetTrigger (GlobalUtilities.ATTACK_1_WEAPON);
			} else {
				this._playerAnimator.SetTrigger (GlobalUtilities.ATTACK_1_NO_WEAPON);
			}
		}

		private void HandleAttack2() {
			if (this._hasWeapon) {
				this._playerAnimator.SetTrigger (GlobalUtilities.ATTACK_2_WEAPON);
			} else {
				this._playerAnimator.SetTrigger (GlobalUtilities.ATTACK_2_NO_WEAPON);
			}
		}

		private void HandleMeleeWeaponAttack(GameObject weapon) {
			if (weapon == null || !this._hasWeapon) {
				return;
			} 
			Weapon weaponScript = weapon.GetComponent<Weapon> ();

			if (weaponScript == null || weaponScript.WeaponType != Weapon.MELEE) {
				return;
			}

			MeleeWeapon meleeWeaponScript = weapon.GetComponent<MeleeWeapon> ();

			//make sure we are doing an attacking animation
			bool isAttacking = (this._playerAnimator.GetCurrentAnimatorStateInfo (1).IsName (GlobalUtilities.ATTACK_1_WEAPON)
				|| this._playerAnimator.GetCurrentAnimatorStateInfo (1).IsName (GlobalUtilities.ATTACK_2_WEAPON));

			//make sure that we are in the downward "attacking" swing of our animation set to 50% through the attack
			//so that on the pull back we don't do any damage
			bool isWithinAttackingTimeRange = (this._playerAnimator.GetCurrentAnimatorStateInfo (1).normalizedTime % 1) <= 0.5;

			//check if the weapons should be enabled or disabeled
			if (isAttacking && isWithinAttackingTimeRange) {
				Debug.Log ("Enabling colliders for " + weapon);
				meleeWeaponScript.HitZoneOn = true;
			} else {
				Debug.Log ("Disabling colliders for " + weapon);
				meleeWeaponScript.HitZoneOn = false;
			}
		}

		private void HandleDropWeapon() {
			if (this._rightHandWeapon != null) {
				this.DropWeapon (this._rightHandWeapon);
			}	

			if (this._leftHandWeapon != null) {
				this.DropWeapon (this._leftHandWeapon);
			}

			this._hasWeapon = false;
		}

		private void DropWeapon(GameObject heldWeapon) {
			Weapon weapon = heldWeapon.GetComponent<Weapon> ();
			if (weapon.IsDroppable ()) {
				GameObject dropped = (GameObject)Instantiate (
					weapon.GetDropPrefab(), 
					transform.position + transform.forward + transform.up,
					Quaternion.identity
				);
				GameObject.Destroy (heldWeapon);
				dropped.GetComponent<Rigidbody> ().AddForce (transform.forward, ForceMode.Impulse);
			}
		}

		private void HandleMoving() {
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
			if (this._currentInteractionObject == null) {
				this._currentInteractionObject = hit;
			}
		}

		public void OnTriggerStay(Collider hit) {

			// if we collided a second object while we are interacting don't 
			// interact with it
			if (this._currentInteractionObject != hit) {
				return;
			}

			// we hit something
			bool interact = CrossPlatformInputManager.GetButtonDown(GlobalUtilities.INTERACT);

			//was it a pickup item?
			if (hit.tag == GlobalUtilities.PICKUP_TAG) {
				//we want to first check the type
				ItemPickup pickUp = hit.GetComponent<ItemPickup>();

				if (interact || !pickUp.RequiresInteraction()) {
					this._isInteracting = true;
					switch (pickUp.GetPickupType ()) {
					case ItemPickup.RIGHT_HAND_WEAPON:
						if (this._hasWeapon && this._rightHandWeapon != null) {
							this.DropWeapon (this._rightHandWeapon);
						}
						this._rightHandWeapon = this.AddGear (pickUp.GetPrefab (), this._rightHandWeaponHold);
						this._hasWeapon = true;
						GameObject.Destroy (hit.gameObject);
						this.ClearInteract ();
						break;
					case ItemPickup.RIGHT_HAND_SHIELD:
						break;
					case ItemPickup.LEFT_HAND_WEAPON:
						this._leftHandWeapon = this.AddGear (pickUp.GetPrefab (), this._leftHandWeaponHold);
						this._hasWeapon = true;
						GameObject.Destroy (hit.gameObject);
						this.ClearInteract ();
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

					this._isInteracting = false;
				} else {
					this.ShowButtonInteract("X", pickUp.GetIdentifier());
				}
			}
		}

		public void OnTriggerExit(Collider hit) {
			this.ClearInteract ();
			if (this._currentInteractionObject == hit) {
				this._currentInteractionObject = null;
			}
		}

		private GameObject AddGear(GameObject prefab, GameObject parent) {
			GameObject child = (GameObject)Instantiate (
				prefab, 
				prefab.transform.position,
				prefab.transform.rotation
			);
			child.transform.parent = parent.transform;
			child.transform.localPosition = prefab.transform.position;
			child.transform.localRotation = prefab.transform.rotation;
			child.transform.localScale = prefab.transform.localScale;
			return child;
		}

		private void ShowButtonInteract(string button, string obj) {
			this._interactText.text = "Press '" + button + "' for " + obj;
		}

		private void ClearInteract() {
			this._interactText.text = "";
		}

	}
}
