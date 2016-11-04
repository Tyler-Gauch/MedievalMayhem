using UnityEngine;
using System.Collections;
using MedievalMayhem.Utilities.Event;

/**
 * This calss is used to trigger certain events based off an array of of events.  
 * 
 * One use case is for AnimationEvents. By passing an index back from the animation event we can
 * trigger an event that has been added to the event manager and to this scripts array
 */

namespace MedievalMayhem.Utilities.Event {
	public class EventTrigger : MonoBehaviour {

		public string[] eventNames;

		public void TriggerEvent (int index) {
			if (index > eventNames.Length || index < 0) {
				Debug.LogError ("IndexOutOfBounds: EventTrigger called with invalid index '" + index + "'");
			} else {
				EventManager.TriggerEvent (eventNames[index]);
			}
		}
	}
}
