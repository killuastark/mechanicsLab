using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A small script to make sure that the cursor is unlocked.
//Had to be added to deal with a bug when exiting from locked cursor scene to menu screen
public class UnlockCursor : MonoBehaviour {

	// Use this for initialization
	void Awake () {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
	}
	
}
