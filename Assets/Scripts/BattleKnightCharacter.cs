using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using FullmetalKobzar.MadGod.Battle;

public class BattleKnightCharacter : MonoBehaviour {

	private Animator mAnimator;
	private bool attack = false;
	private bool specialAttack = false;
	private SpriteRenderer spriteRenderer;

	[SerializeField]public GameObject healthPanelPrefab;
	[SerializeField]public float offsetY = 0f;
	[SerializeField]public GameObject reportLabelPrefab;

	private Text healthBarText;

	private GameObject canvas;

	public Character character;

	public bool leftSide;

	private int debuffCount;
	private int buffCount;

	private void Awake()
	{
		this.mAnimator = GetComponent<Animator> ();
		this.spriteRenderer = GetComponent<SpriteRenderer> ();
		this.debuffCount = 0;
		this.buffCount = 0;
	}

	void FixedUpdate () {
		this.mAnimator.SetBool("Dead", this.character.IsDead());
		this.mAnimator.SetBool("Attack", this.attack);
		this.mAnimator.SetBool("SpecialAttack", this.specialAttack);

		this.attack = false;
		this.specialAttack = false;

		int characterDebuffs = this.character.debuffs.Count;
		int characterBuffs = this.character.buffs.Count;
		bool debuffChanged = (this.debuffCount == 0 && characterDebuffs > 0) || (this.debuffCount != 0 && characterDebuffs == 0);
		bool buffChanged = (this.buffCount == 0 && characterBuffs > 0) || (this.buffCount != 0 && characterBuffs == 0);
		this.debuffCount = characterDebuffs;
		this.buffCount = characterBuffs;

		if (debuffChanged || buffChanged) {
			if (this.debuffCount > 0 && this.buffCount > 0) {
				this.SetColor (new Color (255f, 255f, 0f));
			} else if (this.debuffCount > 0) {
				this.SetColor (new Color (255f, 0f, 0f));
			} else if (this.buffCount > 0) {
				this.SetColor (new Color (0f, 255f, 0f));
			} else {
				this.SetColor (new Color (255f, 255f, 255f));
			}
		}

		if (this.reportLabelPrefab && this.character.reports.Count > 0) {
			foreach (EffectReport report in this.character.reports) {
				GameObject reportLabelObj = GameObject.Instantiate (this.reportLabelPrefab) as GameObject;
				reportLabelObj.transform.SetParent (this.canvas.transform, false);
				reportLabelObj.transform.localScale = new Vector3 (1, 1, 1);

				Vector2 point = Camera.main.WorldToScreenPoint (this.transform.position);
				point.x = point.x - this.canvas.GetComponent<RectTransform> ().anchoredPosition.x;
				point.y = point.y + 50f;
				reportLabelObj.GetComponent<RectTransform> ().anchoredPosition = point;

				ReportLabel reportLabel = reportLabelObj.GetComponent<ReportLabel> ();

				if (report.type == EffectReportType.HEAL) {
					reportLabel.SetText (report.value.ToString ("F2"));
					reportLabel.SetColor (reportLabel.healColor);
				} else if (report.type == EffectReportType.DAMAGE) {
					if (report.isCrit) {
						reportLabel.SetText ("**" + report.value.ToString ("F2") + "**");
					} else {
						reportLabel.SetText (report.value.ToString ("F2"));
					}
					reportLabel.SetColor (reportLabel.damageColor);
				} else if (report.isMiss) {
					reportLabel.SetText ("Промах");
				} else if (report.isDodge) {
					reportLabel.SetText ("Уклон");
				} else if (report.isEffectAdd || report.isEffectRemove) {
					reportLabel.SetText (report.effectName);
				} else {
					reportLabel.SetText ("");
				}
			}
			this.character.reports.Clear ();
		}
	}

	void Update() {
		if (this.character == null || this.healthBarText == null)
			return;

		this.healthBarText.text = this.character.currHealth.ToString("F2") + " / " + this.character.maxHealth.ToString("F2");
	}
	
	public void ActionAnimation(string actionName) {
		if (actionName == "attack") {
			this.attack = true;
		} else {
			this.specialAttack = true;
		}
	}

	public void Flip()
	{
		// Multiply the player's x local scale by -1.
		Vector3 theScale = this.transform.localScale;
		theScale.x *= -1;
		this.transform.localScale = theScale;
	}

	public void SetColor(Color color) 
	{
		this.spriteRenderer.color = color;
	}

	public void SetCharacter (Character character, GameObject canvas)
	{
		this.canvas = canvas;
		this.character = character;
		this.character.currHealth = this.character.maxHealth;
		this.character.currActionPoints = this.character.maxActionPoints;
		GameObject healthPanel = GameObject.Instantiate (this.healthPanelPrefab) as GameObject;
		healthPanel.transform.SetParent (canvas.transform, false);
		healthPanel.transform.localScale = new Vector3 (1, 1, 1);

		Vector2 point = Camera.main.WorldToScreenPoint (this.transform.position);
		point = point - canvas.GetComponent<RectTransform> ().anchoredPosition;
		point.y = point.y + 80.0f;
		healthPanel.GetComponent<RectTransform> ().anchoredPosition = point;

		this.healthBarText = healthPanel.GetComponentInChildren<Text> ();
	}

}
