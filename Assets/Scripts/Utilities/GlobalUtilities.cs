using UnityEngine;
using System.Collections;

namespace MedievalMayhem.Utilities {
	public static class GlobalUtilities
	{

		/*
		 * These strings are so if we change the animation properties
		 * we don't need to change everywhere they are used. Except for the FirstPersonController.cs
		 * 
		 * Please use these references not const strings when using the animators
		 */

		// PlayerAnimator
		public const string IDLE_NO_WEAPON = "IdleNoWeapon";
		public const string IDLE_WITH_WEAPON = "IdleWithWeapon";
		public const string RUN = "Run";
		public const string WALK = "Walk";
		public const string ATTACK_2_NO_WEAPON = "PunchLeft";
		public const string ATTACK_1_NO_WEAPON = "PunchRight";
		public const string ATTACK_2_WEAPON = "SwingLeftRight";
		public const string ATTACK_1_WEAPON = "SwingUpDown";
		public const string DEFAULT_ANIM = "Default";

		//animator event variables
		public const int ANIM_EVENT_RIGHT_HAND 			= 1;
		public const int ANIM_EVENT_LEFT_HAND  			= 2;

		//input
		public const string ATTACK_1 						= "Fire1";
		public const string ATTACK_2 						= "Fire2";
		public const string INPUT_RUN 						= "Run";
		public const string INPUT_LEFT_RIGHT 				= "Horizontal";
		public const string INPUT_FORWARD_BACKWARD 		= "Vertical";
		public const string INTERACT 						= "Interact"; // the InputManager Key
		public const string DROP_WEAPON 					= "DropWeaponRight";

		//tags
		public const string PICKUP_TAG 					= "Pickup";
		public const string RIGHT_HAND_TAG 				= "RightHand";
		public const string LEFT_HAND_TAG 					= "LeftHand";
		public const string ENEMY_TAG 						= "Enemy";
	}
}

