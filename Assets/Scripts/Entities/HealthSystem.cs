using UnityEngine;
using System.Collections;
using MedievalMayhem.Utilities.Event;

namespace MedievalMayhem.Entites {
	public class HealthSystem : MonoBehaviour
	{
		public int maxHealth = 100;
		private int _currentHealth;

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

		void Start() {
			this._currentHealth = this.maxHealth;
		}

		void Update() {
			if (this.IsDead () && !this._deadTriggered) {
				this._deadTriggered = true;
				EventManager.TriggerEvent (this._deadEvent);
			}
		}

		public void Damage(int damage) {
			this._currentHealth = Mathf.Max(this._currentHealth - damage, 0);
			Debug.Log ("Damaged(" + damage + ") " + this._currentHealth);
		}

		public bool IsDead() {
			return (this._currentHealth == 0);
		}

		public bool Heal(int health) {
			if (this._currentHealth < this.maxHealth) {
				this._currentHealth = Mathf.Min (this._currentHealth + health, this.maxHealth);
				return true;
			} else {
				return false;
			}
		}

		public void Kill() {
			this._currentHealth = 0;
		}
	}
}
