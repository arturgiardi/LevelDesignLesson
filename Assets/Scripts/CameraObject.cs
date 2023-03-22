using System;
using DG.Tweening;
using GEngine.Util;
using UnityEngine;

[System.Serializable]
public class CameraObject
{
    [field: SerializeField] private Transform CameraTransform { get; set; }
    [field: SerializeField] private Camera Camera { get; set; }
    [field: SerializeField] private float SmoothTime { get; set; } = .07f;
    [field: SerializeField] private Transform CameraLimitMin { get; set; }
    [field: SerializeField] private Transform CameraLimitMax { get; set; }
    Vector3 velocity = Vector3.zero;
    
    public void Follow(Transform objectToFollow)
    {
        if (!objectToFollow)
            return;

        CameraTransform.position = Vector3.SmoothDamp(CameraTransform.position,
            GetCameraPosition(objectToFollow.position),
            ref velocity,
            SmoothTime);
        SetCameraLimits();
    }

    

    public void SetCameraPosition(Vector3 position)
    {
        CameraTransform.position = GetCameraPosition(position);
        SetCameraLimits();
    }

    private Vector3 GetCameraPosition(Vector3 position)
    {
        return new Vector3(position.x, position.y, CameraTransform.position.z);
    }

    private void SetCameraLimits()
    {
        var x = 0f;
        var y = 0f;

        if(CameraLimitMin && CameraLimitMin.gameObject.activeSelf)
        {
            var min = Camera.BoundsMin();
            x = min.x < CameraLimitMin.position.x ? CameraLimitMin.position.x - min.x : x; 
            y = min.y < CameraLimitMin.position.y ? CameraLimitMin.position.y - min.y : y;
        }

        if(CameraLimitMax && CameraLimitMax.gameObject.activeSelf)
        {
            var max = Camera.BoundsMax();
            x = max.x > CameraLimitMax.position.x ? -(max.x - CameraLimitMax.position.x) : x; 
            y = max.y > CameraLimitMax.position.y ? -(max.y - CameraLimitMax.position.y) : y;
        }

        CameraTransform.position = new Vector3(CameraTransform.position.x + x, 
            CameraTransform.position.y + y, 
            CameraTransform.position.z);
    }

    internal void GoTo(Vector3 position, float time, Action callback)
    {
        position = GetClampedPosition(position);
        Camera.transform.DOMove(position, time).OnComplete(() => callback?.Invoke());
    }

    private Vector3 GetClampedPosition(Vector3 position)
    {
        var x = position.x;
        var y = position.y;
        var z = Camera.transform.position.z;
        var distanceToCameraBorder = (Vector2)Camera.transform.position - Camera.BoundsMin();


        if(CameraLimitMin && CameraLimitMin.gameObject.activeSelf)
        {
            var min = (Vector2)CameraLimitMin.position + distanceToCameraBorder;
            x = min.x < x  ? x : min.x; 
            y = min.y < y ? y : min.y;
        }

        if(CameraLimitMax && CameraLimitMax.gameObject.activeSelf)
        {
            var max = (Vector2)CameraLimitMax.position - distanceToCameraBorder;
            x = max.x > x ? x : max.x;  
            y = max.y > y ? y : max.y;
        }

        return new Vector3(x, y, z);
    }
}