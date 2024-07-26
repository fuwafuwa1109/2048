using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuButtonManager : MonoBehaviour
{
    [SerializeField] private Button retry;
    [SerializeField] private Button home;
    [SerializeField] Image retryimage;
    [SerializeField] Image homeimage;
    [SerializeField] private TextMeshProUGUI GameOver;



    GameController controller;
    TileManager tilemanager;

    Color[,] tilecolor = new Color[4,4];
    Color boxcolor;
    Color retrycolor;
    Color homecolor;

    Material[,] tilematerial = new Material[4, 4];
    Material boxmaterial;
    

    float counttime = 0;
    int max = 4;
    bool openmenu = false;
    bool closemenu = false;
    bool gameover = true;
    int indent;

    string pastgamemode;
    private void Awake()
    {
        controller = GameController.Instance;
    }

    private void Start()
    {


        /*Material material = retryimage.material;
        Color color1 = material.color;
        color1.a = 1;
        material.color = color1;*/

        
        

        retrycolor = retryimage.color;
        homecolor = homeimage.color;

        retrycolor.a = 0;
        homecolor.a = 0;
        retryimage.color = retrycolor;
        homeimage.color = homecolor;

        Color color = GameOver.color;
        color.a = 0;
        GameOver.color = color;


        tilemanager = FindObjectOfType<TileManager>();

        GameOver.gameObject.SetActive(false);

        



    }

    


    private void Update()
    {
        if (openmenu)
        {
            counttime = Time.deltaTime * 3;
            for (int i = 0; i < max; i++)
            {
                for (int j = 0; j < max; j++)
                {
                    if (controller.tileBoard[j,i])
                    {
                        tilecolor[j, i].a -= counttime;
                        tilematerial[j, i].color = tilecolor[j, i];
                    }
                }
            }

            

            boxcolor.a -= counttime;
            boxmaterial.color = boxcolor;

            retrycolor.a += counttime;
            retryimage.color = retrycolor;

            homecolor.a += counttime;
            homeimage.color = homecolor;

            

            if (retrycolor.a >= 1)
            {
                controller.boxset.SetActive(false);
                for (int i = 0; i < max; i++)
                {
                    for (int j = 0; j < max; j++)
                    {
                        if (controller.tileBoard[j, i])
                        {
                            controller.tileBoard[j,i].SetActive(false);
                        }
                    }
                }
                openmenu = false;
                
            }


        }

        if (closemenu)
        {
            counttime = Time.deltaTime * 3;
            for (int i = 0; i < max; i++)
            {
                for (int j = 0; j < max; j++)
                {
                    if (controller.tileBoard[j, i])
                    {
                        tilecolor[j, i].a += counttime;
                        tilematerial[j, i].color = tilecolor[j, i];
                    }
                }
            }

            

            boxcolor.a += counttime;
            boxmaterial.color = boxcolor;

            retrycolor.a -= counttime;
            retryimage.color = retrycolor;

            homecolor.a -= counttime;
            homeimage.color = homecolor;



            if (retrycolor.a <= 0)
            {
                retry.gameObject.SetActive(false);
                home.gameObject.SetActive(false);
                closemenu = false;
                controller.gamestat = pastgamemode;

            }
        }

        if (controller.gamestat == "gameover" && controller.boxset.activeSelf && controller.checkgameover)
        {
            if (gameover)
            {
                retry.gameObject.SetActive(true);
                home.gameObject.SetActive(true);
                GameOver.gameObject.SetActive(true);
                boxmaterial = controller.boxset.GetComponent<SpriteRenderer>().material;
                boxcolor = boxmaterial.color;
                for (int i = 0; i < max; i++)
                {
                    for (int j = 0; j < max; j++)
                    {
                        if (controller.tileBoard[j, i])
                        {
                            tilematerial[j, i] = controller.tileBoard[j, i].GetComponent<SpriteRenderer>().material;
                            tilecolor[j, i] = tilematerial[j, i].color;
                        }
                    }
                }

                gameover = false;
            }
            counttime = Time.deltaTime * 0.5f;
            for (int i = 0; i < max; i++)
            {
                for (int j = 0; j < max; j++)
                {
                    if (controller.tileBoard[j, i])
                    {
                        tilecolor[j, i].a -= counttime;
                        tilematerial[j, i].color = tilecolor[j, i];
                    }
                }
            }



            boxcolor.a -= counttime;
            boxmaterial.color = boxcolor;

            retrycolor.a += counttime;
            retryimage.color = retrycolor;

            homecolor.a += counttime;
            homeimage.color = homecolor;

            Color textcolor = GameOver.color;
            textcolor.a += counttime;
            GameOver.color = textcolor;

            


            if (retrycolor.a >= 1)
            {
                controller.boxset.SetActive(false);
                for (int i = 0; i < max; i++)
                {
                    for (int j = 0; j < max; j++)
                    {
                        if (controller.tileBoard[j, i])
                        {
                            controller.tileBoard[j, i].SetActive(false);
                        }
                    }
                }

            }
        }

        if (controller.gamestat == "prepare")
        {
            controller.boxset.SetActive(true);
            boxcolor.a = 1;
            boxmaterial.color = boxcolor;

            retrycolor.a = 0;
            retryimage.color = retrycolor;
            retry.gameObject.SetActive(false);

            homecolor.a = 0;
            homeimage.color = homecolor;
            home.gameObject.SetActive(false);

            Color textcolor = GameOver.color;
            textcolor.a = 0;
            GameOver.color = textcolor;
            GameOver.gameObject.SetActive(false);

            controller.gamestat = "restart";
            gameover = true;

        }
    }

    public void OnButtonClick()
    {

        boxmaterial = controller.boxset.GetComponent<SpriteRenderer>().material;
        boxcolor = boxmaterial.color;
        
        if (retry.gameObject.activeSelf)
        {

            openmenu = false;
            closemenu = true;
            controller.boxset.SetActive(true);
            for (int i = 0; i < max; i++)
            {
                for (int j = 0; j < max; j++)
                {
                    if (controller.tileBoard[j, i])
                    {
                        controller.tileBoard[j, i].SetActive(true);
                        tilematerial[j, i] = controller.tileBoard[j, i].GetComponent<SpriteRenderer>().material;
                        tilecolor[j, i] = tilematerial[j, i].color;
                    }
                }
            }
            

            
        }

        if (!retry.gameObject.activeSelf)
        {
            pastgamemode = controller.gamestat;
            controller.gamestat = "stop";
            openmenu = true;
            closemenu = false;
            retry.gameObject.SetActive(true);
            home.gameObject.SetActive(true);
            for (int i = 0; i < max; i++)
            {
                for (int j = 0; j < max; j++)
                {
                    if (controller.tileBoard[j, i])
                    {
                        tilematerial[j, i] = controller.tileBoard[j, i].GetComponent<SpriteRenderer>().material;
                        tilecolor[j, i] = tilematerial[j, i].color;
                    }
                }
            }

            
        }
    }

    
}
