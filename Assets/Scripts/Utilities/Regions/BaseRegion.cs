using UnityEngine;
using System.Collections;
using MedievalMayhem.Utilities;

namespace MedievalMayhem.Utilities.Regions {
	[RequireComponent(typeof(BoxCollider))]
	public class BaseRegion : BaseGameObject
	{
		public const string REGION_BASE_TAG_NAME = "Region";
		 
		private BoxCollider _region;

		// Use this for initialization
		protected override void Start () {
			this._region = GetComponent<BoxCollider> ();
			this._region.isTrigger = true;
		}

		protected virtual void OnTriggerEnter(Collider hit) {
		}

		protected virtual void OnTriggerExit(Collider hit) {
		}

		protected virtual void OnTriggerStay(Collider hit) {
		}

		public override string GetGameObjectName () {
			return REGION_BASE_TAG_NAME;
		}
	}
}
