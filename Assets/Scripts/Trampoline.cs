using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : MonoBehaviour
{
    [SerializeField]
    private float _power;
    private Animator animator;


    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
       
        animator.SetInteger("State", 1);
        var rb = collision.gameObject.GetComponent<Rigidbody2D>();
        float t; Vector3 v;
        transform.rotation.ToAngleAxis(out t, out v);

        rb.velocity = new Vector2(-v.z * _power, Mathf.Abs(rb.velocity.y) + _power);

    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        animator.SetInteger("State", 0);
    }
}
