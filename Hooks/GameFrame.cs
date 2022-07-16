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
        OnUpdate?.Invoke();
    }

    void LateUpdate()
    {
        OnLateUpdate?.Invoke();
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