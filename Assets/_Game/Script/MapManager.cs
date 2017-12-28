using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapManager : MonoBehaviour {
    GameObject tiles;//地图
    Transform transform;
    Button typeBtn;//类型按钮
    
    mapType type;//地图类型变量
    public enum mapType//地图类型
    {
        Dirt,
        Grass,
        Mars,
        Sand,
        Stone
    }
	// Use this for initialization
	void Start () {
        tiles = Resources.Load<GameObject>("Prefabs/Titles");
        transform = GameObject.Find("Canvas/Maps").transform;

        Transform tempTransform = GameObject.Find("Canvas/Button").transform;   //Button's transform
        foreach (Transform item in tempTransform)                               //添加按钮事件
        {
            item.GetComponent<Button>().onClick.AddListener(
                () =>
                {
                    if (Enum.IsDefined(typeof(mapType), item.name))
                    {
                        ChangeMap((mapType)Enum.Parse(typeof(mapType), item.name));
                    }
                });
        }
        InitMap();
	}
   
    /// <summary>
    /// 更换地图
    /// </summary>
    /// <param name="m"></param>
    void ChangeMap(mapType m)
    {
        //tiles.GetComponent<Image>().sprite = Resources.Load<Sprite>("PNG/Tiles/Terrain" + m.ToString()+"/"+m.ToString().ToLower()+"_"+ num.ToString());
        foreach (Transform item in transform)
        {
            int num = UnityEngine.Random.Range(1, 19);

            string path = "PNG/Tiles/Terrain/" + m.ToString() + "/" + m.ToString().ToLower() + "_" + num.ToString("d2");
            Sprite s = Resources.Load<Sprite>(path);
            item.GetComponent<Image>().sprite = s;
        }
    }
    /// <summary>
    /// 初始化地图
    /// </summary>
    private void InitMap()
    {
        for (int i = -5; i < 5; i++)
        {
            for (int j = -4; j < 4; j++)
            {
                if (j % 2 == 0)
                {
                    GameObject g = Instantiate(tiles, transform, false) as GameObject;
                    g.GetComponent<RectTransform>().anchoredPosition = new Vector2(120 * i+60, 104 * j + 60);
                }
                else
                {
                    GameObject g = Instantiate(tiles, transform, false) as GameObject;
                    g.GetComponent<RectTransform>().anchoredPosition = new Vector2(120 * i, 104 * j + 60);
                }
                
            }
        }
    }
}
