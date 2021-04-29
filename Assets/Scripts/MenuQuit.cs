using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MenuQuit : MonoBehaviour {

	public void Quit() {
		Invoke("QuitDelay", .5f);
	}

	private void QuitDelay() {
		Application.Quit();
	}
}
