using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyTools.Singleton;

public class GameView : MonoSingleton<GameView>
{
#pragma warning disable 649
    [SerializeField] PlayerController playerController;
#pragma warning restore 649

    public PlayerController Player => this.playerController;

    protected override void OnValidate()
    {
        base.OnValidate();
        ValidateFind(ref this.playerController);
    }

    public void Init()
    {
        this.playerController.Init();
    }
}
