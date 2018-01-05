using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using ILRuntime.CLR.Method;
using ILRuntime.CLR.TypeSystem;
using UnityEngine;

public static class DllHelper
{
    public static Assembly assembly;

    public static void Invoke(string type, string method)
    {
        if (assembly == null)
        {
            byte[] assBytes = File.ReadAllBytes(Application.streamingAssetsPath + "/HotFix_Project.dll");
            byte[] mdbBytes = File.ReadAllBytes(Application.streamingAssetsPath + "/HotFix_Project.dll.mdb");

            assembly = Assembly.Load(assBytes, mdbBytes);

            Invoke("Celebi.ObjectManager", "Update");
        }
        Type hotfixInit = assembly.GetType(type);
        hotfixInit.GetMethod(method).Invoke(null, null);
    }

}
