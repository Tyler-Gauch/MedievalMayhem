using UnityEngine;
using System.Collections;
using MedievalMayhem.Utilities.Event;

namespace MedievalMayhem.Entites {
	public class Enemy : BaseEntity	{

		protected override void Dead ()	{
			base.Dead ();
			Debug.Log ("You have killed: " + this.gameObjectName);
		}

	}
}

