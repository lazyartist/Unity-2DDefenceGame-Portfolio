using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerManager : SingletonBase<PlayerManager> {
    public int Sugar { get; private set; }
    public int Health { get; private set; }

    public UnityEvent Event_Sugar_Changed = new UnityEvent();
    public UnityEvent Event_Health_Changed = new UnityEvent();

    public void Init(int sugar, int health)
    {
        Sugar = sugar;
        Health = health;

        Event_Sugar_Changed.Invoke();
        Event_Health_Changed.Invoke();
    }
    
    public void ChangeSugar(int value)
    {
        Sugar += value;

        if (Sugar < 0)
        {
            Sugar = 0;
        }

        Event_Sugar_Changed.Invoke();
    }

    public bool ApplyDamage(int value)
    {
        Health -= value;

        if (Health < 0)
        {
            Health = 0;
        }

        Event_Health_Changed.Invoke();

        return Health == 0;
    }
}
