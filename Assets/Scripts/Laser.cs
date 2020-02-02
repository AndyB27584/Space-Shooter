using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private float _speed = 20.0f;
    private bool _isEnemyLaser = false;
    private Player player;
    
    void Start()
    {
        if (transform.parent != null)
        {
            if (transform.parent.tag == "Enemy_Laser")
            {
                _isEnemyLaser = true;
            }
        }

        player = GameObject.Find("Player").GetComponent<Player>();
        if (player == null)
        {
            Debug.LogError("Player is NULL");
        }
    }

    void Update()
    {
        LaserMovement();
    }

    private void LaserMovement()
    {
        Vector3 direction;
        if (!_isEnemyLaser)
        {
            direction = Vector3.up;
        }
        else
        {
            direction = Vector3.down;
        }
        Vector3 velocity = direction * _speed;

        transform.Translate(velocity * Time.deltaTime);

        if (transform.position.y > 8.0f)
        {
            if(transform.parent != null)
            {
                Destroy(this.transform.parent.gameObject);
            }
            else
            {
                Destroy(this.gameObject);
            }       
        }
        if (transform.position.y < -7.0f)
        {
            if (transform.parent != null)
            {
                Destroy(this.transform.parent.gameObject);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Enemy" && !_isEnemyLaser)
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                player.UpdatePlayerScore(10);
                enemy.DestroyEnemy();
                Destroy(this.gameObject);
            }
        }

        if(other.tag == "Player" && _isEnemyLaser)
        {
                player.DamagePlayer();
                Destroy(this.gameObject);
        }
    }
}
