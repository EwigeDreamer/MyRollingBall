using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyTools.ValueInfo;
using System;

public class HealthComponent : MonoBehaviour
{
    public event Action OnFull = delegate { };
    public event Action OnHeal = delegate { };
    public event Action OnDamage = delegate { };
    public event Action OnBeforeDead = delegate { };
    public event Action OnAfterDead = delegate { };

#pragma warning disable 649
    [SerializeField] IntInfo hp = new IntInfo(0, 100, 100);
#pragma warning restore 649

    public IntInfo HP => this.hp;

    public void SetDamage(int hp)
    {
        if (this.hp.IsZero) return;
        hp = Mathf.Max(0, hp);
        if (hp == 0) return;
        this.hp.Value -= hp;
        OnDamage();
        if (this.hp.IsZero)
        {
            OnBeforeDead();
            OnAfterDead();
        };
    }

    public void SetHeal(int hp)
    {
        if (this.hp.IsMax) return;
        hp = Mathf.Max(0, hp);
        if (hp == 0) return;
        this.hp.Value += hp;
        OnHeal();
        if (this.hp.IsMax) OnFull();
    }

    public void SetMin(int hp)
    {
        hp = Mathf.Max(0, hp);
        this.hp = this.hp.SetMin(hp);
    }
    public void SetMax(int hp)
    {
        hp = Mathf.Max(1, hp);
        this.hp = this.hp.SetMax(hp);
    }
    public void ResetHealth()
    {
        if (this.hp.IsMax) return;
        this.hp.Value = this.hp.ToMax();
        OnHeal();
        OnFull();
    }
    public void ForceKill()
    {
        if (this.hp.IsZero) return;
        this.hp = this.hp.SetMin(0);
        this.hp = this.hp.ToMin();
        OnDamage();
        OnBeforeDead();
        OnAfterDead();
    }
}
