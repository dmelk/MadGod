using UnityEngine;
using System.Collections;

public class KnightCharacter : MonoBehaviour {

	[SerializeField] private float maxSpeed = 10f;                    
	[SerializeField] private float jumpForce = 400f;        
	[SerializeField] private bool airControl = false;       
	[SerializeField] private LayerMask whatIsGround;   
	[SerializeField] private Transform[] groundPoints;
	[SerializeField] private GameController gameController;

	public bool dead = false;

	private float groundRadius = 0.2f;
	private bool grounded;
	private Rigidbody2D mRigidbody;
	private bool facingRight = true;  
	private Animator mAnimator;
	private GameObject enemy;

	private void Awake()
	{
		this.mRigidbody = GetComponent<Rigidbody2D>();
		this.mAnimator = GetComponent<Animator>();
		this.dead = false;
	}

	public void Reset() {
		this.grounded = false;
		if (!this.facingRight)
			this.Flip ();
		this.dead = false;
	}

	private void FixedUpdate()
	{
		this.grounded = this.IsGrounded ();

		this.mAnimator.SetBool("Ground", this.grounded);
		this.mAnimator.SetBool("Dead", this.dead);

		// Set the vertical animation
		this.mAnimator.SetFloat ("vSpeed", mRigidbody.velocity.y);
	}
		
	public void Move(float move, bool jump)
	{
		// we are dead
		if (this.dead) {
			return;
		}
		
		//only control the player if grounded or airControl is turned on
		if (this.grounded || this.airControl)
		{
			// The Speed animator parameter is set to the absolute value of the horizontal input.
			this.mAnimator.SetFloat("Speed", Mathf.Abs(move));

			// Move the character
			this.mRigidbody.velocity = new Vector2(move*maxSpeed, mRigidbody.velocity.y);

			// If the input is moving the player right and the player is facing left...
			if (move > 0 && !this.facingRight)
			{
				// ... flip the player.
				this.Flip();
			}
			// Otherwise if the input is moving the player left and the player is facing right...
			else if (move < 0 && this.facingRight)
			{
				// ... flip the player.
				this.Flip();
			}
		}
		// If the player should jump...
		if (this.grounded && jump && this.mAnimator.GetBool("Ground"))
		{
			// Add a vertical force to the player.
			this.grounded = false;
			this.mAnimator.SetBool("Ground", false);
			this.mRigidbody.AddForce(new Vector2(0f, this.jumpForce));
		}
	}

	public void Talk () {
		if (this.enemy == null)
			return;
		this.gameController.ShowTalkMenu (this.enemy.GetComponent<DragonNPC> ());
	}

	private bool IsGrounded() {
		foreach (Transform point in this.groundPoints) {
			Collider2D[] colliders = Physics2D.OverlapCircleAll(point.position, this.groundRadius, this.whatIsGround);
			for (int i = 0; i < colliders.Length; i++)
			{
				if (colliders [i].gameObject != gameObject) {
					return true;
				}
			}
		}
		return false;
	}

	private void Flip()
	{
		// Switch the way the player is labelled as facing.
		this.facingRight = !this.facingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = this.transform.localScale;
		theScale.x *= -1;
		this.transform.localScale = theScale;
	}

	private void OnTriggerEnter2D(Collider2D other) {
		// check if we are in death zone
		if (other.tag == "DeathCollider") {
			this.dead = true;
			return;
		}

		if (other.tag == "Enemy") {
			this.enemy = other.gameObject;
			Physics2D.IgnoreCollision (this.gameObject.GetComponent<BoxCollider2D> (), other.gameObject.GetComponent<BoxCollider2D> ());
			this.gameController.ShowActionButton (other.gameObject.GetComponent<Transform> ());
		}
	}

	private void OnTriggerExit2D(Collider2D other) {
		if (other.tag == "Enemy") {
			this.enemy = null;
			Physics2D.IgnoreCollision (this.gameObject.GetComponent<BoxCollider2D> (), other.gameObject.GetComponent<BoxCollider2D> (), false);
			this.gameController.HideActionButton ();
		}
	}

}
