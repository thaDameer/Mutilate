using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthScript : MonoBehaviour
{
    public int health;
    int currentHealth;
    bool damaged;
    bool isDead;
    private Transform player;
    public GameObject deathParticle;
    

    void Awake()
    {

        currentHealth = health;
        player = GameObject.Find("Player").GetComponent<Transform>();
    }

    private void Update()
    {
        
    }

    public void TakeDamage(int damageAmount, SpriteRenderer sprite)
    {
        damaged = true;
        currentHealth -= damageAmount;
        StartCoroutine(FlashingSprite(sprite));
        if(player.position.x > sprite.transform.position.x)
        {
            sprite.GetComponentInParent<Rigidbody2D>().AddForce(-Vector2.right * 50f,ForceMode2D.Impulse);
        } else if(player.position.x < sprite.transform.position.x)
        {
            sprite.GetComponentInParent<Rigidbody2D>().AddForce(-Vector2.right * 50f,ForceMode2D.Impulse);
        }

        if(currentHealth <= 0 && !isDead)
        {
            Death();
        }
    }
    void Death()
    {

        isDead = true;
        var cloneEffect = Instantiate(deathParticle, transform.position, Quaternion.identity);
        Destroy(cloneEffect, 1f);
        Destroy(gameObject);

    }

    IEnumerator FlashingSprite(SpriteRenderer sprite)
    {
        var spriteColorRef = sprite.color;
        sprite.color = Color.black;
        yield return new WaitForSeconds(0.05f);
        sprite.color = spriteColorRef;
        yield return new WaitForSeconds(0.05f);
        sprite.color = Color.black;
        yield return new WaitForSeconds(0.05f);
        sprite.color = spriteColorRef;
        yield return new WaitForSeconds(0.05f);
        sprite.color = Color.black;
        yield return new WaitForSeconds(0.05f);
        sprite.color = spriteColorRef;
    }
}
