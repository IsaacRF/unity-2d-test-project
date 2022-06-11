using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CharacterControllerScript : MonoBehaviour {

	//Configuration
	public int rupees = 0;
	public float WALKSPEED = 5f;
	public float RUNSPEED = 10f;
	public float MAXSPEED = 0f;
	public float JUMPFORCE = 400f;
	bool facingRight = true;
	bool grounded = false;				//True when the character is standing on any ground surface
	public Transform groundCheck;		//Where the circle to look for ground will be generated
	float groundCheckRadius = 0.2f;		//How big is the circle used to check if there is ground near
	public LayerMask whatIsGround;		//Tell the character what elements are ground (can he stand on) to stop falling animation

	//Physics
	public Rigidbody2D charRigidBody2D;
	Animator charAnimator;

	//Sounds
	public AudioClip soundRupee;
	public AudioClip soundAttack1;
	public AudioClip soundAttack2;
	public AudioClip soundAttack3;
	public AudioClip soundAttack4;

    //Scene elements
    public List<Object> lstPlatformPrefabs;
    public Transform PlatformsPrefab1;
    public Transform PlatformsPrefab2;
    public Transform PlatformsPrefab3;
    public Transform PlatformsPrefab4;

    //UI Elements
    public Text txtRupees;

	// Use this for initialization
	void Start () {
		//Get the first Animator component of the Character Game Object
		charAnimator = GetComponent<Animator> ();

        //Instantiate platforms prefabs list
        lstPlatformPrefabs = new List<Object>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		//Check if the character is grounded using a circle in groundCheck.position of groundCheckRadius taking care of whatIsGround
		grounded = Physics2D.OverlapCircle (groundCheck.position, groundCheckRadius, whatIsGround);
		charAnimator.SetBool ("Ground", grounded);
		charAnimator.SetFloat ("VSpeed", charRigidBody2D.velocity.y);

		//Get input direction
		float move = Input.GetAxis ("Horizontal");

        //Set max speed according to keys pressed (Run or walk)
		if (Input.GetKey (KeyCode.LeftShift)) {
			MAXSPEED = RUNSPEED;
        } else {
			MAXSPEED = WALKSPEED;
		}

        //Set the parameter called "Speed" that is inside the Animator so it changes the character animation
        charAnimator.SetFloat ("Speed", Mathf.Abs(move * MAXSPEED));

		//Increment current velocity
		charRigidBody2D.velocity = new Vector2(move * MAXSPEED, charRigidBody2D.velocity.y);

		//Flip character Sprite if needed and if the character it's on the ground
		if (move > 0 && !facingRight && grounded) {
			Flip ();
		} else if (move < 0 && facingRight && grounded) {
			Flip();
		}
	}

	void Update ()	{
		//Jump
		if (grounded && Input.GetKeyDown (KeyCode.Space)) {
			charAnimator.SetBool("Ground", false);
			charRigidBody2D.AddForce(new Vector2(0, JUMPFORCE));
		}

		if (grounded && Input.GetKeyDown (KeyCode.LeftControl)) {
			//Play attack animation once
			charAnimator.SetTrigger("Attack");
			//Play random attack sound
			int rnd = Random.Range(1, 4);
			switch (rnd) {
			
			case 1:
				AudioSource.PlayClipAtPoint (soundAttack1, this.transform.position);
				break;
			case 2:
				AudioSource.PlayClipAtPoint (soundAttack2, this.transform.position);
				break;
			case 3:
				AudioSource.PlayClipAtPoint (soundAttack3, this.transform.position);
				break;
			case 4:
				AudioSource.PlayClipAtPoint (soundAttack4, this.transform.position);
				break;
			}

		}
	}

	void OnTriggerEnter2D (Collider2D collision) {
		Debug.Log("<color=red>Collision with: </color> " + collision.gameObject.tag);

        switch (collision.gameObject.tag)
        {
            case "Rupee":
                if (soundRupee != null)
                {
                    AudioSource.PlayClipAtPoint(soundRupee, this.transform.position);
                }
                Destroy(collision.gameObject);
                rupees += 1;
                if (txtRupees != null)
                {
                    txtRupees.text = rupees.ToString("000");
                }
                break;
            case "LoadSign":
                //Delete LoadSign
                Destroy(collision.gameObject);

                //Load random platforms prefab
                int rnd = Random.Range(1, 5);
                switch (rnd)
                {
                    case 1:
                        lstPlatformPrefabs.Add(Instantiate(PlatformsPrefab1, new Vector3(transform.position.x + 10, transform.position.y), transform.rotation));
                        break;
                    case 2:
                        lstPlatformPrefabs.Add(Instantiate(PlatformsPrefab2, new Vector3(transform.position.x + 5, transform.position.y - 5), transform.rotation));
                        break;
                    case 3:
                        lstPlatformPrefabs.Add(Instantiate(PlatformsPrefab3, new Vector3(transform.position.x + 2, transform.position.y - 15), transform.rotation));
                        break;
                    case 4:
                        lstPlatformPrefabs.Add(Instantiate(PlatformsPrefab4, new Vector3(transform.position.x + 5, transform.position.y), transform.rotation));
                        break;
                    default:
                        break;
                }

                //Unload old platforms prefabs and delete from list
                if (lstPlatformPrefabs.Count > 2)
                {
                    Transform platformPrefab = (Transform)lstPlatformPrefabs[0];
                    Destroy(platformPrefab.gameObject);
                    lstPlatformPrefabs.RemoveAt(0);
                }
                break;
            default:
                break;
        }
	}

	/**
	 * Turns the character sprite to the opposite side
	 */
	void Flip() {
		facingRight = !facingRight;

		//Invert the x axis
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;

		//Apply the scale change
		transform.localScale = theScale;
	}
}
