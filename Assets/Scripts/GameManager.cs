using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public float turnDelay = 0.1f;
    public static GameManager instance = null;

    public int playerFoodPoints = 100;
    [HideInInspector] public bool playersTurn = true;
    // Start is called before the first frame update

    public BoardManager boardScript;
    private int level = 3;
    private List<Enemy> enemies;
    private bool enemiesMoving;
    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        enemies = new List<Enemy>();

        boardScript = GetComponent<BoardManager>();
        InitGame();
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        level++;
        boardScript = GetComponent<BoardManager>();
        InitGame();
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void InitGame(){
        enemies.Clear();
        playersTurn = true;
        enemiesMoving = false;
        boardScript.SetupScene(level);
    }

    public void GameOver(){
        enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (playersTurn || enemiesMoving)
        {
            return;
        }   
        StartCoroutine(MoveEnemies());
    }

    public void AddEnemiesToList(Enemy script)
    {
        enemies.Add(script);
    }

    IEnumerator MoveEnemies()
    {
        enemiesMoving = true;
        yield return new WaitForSeconds(turnDelay);
        if (enemies.Count == 0)
        {
            yield return new WaitForSeconds(turnDelay);
        }
        for (int i = 0; i < enemies.Count; ++i)
        {
            enemies[i].MoveEnemy();
            yield return new WaitForSeconds(enemies[i].moveTime);

        }
        playersTurn = true;
        enemiesMoving = false;
    }
}
