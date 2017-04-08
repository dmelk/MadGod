using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using FullmetalKobzar.Core.Dialog;
using FullmetalKobzar.Core.Translation;
using Zenject;

public class DialogueWindow : MonoBehaviour {

	private const string resource = "dialog";

	public GameObject npcReplicaPrefab;
	public GameObject playerReplicaPrefab;
	public GameObject replicaButtonPrefab;

	public GameController gameController;

	public IDialog dialog;

	private List<GameObject> replicaButtons;
	private int finalState;
	private bool isBattle;
	private bool waitForPlayer;
	private RectTransform dialogueContent;
	private RectTransform replicasContent;

	[Inject]
	private ITranslator translator;

	void Start () {
		this.replicaButtons = new List<GameObject> ();
		this.finalState = 0;
		this.isBattle = false;
		this.waitForPlayer = false;


		foreach (RectTransform rect in this.gameObject.GetComponentsInChildren<RectTransform> ()) {
			bool setPosition = false;
			if (rect.name == "DialogueContent") {
				this.dialogueContent = rect;
				setPosition = true;
			} else if (rect.name == "ReplicasContent") {
				this.replicasContent = rect;
				setPosition = true;
			}
			if (setPosition) {
				Vector2 point = rect.anchoredPosition;
				point.x = 30;
				rect.anchoredPosition = point;
			}
		}
	}
	
	void Update () {
		if (this.dialog == null) {
			return;
		}
		if (this.finalState == 2) {
			return;
		}
		if (this.finalState == 1) {
			GameObject replicaButtonObject = Instantiate (this.replicaButtonPrefab) as GameObject;
			replicaButtonObject.transform.SetParent (this.replicasContent, false);
			replicaButtonObject.transform.localScale = new Vector3 (1, 1, 1);

			replicaButtonObject.GetComponent<Text> ().text = this.translator.Translate("Dialogue ended. Click to close", DialogueWindow.resource);

			Button button = replicaButtonObject.GetComponent<Button> ();
			button.onClick.AddListener (() => EndDialogue ());
			this.finalState = 2;
			return;
		}

		if (this.waitForPlayer)
			return;

		IReplica replica = this.dialog.GetCurrentReplica ();
		if (replica is SimpleReplica) {
			SimpleReplica simpleReplica = replica as SimpleReplica;
			if (simpleReplica.IsPlayerReplica ()) {
				this.waitForPlayer = true;

				GameObject replicaButtonObject = Instantiate (this.replicaButtonPrefab) as GameObject;
				replicaButtonObject.transform.SetParent (this.replicasContent, false);
				replicaButtonObject.transform.localScale = new Vector3 (1, 1, 1);

				replicaButtonObject.GetComponent<Text> ().text = this.translator.Translate(simpleReplica.text, DialogueWindow.resource);

				Button button = replicaButtonObject.GetComponent<Button> ();
				SimpleReplica tmpReplica = simpleReplica;
				button.onClick.AddListener (() => ChooseReplica (tmpReplica));

				this.replicaButtons.Add (replicaButtonObject);
			} else {
				this.ChooseReplica (simpleReplica);
			}
			return;
		}
		CompositeReplica compositeReplica = replica as CompositeReplica;
		this.waitForPlayer = true;
		foreach (string key in compositeReplica.replicas) {
			SimpleReplica simpleReplica = this.dialog.GetReplica (key) as SimpleReplica;

			GameObject replicaButtonObject = Instantiate (this.replicaButtonPrefab) as GameObject;
			replicaButtonObject.transform.SetParent (this.replicasContent, false);
			replicaButtonObject.transform.localScale = new Vector3 (1, 1, 1);

			replicaButtonObject.GetComponent<Text> ().text = this.translator.Translate(simpleReplica.text, DialogueWindow.resource);

			Button button = replicaButtonObject.GetComponent<Button> ();
			button.onClick.AddListener (() => ChooseReplica (simpleReplica));

			this.replicaButtons.Add (replicaButtonObject);
		}
	}

	private void ChooseReplica(SimpleReplica replica) {
		foreach (GameObject replicaButtonObject in this.replicaButtons) {
			Destroy (replicaButtonObject);
		}
		this.replicaButtons.Clear ();

		GameObject replicaObject;
		if (replica.IsPlayerReplica ()) {
			replicaObject = Instantiate (this.playerReplicaPrefab) as GameObject;
		} else {
			replicaObject = Instantiate (this.npcReplicaPrefab) as GameObject;
		}
		replicaObject.transform.SetParent (this.dialogueContent, false);
		replicaObject.transform.localScale = new Vector3 (1, 1, 1);

		replicaObject.GetComponent<Text> ().text = this.translator.Translate(replica.text, DialogueWindow.resource);;

		ITransition transition = replica.GetTransition ();
		if (transition != null) {
			this.dialog.SetCurrentReplica (transition.GetToReplicaKey ());
		}
		this.isBattle = replica.IsBattle ();
		this.finalState = (replica.IsFinal ())? 1 : 0;
		this.waitForPlayer = false;
	}

	private void EndDialogue() {
		if (this.isBattle) {
			Time.timeScale = 1f;
			UnityEngine.SceneManagement.SceneManager.LoadScene ("BattleScene");
			return;
		}

		gameController.EndDialogue ();
	}
}
