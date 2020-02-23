using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReverseAnimation : MonoBehaviour
{
    public Animator Animator;

    float _aniPlayDirection = 1f;
    bool _dirty = false;

    void Start()
    {
        Animator = GetComponent<Animator>();
        SetSpeed();
    }

    public void OnAniEvent_Start()
    {
        if(_aniPlayDirection == -1f)
        {
            ReverseSpeed();
            SetSpeed();
        }
    }

    public void OnAniEvent_End()
    {
        if (_aniPlayDirection == 1f)
        {
            ReverseSpeed();
            SetSpeed();
        }
    }

    void ReverseSpeed()
    {
        _aniPlayDirection *= -1;
    }

    void SetSpeed()
    {
        Animator.SetFloat("AniPlayDirection", _aniPlayDirection);
    }
}
