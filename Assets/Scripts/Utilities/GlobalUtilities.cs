using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace MedievalMayhem.Utilities {
	public class GlobalUtilities : MonoBehaviour
	{
		/*
		 * These strings are so if we change the animation properties
		 * we don't need to change everywhere they are used. Except for the FirstPersonController.cs
		 * 
		 * Please use these references not const strings when using the animators
		 */

		// PlayerAnimator
		public const string IDLE_NO_WEAPON 					= "IdleNoWeapon";
		public const string IDLE_WITH_WEAPON 				= "IdleWithWeapon";
		public const string RUN 							= "Run";
		public const string WALK 							= "Walk";
		public const string ATTACK_2_NO_WEAPON 				= "PunchLeft";
		public const string ATTACK_1_NO_WEAPON 				= "PunchRight";
		public const string ATTACK_2_WEAPON 				= "SwingLeftRight";
		public const string ATTACK_1_WEAPON 				= "SwingUpDown";
		public const string DEFAULT_ANIM 					= "Default";

		//animator event variables
		public const int ANIM_EVENT_RIGHT_HAND 				= 1;
		public const int ANIM_EVENT_LEFT_HAND  				= 2;

		//input
		public const string ATTACK_1 						= "Fire1";
		public const string ATTACK_2 						= "Fire2";
		public const string INPUT_RUN 						= "Run";
		public const string INPUT_LEFT_RIGHT 				= "Horizontal";
		public const string INPUT_FORWARD_BACKWARD 			= "Vertical";
		public const string INTERACT 						= "Interact"; // the InputManager Key
		public const string DROP_WEAPON 					= "DropWeaponRight";
		public const string THROW_WEAPON 					= "ThrowWeapon";
		public const string RELOAD 							= "Reload";

		//tags
		public const string PICKUP_TAG 						= "Pickup";
		public const string RIGHT_HAND_TAG 					= "RightHand";
		public const string LEFT_HAND_TAG 					= "LeftHand";
		public const string ENEMY_TAG 						= "Enemy";
		public const string LOCAL_PLAYER_TAG 				= "Player";
		public const string GROUND_TAG 						= "Ground";

		//layers
		public const int DEFAULT_LAYER						= 0;
		public const int TRANSPARENT_FW_LAYER				= 1;
		public const int IGNORE_RAYCAST_LAYER 				= 2;
		public const int WATER_LAYER 						= 4;
		public const int UI_LAYER 							= 5;
		public const int NO_CAMERA_CLIPPING_LAYER			= 8;
		public const int NOT_REGION_TARGET 					= 9;

		public static Text InteractText;
		public static Text WarningText;

		//so we can assign the object in the editor and 
		//then assign it to the static variable
		public Text interactText;
		public Text warningText;

		void Awake() {
			InteractText = interactText;
			WarningText = warningText;
		}

		public static void ShowButtonInteract(string button, string obj) {
			ShowInteractMessage ("Press " + button + " for " + obj);
		}

		public static void ShowInteractMessage(string message) {
			InteractText.text = message;
		}

		public static void ClearInteractText() {
			InteractText.text = "";
		}

		public static void ShowWarningMessage(string message) {
			WarningText.text = message;
		}

		public static void ClearWarningText() {
			WarningText.text = "";
		}
	}
}

