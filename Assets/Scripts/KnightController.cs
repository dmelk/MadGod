using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof (KnightCharacter))]
public class KnightController : MonoBehaviour {

	private KnightCharacter m_Character;
	private bool m_Jump;

	private void Awake()
	{
		m_Character = GetComponent<KnightCharacter>();
	}


	private void Update()
	{
		if (!m_Jump)
		{
			// Read the jump input in Update so button presses aren't missed.
			m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
		}

		if (Input.GetKeyDown (KeyCode.E)) {
			this.m_Character.Talk ();
		}
	}


	private void FixedUpdate()
	{
		// Read the inputs.
		float h = CrossPlatformInputManager.GetAxis("Horizontal");
		// Pass all parameters to the character control script.
		m_Character.Move(h, m_Jump);
		m_Jump = false;
	}
}
