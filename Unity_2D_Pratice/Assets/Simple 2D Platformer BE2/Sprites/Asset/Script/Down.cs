using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Down : MonoBehaviour
{
    Rigidbody2D down;
    Animator anim;

    public int jumpCnt;
    // Start is called before the first frame update
    void Start()
    {
        down = GetComponent<Rigidbody2D>();//.
        anim = GetComponent<Animator>();

    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Equals("Player"))//.
        {
            Invoke("FDown", 0.5f);
            Destroy(gameObject, 1f);
        }
    }

    void FDown()//.
    {
        down.isKinematic = false;
    }

    void FixedUpdate()
    {
        RaycastHit2D rayHit = Physics2D.Raycast(down.position, Vector3.down, 1, LayerMask.GetMask("Down"));
        if (rayHit.collider != null)
        {
            if (rayHit.distance < 0.5f)
            {
                anim.SetBool("isJumping", false);
                jumpCnt = 0; //¶¥¿¡ ´êÀ¸¸é Á¡ÇÁÈ½¼ö 0È¸·Î ÃÊ±âÈ­ //********************
            }
        }
    }
}
