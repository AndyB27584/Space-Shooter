using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private int _playerTypeIndex; //check the switch statement in Start() for Player types
    [SerializeField]
    private GameObject _laserPrefab, _tripleShotPrefab, _explosionPrefab;
    private SpawnManager _spawnManager;
    private UIManager _uIManager;
    private GameManager _gameManager;
    private float _speed = 9.0f, _coolDown = 0.5f, _timeToFire = 0.0f;
    private float _tripleShotLength = 5.0f, _tripleShotTime = -1.0f;
    private float _speedPULength = 5.0f, _speedPUTime = -1.0f, _speedPUMultiplier = 1.5f;
    private float _shieldPULength = 5.0f, _shieldPUTime = -1.0f;
    private int _lives = 3, _score;
    private Animator _animator;
    private SpriteRenderer _shieldSpriteRenderer;
    [SerializeField]
    private GameObject [] _playerDamage;
    
    private AudioSource _powerupAudio;
    


    void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.LogError("Spawn Manager is NULL");
        }

        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        if (_gameManager == null)
        {
            Debug.LogError("Game Manager is NULL");
        }

        _uIManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (_uIManager == null)
        {
            Debug.LogError("UI Manager is NULL");
        }

        _powerupAudio = GetComponent<AudioSource>();
        _animator = GetComponent<Animator>();
        _shieldSpriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>(); //Child index 0 is the shield gameobject
        _playerDamage[0].SetActive(false);//Damage prefab on left engine
        _playerDamage[1].SetActive(false); //Damage prefab on right engine

        switch (_playerTypeIndex)
        {
            case 0: //single player
                transform.position = new Vector3(0, 0, 0);
                break;
            case 1: //p1 in coop mode
                transform.position = new Vector3(-4.8f, 0, 0);
                break;
            case 2: //p2 in coop mode
                transform.position = new Vector3(4.8f, 0, 0);
                break;
            default:
                break;
        }
        
    }

    void Update()
    {
        PlayerMovement();
        FireLaser();
        ShieldRender();
    }

    private void ShieldRender()
    {
        if (_shieldPUTime > Time.time)
        {
            _shieldSpriteRenderer.enabled = true;
        }
        else
        {
            if (_shieldSpriteRenderer.enabled)
            {
                _shieldSpriteRenderer.enabled = false;
            }
        }
    }


    private void PlayerMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        _animator.SetFloat("playerTurning", horizontalInput);

        Vector3 _velocity;
        Vector3 _direction = new Vector3(horizontalInput, verticalInput, 0);
        if (_speedPUTime > Time.time)
        {
            _velocity = _direction * _speed * _speedPUMultiplier;
        }
        else
        {
            _velocity = _direction * _speed;
        }

        transform.Translate(_velocity * Time.deltaTime);

        float yPosition = Mathf.Clamp(transform.position.y, -3.8f, 2.5f);
        float xPosition = transform.position.x;

        if (xPosition < -11.5f)
        {
            xPosition = 11.5f;
        }
        if (xPosition > 11.5f)
        {
            xPosition = -11.5f;
        }

        transform.position = new Vector3(xPosition, yPosition, 0);
    }

    private void FireLaser()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _timeToFire)
        {
            if (_tripleShotTime > Time.time)
            {
                Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
            }
            else
            {
                Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.015f, 0), Quaternion.identity);
            }
            _powerupAudio.Play();
            _timeToFire = Time.time + _coolDown;
        }
    }

    public void DamagePlayer()
    {
        if (_shieldPUTime > Time.time)
        {
            _shieldPUTime = Time.time;
        }
        else
        {
            _lives--;
            _uIManager.UpdateLivesImage(_lives);
            if (_lives == 2)
            {
                int randomDamageIndex = Random.Range(0, 2);
                _playerDamage[randomDamageIndex].SetActive(true);
            }

            if (_lives == 1)
            {
                if (_playerDamage[0].activeSelf)
                {
                    _playerDamage[1].SetActive(true);
                }
                else
                {
                    _playerDamage[0].SetActive(true);
                }
            }

            if  (_lives <= 0)
            {
                DestroyPlayer();
            }
        }

    }

    private void DestroyPlayer()
    {
        Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        _spawnManager.StopSpawning();
        _uIManager.GameOverUISequence();
        _gameManager.GameOver();
        Destroy(this.gameObject);
    }

    public void SetTripleShotPowerUp()
    {
        _tripleShotTime = Time.time + _tripleShotLength;
    }

    public void SetSpeedPowerUp()
    {
        _speedPUTime = Time.time + _speedPULength;
    }

    public void SetShieldPowerUp()
    {
        _shieldPUTime = Time.time + _shieldPULength;
    }

    public void UpdatePlayerScore(int increment)
    {
        _score += increment;
        _uIManager.UpdateScoreText(_score);
    }
}
