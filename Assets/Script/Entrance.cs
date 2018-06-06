using UnityEngine;
using XLua;
using UnityEngine;
using System.IO;
using ClientStructure;

public class Entrance : MonoBehaviour
{
    private LuaEnv luaenv = null;

    private static string PUBLIC_KEY =
        "BgIAAACkAABSU0ExAAQAAAEAAQCJJDTLUiQ3Y4G6rWaRcH/CmoDj9gddUKdboYm0XqY+hLrj+t9H6DGkauBnm3s5DE5f2T5Ww86+s5LKQUrtKbtp/3hRNwtlIBydkD4Tuxo+00jxccV5RPrLVRou9BH1MDXDWnh+zntsXEPOLnE41y0oXEigZ4fGXoCJXKPpT4zHrw==";
    private Camera mainCamera;

    // Use this for initialization
    void Start()
    {
        luaenv = new XLua.LuaEnv();

#if UNITY_EDITOR
        luaenv.AddLoader(new LuaLoader(PUBLIC_KEY, (ref string filePath) =>
            {
                filePath = Application.dataPath + "/StreamingAssets/"+ filePath.Replace('.', '/') + ".lua";
                if (File.Exists(filePath))
                {
                    return File.ReadAllBytes(filePath);
                }
                else
                {
                    return null;
                }
            }));
        #else
         luaenv.AddLoader(new LuaLoader(PUBLIC_KEY, (ref string filePath) =>
             {
                 filePath = filePath.Replace('.', '/') + ".lua";
                 TextAsset file = (TextAsset) Resources.Load(filePath);
                 if (file != null)
                 {
                     return file.bytes;
                 }
                 else
                 {
                     return null;
                 }
             }));

#endif
        luaenv.DoString(@"
            require 'HelloWorld'");


        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        mainCamera.gameObject.AddComponent<HelloWorld>();
    }

    private void OnDestroy()
    {
        luaenv.DoString(@"
            require 'DisposeLua'");
        luaenv.Dispose();
    }
}