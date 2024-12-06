using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetterJumping : MonoBehaviour
{
    private Rigidbody2D rb;
    private float fallMultiplier = 2.5f;
    private float lowJumpMultiplier = 2f;
    private int m_Black = 1;

    private void Awake()
    {
        EventCenter.Instance.Subscribe("ChangeStatus", ChangeStatus);
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        

    }

    void ChangeStatus()
    {
        fallMultiplier = -fallMultiplier;
        lowJumpMultiplier = -lowJumpMultiplier;
        m_Black = -m_Black;
    }
    void Update()
    {
        if(m_Black*rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }else if(m_Black * rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }
}
