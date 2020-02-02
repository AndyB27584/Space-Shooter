using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab, _enemyContainer;
    [SerializeField]
    private GameObject[] _powerUps;
    private bool _isGameOver = false;

   
    void Start()
    {
       
    }

    void Update()
    {
        
    }

    IEnumerator SpawnEnemy()
    {
        while (!_isGameOver)
        {
            float randomXPosition = Random.Range(-9.45f, 9.45f);
            GameObject enemy = Instantiate(_enemyPrefab, new Vector3(randomXPosition, 7.1f, 0),Quaternion.identity);
            enemy.transform.parent = _enemyContainer.transform;

            float randomTime = Random.Range(2.0f, 7.0f);
            yield return new WaitForSeconds(randomTime);
        }
    }

    IEnumerator SpawnPowerUp()
    {
        while (!_isGameOver)
        {
            float randomXPosition = Random.Range(-9.5f, 9.5f);
            int randomPowerUpIndex = Random.Range(0, _powerUps.Length);
            Instantiate(_powerUps[randomPowerUpIndex], new Vector3(randomXPosition, 7.1f, 0), Quaternion.identity);

            float randomTime = Random.Range(3.0f, 7.0f);
            yield return new WaitForSeconds(randomTime);
        }
    }

    public void StopSpawning()
    {
        _isGameOver = true;

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if(enemies != null)
        {
            foreach (GameObject enemy in enemies)
            {
                Enemy enemyScript = enemy.GetComponent<Enemy>();
                enemyScript.DestroyEnemy();
            }
        }
    }

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemy());
        StartCoroutine(SpawnPowerUp());
    }
}
