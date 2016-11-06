using UnityEngine;
using System.Collections;
using MedievalMayhem.Utilities;

namespace MedievalMayhem.Weapons {
	[RequireComponent(typeof(BoxCollider))]
	public class MeleeWeaponHitZone : BaseGameObject
	{
		private BoxCollider _hitZone;
		private bool _hitZoneOn = false;
		public bool HitZoneOn {
			get { 
				return this._hitZoneOn;
			}

			set { 
				this._hitZoneOn = value;
			}
		}
		// Use this for initialization
		protected override void Start ()
		{
			this._hitZone = GetComponent<BoxCollider> ();
		}

		protected override void Update ()
		{
			this._hitZone.enabled = HitZoneOn;
		}
	}
}

