using UnityEngine;
using System.Collections;
using MedievalMayhem.Entites;

namespace MedievalMayhem.Utilities {
	public class PlayerAnimationEventHandler : MonoBehaviour {

		public PlayerFighting playerFightingScript;

		// Use this for initialization
		void Start () {
			if (this.playerFightingScript == null) {
				Debug.LogError ("PlayerAnimationEventHandler used without reference to PlayerScript");
			}
		}

		/**
		 * Calls the playerfighting script to enable the hitzones on the current melee weapon
		 */
		public void TurnHitZoneOn(int side) {
			this.playerFightingScript.TurnHitZoneOn (side);
		}

		/**
		 * Calls the playerfighting script to disable the hitzones on the current melee weapon
		 */
		public void TurnHitZoneOff(int side) {
			this.playerFightingScript.TurnHitZoneOff (side);
		}
	}
}
