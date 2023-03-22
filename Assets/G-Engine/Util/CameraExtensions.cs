using System;
using UnityEngine;

namespace GEngine.Util
{
    public static class CameraExtensions
    {
        public static Vector2 BoundsMin(this Camera camera)
        {
            return (Vector2)camera.transform.position - camera.Extents();
        }

        public static Vector2 BoundsMax(this Camera camera)
        {
            return (Vector2)camera.transform.position + camera.Extents();
        }

        public static Vector2 Extents(this Camera camera)
        {
            if (camera.orthographic)
                return new Vector2(camera.orthographicSize * Screen.width / Screen.height, camera.orthographicSize);
            else
                throw new InvalidOperationException($"Câmera não ortográfica {camera}");
        }
    }
}