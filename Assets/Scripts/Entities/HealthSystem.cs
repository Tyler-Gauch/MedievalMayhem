using UnityEngine;
using System.Collections;
using MedievalMayhem.Utilities.Event;

namespace MedievalMayhem.Entites {
	public class HealthSystem : MonoBehaviour
	{
		public int maxHealth = 100;

		//this is the event that will be called when the entity is killed
		private string _deadEvent = null;
		public string DeadEvent {
			get { 
				return this._deadEvent;
			}

			set { 
				this._deadEvent = value;
			}
		}

		//if we trigger the event we don't want to call it any more
		private bool _deadTriggered = false;

		void Update() {
			if (IsDead () && !this._deadTriggered) {
				_deadTriggered = true;
				EventManager.TriggerEvent (_deadEvent);
			}
		}

		public void Damage(int damage) {
			maxHealth -= damage;
			if (maxHealth < 0) {
				maxHealth = 0;
			}
		}

		public bool IsDead() {
			return (maxHealth == 0);
		}
	}
}
