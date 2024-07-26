using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGrid : MonoBehaviour
{
    private int max = 4;
    private int zerocount = 0;

    



    //***********************タイルをスポーンさせるスクリプト**********************
    public void SpawnTile(int[,] GameBoard, GameObject[,] TileBoard, GameObject tile)
    {
        zerocount = 0;
        for (int x = 0; x < max; x++)
        {
            for (int y = 0; y < max; y++)
            {
                if (GameBoard[y, x] == 0)
                {
                    zerocount++;
                }
            }
        }

        int spawnindex = Random.Range(0, zerocount);
        int spawntype = Random.Range(0, 100);

        for (int x = 0; x < max; x++)
        {
            for (int y = 0; y < max; y++)
            {
                if (GameBoard[y,x] == 0)
                {
                    zerocount--;
                    if (zerocount == spawnindex)
                    {
                        TileBoard[y,x] = Instantiate(tile, new Vector3(x,3-y,0), Quaternion.identity);
                        if (spawntype > 90)
                        {
                            GameBoard[y, x] = 4;
                        }
                        else
                        {
                            GameBoard[y, x] = 2;


                        }
                        return;
                    }

                }
            }
        }

        
        
    }

    //*********************各オブジェクトの動くべき距離についてフレームレートに従わせながら動かす*********************

    public void MoveTile(int[,] moveboard, GameObject[,] Tile, string wheretogo, float movetime)
    {
        switch (wheretogo)
        {
            case "up":
                for (int x = 0; x < max; x++)
                {
                    for (int y = 0; y < max; y++)
                    {
                        if (moveboard[y,x] != 0)
                        {
                            Vector3 pos = Tile[y, x].transform.position;
                            if (pos.y >= 0 && pos.y <= 3 && pos.y != 3 - y + moveboard[y, x])
                            {
                                if (pos.y + Time.deltaTime * 15 < 3 - y + moveboard[y, x])
                                {
                                    pos.y = 3 - y + movetime * 15;
                                }
                                else
                                {
                                    pos.y = 3 - y + moveboard[y, x];
                                }
                            }
                            Tile[y, x].transform.position = pos;
                        }
                    }
                }
                break;
            case "down":
                for (int x = 0; x < max; x++)
                {
                    for (int y = 0; y < max; y++)
                    {
                        if (moveboard[y, x] != 0)
                        {
                            Vector3 pos = Tile[y, x].transform.position;
                            if (pos.y >= 0 && pos.y <= 3 && pos.y != 3 - y - moveboard[y,x])
                            {
                                if (pos.y - Time.deltaTime * 15 > 3 - y - moveboard[y, x])
                                {
                                    pos.y = 3 - y - movetime * 15;

                                }
                                else
                                {
                                    pos.y = 3 - y - moveboard[y, x];
                                }
                            }
                            Tile[y, x].transform.position = pos;
                        }
                    }
                }
                break;
            case "left":
                for (int y = 0; y < max; y++)
                {
                    for (int x = 0; x < max; x++)
                    {
                        if (moveboard[y, x] != 0)
                        {
                            Vector3 pos = Tile[y, x].transform.position;
                            if (pos.x >= 0 && pos.x <= 3 && pos.x != x - moveboard[y, x])
                            {
                                if (pos.x - Time.deltaTime * 15 > x - moveboard[y, x])
                                {
                                    pos.x = x - movetime * 15;

                                }
                                else
                                {
                                    pos.x = x - moveboard[y, x];
                                }
                            }
                            Tile[y, x].transform.position = pos;
                        }
                    }
                }
                break;

            case "right":
                for (int y = 0; y < max; y++)
                {
                    for (int x = 0; x < max; x++)
                    {
                        if (moveboard[y, x] != 0)
                        {
                            Vector3 pos = Tile[y, x].transform.position;
                            if (pos.x >= 0 && pos.x <= 3 && pos.x != x + moveboard[y, x])
                            {
                                if (pos.x + Time.deltaTime * 15 < x + moveboard[y, x])
                                {
                                    pos.x = x + movetime * 15;

                                }
                                else
                                {
                                    pos.x = x + moveboard[y, x];
                                }
                            }
                            Tile[y, x].transform.position = pos;
                        }
                    }
                }
                break;

        }
    }



    


    public GameObject[,] MergeAndRound(int[,] pastBoard, int[,] moveboard, GameObject[,] TileBoard, string wheretogo)
    {
        GameObject[,] TileBoardCopy = new GameObject[4, 4];

        

        for (int x = 0; x < max; x++)
        {
            for (int y = 0; y < max; y++)
            {
                if (pastBoard[y,x] != 0)
                {
                    Vector3 pos = TileBoard[y, x].transform.position;
                    if (wheretogo == "up")
                    {
                        pos.y = 3 - y + moveboard[y, x];
                        TileBoard[y, x].transform.position = pos;
                    }

                    if (wheretogo == "down")
                    {
                        pos.y = 3 - y - moveboard[y, x];
                        TileBoard[y, x].transform.position = pos;
                    }

                    if (wheretogo == "left")
                    {
                        pos.x = x - moveboard[y, x];
                        TileBoard[y, x].transform.position = pos;
                    }

                    if (wheretogo == "right")
                    {
                        pos.x = x + moveboard[y, x];
                        TileBoard[y, x].transform.position = pos;
                    }

                    int indexX = Mathf.Clamp((int)TileBoard[y, x].transform.position.x, 0, max - 1);
                    int indexY = Mathf.Clamp(3 - (int)TileBoard[y, x].transform.position.y, 0, max - 1);

                    
                    


                    if (TileBoardCopy[indexY, indexX])
                    {
                        GameObject tileToDestroy = TileBoard[y, x];
                        TileBoard[y, x] = null;

                        Destroy(tileToDestroy);
                        continue;
                    }
                    TileBoardCopy[indexY, indexX] = TileBoard[y, x];
                    


                }
            }
        }

        return TileBoardCopy;


    }

    public void RoundPosition(int[,] GameBoard, GameObject[,] tile)
    {
        for (int x = 0; x < max; x++)
        {
            for (int y = 0; y < max; y++)
            {
                if (GameBoard[y,x] != 0)
                {
                    Vector3 position = tile[y, x].transform.position;
                    position.x = Mathf.Round(position.x);
                    position.y = Mathf.Round(position.y);
                    tile[y, x].transform.position = position;
                }
            }
        }
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



}
