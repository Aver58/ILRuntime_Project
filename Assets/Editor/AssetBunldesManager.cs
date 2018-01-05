using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class AssetBunldesManager : MonoBehaviour
{
    #region PlayerPrefs
    //   InputField input;
    //void Start () {
    //       input = GetComponent<InputField>();
    //       input.onValueChanged.AddListener(delegate { Change(); });

    //   }
    //   void Change()
    //   {
    //       PlayerPrefs.SetString("Name", input.text);//保存数据
    //       string mName = PlayerPrefs.GetString("Name", "DefaultValue");

    //       print(mName);
    //   }
    #endregion

    [MenuItem("Tools/Create AssetBunldes Main")]
    static void CreateAssetBunldesMain()
    {
        //获取在Project视图中选择的所有游戏对象
        Object[] SelectedAsset = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);

        //遍历所有的游戏对象
        foreach (Object obj in SelectedAsset)
        {
            //string sourcePath = AssetDatabase.GetAssetPath(obj);
            //本地测试：建议最后将Assetbundle放在StreamingAssets文件夹下，如果没有就创建一个，因为移动平台下只能读取这个路径
            //StreamingAssets是只读路径，不能写入
            //服务器下载：就不需要放在这里，服务器上客户端用www类进行下载。
            string targetPath = Application.dataPath + "/StreamingAssets/";
            if (BuildPipeline.BuildAssetBundles(targetPath, BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.StandaloneWindows))
            {
                Debug.Log(obj.name + "资源打包成功");
            }
            else
            {
                Debug.Log(obj.name + "资源打包失败");
            }
        }
        //刷新编辑器
        AssetDatabase.Refresh();
    }

    [MenuItem("Tools/Create AssetBunldes ALL")]
    static void CreateAssetBunldesALL()
    {
        Caching.ClearCache();

        string Path = Application.dataPath + "/StreamingAssets/";

        Object[] SelectedAsset = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);

        foreach (Object obj in SelectedAsset)
        {
            Debug.Log("Create AssetBunldes name :" + obj);
        }

        //这里注意第二个参数就行
        if (BuildPipeline.BuildAssetBundles(Path, BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.StandaloneWindows))
        {
            Debug.Log( "资源打包成功");
            AssetDatabase.Refresh();
        }
        else
        {
            Debug.Log( "资源打包失败");
        }
    }
}
