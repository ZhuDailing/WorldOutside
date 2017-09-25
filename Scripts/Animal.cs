using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour {

	public GameObject DeleteEffect; // effect when delete animals

	public GameObject SelectAnimal; // show different appearances when selecting or not
	public GameObject NoSelectAnimal;

	public AnimalControl myAnimalControl;
	public int rowNumber;
	public int columnNumber;

	public int type; // type of animals, for counting score

	// Use this for initialization
	void Start () {
		SelectAnimal.SetActive (false);
		NoSelectAnimal.SetActive (true);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnMouseDown(){
		myAnimalControl.SelectAnimal (this); // when click the animal, get the info of this animal

	}

	public void UpdatePosition(){
		transform.position = new Vector3 (columnNumber-3, 1.5f-rowNumber, 0);

	}

	public void iTweenUpdatePosition(){ // an animation for change the position
		iTween.MoveTo (this.gameObject, new Vector3 (columnNumber-3, 1.5f-rowNumber, 0), 0.3f);
	}

	public void SetSelectAnimal(bool isSelect){ // change the appearance
		if (isSelect) {
			SelectAnimal.SetActive (true);
			NoSelectAnimal.SetActive (false);
		} else {
			SelectAnimal.SetActive (false);
			NoSelectAnimal.SetActive (true);
		}
	}

	public void RemoveAnimal(){ // destroy the current animal, show the deleteEffect, then detroy the effect
		
		Destroy (this.gameObject, 0.1f);
		GameObject obj = Instantiate (DeleteEffect, this.transform.position, Quaternion.identity) as GameObject;
		Destroy (obj, 0.5f);

	}
}
