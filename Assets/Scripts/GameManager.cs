using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [Header("#Game Settings")]
    public bool IsLive;
    public float GameTime = 0f;
    public float MaxGameTime = 60f;
    [Header("#Player Settings")]
    public int Level;
    public int Kill;
    public int Exp;
    public float Health;
    public float MaxHealth = 100;
    public int[] NextExp = { 3, 5, 10, 100, 150, 210, 280, 360, 450, 600 };
    [Header("#References")]
    public Player Player;
    public PoolManager PoolManager;
    public LevelUp UILevelUp;
    public Result UIResult;
    public GameObject EnemyCleaner;

    void Awake()
    {
        Instance = this;
    }

    public void GameStart()
    {
        Health = MaxHealth;
        UILevelUp.Select(0);
        Resume();   
    }

    public void GameOver()
    {
        StartCoroutine(GameOverCoroutine());
    }

    IEnumerator GameOverCoroutine()
    {
        IsLive = false;
        yield return new WaitForSeconds(0.5f);
        UIResult.gameObject.SetActive(true);
        UIResult.Lose();
        Stop();
    }

    public void GameVictory()
    {
        StartCoroutine(GameVictoryCoroutine());
    }

    IEnumerator GameVictoryCoroutine()
    {
        IsLive = false;
        EnemyCleaner.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        UIResult.gameObject.SetActive(true);
        UIResult.Win();
        Stop();
    }

    public void GameRetry()
    {
        SceneManager.LoadScene(0);
    }

    void Update()
    {
        if (!IsLive)
            return;

        GameTime += Time.deltaTime;

        if (GameTime >= MaxGameTime)
        {
            GameTime = MaxGameTime;
            GameVictory();
        }
    }

    public void GetExp()
    {
        if (!IsLive)
            return;

        Exp++;
        if (Exp >= NextExp[Mathf.Min(Level, NextExp.Length - 1)])
        {
            Level++;
            Exp = 0;
            UILevelUp.Show();
        }
    }

    public void Stop()
    {
        IsLive = false;
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        IsLive = true;
        Time.timeScale = 1f;
    }
}
