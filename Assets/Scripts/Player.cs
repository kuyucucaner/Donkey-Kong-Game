using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{   
    private SpriteRenderer spriteRenderer;
    public Sprite[] runSprites;
    public Sprite climbSprite;
    public int spriteIndex;


    private Rigidbody2D rigidBody2D;
    private Vector2 direction;
    public new  Collider2D collider;
    public Collider2D[] results;
    public float moveSpeed = 3f;
    public float jumpStrength = 1.0f;

    public bool grounded;
    public bool climbing;
    private void Awake()
    {   
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidBody2D = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        results = new Collider2D[4];
    }

    private void OnEnable()
    {
        InvokeRepeating(nameof(AnimeteSprite), 1f / 12f, 1f / 12f);
    }
    private void OnDisable()
    {
        CancelInvoke();
    }

    private void CheckCollision()
    {
        grounded = false;
        climbing = false;
        Vector2 size = collider.bounds.size;
        size.y += 0.1f;
        size.x /= 2f;
       int amount =  Physics2D.OverlapBoxNonAlloc(transform.position, size, 0f,results);

        for(int i = 0; i < amount; i++)
        {
            GameObject hit = results[i].gameObject;

            if (hit.layer == LayerMask.NameToLayer("Ground") || hit.layer == LayerMask.NameToLayer("Ground2"))
            {
                grounded=hit.transform.position.y < transform.position.y - 0.5f;

                Physics2D.IgnoreCollision(collider, results[i], !grounded);
            }
            else if (hit.layer == LayerMask.NameToLayer("Ladder"))
            {
                climbing = true;
            }
        }
    }
    private void Update()
    {
        CheckCollision();

        if (climbing)
        {
            direction.y = Input.GetAxis("Vertical") * moveSpeed;
        }
        else if (grounded && Input.GetButtonDown("Jump"))
        {
            direction = Vector2.up * jumpStrength;
        }
        else
        {
            direction += Physics2D.gravity * Time.deltaTime;
        }
        direction.x = Input.GetAxis("Horizontal") * moveSpeed;
        if (grounded)
        {
            direction.y = Mathf.Max(direction.y, -1f);  
        }
  

        if(direction.x > 0f)
        {
            transform.eulerAngles = Vector3.zero;
        }
        else if (direction.x < 0f)
        {
            transform .eulerAngles = new Vector3(0f,180f,0f);
        }
    }
    private void FixedUpdate()
    {
        rigidBody2D.MovePosition(rigidBody2D.position + direction * Time.fixedDeltaTime);
    }

    private void AnimeteSprite()
    {
        if (climbing)
        {
            spriteRenderer.sprite = climbSprite;
        }
        else if(direction.x !=  0f)
        {
            spriteIndex++;

            if(spriteIndex >= runSprites.Length)
            {
                spriteIndex = 0;
            }

            spriteRenderer.sprite = runSprites[spriteIndex];
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Objective"))
        {   
            enabled = false;
            FindObjectOfType<GameManager>().LevelComplete();
        }
        else if(collision.gameObject.CompareTag("Obstacle"))
        {
            enabled = false;
            FindObjectOfType<GameManager>().LevelFailed();
        }
    }
}
