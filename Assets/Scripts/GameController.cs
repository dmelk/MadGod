using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using Zenject;

public class GameController : MonoBehaviour {

	[SerializeField] private UnityStandardAssets.ImageEffects.Grayscale grayscale;
	[SerializeField] private GUIText deathLabel;
	[SerializeField] private GUIText rebornLabel;
	[SerializeField] private KnightCharacter player;
	[SerializeField] private float rebornWait = 5.0f; 
	[SerializeField] private GameObject canvas;
	[SerializeField] private GameObject pauseMenuPrefab;
	[SerializeField] private GameObject actionButtonPrefab;
	[SerializeField] private GameObject dialgueWindowPrefab;

	[Inject]
	private DiContainer container;

	private Transform actionButtonTarget;
	private GameObject actionButton;
	private GameObject pauseMenu;
	private GameObject dialogueWindow;
	private bool isPaused;
	private bool reborn;
	private bool previousDeadState;
	private int deathCount = 0;
	private bool timeStopped = false;
	private string[] deadTexts = {
		"Don't worry,\ndeath is just part of game process.", 
		"Nice try! Seems you are understanding.",
		"Everything has the end \nand everything has the beginning.",
		"Once more? Death is just a feeling..."
	};

	private void Awake()
	{
		Time.timeScale = 1f;
		this.Reset ();
		this.previousDeadState = this.player.dead;
	}

	private void Reset() 
	{
		this.deathLabel.enabled = false;
		this.rebornLabel.enabled = false;
		this.grayscale.isActive = false;
		this.reborn = false;
		this.isPaused = false;
	}

	private void FixedUpdate()
	{
		if (!this.previousDeadState && this.player.dead) {
			this.reborn = false;
			this.deathLabel.enabled = true;
			this.grayscale.isActive = true;
			StartCoroutine (this.allowReborn ());
		}
		this.previousDeadState = this.player.dead;

		if (this.reborn && Input.GetKeyDown (KeyCode.R)) {
			this.Reset ();
			this.player.Reset ();

			GameObject[] respawnPoints = GameObject.FindGameObjectsWithTag ("RespawnPoint");
			GameObject closestPoint = null;
			float distance = Mathf.Infinity;
			Vector3 playerPosition = this.player.transform.position;

			foreach (GameObject respawnPoint in respawnPoints) {
				Vector3 diff = respawnPoint.transform.position - playerPosition;
				float currDistance = diff.sqrMagnitude;
				if (currDistance < distance) {
					currDistance = distance;
					closestPoint = respawnPoint;
				}
			}

			this.player.transform.position = new Vector3 (closestPoint.transform.position.x, closestPoint.transform.position.y, this.player.transform.position.z);
		}
	}

	void Update() {
		this.UpdateActionButtonPosition ();

		if (Input.GetKeyDown (KeyCode.Escape)) {
			if (this.isPaused) {
				this.Resume ();
			} else {
				this.isPaused = true;
				this.timeStopped = (Time.timeScale == 0f);
				Time.timeScale = 0f;
				// remove dead label
				if (this.player.dead) {
					if (this.reborn) {
						this.rebornLabel.enabled = false;
					} else {
						this.deathLabel.enabled = false;
					}
				}
				this.pauseMenu = this.container.InstantiatePrefab (this.pauseMenuPrefab);
				this.pauseMenu.transform.SetParent (this.canvas.transform, false);
				this.pauseMenu.GetComponent<RectTransform> ().localPosition = new Vector3 (0.0f, 0.0f, 0.0f);
				this.pauseMenu.transform.localScale = new Vector3 (1, 1, 1);
				foreach (Button button in this.pauseMenu.GetComponentsInChildren<Button> ()) {
					if (button.name == "ResumeBtn") {
						button.onClick.AddListener (Resume);
					} else if (button.name == "MainMenuBtn") {
						button.onClick.AddListener (ToMainMenu);
					} else if (button.name == "ExitBtn") {
						button.onClick.AddListener (ExitGame);
					}
				}
			}
		}
	}

	IEnumerator allowReborn()
	{
		yield return new WaitForSeconds (this.rebornWait);

		this.rebornLabel.text = this.deadTexts[(this.deathCount >= this.deadTexts.Length)? this.deadTexts.Length - 1 : this.deathCount] 
			+ "\nPress 'R' to reborn";

		this.deathLabel.enabled = false;
		this.rebornLabel.enabled = true;

		this.deathCount ++;
		this.reborn = true;
	}

	public void Resume() {
		// remove pause menu
		this.isPaused = false;
		Destroy (this.pauseMenu);
		// greyscale if dead
		if (this.player.dead) {
			if (this.reborn) {
				this.rebornLabel.enabled = true;
			} else {
				this.deathLabel.enabled = true;
			}
		}
		// resume game
		if (!this.timeStopped) 
			Time.timeScale = 1f;
	}

	public void ToMainMenu() {
		UnityEngine.SceneManagement.SceneManager.LoadScene ("MainMenu");
	}

	public void ExitGame() {
		Application.Quit ();
	}

	public void ShowActionButton(Transform target) {
		if (this.actionButton != null)
			this.HideActionButton ();
		this.actionButton = Instantiate (this.actionButtonPrefab) as GameObject;
		this.actionButton.transform.SetParent (this.canvas.transform, false);
		this.actionButton.transform.localScale = new Vector3 (1, 1, 1);
		this.actionButtonTarget = target;
		this.UpdateActionButtonPosition ();
	}

	public void HideActionButton() {
		if (this.actionButton != null)
			Destroy (this.actionButton);
	}

	public void UpdateActionButtonPosition() {
		if (this.actionButton == null)
			return;
		Vector2 point = Camera.main.WorldToScreenPoint (this.actionButtonTarget.position);
		point = point - this.canvas.GetComponent<RectTransform> ().anchoredPosition;
		point.y = point.y + 100.0f;
		this.actionButton.GetComponent<RectTransform> ().anchoredPosition = point;
	}

	public void ShowTalkMenu (DragonNPC npc) {
		Time.timeScale = 0f;

		this.dialogueWindow = this.container.InstantiatePrefab (this.dialgueWindowPrefab);
		this.dialogueWindow.transform.SetParent (this.canvas.transform, false);
		this.dialogueWindow.GetComponent<RectTransform> ().localPosition = new Vector3 (0.0f, 0.0f, 0.0f);
		this.dialogueWindow.transform.localScale = new Vector3 (1, 1, 1);
		DialogueWindow window = this.dialogueWindow.GetComponent<DialogueWindow> ();
		window.gameController = this;
		window.dialog = npc.GetDialog ();
		window.dialog.Start ();
	}

	public void EndDialogue() {
		Destroy (this.dialogueWindow);

		Time.timeScale = 1f;
	}

}
