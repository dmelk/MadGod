using UnityEngine;
using System.Collections;
using FullmetalKobzar.Core.Dialog;
using Zenject;

public class DragonNPC : MonoBehaviour {

	[Inject]
	private IDialogFactory dialogFactory;

	void Start () {
		BoxCollider2D boxCollider = this.GetComponent<BoxCollider2D> ();
		CircleCollider2D circleCollider = this.GetComponent<CircleCollider2D> ();

		Physics2D.IgnoreCollision (boxCollider, circleCollider);
	}

	public IDialog GetDialog () {
		return this.dialogFactory.Create ("dragon");
	}
}
