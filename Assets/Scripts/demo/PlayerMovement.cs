using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Collision coll;
    [HideInInspector]
    public Rigidbody2D rb;

    [Space]
    [Header("Stats")]
    private float speed = 10;
    private float jumpForce = 15;
    private float slideSpeed = 5;
    private float wallJumpLerp = 10;
    private float dashSpeed = 20;

    [Space]
    [Header("Booleans")]
    public bool canMove;
    public bool wallGrab;
    public bool wallJumped;
    public bool wallSlide;
    public bool isDashing;

    [Space]
    private bool groundTouch;
    private bool hasDashed;

    public int side = 1;

    // New boolean to switch between modes
    private bool isBlackMode = true;

    // Start is called before the first frame update
    void Start()
    {
        coll = GetComponent<Collision>();
        rb = GetComponent<Rigidbody2D>();

        // Subscribe to the initial event based on the default state of isBlackMode
        if (isBlackMode)
        {
            SubscribeToBlackEvent();
        }
        else
        {
            SubscribeToWhiteEvent();
        }
    }
    #region Update
    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        float xRaw = Input.GetAxisRaw("Horizontal");
        float yRaw = Input.GetAxisRaw("Vertical");
        Vector2 dir = new Vector2(x, y);

        Walk(dir);

        // Check if player is attempting to grab a wall
        if (coll.onWall && Input.GetButton("Fire3") && canMove)
        {
            wallGrab = true;
            wallSlide = false;
        }

        // Stop wall grab and wall slide if "Fire3" is released or if not on wall
        if (Input.GetButtonUp("Fire3") || !coll.onWall || !canMove)
        {
            wallGrab = false;
            wallSlide = false;
        }

        if (coll.onGround && !isDashing)
        {
            wallJumped = false;
        }
// Wall Slide Logic
        if (coll.onWall && !coll.onGround)
        {
            if (x != 0 && !wallGrab)
            {
                wallSlide = true;
                WallSlide();
            }
        }

        if (!coll.onWall || coll.onGround)
            wallSlide = false;

        // Jump and Wall Jump
        if (Input.GetButtonDown("Jump"))
        {
            if (coll.onGround)
                Jump(Vector2.up, false);
            if (coll.onWall && !coll.onGround)
                WallJump();
        }

        // Ground Touch logic
        if (coll.onGround && !groundTouch)
        {
            GroundTouch();
            groundTouch = true;
        }

        if (!coll.onGround && groundTouch)
        {
            groundTouch = false;
        }

        // Handle wall particles
        WallParticle(y);

        // Prevent movement while in wall grab, wall slide or during dash
        if (wallGrab || wallSlide || !canMove)
            return;

        // Handle horizontal flip based on direction
        if (x > 0)
        {
            side = 1;
        }
        if (x < 0)
        {
            side = -1;
        }

        // Listen for the S key to toggle the mode
        if (Input.GetKeyDown(KeyCode.S))
        {
            EventCenter.Instance.TriggerEvent("ChangeStatus");
            isBlackMode = !isBlackMode;
            if(isBlackMode)
            {

                SubscribeToBlackEvent();
            }
            else
            {

                SubscribeToWhiteEvent();
       
            }
        }
    }
    #endregion
    void GroundTouch()
    {
        hasDashed = false;
        isDashing = false;

        side = (rb.velocity.x > 0) ? 1 : -1;
    }

    private void WallSlide()
    {
        if (coll.wallSide != side)
            side = coll.wallSide;

        if (!canMove)
            return;

        float slideSpeed = this.slideSpeed;
        rb.velocity = new Vector2(rb.velocity.x, Mathf.Sign(rb.velocity.y) * slideSpeed);
    }

    private void WallParticle(float y)
    {
        // Handle wall particle effects (if any) here
    }

    private void Jump(Vector2 direction, bool isWallJump)
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);  // Resets Y velocity before applying jump force
        rb.AddForce(direction * jumpForce, ForceMode2D.Impulse);
    }

    private void WallJump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);  // Reset Y velocity before jumping
        rb.AddForce(new Vector2(coll.wallSide * jumpForce, jumpForce), ForceMode2D.Impulse);
        wallJumped = true;
    }

    private void Walk(Vector2 dir)
    {
        if (!canMove) return;

        float targetSpeed = dir.x * speed;
        rb.velocity = new Vector2(targetSpeed, rb.velocity.y);
    }

    // Subscribe to Black event (moves player up and changes gravity)
    private void SubscribeToBlackEvent()
    {
        rb.transform.position += new Vector3(0, 1, 0);  // Move up 1 unit
        jumpForce = 15;  // Set jump force
        rb.gravityScale = 3;  // Set gravity scale to 3
    }

    // Unsubscribe from Black event
    private void UnsubscribeFromBlackEvent()
    {
        // Currently nothing to remove here, could add event handling in the future
    }

    // Subscribe to White event (moves player down and changes gravity)
    private void SubscribeToWhiteEvent()
    {
        rb.transform.position -= new Vector3(0, 2, 0);  // Move down 1 unit
        jumpForce = -15;  // Set jump force to negative
        rb.gravityScale = -3;  // Set gravity scale to negative
    }

    // Unsubscribe from White event
    private void UnsubscribeFromWhiteEvent()
    {
        // Currently nothing to remove here, could add event handling in the future
    }
}
