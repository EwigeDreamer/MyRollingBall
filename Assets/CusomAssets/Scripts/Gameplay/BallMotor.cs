using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyTools.Helpers;
using System;
using MyTools.Extensions.Vectors;

public class BallMotor : MonoValidate
{
    [SerializeField] Rigidbody rb;

    List<Func<Vector3>> getVelocityFunctions = new List<Func<Vector3>>();

    public event Func<Vector3> GetVelocity
    {
        add => getVelocityFunctions.Add(value);
        remove => getVelocityFunctions.Remove(value);
    }

    protected override void OnValidate()
    {
        base.OnValidate();
        ValidateGetComponent(ref this.rb);
    }

    private void FixedUpdate()
    {
        var v = this.rb.velocity.SetX(0f).SetY(0f);
        this.getVelocityFunctions.ForEach(f => v += f());
        this.rb.velocity = v;
    }
}
