using System.Collections;
using System.Collections.Generic;
using System.IO.Enumeration;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore;

public class PlayerControls : MonoBehaviour
{
    [Header("Movement")]
    [Tooltip("speed of horizontal movement")]
    [Range(5, 15)]
    [SerializeField] float speed = 1f;
    private Vector2 movementInput = Vector2.zero;
    private Animator myAnimator;

    [Header("Jumping")]
    [Tooltip("Jumping height")]
    [Range(0, 100)]
    [SerializeField] float thrust = 300;
    bool grounded = true;
    bool jumped;

    Rigidbody2D rb;
    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    
    }
    //On jump checks the controls for jumping is pressed
    public void OnJump(InputAction.CallbackContext context)
    {
        jumped = context.action.triggered;
    }
    //jumping class includes jumping mechanics
    void Jump()
    {
        if (jumped && grounded)
        {
            rb.AddForce(transform.up * thrust * 100);
            grounded = false;
        }
    }
    void Move()
    {
        rb.velocity = new Vector2(movementInput.x * speed, rb.velocity.y);
    }
    // Start is called before the first frame update
    void Start()
    {
        //Ridgetbody2D is assaigned player
        rb = GetComponent<Rigidbody2D>();
         myAnimator = GetComponent<Animator>();
    }


    // FixedUpdate is called once per set update
    void FixedUpdate()

    {
        Jump();
        Move();
        myAnimator.SetBool("isJumping",!grounded);
    }



    //OnCollisionEnter2D is called when one object collides with another.
    //in this case the object is set to floor
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Floor")
        {
            //this bool will make jumping possible if true
            grounded = true;
        }
    }

    
    //{
    //    rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -_MaxFallSpeed, _MaxFallSpeed * 2));
    //    if (moveInput < 0 || moveInput > 0)
    //    {
    //        TurnCheck();

    //    }

    //}

    //private void TurnCheck()
    //{
    //    if (UserInput.instance.moveInput.x > 0 && !IsFaceingRight)
    //    {
    //        Turn();
    //    }
    //    else if (UserInput.instance.moveInput.x < 0 && IsFaceingRight)
    //    {
    //        Turn();
    //    }
    //}
    //private void Turn()
    //{
    //    if (IsFacingRight)
    //    {
    //        Vector2 rotator = new Vector2(transform.rotation.x, 180f);
    //        transform.rotation = Quaternion.Euler(rotator);
    //        IsFacingRight = !isFacingRight;
    //    }
    //    else
    //    {
    //        Vector2 rotator = new Vector2(transform.rotation.x, 0f);
    //        transform.rotation = Quaternion.Euler(rotator);
    //        IsFacingRight = !isFacingRight;

    //    }

    //}


}
