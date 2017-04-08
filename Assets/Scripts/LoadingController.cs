using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingController : MonoBehaviour {

	void Start () {
		UnityEngine.SceneManagement.SceneManager.LoadScene ("MainMenu");
	}
	
}
