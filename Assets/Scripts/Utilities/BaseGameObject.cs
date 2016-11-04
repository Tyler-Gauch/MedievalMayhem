using UnityEngine;
using System.Collections;

// all game obejcts inherit from this class and it provides common functionality that all gameobjects need.  
// each set of game objects such as entities and weapons have their own base classes that extend this
abstract public class BaseGameObject : MonoBehaviour
{
	private static int gameObjectNumber = 0;

	//this tag is used to create object specific
	//names that can be used create specific event names
	public string gameObjectName;

	//events can be registered here.
	protected virtual void OnEnable() {
	}

	//events can be unregistered here.
	protected virtual void OnDisable() {
	}
	// Use this for initialization
	protected virtual void Start ()
	{
		BaseGameObject.gameObjectNumber++;

		if (gameObjectName == "" || gameObjectName == null) {
			gameObjectName = GetBaseEventTagName() + gameObjectNumber;
		}
	}

	protected virtual void Update() {
		//method stub to allow for proper overriding
	}

	protected virtual string GetBaseEventTagName (){
		return "GameObject";
	}
}

