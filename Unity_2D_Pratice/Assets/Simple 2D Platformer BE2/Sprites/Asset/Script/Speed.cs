using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speed : MonoBehaviour
{
  
    public GameObject coin;
    SpriteRenderer coinSprite;
    CircleCollider2D circlecollider;
    

    // Start is called before the first frame update
    void Start()
    {
        coinSprite = GetComponent<SpriteRenderer>();
        circlecollider = GetComponent<CircleCollider2D>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            coinSprite.enabled = false;
            circlecollider.enabled = false;
            PlayerMove.maxSpeed = 1000;
            Invoke("OffSpeed", 3);
        }

    }

    void OffSpeed()
    {
        PlayerMove.maxSpeed = 4;
    }
}
