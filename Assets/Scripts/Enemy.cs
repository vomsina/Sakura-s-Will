using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{

    public float speed = 10f;
    public float diapason = 10f;
    private SpriteRenderer sprite;

    public Transform attackPos;
    public float attackRabge;
    public LayerMask player;
    private Rigidbody2D rb2d;
    public bool rotate = false;
    public bool boss = false;
    public GameObject GameOver;
    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        StartCoroutine(c_Move());
        sprite = GetComponent<SpriteRenderer>();
        health = 100;
    }

    IEnumerator c_Move()
    {
        var min = transform.position.x - diapason;
        var max = transform.position.x + diapason;

        var direction = Mathf.Sign(speed);

        while (true)
        {
            if (transform.position.x > max && direction > 0.0f)
            {
                direction = -direction;
                if(rotate)
                sprite.flipX = false;

            }
            else if (transform.position.x < min && direction < 0.0f)
            {
                direction = -direction;
                if(rotate)
                sprite.flipX = true;
            }

            rb2d.velocity = new Vector2(speed * direction, rb2d.velocity.y);

            yield return null;
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(attackPos.position, attackRabge);
    }
    void OnAttack()
    {

        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackPos.position, attackRabge, player);
        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].GetComponent<Entity>().GetDamage();
            
        }
    }
    public override void GetDamage()
    {
        health -= 15;
        if (health <= 0)
        {
            if (boss)
            {
                GameOver.SetActive(true);
            }
            Die();

        }
    }
    public void EnemyHurt()
    {
        health -= 0.5f;
        if (health < 0)
        {
            if(boss)
            {
                GameOver.SetActive(true);
            }
            Destroy(this.gameObject);

        }
    }
}
