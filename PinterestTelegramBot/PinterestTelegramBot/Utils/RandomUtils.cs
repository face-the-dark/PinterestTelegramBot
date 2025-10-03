using System;

namespace PinterestTelegramBot.Utils
{
    public static class RandomUtils
    {
        private static readonly Random s_random = new Random();

        public static int Next(int max) => 
            s_random.Next(max);
    }
}