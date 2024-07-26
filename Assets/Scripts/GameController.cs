using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject tileprefab;
    [SerializeField] private GameObject box;
    [SerializeField] private Button menu;
    [SerializeField] public TextMeshProUGUI Score;
    public GameObject boxset;
    public int[,] GameBoard = new int[4, 4];
    private int[,] pastBoard = new int[4, 4];
    public GameObject[,] tileBoard = new GameObject[4, 4];
    private int max = 4;
    public int score = 0;
    public string gamestat = "wait";
    int[,] movedistance = new int[4, 4];
    public float movetime;
    public bool[,] textupdate = new bool[4, 4];
    public bool checkgameover = false;
    public int tilecount = 0;

    Move move;
    TileGrid grid;
    TileManager[,] tilemanager = new TileManager[4, 4];


    public static GameController Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    void Start()
    {
        grid = gameObject.AddComponent<TileGrid>();
        move = gameObject.AddComponent<Move>();
        Score.text = "0";

        for (int x = 0; x < max; x++)
        {
            for (int y = 0; y < max; y++)
            {
                GameBoard[x, y] = 0;
                textupdate[y, x] = false;
            }
        }

        boxset = Instantiate(box, new Vector3(1.5f, 1.5f, 0), Quaternion.identity);

        grid.SpawnTile(GameBoard, tileBoard, tileprefab);
        grid.SpawnTile(GameBoard, tileBoard, tileprefab);


    }

    private void Update()
    {
        switch (gamestat)
        {
            case "wait":
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    gamestat = "up";
                    pastBoard = CopyGameBoard(GameBoard);
                    movedistance = move.MoveTileDistance(gamestat, GameBoard);
                    for (int x = 0; x < max; x++)
                    {
                        for (int y = 0; y < max; y++)
                        {
                            if (movedistance[y, x] != 0)
                            {
                                return;
                            }
                        }
                    }
                    gamestat = "wait";
                    return;
                }

                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    gamestat = "down";
                    pastBoard = CopyGameBoard(GameBoard);
                    movedistance = move.MoveTileDistance(gamestat, GameBoard);
                    for (int x = 0; x < max; x++)
                    {
                        for (int y = 0; y < max; y++)
                        {
                            if (movedistance[y, x] != 0)
                            {
                                return;
                            }
                        }
                    }
                    gamestat = "wait";
                    return;

                }

                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    gamestat = "left";
                    pastBoard = CopyGameBoard(GameBoard);
                    movedistance = move.MoveTileDistance(gamestat, GameBoard);
                    for (int x = 0; x < max; x++)
                    {
                        for (int y = 0; y < max; y++)
                        {
                            if (movedistance[y, x] != 0)
                            {
                                return;
                            }
                        }
                    }
                    gamestat = "wait";
                    return;

                }

                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    gamestat = "right";
                    pastBoard = CopyGameBoard(GameBoard);
                    movedistance = move.MoveTileDistance(gamestat, GameBoard);
                    for (int x = 0; x < max; x++)
                    {
                        for (int y = 0; y < max; y++)
                        {
                            if (movedistance[y, x] != 0)
                            {
                                return;
                            }
                        }
                    }
                    gamestat = "wait";
                    return;

                }
                break;

            case "up":
            case "down":
            case "left":
            case "right":
                movetime += Time.deltaTime;
                grid.MoveTile(movedistance, tileBoard, gamestat, movetime);

                if (movetime > 0.2f)
                {
                    GameObject[,] tileBoardcopy = grid.MergeAndRound(pastBoard, movedistance, tileBoard, gamestat);
                    for (int y = 0; y < max; y++)
                    {
                        for (int x = 0; x < max; x++)
                        {
                            tileBoard[y, x] = tileBoardcopy[y, x];
                        }
                    }
                    WhichTextToUpdate(textupdate, GameBoard);
                    movetime = 0;
                    gamestat = "wait";
                    grid.SpawnTile(GameBoard, tileBoard, tileprefab);

                    //DebugLogTwoDimensionalGameObject(tileBoard);
                    //DebugLogTwoDimensionalArray(GameBoard);
                    //DebugLogTwoDimensionalArray(pastBoard);
                    //DebugLogTwoDimensionalArray(movedistance);
                    //vDebugLogGameOver();
                    if (JudgeGameOver(GameBoard))
                    {
                        gamestat = "gameover";
                        menu.gameObject.SetActive(false);
                    }
                }
                break;

            case "restart":
                grid.SpawnTile(GameBoard, tileBoard, tileprefab);
                grid.SpawnTile(GameBoard, tileBoard, tileprefab);
                gamestat = "wait";

                break;
        }
    }

    private int[,] CopyGameBoard(int[,] GameBoard)
    {
        int[,] newboard = new int[4, 4];

        for (int i = 0; i < max; i++)
        {
            for (int j = 0; j < max; j++)
            {
                newboard[j, i] = GameBoard[j, i];
            }
        }
        return newboard;
    }

    private void DebugLogTwoDimensionalArray(int[,] output)
    {
        int row = output.GetLength(0);
        int col = output.GetLength(1);
        string text = "";
        for (int i = 0; i < col; i++)
        {
            for (int j = 0; j < row; j++)
            {
                text += output[i, j] + "\t";
            }
            text += "\n";
        }

        Debug.Log(text);
    }


    private void DebugLogTwoDimensionalGameObject(GameObject[,] tileboard)
    {
        int[,] output = new int[4, 4];
        for (int x = 0; x < max; x++)
        {
            for (int y = 0; y < max; y++)
            {
                if (tileboard[y, x])
                {
                    output[y, x] = 1;
                }
                else
                {
                    output[y, x] = 0;
                }

            }
        }

        DebugLogTwoDimensionalArray(output);
    }

    private void WhichTextToUpdate(bool[,] textupdate, int[,] movedistance)
    {
        for (int y = 0; y < max; y++)
        {
            for (int x = 0; x < max; x++)
            {
                if (movedistance[y, x] != 0)
                {
                    textupdate[y, x] = true;
                }
            }
        }

   


    }

    private bool JudgeGameOver(int[,] GameBoard)
    {

        for (int x = 0; x < max; x++)
        {
            for (int y = 0; y < max; y++)
            {
                if (GameBoard[y,x] == 0)
                {
                    return false;
                }
            }
        }

        for (int x = 0; x < max; x++)
        {
            for (int y = 0; y < max-1; y++)
            {
                if (GameBoard[y, x] == GameBoard[y + 1, x] || GameBoard[x, y] == GameBoard[x, y + 1])
                {
                    return false;
                }

            }
        }

        return true;
    }

    public void RestartGame()
    {
        gamestat = "prepare";
        menu.gameObject.SetActive(true);
        movetime = 0;
        score = 0;
        Score.text = "0";
        checkgameover = false;
        for (int x = 0; x < max; x++)
        {
            for (int y = 0; y < max; y++)
            {
                GameBoard[y, x] = 0;
                pastBoard[y, x] = 0;
                movedistance[y, x] = 0;
                textupdate[y, x] = false;
            }
        }

        for (int x = 0; x < max; x++)
        {
            for (int y = 0; y < max; y++)
            {
                Destroy(tileBoard[y, x]);
            }
        }
    }

    private void DebugLogGameOver()
    {
        int count = 1;
        for (int i = 0; i < max; i++)
        {
            for (int j = 0; j < max; j++)
            {
                if (GameBoard[i,j] == 0)
                {
                    GameBoard[i, j] = count;
                    count += 2;
                }
            }
        }
    }

    public void OnRestartButtonClick()
    {
        SceneManager.LoadScene(0);
    }
}

