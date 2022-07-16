using System;
using UnhollowerRuntimeLib;
using UnityEngine;

namespace Wetstone.Hooks;

public class GameFrame : MonoBehaviour
{
    private static GameFrame? _instance;
    public delegate void GameFrameUpdateEventHandler();

    /// <summary>
    /// Event emitted every frame update
    /// </summary>
    public static event GameFrameUpdateEventHandler? OnUpdate;

    /// <summary>
    /// Event emitted every frame late update
    /// </summary>
    public static event GameFrameUpdateEventHandler? OnLateUpdate;

    void Update()
    {
        try
        {
            OnUpdate?.Invoke();
        }
        catch (Exception ex)
        {
            WetstonePlugin.Logger.LogError("Error dispatching OnUpdate event:");
            WetstonePlugin.Logger.LogError(ex);
        }
    }

    void LateUpdate()
    {
        try
        {
            OnLateUpdate?.Invoke();
        }
        catch (Exception ex)
        {
            WetstonePlugin.Logger.LogError("Error dispatching OnLateUpdate event:");
            WetstonePlugin.Logger.LogError(ex);
        }
    }

    public static void Initialize()
    {
        if (!ClassInjector.IsTypeRegisteredInIl2Cpp<GameFrame>())
        {
            ClassInjector.RegisterTypeInIl2Cpp<GameFrame>();
        }

        _instance = WetstonePlugin.Instance.AddComponent<GameFrame>();
    }

    public static unsafe void Uninitialize()
    {
        OnUpdate = null;
        OnLateUpdate = null;
        Destroy(_instance);
        _instance = null;
    }

}