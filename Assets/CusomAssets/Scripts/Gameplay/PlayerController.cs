using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyTools.Helpers;

public class PlayerController : MonoValidate
{
    [SerializeField] Player player;

    public Player Player => this.player;

    protected override void OnValidate()
    {
        base.OnValidate();
        ValidateFind(ref this.player);
    }

    public void Init()
    {

    }
}
