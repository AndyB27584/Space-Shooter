using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyLaserPrefab;
    private float _speed = 3;
    private Animator animator;
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip _explosionClip;
    private CapsuleCollider2D capsuleCollider2D;
    private bool isDead = false;
    
    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();

        StartCoroutine(FireLasers());
    }

    void Update()
    {
        EnemyMovement();
    }

    private void EnemyMovement()
    {
        Vector3 direction = Vector3.down;
        Vector3 velocity = direction * _speed;

        transform.Translate(velocity * Time.deltaTime);

        if (transform.position.y < -7.0f)
        {
            if (isDead)
            {
                SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
                spriteRenderer.enabled = false;
            }
            else
            {
                float xPosition = Random.Range(-9.45f, 9.45f);
                transform.position = new Vector3(xPosition, 7.1f, 0f);
            }
        }
    }

    public void DestroyEnemy()
    {
        StopAllCoroutines();
        isDead = true;
        animator.SetBool("isEnemyDead", true);
        audioSource.clip = _explosionClip;
        audioSource.Play();
        capsuleCollider2D.enabled = false;
        Destroy(this.gameObject,3.0f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.DamagePlayer();
                DestroyEnemy();
            }
            else
            {
                Debug.LogError("Player is NULL");
            }
        }
    }

    IEnumerator FireLasers()
    {
        while (true)
        {
            float randomTime = Random.Range(1.0f, 3.0f);
            yield return new WaitForSeconds(randomTime);
            Instantiate(_enemyLaserPrefab, transform.position, Quaternion.identity);
            audioSource.Play();
        }
    }
}
