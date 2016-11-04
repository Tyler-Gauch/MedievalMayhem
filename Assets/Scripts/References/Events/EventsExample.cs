using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using MedievalMayhem.Utilities.Event;

namespace MedievalMayhem.References.Events {
	public class EventsExample : MonoBehaviour
	{
		private UnityAction exampleListener;

		void Awake() {
			exampleListener = new UnityAction (ExampleFunction);
		}

		//add 
		void OnEnable() {
			EventManager.StartListening ("eventsExample1", exampleListener);
			EventManager.StartListening ("eventsExample2", Example2Function);
		}

		void OnDisable() {
			EventManager.StopListening ("eventsExample1", exampleListener);
			EventManager.StopListening ("eventsExample2", Example2Function);
		}

		void ExampleFunction() {
			Debug.Log ("Running an Event Woot Woot!!");
		}

		void Example2Function() {
			Debug.Log ("This was created without a UnityAction!!!");
		}
	}
}

