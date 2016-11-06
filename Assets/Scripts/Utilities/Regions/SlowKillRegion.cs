using UnityEngine;

using System.Collections;
using System.Collections.Generic;

using MedievalMayhem.Utilities;
using MedievalMayhem.Entites;

namespace MedievalMayhem.Utilities.Regions { 
	public class SlowKillRegion : BaseRegion {

		[Tooltip("The damage to inflict")]
		public int damage = 30;
	
		[Tooltip("The interval in which to inflict damage (In seconds)")]
		public float damageInterval = 1;

		[Tooltip("The time before damage will be inflicted (In seconds)")]
		public float safeInterval = 5;

		//a list of the health systems currently in our region to damage
		private Dictionary<string, SlowKillData> _healthSystems;
		private float _lastDamageInterval = 0;

		private class SlowKillData	{
			public HealthSystem health;
			public float safeTimeLeft;
			public bool isPlayer;

			public SlowKillData(HealthSystem health, float safeTimeLeft, bool isPlayer) {
				this.health = health;
				this.safeTimeLeft = safeTimeLeft;
				this.isPlayer = isPlayer;
			}
		}

		protected override void Start() {
			base.Start ();
			this._healthSystems = new Dictionary<string, SlowKillData> ();
		}

		protected override void Update() {
			if (Time.realtimeSinceStartup - this._lastDamageInterval > this.damageInterval) {
				this._lastDamageInterval = Time.realtimeSinceStartup;

				foreach (KeyValuePair<string, SlowKillData> data in this._healthSystems) {
					if (data.Value.safeTimeLeft > 0) {
						data.Value.safeTimeLeft -= this.damageInterval; // remove however many seconds we waited
					} else {
						data.Value.health.Damage (this.damage);
					}
					if (data.Value.isPlayer) {
						GlobalUtilities.ShowWarningMessage ("You have " + data.Value.safeTimeLeft + " seconds to return to the battle field");
					}
				}
			}
		}

		protected override void OnTriggerEnter(Collider hit) {
			HealthSystem health = hit.gameObject.GetComponent<HealthSystem> ();

			if (health != null) {
				SlowKillData skd = new SlowKillData (health, this.safeInterval, hit.CompareTag (GlobalUtilities.LOCAL_PLAYER_TAG));
				this._healthSystems.Add (hit.GetComponent<BaseGameObject>().GetGameObjectName(), skd);
			}
		}

		protected override void OnTriggerExit(Collider hit) {
			HealthSystem health = hit.gameObject.GetComponent<HealthSystem> ();

			if (health != null) {
				this._healthSystems.Remove (hit.GetComponent<BaseGameObject>().GetGameObjectName());
				if (hit.CompareTag (GlobalUtilities.LOCAL_PLAYER_TAG)) {
					GlobalUtilities.ClearWarningText ();
				}
			}
		}

	}
}

