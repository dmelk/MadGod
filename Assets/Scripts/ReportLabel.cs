using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReportLabel : MonoBehaviour {

	[SerializeField]public Color healColor;
	[SerializeField]public Color damageColor;
	[SerializeField]public float speed = 3f;

	private Text text;

	private void Awake()
	{
		this.text = GetComponent<Text> ();
	}

	public void SetText(string text) 
	{
		this.text.text = text;
	}

	public void SetColor(Color color) 
	{
		this.text.color = color;
	}

	void FixedUpdate () {
		Vector3 position = this.transform.position;
		position.y += this.speed;
		this.transform.position = position;

		if (this.transform.position.y >= 780)
			Destroy (this.gameObject);
	}
}
