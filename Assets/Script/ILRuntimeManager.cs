//using UnityEngine;
//using System.Collections;
//using System.IO;
//using ILRuntime.Runtime.Enviorment;
//using System;
//using Celebi.Framework;
//using Mono.Cecil.Pdb;

//public class ILRuntimeManager : MonoSingleton<ILRuntimeManager>
//{

//    private void Update()
//    {
//        Invoke("Celebi.ObjectManager", "Update", null, null);
//    }

//    private void OnDestroy()
//    {
//        Instance = null;
//    }

//    //AppDomain是ILRuntime的入口，最好是在一个单例类中保存，整个游戏全局就一个，这里为了示例方便，每个例子里面都单独做了一个
//    //大家在正式项目中请全局只创建一个AppDomain
//    public ILRuntime.Runtime.Enviorment.AppDomain appdomain;
//#if UNITY_EDITOR
//    public IEnumerator LoadHotFixAssembly()
//    {
//        yield return 0;
//    }

//#else
//    //public IEnumerator LoadHotFixAssembly()
//    //{
//    //    //首先实例化ILRuntime的AppDomain，AppDomain是一个应用程序域，每个AppDomain都是一个独立的沙盒
//    //    appdomain = new ILRuntime.Runtime.Enviorment.AppDomain();
//    //    //正常项目中应该是自行从其他地方下载dll，或者打包在AssetBundle中读取，平时开发以及为了演示方便直接从StreammingAssets中读取，
//    //    //正式发布的时候需要大家自行从其他地方读取dll

//    //    //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
//    //    //这个DLL文件是直接编译HotFix_Project.sln生成的，已经在项目中设置好输出目录为StreamingAssets，在VS里直接编译即可生成到对应目录，无需手动拷贝
//    //    WWW www = new WWW("file://" + Application.persistentDataPath + "/HotFix_Project.dll");
//    //    while (!www.isDone)
//    //        yield return null;
//    //    if (!string.IsNullOrEmpty(www.error))
//    //        UnityEngine.Debug.LogError(www.error);
//    //    byte[] dll = www.bytes;
//    //    www.Dispose();

//    //    //PDB文件是调试数据库，如需要在日志中显示报错的行号，则必须提供PDB文件，不过由于会额外耗用内存，正式发布时请将PDB去掉，下面LoadAssembly的时候pdb传null即可
//    //    www = new WWW("file://" + Application.persistentDataPath + "/HotFix_Project.pdb");
//    //    while (!www.isDone)
//    //        yield return null;
//    //    if (!string.IsNullOrEmpty(www.error))
//    //        UnityEngine.Debug.LogError(www.error);
//    //    byte[] pdb = www.bytes;
//    //    using (System.IO.MemoryStream fs = new MemoryStream(dll))
//    //    {
//    //        using (System.IO.MemoryStream p = new MemoryStream(pdb))
//    //        {
//    //            appdomain.LoadAssembly(fs, p, new Mono.Cecil.Pdb.PdbReaderProvider());
//    //        }
//    //    }

//    //    InitializeILRuntime();
//    //    OnHotFixLoaded();
//    //}

//    public IEnumerator LoadHotFixAssembly()
//    {
//        appdomain = new ILRuntime.Runtime.Enviorment.AppDomain();
//        byte[] dll = ResourceManager.Load<TextAsset>("Common/Cc").bytes;
//        Debug.Log(dll.Length);
//        using (System.IO.MemoryStream fs = new MemoryStream(dll))
//        {
//            appdomain.LoadAssembly(fs);
//        }

//        InitializeILRuntime();
//        OnHotFixLoaded();
//        yield break;
//    }

//#endif

//    public void Invoke(string type, string method, object instance, params object[] p)
//    {
//#if UNITY_EDITOR
//        DllHelper.Invoke(type, method);
//#else
//        if (appdomain == null) return;
//        appdomain.Invoke(type, method, null, null);
//#endif
//    }

//    void InitializeILRuntime()
//    {
//        //这里做一些ILRuntime的注册，HelloWorld示例暂时没有需要注册的
//        LitJson.JsonMapper.RegisterILRuntimeCLRRedirection(appdomain);
//        appdomain.RegisterCrossBindingAdaptor(new CoroutineAdapter());
//        appdomain.RegisterCrossBindingAdaptor(new MegoEngine.IL.Adapters.IExtensibleAdapter());
//        new MegoEngine.ProtobufAdapter(appdomain);
//        //ProtoBuf.PType.RegisterFunctionCreateInstance(PType_CreateInstance);
//        //ProtoBuf.PType.RegisterFunctionGetRealType(PType_GetRealType);
//    }

//    //#region Protobuf服务注册
//    //object PType_CreateInstance(string typeName)
//    //{
//    //    return appdomain.Instantiate(typeName);
//    //}

//    //Type PType_GetRealType(object o)
//    //{
//    //    var type = o.GetType();
//    //    if (type.FullName == "ILRuntime.Runtime.Intepreter.ILTypeInstance")
//    //    {
//    //        var ilo = o as ILRuntime.Runtime.Intepreter.ILTypeInstance;
//    //        type = ProtoBuf.PType.FindType(ilo.Type.FullName);
//    //    }
//    //    return type;
//    //}
//    //#endregion

//    void OnHotFixLoaded()
//    {
//        appdomain.DelegateManager.RegisterMethodDelegate<System.Boolean>();
//        appdomain.DelegateManager.RegisterMethodDelegate<System.Object>();
//        appdomain.DelegateManager.RegisterMethodDelegate<System.Int32, System.Boolean>();
//        appdomain.DelegateManager.RegisterMethodDelegate<SocketEvents, System.Object>();
//        appdomain.DelegateManager.RegisterMethodDelegate<UnityEngine.EventSystems.BaseEventData>();
//        appdomain.DelegateManager.RegisterMethodDelegate<ILRuntime.Runtime.Intepreter.ILTypeInstance>();

//        appdomain.DelegateManager.RegisterFunctionDelegate<System.Byte, System.Byte, System.Int32>();
//        appdomain.DelegateManager.RegisterFunctionDelegate<ILRuntime.Runtime.Intepreter.ILTypeInstance, ILRuntime.Runtime.Intepreter.ILTypeInstance, System.Int32>();

//        appdomain.DelegateManager.RegisterDelegateConvertor<UnityEngine.Events.UnityAction>((act) =>
//        {
//            return new UnityEngine.Events.UnityAction(() =>
//            {
//                ((System.Action)act)();
//            });
//        });

//        appdomain.DelegateManager.RegisterDelegateConvertor<UnityEngine.Events.UnityAction<bool>>((act) =>
//        {
//            return new UnityEngine.Events.UnityAction<bool>((value) =>
//            {
//                ((System.Action<bool>)act)(value);
//            });
//        });

//        appdomain.DelegateManager.RegisterDelegateConvertor<UnityEngine.Events.UnityAction<UnityEngine.EventSystems.BaseEventData>>((act) =>
//        {
//            return new UnityEngine.Events.UnityAction<UnityEngine.EventSystems.BaseEventData>((arg0) =>
//            {
//                ((System.Action<UnityEngine.EventSystems.BaseEventData>)act)(arg0);
//            });
//        });

//        appdomain.DelegateManager.RegisterDelegateConvertor<EventHandle>((act) =>
//        {
//            return new EventHandle((args) =>
//            {
//                ((System.Action<object>)act)(args);
//            });
//        });

//        appdomain.DelegateManager.RegisterDelegateConvertor<System.Comparison<System.Byte>>((act) =>
//        {
//            return new System.Comparison<System.Byte>((x, y) =>
//            {
//                return ((System.Func<System.Byte, System.Byte, System.Int32>)act)(x, y);
//            });
//        });

//        appdomain.DelegateManager.RegisterDelegateConvertor<System.Comparison<ILRuntime.Runtime.Intepreter.ILTypeInstance>>((act) =>
//        {
//            return new System.Comparison<ILRuntime.Runtime.Intepreter.ILTypeInstance>((x, y) =>
//            {
//                return ((System.Func<ILRuntime.Runtime.Intepreter.ILTypeInstance, ILRuntime.Runtime.Intepreter.ILTypeInstance, System.Int32>)act)(x, y);
//            });
//        });


//        appdomain.DelegateManager.RegisterDelegateConvertor<DG.Tweening.TweenCallback>((act) =>
//        {
//            return new DG.Tweening.TweenCallback(() =>
//            {
//                ((System.Action)act)();
//            });
//        });

//    }

//}
