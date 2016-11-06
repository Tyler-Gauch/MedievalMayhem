using UnityEngine;
using System.Collections;
using MedievalMayhem.Utilities;
using MedievalMayhem.Entites;

namespace MedievalMayhem.Utilities.Regions {
	public class InstantKillRegion : BaseRegion
	{
		[Tooltip("The message to display to the user")]
		public string warningMessage;

		protected override void OnTriggerEnter(Collider hit) {
			HealthSystem health = hit.gameObject.GetComponent<HealthSystem> ();

			//someone got out of the map some how 
			//someone fell off the map etc
			if (health != null) {
				health.Kill ();
				if (hit.CompareTag (GlobalUtilities.LOCAL_PLAYER_TAG)) {
					GlobalUtilities.ShowWarningMessage ("You have fallen to your death");
				}
			} else if (hit.gameObject.layer != GlobalUtilities.NOT_REGION_TARGET){
				//something like a gun fell off the map and its targetable by the region
				GameObject.Destroy(hit.gameObject);
			}
				
		}
	}
}
