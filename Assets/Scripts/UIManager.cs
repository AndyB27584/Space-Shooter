using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    private Text _scoreText;
    [SerializeField]
    private Sprite[] _livesSprites;
    private Image _livesImage;
    private Text _gameOverText, _restartText;
    private GameManager _gameManager;
    private GameObject _pauseMenu;
    private bool _isGamePaused = false;
    

    // Start is called before the first frame update
    void Start()
    {
        _scoreText = transform.GetChild(0).GetComponent<Text>(); //Score Text is Child Index 0
        _livesImage = transform.GetChild(1).GetComponent<Image>();
        _gameOverText = transform.GetChild(2).GetComponent<Text>();
        _restartText = transform.GetChild(3).GetComponent<Text>();
        _pauseMenu = transform.GetChild(4).gameObject;

        _livesImage.sprite = _livesSprites[3];//int lives
        _gameOverText.enabled = false;
        _restartText.enabled = false;
        _pauseMenu.SetActive(false);

        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        if (_gameManager == null)
        {
            Debug.LogError("Game Manager is NULL");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseMenu();
        }
    }

    public void UpdateScoreText(int score)
    {
        _scoreText.text = "Score: " + score;
    }

    public void UpdateLivesImage(int lives)
    {
        if (lives < 0)
        {
            lives = 0;
        }
        _livesImage.sprite = _livesSprites[lives];
    }

    public void GameOverUISequence()
    {
        _restartText.enabled = true;
        StartCoroutine(FlashGameOver());
    }

    IEnumerator FlashGameOver()
    {
        while (true)
        {
            _gameOverText.enabled = true;
            yield return new WaitForSeconds(0.5f);
            _gameOverText.enabled = false;
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void PauseMenu()
    {
        if (!_isGamePaused)
        {
            _gameManager.PauseGame();
            _pauseMenu.SetActive(true);
            _isGamePaused = true;
        }
        else
        {
            _gameManager.UnpauseGame();
            _pauseMenu.SetActive(false);
            _isGamePaused = false;
        }
    }
}
