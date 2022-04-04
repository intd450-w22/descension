using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Util.Helpers
{
    public static class DebugHelper
    {
        public static void DrawBoxCast2D(Vector2 origin, Vector2 size, float angle, Vector2 direction, float distance, float duration = 0, Color color = new Color())
        {
            Quaternion orientation = Quaternion.Euler(0f, 0f, angle);
            direction.Normalize();
            Box bottomBox = new Box(origin, size / 2, orientation);
            Box topBox = new Box(origin + (direction * distance), size / 2, orientation);
             
            Debug.DrawLine(bottomBox.BottomLeft, bottomBox.TopLeft, color, duration);
            Debug.DrawLine(bottomBox.TopLeft, topBox.TopRight, color, duration);
            Debug.DrawLine(topBox.TopRight, topBox.BottomRight, color, duration);
            Debug.DrawLine(bottomBox.BottomLeft, topBox.BottomRight, color, duration);
        }
    }

    public class Box
    {
        public Vector3 TopLeft;
        public Vector3 TopRight;
        public Vector3 BottomLeft;
        public Vector3 BottomRight;
        public Vector3 Origin;

        public Box(Vector3 origin, Vector3 halfExtents, Quaternion rotation)
        {
            halfExtents /= 2; 
            TopLeft     = RotatePointAroundPivot(origin + new Vector3(-halfExtents.x, halfExtents.y), origin, rotation);
            TopRight    = RotatePointAroundPivot(origin + new Vector3(halfExtents.x, halfExtents.y), origin, rotation);
            BottomLeft  = RotatePointAroundPivot(origin + new Vector3(-halfExtents.x, -halfExtents.y), origin, rotation);
            BottomRight = RotatePointAroundPivot(origin + new Vector3(halfExtents.x, -halfExtents.y), origin, rotation);
  
            Origin = origin;
        }
        
        static Vector2 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Quaternion rotation)
        {
            Vector3 direction = point - pivot;
            return pivot + rotation * direction;
        }
        
        public void Draw()
        {
            Debug.Log("Drawing " + Origin + " " + BottomLeft + " " + BottomRight + " " + TopLeft + " " + TopRight);
            Debug.DrawLine(TopLeft, TopRight, Color.blue);
            Debug.DrawLine(TopRight, BottomRight, Color.green);
            Debug.DrawLine(BottomRight, BottomLeft, Color.red);
            Debug.DrawLine(BottomLeft, TopLeft, Color.magenta);
        }
    }
    
}



