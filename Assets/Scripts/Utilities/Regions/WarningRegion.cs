using UnityEngine;
using System.Collections;

namespace MedievalMayhem.Utilities.Regions {
	public class WarningRegion : BaseRegion {

		protected override void OnTriggerEnter(Collider hit) {
			if (hit.CompareTag (GlobalUtilities.LOCAL_PLAYER_TAG)) {
				GlobalUtilities.ShowWarningMessage ("Warning! You are leaving the game area please turn back!");
			}
		}

		protected override void OnTriggerExit(Collider hit) {
			if (hit.CompareTag (GlobalUtilities.LOCAL_PLAYER_TAG)) {
				GlobalUtilities.ClearWarningText();
			}
		}
	}
}
