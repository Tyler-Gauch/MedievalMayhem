﻿using UnityEngine;
using System.Collections;
using MedievalMayhem.Utilities;
using MedievalMayhem.Utilities.Event;
using MedievalMayhem.Items;
using MedievalMayhem.Weapons;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.UI;

namespace MedievalMayhem.Entites {
	public class PlayerFighting : BaseEntity {

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
		private GameObject _rightHandFist = null;
		private GameObject _leftHandFist = null;

		void Awake() {
			GetComponentInChildren<EventTrigger> ().eventNames = new string[]{
				this.gameObjectName + "TurnOnRightHitzones", 
				this.gameObjectName + "TurnOffRightHitzones",
				this.gameObjectName + "TurnOnLeftHitzones",
				this.gameObjectName + "TurnOffLeftHitzones",
				this.gameObjectName + "TurnOnAllHitzones",
				this.gameObjectName + "TurnOffAllHitzones"
			};
		}

		protected override void OnEnable() {
			base.OnEnable ();
			EventManager.StartListening (this.gameObjectName + "TurnOnRightHitzones", TurnHitZoneOnRight);
			EventManager.StartListening (this.gameObjectName + "TurnOffRightHitzones", TurnHitZoneOffRight);
			EventManager.StartListening (this.gameObjectName + "TurnOnLeftHitzones", TurnHitZoneOnLeft);
			EventManager.StartListening (this.gameObjectName + "TurnOffLeftHitzones", TurnHitZoneOffLeft);
			EventManager.StartListening (this.gameObjectName + "TurnOnAllHitzones", TurnHitZoneOnLeft);
			EventManager.StartListening (this.gameObjectName + "TurnOffAllHitzones", TurnHitZoneOffLeft);
			EventManager.StartListening (this.gameObjectName + "Dead", Dead);
		}

		protected override void OnDisable() {
			base.OnDisable ();
			EventManager.StopListening (this.gameObjectName + "TurnOnRightHitzones", TurnHitZoneOnRight);
			EventManager.StopListening (this.gameObjectName + "TurnOffRightHitzones", TurnHitZoneOffRight);
			EventManager.StopListening (this.gameObjectName + "TurnOnLeftHitzones", TurnHitZoneOnLeft);
			EventManager.StopListening (this.gameObjectName + "TurnOffLeftHitzones", TurnHitZoneOffLeft);
			EventManager.StopListening (this.gameObjectName + "TurnOnAllHitzones", TurnHitZoneOnAll);
			EventManager.StopListening (this.gameObjectName + "TurnOffAllHitzones", TurnHitZoneOffAll);
			EventManager.StopListening (this.gameObjectName + "Dead", Dead);
		}

		protected override void Start() {

			base.Start ();

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

			_rightHandFist = GameObject.FindWithTag (GlobalUtilities.RIGHT_HAND_TAG);
			_leftHandFist = GameObject.FindWithTag (GlobalUtilities.LEFT_HAND_TAG);


			if (this._hasWeapon) {
				this._playerAnimator.Play (GlobalUtilities.IDLE_WITH_WEAPON);
			} else {
				this._playerAnimator.Play (GlobalUtilities.IDLE_NO_WEAPON);
			}


		}

		protected override void Update() {

			// Check if we are trying to attack
			bool attack1 = CrossPlatformInputManager.GetButtonDown(GlobalUtilities.ATTACK_1);
			bool attack2 = CrossPlatformInputManager.GetButtonDown (GlobalUtilities.ATTACK_2);
			bool dropWeapon = CrossPlatformInputManager.GetButtonDown (GlobalUtilities.DROP_WEAPON);

			if (attack1) {
				this.HandleAttack1 ();
			} else if (attack2) {
				this.HandleAttack2 ();
			}

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

		public void TurnHitZoneOnAll() {
			TurnHitZoneOnLeft ();
			TurnHitZoneOnRight();
		}

		public void TurnHitZoneOffAll() {
			TurnHitZoneOffLeft ();
			TurnHitZoneOffRight ();
		}

		public void TurnHitZoneOnRight() {
			if (this._hasWeapon) {
				HandleMeleeWeaponAttack (this._rightHandWeapon, true);
			} else {
				HandleMeleeWeaponAttack (this._rightHandFist, true);
			}
		}

		public void TurnHitZoneOnLeft() {
			if (this._hasWeapon){
				HandleMeleeWeaponAttack (this._leftHandWeapon, true);
			} else {
				HandleMeleeWeaponAttack (this._leftHandFist, true);
			}
		}

		public void TurnHitZoneOffRight() {
			if (this._hasWeapon) {
				HandleMeleeWeaponAttack (this._rightHandWeapon, false);
			} else {
				HandleMeleeWeaponAttack (this._rightHandFist, false);
			}
		}

		public void TurnHitZoneOffLeft() {
			if (this._hasWeapon){
				HandleMeleeWeaponAttack (this._leftHandWeapon, false);
			} else {
				HandleMeleeWeaponAttack (this._leftHandFist, false);
			}
		}

		private void HandleMeleeWeaponAttack(GameObject weapon, bool enable) {
			if (weapon == null) {
				return;
			} 
			BaseWeapon weaponScript = weapon.GetComponent<BaseWeapon> ();

			if (weaponScript == null || weaponScript.WeaponType != BaseWeapon.MELEE) {
				return;
			}

			MeleeWeapon meleeWeaponScript = weapon.GetComponent<MeleeWeapon> ();

			//check if the weapons should be enabled or disabeled
			meleeWeaponScript.HitZoneOn = enable;
		}

		private void HandleDropWeapon() {
			if (this._rightHandWeapon != null) {
				this._hasWeapon = this.DropWeapon (this._rightHandWeapon);
			}	

			if (this._leftHandWeapon != null) {
				this._hasWeapon = this.DropWeapon (this._leftHandWeapon);
			}
		}

		private bool DropWeapon(GameObject heldWeapon) {
			BaseWeapon weapon = heldWeapon.GetComponent<BaseWeapon> ();
			if (weapon.IsDroppable ()) {
				GameObject dropped = (GameObject)Instantiate (
					                     weapon.GetDropPrefab (), 
					                     transform.position + transform.forward + transform.up,
					                     Quaternion.identity
				                     );
				GameObject.Destroy (heldWeapon);
				dropped.GetComponent<Rigidbody> ().AddForce (transform.forward, ForceMode.Impulse);
				return false; //dropped weapon
			} 

			return true; //still have weapon
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

				if ((interact && !this._isInteracting) || !pickUp.RequiresInteraction()) {
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

		//this gets called when the player is dead
		protected override void Dead() {
			base.Dead ();
			Debug.Log (this.gameObjectName + " Is Dead!");
		}
	}
}
