using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] private GameObject TextPrefab;

    private TextMeshProUGUI numbertext;
    private string CanvasTag = "Title";
    private string[] textlist = new string[5] { "2", "0", "4", "4", "4" };
    private float counttime;
    private bool big = false;
    private bool small = false;
    private string thisindex;
    private Color[] ColorPallete = new Color[5];
    private Dictionary<string, GameObject> instantiatedObjects = new Dictionary<string, GameObject>();


    Renderer ObjectColor;

    TitleManager titlemanager;

    

    private void Start()
    {
        ObjectColor = GetComponent<Renderer>();
        ColorPallete[0] = new Color(0, 1, 0.66f);
        ColorPallete[1] = new Color(0.8f, 1f, 0);
        ColorPallete[2] = new Color(0, 1, 0.33f);
        ColorPallete[3] = new Color(0, 1, 0.33f);
        ColorPallete[4] = new Color(0, 1, 0.33f);

        GameObject canvasObject = GameObject.FindWithTag(CanvasTag);
        Canvas canvas = canvasObject.GetComponent<Canvas>();
        GameObject textObject = Instantiate(TextPrefab, canvas.transform);
        numbertext = textObject.GetComponent<TextMeshProUGUI>();


        titlemanager = TitleManager.Instance;

        for (int i = 0; i < 5; i++)
        {
            if (titlemanager.Tile[i] == gameObject)
            {
                numbertext.text = textlist[i];
                ObjectColor.material.color = ColorPallete[i];
                numbertext.color = GetSimilarColor(ObjectColor.material.color);
                instantiatedObjects[i.ToString()] = textObject;
                thisindex = i.ToString();


                
            }
        }

       
    }

    private void Update()
    {
        MoveText(numbertext);
    }

    private void MoveText(TextMeshProUGUI numbertext)
    {
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

        if (titlemanager.stat == "merging" && thisindex == "4")
        {
            Destroy(gameObject);
        }

        if (titlemanager.stat == "merging" && thisindex == "3" && !big && !small)
        {
            big = true;
            numbertext.text = "8";
        }

        if (big)
        {
            transform.localScale += new Vector3(0.015f, 0.015f, 0.015f);
            if (transform.localScale.x > 1.5f)
            {
                big = false;
                small = true;
            }
        }

        if (small)
        {
            transform.localScale -= new Vector3(0.015f, 0.015f, 0.015f);
            if (transform.localScale.x <= 1.3f)
            {
                transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
                small = false;
                
                titlemanager.stat = "menu";
            }
        }
    }

    private Color GetSimilarColor(Color color)
    {
        Color.RGBToHSV(color, out float h, out float s, out float v);


        v = Mathf.Clamp01(v * 0.5f);


        return Color.HSVToRGB(h, s, v);
    }

    private void OnDestroy()
    {
        if (instantiatedObjects.ContainsKey(thisindex))
        {
            Destroy(instantiatedObjects[thisindex]);
            instantiatedObjects.Remove(thisindex);
        }
    }

    
}
