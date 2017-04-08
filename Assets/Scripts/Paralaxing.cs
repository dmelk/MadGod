using UnityEngine;
using System.Collections;

public class Paralaxing : MonoBehaviour {

	[SerializeField]private Transform[] backgrounds;
	[SerializeField]private float smoothing = 1.0f;
	[SerializeField]private float multiplierY = 1.0f;

	private Transform camera;
	private Vector3 previousCameraPosition;
	private float[] paralaxScales;

	void Awake() {
		this.camera = Camera.main.transform;
	}

	// Use this for initialization
	void Start () {
		this.previousCameraPosition = this.camera.position;

		this.paralaxScales = new float[this.backgrounds.Length];

		for (int i = 0; i < this.backgrounds.Length; i++) {
			this.paralaxScales [i] = backgrounds [i].position.z*-1;
		}
	}
	
	// Update is called once per frame
	void Update () {
		for (int i = 0; i < this.backgrounds.Length; i++) {
			float paralaxX = (this.previousCameraPosition.x - this.camera.position.x) * this.paralaxScales [i];
			float backgroundTargetPositionX = this.backgrounds [i].position.x + paralaxX;
			float backgroundTargetPositionY = this.backgrounds [i].position.y;// + (this.camera.position.y - this.previousCameraPosition.y) * this.multiplierY;

			Vector3 backgroundTargetPosition = new Vector3 (backgroundTargetPositionX, backgroundTargetPositionY, this.backgrounds [i].position.z);

			this.backgrounds [i].position = Vector3.Lerp (this.backgrounds [i].position, backgroundTargetPosition, this.smoothing * Time.deltaTime);
		}

		this.previousCameraPosition = this.camera.position;
	}
}
