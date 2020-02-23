using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ParabolaAlgorithm
{
    public float HeightLimit { get; private set; }
    public float TimeToTopmostHeight { get; private set; }
    public float TimeToEndPosition { get; private set; }

    private Vector3 _startPosition;
    private Vector3 _endPosition;

    private float _vx;
    private float _vy;
    private float _gravity;

    public Vector3 GetPosition(float elasedTime)
    {
        float x = _vx * elasedTime;
        float y = (_vy * elasedTime) - (0.5f * _gravity * elasedTime * elasedTime);

        return new Vector3(_startPosition.x + x, _startPosition.y + y, 0f);
    }

    public void Init(float heightLimit, float timeToTopmostHeight, Vector3 startPosition, Vector3 endPosition)
    {
        this.HeightLimit = heightLimit;
        this.TimeToTopmostHeight = timeToTopmostHeight;

        _startPosition = startPosition;
        _endPosition = endPosition;

        CalcParabolaTrack();
    }

    void CalcParabolaTrack()
    {
        float endHeight = _endPosition.y - _startPosition.y;
        float height = HeightLimit;
        //float height = MaxHeight - transform.position.y;
        _gravity = 2 * height / (TimeToTopmostHeight * TimeToTopmostHeight);

        _vy = Mathf.Sqrt(2 * _gravity * height);

        float a = _gravity;
        float b = -2 * _vy;
        float c = 2 * endHeight;

        TimeToEndPosition = (-b + Mathf.Sqrt(b * b - 4 * a * c)) / (2 * a);

        //Debug.Log(TimeToEndPosition.ToString("#.###"));

        _vx = (_endPosition.x - _startPosition.x) / TimeToEndPosition;
    }
} 