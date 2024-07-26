using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{

    int max = 4;
    //************************盤面の数字とタイルの移動距離を計算するロジック***********************

    public int[,] MoveTileDistance(string wheretogo, int[,] GameBoard)
    {
        int[,] PastBoard = new int[4, 4];
        int[,] output = new int[4, 4];
        int[] updateline = new int[4] { 0, 0, 0, 0 };
        int updatecount = 0;


        for (int x = 0; x < max; x++)
        {
            for (int y = 0; y < max; y++)
            {
                PastBoard[y, x] = GameBoard[y, x];
            }
        }



        switch (wheretogo)
        {
            case "up":
                for (int x = 0; x < max; x++)
                {
                    updateline = new int[4] { 0, 0, 0, 0 };
                    updatecount = 0;
                    for (int y = 0; y < max; y++)
                    {
                        if (GameBoard[y, x] != 0)
                        {
                            updateline[updatecount] = GameBoard[y, x];
                            output[y, x] = y - updatecount;
                            updatecount++;
                        }
                        else
                        {
                            output[y, x] = 0;
                        }

                    }


                    updateline = SortIndex(updateline, output, PastBoard, x, wheretogo);

                    for (int y = 0; y < max; y++)
                    {
                        GameBoard[y, x] = updateline[y];
                    }
                }

                return output;

            case "down":
                for (int x = 0; x < max; x++)
                {
                    updateline = new int[4] { 0, 0, 0, 0 };
                    updatecount = 0;
                    for (int y = max - 1; y >= 0; y--)
                    {
                        if (GameBoard[y, x] != 0)
                        {
                            updateline[updatecount] = GameBoard[y, x];
                            output[y, x] = max - 1 - updatecount - y;
                            updatecount++;
                        }
                        else
                        {
                            output[y, x] = 0;
                        }
                    }

                    updateline = SortIndex(updateline, output, PastBoard, x, wheretogo);

                    for (int y = max - 1; y >= 0; y--)
                    {
                        GameBoard[y, x] = updateline[3 - y];
                    }
                }

                return output;

            case "left":
                for (int y = 0; y < max; y++)
                {
                    updateline = new int[4] { 0, 0, 0, 0 };
                    updatecount = 0;
                    for (int x = 0; x < max; x++)
                    {
                        if (GameBoard[y, x] != 0)
                        {
                            updateline[updatecount] = GameBoard[y, x];
                            output[y, x] = x - updatecount;
                            updatecount++;
                        }
                        else
                        {
                            output[y, x] = 0;
                        }
                    }

                    updateline = SortIndex(updateline, output, PastBoard, y, wheretogo);

                    for (int x = 0; x < max; x++)
                    {
                        GameBoard[y, x] = updateline[x];
                    }
                }

                return output;

            case "right":
                for (int y = 0; y < max; y++)
                {
                    updateline = new int[4] { 0, 0, 0, 0 };
                    updatecount = 0;
                    for (int x = max - 1; x >= 0; x--)
                    {
                        if (GameBoard[y, x] != 0)
                        {
                            updateline[updatecount] = GameBoard[y, x];
                            output[y, x] = max - 1 - updatecount - x;
                            updatecount++;
                        }
                        else
                        {
                            output[y, x] = 0;
                        }
                    }

                    updateline = SortIndex(updateline, output, PastBoard, y, wheretogo);

                    for (int x = max - 1; x >= 0; x--)
                    {
                        GameBoard[y, x] = updateline[3 - x];
                    }
                }

                return output;
            default:
                Debug.Log("文字が違っています");

                return output;



        }
    }

    private int[] SortIndex(int[] list, int[,] array, int[,] past, int index, string wheretogo)
    {
        int[] updateline = new int[4] { 0, 0, 0, 0 };
        int linecount = 1;
        int speedincrease = 0;
        int nonzerocount = 0;
        updateline[0] = list[0];

        for (int i = 0; i < max; i++)
        {
            if (list[i] != 0)
            {
                nonzerocount++;
            }
        }

        for (int i = 1; i < max; i++)
        {
            if (list[i] == list[i - 1] && list[i] != 0)
            {
                updateline[linecount - 1] = list[i] + list[i - 1];
                list[i] = 0;
                speedincrease = nonzerocount - linecount;
                if (wheretogo == "up")
                {
                    for (int j = max - 1; j >= 0; j--)
                    {
                        if (speedincrease > 0)
                        {
                            if (past[j, index] != 0)
                            {
                                array[j, index] = array[j, index] + 1;
                                speedincrease--;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                if (wheretogo == "down")
                {
                    for (int j = 0; j < max; j++)
                    {
                        if (speedincrease > 0)
                        {
                            if (past[j, index] != 0)
                            {
                                array[j, index] = array[j, index] + 1;
                                speedincrease--;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                if (wheretogo == "left")
                {
                    for (int j = max - 1; j >= 0; j--)
                    {
                        if (speedincrease > 0)
                        {
                            if (past[index, j] != 0)
                            {
                                array[index, j] = array[index, j] + 1;
                                speedincrease--;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                if (wheretogo == "right")
                {
                    for (int j = 0; j < max; j++)
                    {
                        if (speedincrease > 0)
                        {
                            if (past[index, j] != 0)
                            {
                                array[index, j] = array[index, j] + 1;
                                speedincrease--;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }


            }
            else if (list[i] != 0)
            {
                updateline[linecount] = list[i];
                linecount++;
            }
        }

        return updateline;
    }

    


}
