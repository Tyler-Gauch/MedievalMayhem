using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace MedievalMayhem.Utilities.Event {
	public class EventManager : MonoBehaviour {

		// list of all events and their functions
		protected Dictionary<string, UnityEvent> _eventDictionary;

		//the current manager in the game (we only want one)
		protected static EventManager _eventManager;

		// retrieve the current active eventManager
		public static EventManager instance {
			get { 
				//if its null lets create one
				if (!_eventManager) {
					// find the event manager in the scene
					_eventManager = FindObjectOfType (typeof(EventManager)) as EventManager;

					//if it hasn't been put in the scene alert the user
					if (!_eventManager) {
						Debug.LogError ("There is no EventManager in the scene.  You will need to add one to a game object in your scene!");
					} else {
						//if we found the event manager initialize it
						_eventManager.Init ();
					}
				}

				//return the event manager
				return _eventManager;
			}
		}

		// initialize the event managers dictionary
		protected void Init() {
			if (_eventDictionary == null) {
				_eventDictionary = new Dictionary<string, UnityEvent> ();
			}
		}

		// called in order to add a new event listener to the manager.
		public static void StartListening(string eventName, UnityAction listener) {
			UnityEvent currentEvent = null;

			//check if we have an event already registered
			if (instance._eventDictionary.TryGetValue (eventName, out currentEvent)) {
				//add the listener to the event
				currentEvent.AddListener (listener);
			} else {
				//if not we create a new event, add the listener, and add it to the manager
				currentEvent = new UnityEvent ();
				currentEvent.AddListener (listener);
				instance._eventDictionary.Add (eventName, currentEvent);
			}
		}

		// called in order to remove an event from the manager
		public static void StopListening(string eventName, UnityAction listener) {
			//sanity check to make sure we didn't clean up too fast
			if (_eventManager == null) {
				return;
			}
				
			UnityEvent currentEvent = null;

			//check if the event exists and remove the listener if it does
			if (instance._eventDictionary.TryGetValue (eventName, out currentEvent)) {
				currentEvent.RemoveListener (listener);
			}
		}

		// used to trigger an event
		public static void TriggerEvent(string eventName) {
			UnityEvent currentEvent = null;

			//check to see if the event exists and run the event if it does
			if (instance._eventDictionary.TryGetValue (eventName, out currentEvent)) {
				currentEvent.Invoke ();
			}
		}

	}
}