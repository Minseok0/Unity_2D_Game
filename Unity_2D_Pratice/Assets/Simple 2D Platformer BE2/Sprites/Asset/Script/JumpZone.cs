using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpZone : MonoBehaviour
{
    [SerializeField] float jumpForce = 400f, speed = 5f, jumpZoneForce = 2f;
    int jumpCount = 1;
    float moveX;

    bool isPlatform = false;
    bool isJumpZone = false;
    Rigidbody2D rigid;

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Platform")
        {
            isPlatform = true;
            jumpCount = 1;
        }
        if(col.gameObject.tag == "JumpZone")
        {
            isJumpZone = true;
        }
    }

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        jumpCount = 0;
    }

    
    void Update()
    {
        Movement();    
    }

    void Movement()
    {
        if(isPlatform)
        {
            if (jumpCount > 0)
            {
                if (Input.GetButtonDown("Jump"))
                {
                    rigid.AddForce(Vector2.up * jumpForce);
                    jumpCount--;
                }
            }
            if (isJumpZone)
            {
                rigid.AddForce(new Vector2(0, jumpZoneForce) * jumpForce);
                isJumpZone = false;
            }
        }

        moveX = Input.GetAxis("Horizontal") * speed;
        rigid.velocity = new Vector2(moveX, rigid.velocity.y);
    }
}
