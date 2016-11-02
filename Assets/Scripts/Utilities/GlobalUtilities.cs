﻿using UnityEngine;
using System.Collections;

namespace MedievalMayhem.Utilities {
	public static class GlobalUtilities
	{

		/*
		 * These strings are so if we change the animation properties
		 * we don't need to change everywhere they are used. Except for the FirstPersonController.cs
		 * 
		 * Please use these references not static strings when using the animators
		 */

		// PlayerAnimator
		public static string IDLE_NO_WEAPON = "IdleNoWeapon";
		public static string IDLE_WITH_WEAPON = "IdleWithWeapon";
		public static string RUN = "Run";
		public static string WALK = "Walk";
		public static string ATTACK_2_NO_WEAPON = "PunchLeft";
		public static string ATTACK_1_NO_WEAPON = "PunchRight";
		public static string ATTACK_2_WEAPON = "SwingLeftRight";
		public static string ATTACK_1_WEAPON = "SwingUpDown";
		public static string DEFAULT_ANIM = "Default";

		//animator event variables
		public static int ANIM_EVENT_RIGHT_HAND = 1;
		public static int ANIM_EVENT_LEFT_HAND  = 2;

		//input
		public static string ATTACK_1 = "Fire1";
		public static string ATTACK_2 = "Fire2";
		public static string INPUT_RUN = "Run";
		public static string INPUT_LEFT_RIGHT = "Horizontal";
		public static string INPUT_FORWARD_BACKWARD = "Vertical";
		public static string INTERACT = "Interact"; // the InputManager Key
		public static string DROP_WEAPON = "DropWeaponRight";

		//tags
		public static string PICKUP_TAG = "Pickup";
		public static string RIGHT_HAND_TAG = "RightHand";
		public static string LEFT_HAND_TAG = "LeftHand";
	}
}

