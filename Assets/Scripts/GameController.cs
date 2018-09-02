using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameState
{
    StartMenu,
    Playing,
    Pause,
    GameOver
}

public class GameController : MonoBehaviour {

    //в начале массива - низ карты
    public readonly int[,] path = {
        { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        { 0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0},
        { 0,1,0,0,0,0,0,0,0,0,0,0,1,0,0,1,0,0,0,0,0,0,0,0,0,0,1,0},
        { 0,1,0,0,0,0,0,0,0,0,0,0,1,0,0,1,0,0,0,0,0,0,0,0,0,0,1,0},
        { 0,1,1,1,1,1,1,0,0,1,1,1,1,0,0,1,1,1,1,0,0,1,1,1,1,1,1,0},
        { 0,0,0,1,0,0,1,0,0,1,0,0,0,0,0,0,0,0,1,0,0,1,0,0,1,0,0,0},
        { 0,0,0,1,0,0,1,0,0,1,0,0,0,0,0,0,0,0,1,0,0,1,0,0,1,0,0,0},
        { 0,1,1,1,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,1,1,1,0},
        { 0,1,0,0,0,0,1,0,0,0,0,0,1,0,0,1,0,0,0,0,0,1,0,0,0,0,1,0},
        { 0,1,0,0,0,0,1,0,0,0,0,0,1,0,0,1,0,0,0,0,0,1,0,0,0,0,1,0},
        { 0,1,1,1,1,1,1,1,1,1,1,1,1,0,0,1,1,1,1,1,1,1,1,1,1,1,1,0},
        { 0,0,0,0,0,0,1,0,0,1,0,0,0,0,0,0,0,0,1,0,0,1,0,0,0,0,0,0},
        { 0,0,0,0,0,0,1,0,0,1,0,0,0,0,0,0,0,0,1,0,0,1,0,0,0,0,0,0},
        { 0,0,0,0,0,0,1,0,0,1,1,1,1,1,1,1,1,1,1,0,0,1,0,0,0,0,0,0},
        { 0,0,0,0,0,0,1,0,0,1,0,0,0,0,0,0,0,0,1,0,0,1,0,0,0,0,0,0},
        { 0,0,0,0,0,0,1,0,0,1,0,0,0,0,0,0,0,0,1,0,0,1,0,0,0,0,0,0},
        { 1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1}, //здесь середина и должно быть перемещение
        { 0,0,0,0,0,0,1,0,0,1,0,0,0,0,0,0,0,0,1,0,0,1,0,0,0,0,0,0},
        { 0,0,0,0,0,0,1,0,0,1,0,0,0,0,0,0,0,0,1,0,0,1,0,0,0,0,0,0},
        { 0,0,0,0,0,0,1,0,0,1,1,1,1,1,1,1,1,1,1,0,0,1,0,0,0,0,0,0},
        { 0,0,0,0,0,0,1,0,0,0,0,0,1,0,0,1,0,0,0,0,0,1,0,0,0,0,0,0},
        { 0,0,0,0,0,0,1,0,0,0,0,0,1,0,0,1,0,0,0,0,0,1,0,0,0,0,0,0},
        { 0,1,1,1,1,1,1,0,0,1,1,1,1,0,0,1,1,1,1,0,0,1,1,1,1,1,1,0},
        { 0,1,0,0,0,0,1,0,0,1,0,0,0,0,0,0,0,0,1,0,0,1,0,0,0,0,1,0},
        { 0,1,0,0,0,0,1,0,0,1,0,0,0,0,0,0,0,0,1,0,0,1,0,0,0,0,1,0},
        { 0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0},
        { 0,1,0,0,0,0,1,0,0,0,0,0,1,0,0,1,0,0,0,0,0,1,0,0,0,0,1,0},
        { 0,1,0,0,0,0,1,0,0,0,0,0,1,0,0,1,0,0,0,0,0,1,0,0,0,0,1,0},
        { 0,1,0,0,0,0,1,0,0,0,0,0,1,0,0,1,0,0,0,0,0,1,0,0,0,0,1,0},
        { 0,1,1,1,1,1,1,1,1,1,1,1,1,0,0,1,1,1,1,1,1,1,1,1,1,1,1,0},
        { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}
    };

    public GameObject points;
    public GameObject enemies;

    public GameState GameState { get; private set; }
    private int score;
    private int lives;
    private int level;
    private int pointCount = 240;
    private readonly int scoreForPoint = 10;
    private readonly int scoreForEnergizer = 50;
    private readonly int scoreForEnemy = 200;
    private int scaredEnemyHit;
    private EnemyController[] enemiesArr;
    private Transform[] pointsArr;

    public int Score {
        get {
            return score;
        }
        private set {
            score = value;
            scoreUI.text = "Score: " + value.ToString();
        } }

    public int Lives {
        get {
            return lives;
        }
        private set {
            if (value == 0)
            {
                GameOver();
            }
            else
            {
                lives = value;
                livesUI.text = "Lives: " + value.ToString();
            }
        } }

    public int Level {
        get {
            return level;
        }
            private set {
            level = value;
            levelUI.text = "Level: " + value.ToString();
        } }

    public PacmanController pacman;

    public Text scoreUI;
    public Text livesUI;
    public Text levelUI;
    public Button buttonPlay;

    private static GameController instance;

    static GameController()
    {

    }

    public static GameController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType(typeof(GameController)) as GameController;
            }
            if (instance == null)
            {
                GameObject obj = new GameObject("GameController");
                instance = obj.AddComponent(typeof(GameController)) as GameController;
                Debug.Log("Could not locate an GameController object. GameController was Generated Automaticly.");
            }
            return instance;
        }
    }

    void Start() {
        pacman.PointIsEaten += Pacman_PointIsEaten;
        pacman.EnergizerIsEaten += Pacman_EnergizerIsEaten;
        pacman.EnemyHit += Pacman_EnemyHit;
        pacman.ScaredEnemyHit += Pacman_ScaredEnemyHit;
        enemiesArr = enemies.GetComponentsInChildren<EnemyController>(true);
        pointsArr = points.GetComponentsInChildren<Transform>(true);
        ToStartMenu();
    }

    private void Pacman_ScaredEnemyHit(object sender, System.EventArgs e)
    {
        AddScore(scoreForEnemy * scaredEnemyHit);
        scaredEnemyHit++;
    }

    private void Pacman_EnemyHit(object sender, System.EventArgs e)
    {
        RemoveLive();
        pacman.ToStartPosition();
    }

    private void Pacman_EnergizerIsEaten(object sender, System.EventArgs e)
    {
        AddScore(scoreForEnergizer);
        StartCoroutine(pacman.Boost());
        if (Level < 19)
        {
            StartCoroutine(Scare());
        }
    }

    private void Pacman_PointIsEaten(object sender, System.EventArgs e)
    {
        AddScore(scoreForPoint);
        pointCount--;
        switch (pointCount)
        {
            case 0:
                NextLevel();
                break;
            case 60:
                enemiesArr[3].gameObject.SetActive(true);
                break;
            case 120:
                enemiesArr[2].gameObject.SetActive(true);
                break;
            case 180:
                enemiesArr[1].gameObject.SetActive(true);
                break;
        }
    }

    public void StartGame()
    {
        GameState = GameState.Playing;
        buttonPlay.gameObject.SetActive(false);
        Score = 0;
        enemiesArr[0].gameObject.SetActive(true);
    }

    private void Pause()
    {
        GameState = GameState.Pause;
    }

    private void GameOver()
    {
        GameState = GameState.GameOver;
        ToStartMenu();
    }

    private void ToStartMenu()
    {
        GameState = GameState.StartMenu;
        buttonPlay.gameObject.SetActive(true);
        Lives = 3;
        Level = 1;
        scaredEnemyHit = 1;
        ShowPoints();
        pacman.ToStartPosition();
        pacman.ToStartSpeed();
        foreach (var enemy in enemiesArr)
        {
            enemy.gameObject.SetActive(false);
            enemy.ToStartPosition();
            enemy.ToStartSpeed();
        }
    }

    private void NextLevel()
    {
        Level++;
        ShowPoints();
        pacman.ToStartPosition();
        pacman.SpeedUp();
        foreach (var enemy in enemiesArr)
        {
            enemy.gameObject.SetActive(false);
            enemy.ToStartPosition();
            enemy.SpeedUp();
        }
        enemiesArr[0].gameObject.SetActive(true);
    }

    private void ShowPoints()
    {
        //показываем снова все точки
        foreach (var point in pointsArr)
        {
            point.gameObject.SetActive(true);
        }
    }

    public void AddScore(int plus)
    {
        Score += plus;
    }

    public void RemoveLive()
    {
        Lives--;
        pacman.ToStartPosition();
    }

   IEnumerator Scare()
    {
        foreach (var enemy in enemiesArr)
        {
            enemy.Scare();
        }
        yield return new WaitForSeconds(4);
        foreach (var enemy in enemiesArr)
        {
            enemy.Unscare();
        }
    }

}
