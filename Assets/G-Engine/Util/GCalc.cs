using UnityEngine;

namespace GEngine.Util
{
    public static class GCalc
    {
        public static Vector2 CalculeDirectionAway(Vector2 originBodyPosition, Vector2 otherBodyPosition)
        {
            var direction = Vector2.zero;

            if (originBodyPosition.x > otherBodyPosition.x)
                direction.x = -1;
            else if (originBodyPosition.x < otherBodyPosition.x)
                direction.x = 1;

            if (originBodyPosition.y > otherBodyPosition.y)
                direction.y = -1;
            else if (originBodyPosition.y < otherBodyPosition.y)
                direction.y = 1;

            return direction;
        }
    }
}