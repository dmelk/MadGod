using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using FullmetalKobzar.MadGod.Battle;
using Zenject;

public class BattleController : MonoBehaviour {

	[SerializeField]private GameObject characterPrefab;
	[SerializeField]private GameObject zombieBossPrefab;
	[SerializeField]private GameObject[] zombiesPrefab;
	[SerializeField]private GameObject battleModeWindowPrefab;
	[SerializeField]private GameObject actionButtonPrefab;
	[SerializeField]private GameObject targetButtonPrefab;
	[SerializeField]private GameObject canvas;
	[SerializeField]private GameObject pauseMenuPrefab;
	[SerializeField]private GameObject actionButtonsContentPanel;
	[SerializeField]private Text battleLog;
	[SerializeField]private RectTransform actionButtonsPanel;

	private string[,] zombies;

	private GameObject pauseMenu;

	private bool isPaused;
	private int turn;
	private int currentInitiative;
	private int animationFrames;
	private bool waitForPlayer;
	private bool combatFinished;
	private bool leftPlayer;

	private int maxLeft = 1;
	private int maxRight = 4;

	private BattleKnightCharacter[] leftSide;
	private BattleKnightCharacter[] rightSide;

	private List<BattleKnightCharacter> currentActingPlayers;

	private List<GameObject> targetButtons;

	private BattleField battleField;

	[Inject]
	private ICharacterFactory characterFactory;

	[Inject]
	private DiContainer container;

	public void Awake() {
		Random.InitState ((int)System.DateTime.Now.Ticks);

		this.zombies = new string[2, 2] {
			{ "Зомби", "level1meele" },
			{ "Зомби стрелок", "level1ranged" },
		};

		this.targetButtons = new List<GameObject> ();

		this.currentActingPlayers = new List<BattleKnightCharacter> ();
		this.turn = 1;
		this.waitForPlayer = false;
		this.currentInitiative = 10;
		this.animationFrames = 0;
		this.battleLog.text = "Ход " + this.turn;

		this.combatFinished = false;

		this.leftSide = new BattleKnightCharacter[this.maxLeft];
		this.rightSide = new BattleKnightCharacter[this.maxRight];

		this.battleField = new BattleField () {
			turn = this.turn,
			leftSide = new Character[this.maxLeft],
			rightSide = new Character[this.maxRight]
		};

		this.InstantiatePlayer ();
		this.InstantiateZombies ();

		RectTransform rectTransform = this.actionButtonsContentPanel.GetComponent<RectTransform> ();
		Vector3 rectPosition = rectTransform.position;
		rectPosition.y = 0f;
		rectTransform.position = rectPosition;
	}

	public void InstantiatePlayer () 
	{
		string tag = "BattlePosition";

		Vector2 characterPosition = new Vector2 (Random.Range (0, 2), Random.Range (0, 2));
		GameObject[] possiblePositions = GameObject.FindGameObjectsWithTag (tag);
		for (var j = 0; j < possiblePositions.Length; j++) {
			Vector2 possiblePosition = possiblePositions [j].GetComponent<BattlePosition> ().position;
			if (possiblePosition != characterPosition)
				continue;
			GameObject gameCharacter;
			gameCharacter = Instantiate (this.characterPrefab, possiblePositions [j].transform.position, possiblePositions [j].transform.rotation) as GameObject;
			this.leftSide [0] = gameCharacter.GetComponent<BattleKnightCharacter> ();
			this.leftSide [0].leftSide = true;
			Character character;
			character = this.characterFactory.create ("Посланник", "player");
			this.battleField.leftSide [0] = character;
			character.position = characterPosition;
			this.leftSide [0].SetCharacter (character, this.canvas);

			Vector3 gamePosition = gameCharacter.transform.position;
			gamePosition.y += this.leftSide [0].offsetY;
			gameCharacter.transform.position = gamePosition;
			break;
		}
	}

	public void InstantiateZombies () 
	{
		string tag = "EnemyPosition";

		Vector2[] usedPositions = new Vector2[this.maxRight];

		// instantiate zombies ;)
		for (int i = 0; i < this.maxRight; i++) {
			bool usedPosition = false;
			Vector2 characterPosition;
			do {
				usedPosition = false;
				characterPosition = new Vector2 (Random.Range (0, 2), Random.Range (0, 2));
				for (int j = 0; j < i; j++) {
					usedPosition = usedPosition || (characterPosition == usedPositions [j]);
				}
			} while (usedPosition);
			usedPositions [i] = characterPosition;
			GameObject[] possiblePositions = GameObject.FindGameObjectsWithTag (tag);
			for (var j = 0; j < possiblePositions.Length; j++) {
				Vector2 possiblePosition = possiblePositions [j].GetComponent<BattlePosition> ().position;
				if (possiblePosition != characterPosition)
					continue;
				GameObject gameCharacter;
				int isMeele = Random.Range (0, 2);
				if (i == 0) {
					gameCharacter = Instantiate (this.zombieBossPrefab, possiblePositions [j].transform.position, possiblePositions [j].transform.rotation) as GameObject;
				} else {
					gameCharacter = Instantiate (this.zombiesPrefab[isMeele], possiblePositions [j].transform.position, possiblePositions [j].transform.rotation) as GameObject;
				}
				this.rightSide [i] = gameCharacter.GetComponent<BattleKnightCharacter> ();
				this.rightSide [i].Flip ();
				this.rightSide [i].leftSide = false;
				Character character;
				if (i == 0) {
					character = this.characterFactory.create ("Зомби босс", "level1boss");
					this.battleField.rightSide [i] = character;
				} else {
					character = this.characterFactory.create (this.zombies[isMeele, 0], this.zombies[isMeele, 1]);
					this.battleField.rightSide [i] = character;
				}
				character.position = characterPosition;
				this.rightSide [i].SetCharacter (character, this.canvas);

				Vector3 gamePosition = gameCharacter.transform.position;
				gamePosition.y += this.rightSide [i].offsetY;
				gameCharacter.transform.position = gamePosition;
				break;
			}
		}

	}

	void FixedUpdate() {
		if (this.combatFinished) {
			if (Input.GetKey (KeyCode.R)) {
				UnityEngine.SceneManagement.SceneManager.LoadScene ("BattleScene");
			}
			return;
		}

		// wait for animation
		if (this.animationFrames > 0) {
			this.animationFrames--;
			return;
		}

		// skip if we are waiting for player
		if (this.waitForPlayer) {
			return;
		}

		// check if one of sides is dead
		bool allLeftDead = true;
		for (int i = 0; i < this.maxLeft; i++) {
			allLeftDead = allLeftDead && this.leftSide [i].character.IsDead ();
		}
		bool allRightDead = true;
		for (int i = 0; i < this.maxRight; i++) {
			allRightDead = allRightDead && this.rightSide [i].character.IsDead ();
		}

		this.combatFinished = allLeftDead || allRightDead;
		if (this.combatFinished) {
			this.battleLog.text = "Бой окончен. Press 'R' to return to main menu" + "\n" + this.battleLog.text;
			return;
		}

		// determinate who can strike on current initiative
		if (this.currentActingPlayers.Count == 0) {
			if (this.currentInitiative < 0) {
				this.NewBattleRound ();
				return;
			}
			for (int i = 0; i < this.maxLeft; i++) {
				if (this.leftSide [i].character.CanAct (this.currentInitiative)) {
					this.currentActingPlayers.Add (this.leftSide [i]);
				}
			}
			for (int i = 0; i < this.maxRight; i++) {
				if (this.rightSide [i].character.CanAct (this.currentInitiative)) {
					this.currentActingPlayers.Add (this.rightSide [i]);
				}
			}
			for (int n = 0; n < this.currentActingPlayers.Count; n++) {  
				int k = Random.Range (n, this.currentActingPlayers.Count);
				BattleKnightCharacter value = this.currentActingPlayers[k];  
				this.currentActingPlayers[k] = this.currentActingPlayers[n];  
				this.currentActingPlayers[n] = value;  
			}
			if (this.currentActingPlayers.Count == 0) {
				this.currentInitiative--;
			} else {
				this.battleLog.text = "Инициатива " + this.currentInitiative + "\n" + this.battleLog.text;;
			}
		} else {
			foreach (BattleKnightCharacter actingPlayer in this.currentActingPlayers) {
				if (!actingPlayer.character.CanAct (this.currentInitiative)) {
					this.currentActingPlayers.Remove (actingPlayer);
					break;
				}
				this.battleLog.text = "Ход игрока " + actingPlayer.character.name + "\n" + this.battleLog.text;
				if (actingPlayer.character.isPlayer) {
					this.waitForPlayer = true;
					// generate here action buttons
					foreach (Skill playerSkill in actingPlayer.character.PossibleSkills ()) {
						GameObject actionButton = Instantiate (this.actionButtonPrefab) as GameObject;
						actionButton.transform.SetParent (this.actionButtonsContentPanel.transform, false);
						actionButton.transform.localScale = new Vector3 (1, 1, 1);

						string skillButtonTitle = playerSkill.title;
						if (!playerSkill.isUnlimited) {
							skillButtonTitle += " (" + playerSkill.maxUsages.ToString () + ")";
						}
						actionButton.GetComponentInChildren<Text> ().text = skillButtonTitle;

						Button button = actionButton.GetComponent<Button> ();
						Skill tmpSkill = playerSkill;
						BattleKnightCharacter tmpPlayer = actingPlayer;
						button.onClick.AddListener (() => UseSkill (playerSkill, tmpPlayer));
					}
				} else {
					this.animationFrames = 50;
					actingPlayer.ActionAnimation ("attack");
					this.drawReports (actingPlayer.character.Act (this.battleField, actingPlayer.leftSide));
				}
				break;
			}
			if (this.currentActingPlayers.Count == 0) {
				this.currentInitiative--;
			}
		}
	}

	void Update () {
		/*
		if (Input.GetKeyDown (KeyCode.Escape)) {
			if (this.isPaused) {
				this.Resume ();
			} else {
				this.isPaused = true;
				Time.timeScale = 0f;
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
		*/
	}

	public void Resume() {
		// remove pause menu
		this.isPaused = false;
		Destroy (this.pauseMenu);
		Time.timeScale = 1f;
	}

	public void ToMainMenu() {
		UnityEngine.SceneManagement.SceneManager.LoadScene ("MainMenu");
	}

	public void ExitGame() {
		Application.Quit ();
	}

	private void drawReports (List<EffectReport> reports) {
		for (int i = 0; i < reports.Count; i++) {
			EffectReport report = reports[i];
			string text = report.owner.name + ": ";
			if (report.type == EffectReportType.DAMAGE) {
				text += "урон ";
			} else if (report.type == EffectReportType.HEAL) {
				text += "лечение ";
			}
			text += (report.value > 0f) ? report.value.ToString("F2") : report.text;

			this.battleLog.text = text + "\n" + this.battleLog.text;;	
		}
	}

	public void NewBattleRound() {
		this.turn++;
		this.battleField.turn = this.turn;
		for (int i = 0; i < this.maxLeft; i++) {
			if (this.leftSide [i].character.IsDead ())
				continue;
			this.drawReports (this.leftSide [i].character.Update ());
		}
		for (int i = 0; i < this.maxRight; i++) {
			if (this.rightSide [i].character.IsDead ())
				continue;
			this.drawReports (this.rightSide [i].character.Update ());
		}
		this.waitForPlayer = false;
		this.currentInitiative = 10;
		this.animationFrames = 0;
		this.battleLog.text = "Ход " + this.turn + "\n" + this.battleLog.text;;
	}

	public void UseSkill (Skill skill, BattleKnightCharacter character) {
		List<Character> possibleTargets = new List<Character> ();
		possibleTargets.AddRange (this.battleField.GetPossibleTargets (character.character, character.leftSide, skill.targetType));

		List<BattleKnightCharacter> unityTargets = new List<BattleKnightCharacter> ();
		for (var i = 0; i < this.leftSide.Length; i++) {
			if (possibleTargets.Contains (this.leftSide [i].character)) {
				unityTargets.Add (this.leftSide [i]);
			}
		}
		for (var i = 0; i < this.rightSide.Length; i++) {
			if (possibleTargets.Contains (this.rightSide [i].character)) {
				unityTargets.Add (this.rightSide [i]);
			}
		}

		if (unityTargets.Count == 1) {
			this.SelectTarget (skill, character, unityTargets [0]);
			return;
		}

		foreach (GameObject targetButton in this.targetButtons) {
			Destroy (targetButton);
		}
		this.targetButtons.Clear ();

		foreach (BattleKnightCharacter target in unityTargets) {
			GameObject targetButton = GameObject.Instantiate (this.targetButtonPrefab) as GameObject;
			targetButton.transform.SetParent (this.canvas.transform, false);
			targetButton.transform.localScale = new Vector3 (1, 1, 1);

			Vector2 point = Camera.main.WorldToScreenPoint (target.transform.position);
			point = point - this.canvas.GetComponent<RectTransform> ().anchoredPosition;
			point.x = point.x + (target.leftSide ? 1 : -1) * 80.0f;
			targetButton.GetComponent<RectTransform> ().anchoredPosition = point;

			targetButton.GetComponentInChildren<Text> ().text = target.leftSide ? "<=" : "=>";

			Button button = targetButton.GetComponent<Button> ();
			BattleKnightCharacter tmpPlayer = target;
			button.onClick.AddListener (() => SelectTarget (skill, character, tmpPlayer));
			this.targetButtons.Add (targetButton);
		}
	}

	public void SelectTarget (Skill skill, BattleKnightCharacter character, BattleKnightCharacter target) {
		this.animationFrames = 50;
		character.ActionAnimation ("attack");

		this.battleLog.text = "Использовано умение: " + skill.title + "\n" + this.battleLog.text;;

		float hitValue = Random.Range (0, 10001) * 1.0f / 100f;
		float critValue = Random.Range (0, 10001) * 1.0f / 100f;
		float dodgeValue = Random.Range (0, 10001) * 1.0f / 100f;

		this.drawReports (skill.ApplyEffect (hitValue, critValue, dodgeValue, target.character));

		int childCount = this.actionButtonsContentPanel.transform.childCount;
		for (int i = 0; i < childCount; i++) {
			Destroy (this.actionButtonsContentPanel.transform.GetChild (i).gameObject);
		}
		foreach (GameObject targetButton in this.targetButtons) {
			Destroy (targetButton);
		}
		this.targetButtons.Clear ();
		this.waitForPlayer = false;
	}

	/*
	public void PlayerAction(AbstractAbility ability) {
		this.player.ActionAnimation (ability.name);
		this.player.character.currActionPoints -= ability.actionPoints;
		Character target = this.enemies [0].character;
		this.UseAbility (ability, target, this.playerActionResult);
		this.ActivateMonsters ();
		this.waitForPlayer = false;
		for (int i = 0; i < this.playerActionButtons.Count; i++) {
			Destroy (this.playerActionButtons [i]);
		}
		this.playerActionButtons.Clear ();
	}

	public void ActivateMonsters() {
		if (this.activeMonsters.Count == 0) 
			return;
		for (var i = 0; i < this.activeMonsters.Count; i++) {
			List<AbstractAbility> abilities = this.activeMonsters [i].character.GetPossibleAbilities ();
			activeMonsters [i].character.currActionPoints -= abilities [0].actionPoints;
			this.activeMonsters [i].ActionAnimation (abilities [0].name);
			this.UseAbility (abilities [0], this.player.character, this.enemyActionResult);
		}
	}

	*/
		
}
