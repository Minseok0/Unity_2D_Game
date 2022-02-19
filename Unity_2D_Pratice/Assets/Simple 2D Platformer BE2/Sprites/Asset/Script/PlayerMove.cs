using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public GameManager gameManager;
    public static float maxSpeed;
    float moveX;
    public float jumpPower;
    Rigidbody2D rigid;
    public Rigidbody2D down;
    SpriteRenderer spriteRenderer;
    Animator anim;
    CapsuleCollider2D capsulecollider;

    public int maxJumpCnt; //********************
    public int jumpCnt; //********************
    public bool canDoubleJump; //********************

   

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        down = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        capsulecollider = GetComponent<CapsuleCollider2D>();
        anim = GetComponent<Animator>();
        maxSpeed = 4;
    }

    void Update()
    {
        //Jump
        if (Input.GetButtonDown("Jump") && !anim.GetBool("isJumping") && !canDoubleJump) //********************
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse); //********************
            jumpCnt++; //********************

            anim.SetBool("isJumping", true);
            
        }
        else if (Input.GetButtonDown("Jump") && !anim.GetBool("isJumping") && canDoubleJump && jumpCnt < maxJumpCnt) //********************
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse); //********************
            jumpCnt++; //********************
            if (jumpCnt == maxJumpCnt) //********************
            {
                rigid.AddForce(Vector2.up * jumpPower * 0.3f, ForceMode2D.Impulse); //********************
                maxJumpCnt--; //********************
                canDoubleJump = false; //********************
                anim.SetBool("isJumping", true); //********************
            } //********************
           
        }

        //Stop Speed
        if (Input.GetButtonUp("Horizontal")) {
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);
        }

        //Direction Sprite
        if(Input.GetButton("Horizontal"))
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;


        //Animation
        if (Mathf.Abs(rigid.velocity.x) <0.5)
            anim.SetBool("isWalking", false);
        else
            anim.SetBool("isWalking",true);
        
        //Falling Block
        Movement();
    }
    void FixedUpdate()
    {
        //Move Speed
        float h = Input.GetAxisRaw("Horizontal");

        rigid.AddForce(Vector2.right * h*maxSpeed, ForceMode2D.Impulse);

        //Max Speed
        if (rigid.velocity.x > maxSpeed) //Right Max Speed
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        else if (rigid.velocity.x < maxSpeed*(-1)) //Left Max Speed
            rigid.velocity = new Vector2(maxSpeed*(-1), rigid.velocity.y);

        //Landing Platform
        if (rigid.velocity.y < 0)
        {
            Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 1, 0));
            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Platform", "IcedPlatform"));//.
            
            if (rayHit.collider != null)//.
            {
                if (rayHit.distance < 0.5f)
                {
                    anim.SetBool("isJumping", false);
                    jumpCnt = 0; //땅에 닿으면 점프횟수 0회로 초기화
                }
            }
        }


    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            //Attack
            if (rigid.velocity.y < 0 && transform.position.y > collision.transform.position.y)
            {
                OnAttack(collision.transform);
            }
            else //Damaged
                OnDamaged(collision.transform.position);
        }
        //모래
        if (collision.gameObject.tag == "Ground")
        {
            Ground();
        }
        if (collision.gameObject.tag == "Platform")
        {
            rigid.mass = 1;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Item")
        {
            collision.gameObject.SetActive(false);
            // DoubleJump
            bool isDoublePotion = collision.gameObject.name.Contains("DoublePotion");
            if (isDoublePotion) //********************
            {
                canDoubleJump = true; //********************
                maxJumpCnt = 2; //********************
            }
        }
        else if(collision.gameObject.tag == "Finish")
        {
            gameManager.NextStage();
        }
        //무적 아이템
        if (collision.gameObject.tag == "Special")
        {
            Special();
            collision.gameObject.SetActive(false);
        }
    }

    void OnAttack(Transform enemy)
    {
        //Point
        gameManager.stagePoint = +100;

        //Reaction Force
        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);

        //Enemy Die
        EnemyMove enemyMove = enemy.GetComponent<EnemyMove>();
        enemyMove.OnDamaged();
    }

    void OnDamaged(Vector2 targetPos)
    {
        //Health Down
        gameManager.HealthDown();

        //Change Layer (Immotal Active
        gameObject.layer = 8;

        // View Alpha
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);

        //Reaction Force
        int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;
        rigid.AddForce(new Vector2(dirc,1)*8, ForceMode2D.Impulse);

        //Animation
        anim.SetTrigger("doDamaged");

        Invoke("OffDamaged", 2);
    }

    void Special() //무적 아이템
    {
        gameObject.layer = 8;
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);
        anim.SetTrigger("doDamaged");
        Invoke("OffDamaged", 3);
    }

    void Ground() //모래
    {
        rigid.mass = 2;
    }
    void OffDamaged()
    {
        gameObject.layer = 7;
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }

    public void OnDie()
    {
        //Sprite Alpha
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);
        //Sprite Flip Y
        spriteRenderer.flipY = true;
        //Collider Disable
        capsulecollider.enabled = false;
        //Die Effect Jump
        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
    }

    public void VelocityZero()
    {
        rigid.velocity = Vector2.zero;
    }
    void Movement()//.......
    {
        moveX = Input.GetAxis("Horizontal") * 4f;

        if (Input.GetButtonDown("Jump") && down.velocity.y == 0)
            down.AddForce(Vector2.up * 1000f);

        down.velocity = new Vector2(moveX, down.velocity.y);
    }
}
