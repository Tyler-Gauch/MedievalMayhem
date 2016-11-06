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

		public bool hasLifeTime = false;

		[Tooltip("The time, in seconds, for the object to expire")]
		public float timeToLive = 10;

		protected float _timeAlive = 0;

		protected virtual void Awake() {
			//first funciton claled
		}

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

			if (this.hasLifeTime) {
				this._timeAlive += Time.deltaTime;

				if (this._timeAlive >= this.timeToLive) {
					GameObject.Destroy (this.gameObject);
				}
			}
		}

		public virtual string GetGameObjectName (){
			return "GameObject";
		}
	}
}
