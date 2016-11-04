using UnityEngine;
using System.Collections;
using MedievalMayhem.Utilities.Event;

namespace MedievalMayhem.References.Events {
	public class TriggerEventsExample : MonoBehaviour
	{		
		// Update is called once per frame
		void Update ()
		{
			if (Input.GetKeyDown ("q")) {
				EventManager.TriggerEvent ("eventsExample1");
			} else if (Input.GetKeyDown ("w")) {
				EventManager.TriggerEvent ("eventsExample2");
			}
		}
	}
}

