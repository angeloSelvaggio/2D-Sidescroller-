using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{

    //player float variables
    public float speed;
    public float jumpPower;
    public float runSpeed;

    //player bool variables
    private bool jumpAble;
    public bool facingRight;
    public bool isGrounded;
    public bool isFalling;

    //Components for the player that we need 
    private Rigidbody2D playerRb;
    private Collider2D playerColl;
    private Animator anim;

    void Start()
    {
        //get the components from the player gameobject 

        playerRb = GetComponent<Rigidbody2D>();
        playerColl = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();

        //declaring values to variables

        speed = 1f;
        jumpPower = 8f;
        runSpeed = 2f;
        jumpAble = true;
        facingRight = true;
        isGrounded = false;
        isFalling = false;
    }


    void Update()
    {
       //all the functions for movement, jumping, falling and running
        Jump();
        Movement();
        Running();
        Falling();

      
    }

    public void Flip()
    {
        //flip the player when ever he turns his direction
        facingRight = !facingRight;
        transform.Rotate(Vector3.up * 180f);
    }

    //Movementfunction
    public void Movement()
    {
        //Movement logic
       //get the input and save it in a variable
        float move = Input.GetAxis("Horizontal");
      
        //if the move-value is greater then zero  or lesser, then the player walks
           if (move < 0) GetComponent<Rigidbody2D>().velocity = new Vector3(move * speed, GetComponent<Rigidbody2D>().velocity.y);
           if (move > 0) GetComponent<Rigidbody2D>().velocity = new Vector3(move * speed, GetComponent<Rigidbody2D>().velocity.y);

           //set animation when player is moving, or standing
           if(move<0 ||move >0) anim.SetBool("isWalking", true);
           if (move == 0) anim.SetBool("isWalking", false);

           //flip the player any time he changes direction
           if (move < 0 && facingRight) Flip();
           if (move > 0 && !facingRight) Flip();
        
    }

    //Jumpfunction
    public void Jump()
    {
        //if jump key is pressed, in this case space perform jump logic
        if (Input.GetButtonDown("Jump")&& jumpAble&& isGrounded)
        {
            //jump logic

            //set bool isJumping true for animation change
            anim.SetBool("isJumping", true);
            //add impulse to player so he can perform a jjump
            gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, 1f * jumpPower), ForceMode2D.Impulse);
            //set jumpability false so he can't jump multiple times in the air
            jumpAble = false;
            //reset jumpability
            StartCoroutine(JumpReset());
        }
    }

    public void Running()
    {
       //check if leftshift is pressed
        if (Input.GetKey(KeyCode.LeftShift))
        {
            //change animation and speed up the player
            anim.SetBool("isRunning", true);
            speed = 3;
        }
        else
        {
            //change animation and back to normal speed
            anim.SetBool("isRunning", false);
            speed = 1;
        }
    }

    IEnumerator JumpReset()
    {
        //jumpreset function which resets the animation bool isJumping after 0.3 seconds and the jumpability after 3 seconds
        yield return new WaitForSeconds(.3f);
        anim.SetBool("isJumping", false);
        yield return new WaitForSeconds(3f);
        
        jumpAble = true;
    }



    public void Falling()
    {
        //if player is not grounded change animation bool isFalling to the value 'true'
        if (!isGrounded)
        {
            anim.SetBool("isFalling",true);
        }
        else
        {
            //if player is grounded change animation bool isFalling to the value 'false'
            anim.SetBool("isFalling", false);
        }
    }



    private void OnCollisionStay2D(Collision2D collision)
    {
        //checks if player is on ground or on any platform with the tag 'ground'
        if (collision.gameObject.tag == "Ground")
        {
            //change variables jumpability and isGrounded to the value 'true'
            isGrounded = true;
            jumpAble = true;

          
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //checks if player is in air  
        if(collision.gameObject.tag == "Ground")
        {
            //change variables jumpability and isGrounded to the value 'false'
            isGrounded = false;
            jumpAble = false;
            //Debugging
           // Debug.Log("is in Air");
        }
    }
}
