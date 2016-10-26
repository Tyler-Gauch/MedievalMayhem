using UnityEngine;
using System.Collections;
using MedievalMayhem.Utilities;
using UnityStandardAssets.CrossPlatformInput;

namespace MedievalMayhem.Player {
	public class PlayerFighting : MonoBehaviour {

		/**
		 *  _ variables denote private variables
		 */

		[SerializeField] private Animator _playerAnimator;
		[SerializeField] private GameObject primary;

		private bool _hasWeapon;

		public void Start() {

			this._hasWeapon = (primary != null);

			Debug.Log ("Has Weapon: " + this._hasWeapon);

			if (this._hasWeapon) {
				Debug.Log ("Has Weapon");
				this._playerAnimator.Play (GlobalUtilities.IDLE_WITH_WEAPON);
			} else {
				Debug.Log ("Has No Weapon");
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
	}
}
