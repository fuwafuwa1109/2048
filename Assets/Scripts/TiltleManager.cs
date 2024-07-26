using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class TitleManager : MonoBehaviour
{
    [SerializeField] private GameObject TilePrefab;
    [SerializeField] private Button StartButton;
    [SerializeField] private Image ButtonImage;
    public GameObject[] Tile = new GameObject[5];
    public string stat = "0";
    private float speed = 2f;
    private float waitcount;
    private int count = 0;


    public static TitleManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        
    }

    private void Start()
    {
        SetTilePrefab(0, -2.5f, 0.5f);
        SetTilePrefab(1, -1.5f, 2.5f);
        SetTilePrefab(2, -0.5f, 0.5f);
        SetTilePrefab(3,  0.5f, 2.5f);
        SetTilePrefab(4,  1.5f, 1.5f);

        Color color = ButtonImage.color;
        color.a = 0;
        ButtonImage.color = color;
        StartButton.gameObject.SetActive(false);


    }

    private void Update()
    {
        switch (stat)
        {
            case ("0"):
            case ("2"):
                Tile[count].transform.Translate(Vector2.up * speed * Time.deltaTime);
                if (Tile[count].transform.position.y > 1.5f)
                {
                    Vector3 tilepos = Tile[count].transform.position;
                    tilepos.y = 1.5f;
                    Tile[count].transform.position = tilepos;
                    stat = "wait";
                }
                break;

            case ("1"):
            case ("3"):
                Tile[count].transform.Translate(Vector2.down * speed * Time.deltaTime);
                if (Tile[count].transform.position.y < 1.5f)
                {
                    Vector3 tilepos = Tile[count].transform.position;
                    tilepos.y = 1.5f;
                    Tile[count].transform.position = tilepos;
                    stat = "wait";
                }
                break;

            case ("merge"):
                for (int i = 0; i < 4; i++)
                {
                    Tile[i].transform.Translate(Vector2.right * speed * Time.deltaTime);
                }
                if (Tile[0].transform.position.x >= -1.5f)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        Vector3 pos = Tile[i].transform.position;
                        pos.x = -1.5f + i;
                        Tile[i].transform.position = pos;
                    }
                    stat = "merging";
                }
                break;



            case("menu"):
                StartButton.gameObject.SetActive(true);
                Color color = ButtonImage.color;
                color.a += Time.deltaTime * 3;
                ButtonImage.color = color;
                if (ButtonImage.color.a > 1)
                {
                    stat = "";
                }
                break;

            case (""):
                break;
            case ("wait"):
                waitcount += Time.deltaTime;
                if (waitcount > 0.2f && count < 3)
                {
                    waitcount = 0;
                    count++;
                    stat = count.ToString();
                    break;
                }
                else if (count == 3)
                {
                    
                    stat = "merge";
                }
                break;

        }
    }


    private void SetTilePrefab(int index, float posX, float posY)
    {
        Tile[index] = Instantiate(TilePrefab, new Vector3(posX, posY, 0), Quaternion.identity);
    }


    public void OnTitleButtonEnter()
    {
        SceneManager.LoadScene(1);
    }
}


