using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    private float _speed = 2.0f;
    [SerializeField]
    private int _powerupIndex;
    private SpriteRenderer _spriteRenderer;
    private BoxCollider2D _boxCollider2D;
    [SerializeField]
    private AudioClip _powerupClip;
    private AudioSource audioSource;

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = _powerupClip;
    }

    void Update()
    {
        PowerUpMovement();
    }

    private void PowerUpMovement()
    {
        Vector3 direction = Vector3.down;
        Vector3 velocity = direction * _speed;

        transform.Translate(velocity * Time.deltaTime);

        if (transform.position.y < -7.0f)
        {
            Destroy(this.gameObject);
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                switch (_powerupIndex)
                {
                    case 0: player.SetTripleShotPowerUp();
                        break;

                    case 1: player.SetSpeedPowerUp();
                        break;

                    case 2: player.SetShieldPowerUp();
                        break;

                    default:
                        break;
                }
                _boxCollider2D.enabled = false;
                _spriteRenderer.enabled = false;
                audioSource.Play();
                Destroy(this.gameObject,0.8f);
            }
        }
    }
}
