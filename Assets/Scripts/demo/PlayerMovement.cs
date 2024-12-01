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
    public float speed = 10;
    public float jumpForce = 50;
    public float slideSpeed = 5;
    public float wallJumpLerp = 10;
    public float dashSpeed = 20;

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

    // Start is called before the first frame update
    void Start()
    {
        coll = GetComponent<Collision>();
        rb = GetComponent<Rigidbody2D>();
    }

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

        // Wall Grab Logic
        if (wallGrab && !isDashing)
        {
            rb.gravityScale = 0;
            if (x > .2f || x < -.2f)
                rb.velocity = new Vector2(rb.velocity.x, 0);

            float speedModifier = y > 0 ? .5f : 1;

            rb.velocity = new Vector2(rb.velocity.x, y * (speed * speedModifier));
        }
        else
        {
            rb.gravityScale = 3;
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
    }

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
}