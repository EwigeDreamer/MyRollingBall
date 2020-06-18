using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyTools.Helpers;

public class Player : MonoValidate
{
    [SerializeField] BallMotor motor;
    [SerializeField] HealthComponent health;

    public BallMotor Motor => this.motor;
    public HealthComponent Health => this.health;

    protected override void OnValidate()
    {
        base.OnValidate();
        ValidateGetComponent(ref this.motor);
        ValidateGetComponent(ref this.health);
    }
}
