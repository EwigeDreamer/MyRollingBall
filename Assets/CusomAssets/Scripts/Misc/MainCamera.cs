using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyTools.Singleton;

public class MainCamera : MonoSingleton<MainCamera>
{
    static new Camera camera = null;
    public static Camera Camera => camera;

    protected override void Awake()
    {
        base.Awake();
        camera = GetComponent<Camera>();
    }
}
