using UnityEngine;
using System.Collections;
using MedievalMayhem.Utilities.Event;

namespace MedievalMayhem.Entites {
	[RequireComponent(typeof(HealthSystem))]
	public class BaseEntity : BaseGameObject
	{
		protected HealthSystem _health;

		protected override void OnEnable() {
			base.OnEnable ();
			EventManager.StartListening (this.gameObjectName + "Dead", Dead);
		}

		protected override void OnDisable() {
			base.OnDisable ();
			EventManager.StopListening (this.gameObjectName + "Dead", Dead);
		}

		protected override void Start() {
			base.Start ();
			this._health = GetComponent<HealthSystem> ();
			this._health.DeadEvent = this.gameObjectName + "Dead";
		}

		protected override void Update () {
			base.Update ();
		}

		//overrides the name so that all entites have
		//a name that starts with Entity unless changed in the editor
		protected override string GetBaseEventTagName() {
			return "Entity";
		}

		protected virtual void Dead() {
			EventManager.StopListening (this.gameObjectName + "Dead", Dead);
			GameObject.Destroy (gameObject);
		}
	}
}
