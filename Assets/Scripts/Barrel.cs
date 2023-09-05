using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour
{
    private Rigidbody2D rigidBody2D;

    public float speed = 5f;
    private void Awake()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();   
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Ground")) 
        {
            rigidBody2D.AddForce(collision.transform.right * speed , ForceMode2D.Impulse);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Ground2"))
        {
            rigidBody2D.AddForce(-collision.transform.right * speed, ForceMode2D.Impulse);
        }

    }
}
