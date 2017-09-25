using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;
	

public class AnimalControl : MonoBehaviour {

	public ArrayList DeleteList; // Animals that are going to delete

	public GameObject[] myAnimalObjectList; // To store 7 different animals

	public Animal[][] WorkAnimalList;  // Animals currently shown on the screen

	public Text TimerText;
	public Text ScoreText;

	private int Timer;
	private int Score;


	// Use this for initialization
	void Start () {


		WorkAnimalList = new Animal[7] [];
		for (int i = 0; i < 7; i++) {
			WorkAnimalList [i] = new Animal[7];
		}

		DeleteList = new ArrayList ();

		// Add 7x7 animals onto screen
		for (int i = 0; i < 7; i++) {
			for (int j = 0; j < 7; j++) {

				AddAnimal (i, j);

			}
		}

		Timer = 60;
		Score = 0;

		StartCoroutine (Clock ());

	}

	IEnumerator Clock(){
		while (Timer > 0) {
			yield return new WaitForSeconds (1);
			Timer = Timer - 1;
		}
	}
		

	// Randomly pick an animal to create and store all the info it has to WorkAnimalList
	void AddAnimal(int row, int column){
		
		GameObject obj;

		int index = Random.Range (0, myAnimalObjectList.Length);
		obj = Instantiate (myAnimalObjectList [index]);
		//obj.transform.position = new Vector3 (j-3, 1.5f-i, 0);

		Animal ani = obj.GetComponent<Animal> () as Animal;
		ani.myAnimalControl = this;
		ani.type = index;

		ani.rowNumber = row;
		ani.columnNumber = column;
		ani.UpdatePosition ();
		WorkAnimalList [row] [column] = ani;
	}
	
	// Update is called once per frame
	void Update () {
		
		StartCoroutine (RefreshAnimal()); // keep checking if there are anything left in DeleteList
	}

	Animal firstAnimal;
	Animal secondAnimal;

	// Choose two animals, if they can be exchanged, then do it
	public void SelectAnimal(Animal ani){

		if (firstAnimal == null) {// Choose the first animal
			
			firstAnimal = ani;
			firstAnimal.SetSelectAnimal (true);

		} else { // choose the second
			
			secondAnimal = ani;
			firstAnimal.SetSelectAnimal (false);

			if (CheckAdj (firstAnimal, secondAnimal)) { // check if they can exchange with each other
				                                        // if true, change the position

				ChangePosition (firstAnimal, secondAnimal);


				if (CheckDelete ()) {

					StartCoroutine (DeleteAnimal ()); // after the exchange, check if there are animals can be deleted

				} else {
					ChangePosition (secondAnimal, firstAnimal);

				}

				firstAnimal = null;
				secondAnimal = null;


			} else { // else, set the second chosen animal as first chosen animal
				firstAnimal = ani;
				firstAnimal.SetSelectAnimal (true);
				secondAnimal = null;
			}

		}

		//StartCoroutine (DeleteAnimal()); // after the exchange, check if there are animals can be deleted
	}


	void ChangePosition(Animal firstAnimal, Animal secondAnimal){ // Change the position between two animals

		int temp;

		temp = firstAnimal.rowNumber;
		firstAnimal.rowNumber = secondAnimal.rowNumber;
		secondAnimal.rowNumber = temp;

		temp = firstAnimal.columnNumber;
		firstAnimal.columnNumber = secondAnimal.columnNumber;
		secondAnimal.columnNumber = temp;

		WorkAnimalList [firstAnimal.rowNumber] [firstAnimal.columnNumber] = firstAnimal;
		WorkAnimalList [secondAnimal.rowNumber] [secondAnimal.columnNumber] = secondAnimal;

		firstAnimal.iTweenUpdatePosition();
		secondAnimal.iTweenUpdatePosition();
	}


	bool CheckAdj(Animal a1, Animal a2){ // if two chosen animals are next to each other, then return true
		
		if (a1.rowNumber == a2.rowNumber && a1.columnNumber == a2.columnNumber + 1) {
			return true;
		} else if (a1.rowNumber == a2.rowNumber && a1.columnNumber == a2.columnNumber - 1) {
			return true;
		} else if (a1.columnNumber == a2.columnNumber && a1.rowNumber == a2.rowNumber + 1) {
			return true;
		} else if (a1.columnNumber == a2.columnNumber && a1.rowNumber == a2.rowNumber - 1) {
			return true;
		} else {
			return false;
		}
			
	} 


	private int increaseValue = 0;

	void RemoveAnimal(Animal ani){ // delete the animal and refill the screen
		
		ani.RemoveAnimal ();
		increaseValue = (ani.type + 1) * 30;

		Texts.instance.IncreaseScore(increaseValue); // add score

		for (int i = ani.rowNumber; i > 0; i--) { // for those animals above the removed one, get down for 1 row
			WorkAnimalList [i] [ani.columnNumber] = WorkAnimalList [i - 1] [ani.columnNumber];
			WorkAnimalList [i] [ani.columnNumber].rowNumber = i; // remmber to update the info
			WorkAnimalList [i] [ani.columnNumber].iTweenUpdatePosition ();
		}
		AddAnimal (0, ani.columnNumber); // refill the empty position
	}

	IEnumerator DeleteAnimal(){

		yield return new WaitForSeconds(.1f);
		DoDeleteAnimal ();
		
		/*if (CheckDelete ()) {
			yield return new WaitForSeconds(.1f);
			DoDeleteAnimal ();
		}*/
	}

	IEnumerator RefreshAnimal(){

		while (CheckDelete ()) {
			yield return new WaitForSeconds(.5f);
			DoDeleteAnimal ();
		}
	}

	bool CheckDelete(){ // check if there are any animal in screen that can be deleted
		                // if there are, add them to deleteList
		DeleteList.Clear ();
		bool result = false;

		for (int i = 0; i < 7; i++) { // check by columns
			for (int j = 0; j < 5; j++) {
				if (WorkAnimalList [i] [j].type == WorkAnimalList [i] [j + 1].type
				   && WorkAnimalList [i] [j].type == WorkAnimalList [i] [j + 2].type) {

					AddToDeleteList (WorkAnimalList [i] [j]);
					AddToDeleteList (WorkAnimalList [i] [j+1]);
					AddToDeleteList (WorkAnimalList [i] [j+2]);

					result = true;

				}
			}
		}

		for (int i = 0; i < 5; i++) { // check by rows
			for (int j = 0; j < 7; j++) {
				if (WorkAnimalList [i] [j].type == WorkAnimalList [i+1] [j].type
					&& WorkAnimalList [i] [j].type == WorkAnimalList [i+2] [j].type) {

					AddToDeleteList (WorkAnimalList [i] [j]);
					AddToDeleteList (WorkAnimalList [i+1] [j]);
					AddToDeleteList (WorkAnimalList [i+2] [j]);

					result = true;

				}
			}
		}

		return result;
	}


	void AddToDeleteList(Animal ani){ // Add animal to deleteList
		if (DeleteList.IndexOf (ani) == -1) { // Make sure each animal will be added for once
			DeleteList.Add (ani);
		}
	}

	void DoDeleteAnimal(){ // delete all the animals in deleteList
		foreach (Animal ani in DeleteList) {
			RemoveAnimal (ani);
		}
		DeleteList.Clear ();
	}


}
