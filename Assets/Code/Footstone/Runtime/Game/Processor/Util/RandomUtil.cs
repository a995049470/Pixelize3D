using UnityEngine;

namespace Lost.Runtime.Footstone.Game
{
    public static class RandomUtil
    {
        public static float Next(System.Random random, Vector2 range)
        {
            return Next(random, range.x, range.y);
        }

        public static float Next(System.Random random, float min, float max)
        {
            return random.Next(1024) / 1024.0f * (max - min) + min;
        }
    }
}

