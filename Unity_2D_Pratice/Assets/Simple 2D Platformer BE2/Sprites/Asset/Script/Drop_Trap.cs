using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drop_Trap : MonoBehaviour
{
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    CircleCollider2D circlecollider;
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        circlecollider = GetComponent<CircleCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("Player"))
        {
            rigid.isKinematic = false;
           
        }
       
    }

    
}
