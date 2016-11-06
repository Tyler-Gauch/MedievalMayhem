using UnityEngine;
using System.Collections;

// all game obejcts inherit from this class and it provides common functionality that all gameobjects need.  
// each set of game objects such as entities and weapons have their own base classes that extend this
namespace MedievalMayhem.Utilities {
	public class BaseGameObject : MonoBehaviour
	{
		private static int gameObjectNumber = 0;

		//this tag is used to create object specific
		//names that can be used create specific event names
		public string gameObjectName;

		//events can be registered here.
		protected virtual void OnEnable() {
			BaseGameObject.gameObjectNumber++;

			if (gameObjectName == "" || gameObjectName == null) {
				gameObjectName = GetGameObjectName() + gameObjectNumber;
			}
		}

		//events can be unregistered here.
		protected virtual void OnDisable() {
		}
		// Use this for initialization
		protected virtual void Start () {
		}

		protected virtual void Update() {
			//method stub to allow for proper overriding
		}

		public virtual string GetGameObjectName (){
			return "GameObject";
		}
	}
}
