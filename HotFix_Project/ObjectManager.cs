//using System;
//using System.Collections.Generic;
//using UnityEngine;
//using Celebi.Framework;

//public interface IObject
//{
//    void OnInit();
//}

//public interface IOnDestroy
//{
//    void OnDestroy();
//}

//public interface IUpdate
//{
//    void Update();
//}

//namespace Celebi
//{

//    public class CC
//    {
//        public static T Find<T>()
//        {
//            Type t = typeof(T);
//            Debug.Log("查找 " + t + "," + ObjectManager.objects2.ContainsKey(t));
//            if (ObjectManager.objects2.ContainsKey(t))
//            {
//                return (T)ObjectManager.objects2[t];
//            }
//            return default(T);
//        }

//        /// <summary>
//        /// New一个对象，加入到对象管理器
//        /// </summary>
//        /// <typeparam name="T"></typeparam>
//        /// <returns></returns>
//        public static T Make<T>() where T : IObject
//        {
//            Type t = typeof(T);
//            ServerScope scope = getObjectScope(t);
//            if (scope == ServerScope.Singleton || scope == ServerScope.Scene)
//            {
//                var o = Find<T>();
//                if (o != null) return o;
//            }

//            T obj = default(T);
//            obj = Activator.CreateInstance<T>();
//            ObjectManager.Add(obj);
//            obj.OnInit();
//            if (scope == ServerScope.Singleton || scope == ServerScope.Scene)
//            {
//                ObjectManager.objects2.Add(t, obj);
//            }

//            return obj;
//        }

//        public static T Bind<T>(Transform transform) where T : UBase
//        {
//            Type t = typeof(T);
//            ServerScope scope = getObjectScope(t);
//            if (scope == ServerScope.Singleton || scope == ServerScope.Scene)
//            {
//                var o = Find<T>();
//                if (o != null) return o;
//            }

//            T obj = default(T);
//            obj = Activator.CreateInstance<T>();
//            ObjectManager.Add(obj);
//            obj.InitGameObject(transform.gameObject);
//            obj.OnInit();
//            if (scope == ServerScope.Singleton || scope == ServerScope.Scene)
//            {
//                ObjectManager.objects2.Add(t, obj);
//            }

//            return obj;
//        }

//        public static ServerScope getObjectScope(Type t)
//        {
//            object[] attrs = t.GetCustomAttributes(typeof(AppServer), true);
//            for (int i = 0; i < attrs.Length; i++)
//            {
//                var attr = (AppServer)attrs[i];
//                return attr.scope;
//            }
//            return ServerScope.Normal;
//        }

//        public static void Release()
//        {
//            ObjectManager.Destory();
//        }

//    }


//    public class ObjectManager
//    {
//        public static List<IObject> objects = new List<IObject>();//存放所有对象
//        public static Dictionary<Type, IObject> objects2 = new Dictionary<Type, IObject>();//存放单例对象

//        private static Queue<IObject> start = new Queue<IObject>();

//        private static Queue<IObject> updates = new Queue<IObject>();
//        private static Queue<IObject> updates2 = new Queue<IObject>();

//        private static Queue<IObject> destroy = new Queue<IObject>();

//        public static void Add(IObject @object)
//        {
//            if (@object is IUpdate)
//            {
//                updates.Enqueue(@object);
//            }
//            if (@object is IOnDestroy)
//            {
//                destroy.Enqueue(@object);
//            }

//            objects.Add(@object);
//        }

//        public static void Destory()
//        {
//            for (int i = objects.Count - 1; i >= 0; i--)
//            {
//                var key = objects[i];
//                var scope = CC.getObjectScope(key.GetType());
//                if (ServerScope.Singleton == scope)
//                {
//                    continue;
//                }

//                objects.Remove(key);
//                IOnDestroy iDestroy = key as IOnDestroy;
//                Debug.Log(key + "=====================" + (iDestroy == null));
//                if (iDestroy == null)
//                {
//                    continue;
//                }
//                iDestroy.OnDestroy();
//            }

//            //ObjectHelper.Swap(ref mObjects, ref objects2);
//        }

//        public static void Start()
//        {
//            while (start.Count > 0)
//            {
//                var disposer = start.Dequeue();
//                disposer.OnInit();
//            }
//        }

//        public static void Update()
//        {
//            //Start();

//            while (updates.Count > 0)
//            {
//                var disposer = updates.Dequeue();
//                if (!objects.Contains(disposer))
//                {
//                    continue;
//                }
//                updates2.Enqueue(disposer);

//                IUpdate iUpdate = disposer as IUpdate;
//                if (iUpdate == null)
//                {
//                    continue;
//                }
//                try
//                {
//                    iUpdate.Update();
//                }
//                catch (Exception e)
//                {
//                    Debug.LogError(e.ToString());
//                }
//            }


//            ObjectHelper.Swap(ref updates, ref updates2);
//        }

//        public static void Destory(IObject dialog)
//        {
//            objects.Remove(dialog);
//        }
//    }
//}
