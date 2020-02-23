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
        _SetSpeed();
    }

    public void OnAniEvent_Start()
    {
        if(_aniPlayDirection == -1f)
        {
            _ReverseSpeed();
            _SetSpeed();
        }
    }

    public void OnAniEvent_End()
    {
        if (_aniPlayDirection == 1f)
        {
            _ReverseSpeed();
            _SetSpeed();
        }
    }

    void _ReverseSpeed()
    {
        _aniPlayDirection *= -1;
    }

    void _SetSpeed()
    {
        Animator.SetFloat("AniPlayDirection", _aniPlayDirection);
    }
}
