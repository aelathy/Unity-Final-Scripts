using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Private Variables
    private Rigidbody playerRb;
    private GameManager gameManager;
    public Animator playerAnim;
    private AudioSource playerAudio;
    public AudioClip jumpSound;
    public AudioClip fallSound;
    public AudioClip victorySound;
    public float jumpForce;
    public float gravityModifier;
    public bool isOnGround = true;
    private float speed = 5.0f;
    private float turnSpeed = 150.0f;
    private float horizontalInput;
    public float forwardInput;
    private float belowBound = -5;
    private float death = -25;
    public bool gameOver = false;
    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        playerAnim = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        Physics.gravity *= gravityModifier;
    }

    // Update is called once per frame
    void Update()
    {
        // Player input
        bool forwardPressed = Input.GetKey("w");
        bool backwardPressed = Input.GetKey("s");
        horizontalInput = Input.GetAxis("Horizontal");
        forwardInput = Input.GetAxis("Vertical");

        // Move player
        transform.Translate(Vector3.forward * Time.deltaTime * speed * forwardInput);
        // Turn player
        transform.Rotate(Vector3.up, turnSpeed * horizontalInput * Time.deltaTime);

        // If player presses spacebar, jump
        if (Input.GetKeyDown(KeyCode.Space) && isOnGround)
        {
            playerAnim.SetTrigger("jump_trig");
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isOnGround = false;
            playerAnim.SetBool("isRunning", false);
            playerAnim.SetBool("grounded", false);
            playerAudio.PlayOneShot(jumpSound, 1.0f);
        }

        if (transform.position.y < belowBound)
        {
            playerAudio.PlayOneShot(fallSound, 0.1f);
            playerAnim.SetTrigger("fall_trig");
        }

        if (transform.position.y < death)
        {
            Destroy(gameObject);
            gameManager.GameOver();
        }

        if (forwardPressed && isOnGround)
        {
            playerAnim.SetBool("isRunning", true);
        }

        if (!forwardPressed)
        {
            playerAnim.SetBool("isRunning", false);
        }

        if (backwardPressed)
        {
            playerAnim.SetBool("isRunningBack", true);
        }
        else
        {
            playerAnim.SetBool("isRunningBack", false);

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        isOnGround = true;
        playerAnim.SetBool("grounded", true);
        if (collision.gameObject.tag == "Victory Platform")
        {
            gameManager.Victory();
            playerAudio.PlayOneShot(victorySound, 1.0f);
        }
    }

}
