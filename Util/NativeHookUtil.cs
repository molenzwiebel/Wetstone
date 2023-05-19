using BepInEx.Unity.IL2CPP.Hook;
using HarmonyLib;
using MonoMod.RuntimeDetour;
using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Wetstone.Util;

public static class NativeHookUtil
{
    public static NativeDetour Detour<T>(string typeName, string methodName, T to, out T original) where T : System.Delegate?
    {
        return Detour(Type.GetType(typeName), methodName, to, out original);
    }

    public static NativeDetour Detour<T>(Type type, string methodName, T to, out T original) where T : System.Delegate?
    {
        var method = type.GetMethod(methodName, AccessTools.all);
        return Detour(method, to, out original);
    }

    public static NativeDetour Detour<T>(MethodInfo method, T to, out T original) where T : System.Delegate?
    {
        var address = Il2CppMethodResolver.ResolveFromMethodInfo(method);
        WetstonePlugin.Logger.LogInfo($"Detouring {method.DeclaringType?.FullName}.{method.Name} at {address.ToString("X")}");
        var detour = new NativeDetour(address, Marshal.GetFunctionPointerForDelegate(to));
        original = detour.GenerateTrampoline<T>();
        return detour;
    }
    public static INativeDetour IDetour<T>(string typeName, string methodName, T to, out T original) where T : System.Delegate?
    {
        return IDetour(Type.GetType(typeName), methodName, to, out original);
    }

    public static INativeDetour IDetour<T>(Type type, string methodName, T to, out T original) where T : System.Delegate?
    {
        var method = type.GetMethod(methodName, AccessTools.all);
        return IDetour(method, to, out original);
    }

    public static INativeDetour IDetour<T>(MethodInfo method, T to, out T original) where T : System.Delegate?
    {
        var address = Il2CppMethodResolver.ResolveFromMethodInfo(method);
        WetstonePlugin.Logger.LogInfo($"Detouring {method.DeclaringType?.FullName}.{method.Name} at {address.ToString("X")}");
        return INativeDetour.CreateAndApply(address, to, out original);
    }
}
