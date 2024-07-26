using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TileManager : MonoBehaviour
{
    [SerializeField] private GameObject TextPrefab;
    public int score;
    public TextMeshProUGUI numbertext;
    private int max;
    private string CanvasTag = "GameController";
    public Dictionary<string, GameObject> instantiatedObjects = new Dictionary<string, GameObject>();
    string textname;
    Renderer objectcolor;

    float red = 0;
    float green = 1f;
    float blue = 1f;

    bool expandscale = true;
    bool Bigger = false;
    bool Smaller;
    bool firstcheck = false;

    int number;


    GameController controller;



    private void Awake()
    {
        controller = GameController.Instance;
    }

    private void Start()
    {
        GameObject canvasObject = GameObject.FindWithTag(CanvasTag);
        Canvas canvas = canvasObject.GetComponent<Canvas>();
        GameObject textObject = Instantiate(TextPrefab, canvas.transform);
        numbertext = textObject.GetComponent<TextMeshProUGUI>();
        objectcolor = GetComponent<Renderer>();

        int x = (int)transform.position.x;
        int y = 3 - (int)transform.position.y;
        numbertext.text = controller.GameBoard[y, x].ToString();
        textname = controller.tilecount.ToString();
        numbertext.name = textname;
        instantiatedObjects[textname] = textObject;
        controller.tilecount++;
    
        objectcolor.material.color = new Color(red, green, blue);
        transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        int indexX = (int)transform.position.x;
        int indexY = 3 - (int)transform.position.y;
        number = controller.GameBoard[indexY, indexX];
        ChangeColor(number);

        RectTransform recttransform = numbertext.GetComponent<RectTransform>();
        recttransform.sizeDelta = new Vector2(3, 3);

        firstcheck = true;

    }

    private void Update()
    {
        if (expandscale)
        {
            transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
            if (transform.localScale.x > 1.3f)
            {
                transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
                expandscale = false;
                if (controller.gamestat == "gameover")
                {
                    controller.checkgameover = true;
                }
            }
        }

        if (Bigger)
        {
            transform.localScale += new Vector3(0.015f, 0.015f, 0.015f);
            if (transform.localScale.x > 1.5f)
            {
                Bigger = false;
                Smaller = true;
            }
        }

        if (Smaller)
        {
            transform.localScale -= new Vector3(0.015f, 0.015f, 0.015f);
            if (transform.localScale.x <= 1.3f)
            {
                transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
                Smaller = false;
            }
        }
        // タイルのワールド座標を取得
        Vector3 worldPosition = transform.position;
        // ワールド座標をスクリーン座標に変換
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);

        // Canvas の RectTransform の参照を取得
        RectTransform canvasRect = numbertext.canvas.GetComponent<RectTransform>();
        Vector2 canvasPosition;
        // スクリーン座標を Canvas のローカル座標に変換
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPosition, Camera.main, out canvasPosition);

        // テキストオブジェクトの位置を更新
        numbertext.rectTransform.localPosition = canvasPosition;

        Color textcolor = numbertext.color;
        textcolor.a = objectcolor.material.color.a;
        numbertext.color = textcolor;




        int indexX = (int)transform.position.x;
        int indexY = 3 - (int)transform.position.y;
        if (controller.textupdate[indexY, indexX])
        {
            if (number != controller.GameBoard[indexY, indexX])
            {
                Bigger = true;
            }
            if (number != controller.GameBoard[indexY, indexX])
            {
                controller.score += controller.GameBoard[indexY, indexX];
                controller.Score.text = controller.score.ToString();
            }
            number = controller.GameBoard[indexY, indexX];
            numbertext.text = number.ToString();
            ChangeColor(number);
            

            objectcolor.material.color = new Color(red, green, blue);
            controller.textupdate[indexY, indexX] = false;
        }




    }

    private void OnDestroy()
    {
        if (instantiatedObjects.ContainsKey(textname))
        {
            Destroy(instantiatedObjects[textname]);
            instantiatedObjects.Remove(textname);
            firstcheck = false;
        }
    }

    private void OnDisable()
    {
        numbertext.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        if (firstcheck)
        {
            numbertext.gameObject.SetActive(true);

        }
    }


    private void ChangeColor(int number)
    {
        int colormode = (int)Mathf.Log(number, 2.0f) % 18;
        if (colormode <= 3) //R 0 G 1 B down
        {
            red = 0;
            blue = 1f - 0.33f * colormode;  
        }
        else if (colormode <= 6) //R up G 1 B 0
        {
            blue = 0;
            red = 0.33f * (colormode - 3);
        } 
        else if (colormode <= 9) //R 1 G down B 0
        {
            red = 1f;
            green = 1f - 0.33f * (colormode - 6);
        }
        else if (colormode <= 12) // R 1 G 0 B up
        {
            green = 0;
            blue = 0.33f * (colormode - 9);
        }
        else if (colormode <= 15) // R down G 0 B 1
        {
            blue = 1f;
            red = 1f - 0.33f * (colormode - 12);
        }
        else //R 0 G up B 1
        {
            red = 0;
            green = 0.33f * (colormode - 15);
        }
        objectcolor.material.color = new Color(red, green, blue);
        numbertext.color = GetSimilarColor(objectcolor.material.color);

    }

    private Color GetSimilarColor(Color color)
    {
        Color.RGBToHSV(color, out float h, out float s, out float v);

       
        v = Mathf.Clamp01(v * 0.5f);


        return Color.HSVToRGB(h, s, v);
    }



}

