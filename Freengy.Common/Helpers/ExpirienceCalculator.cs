// Created by Laxale 14.05.2018
//
//

using System;
using System.Collections.Generic;
using System.Linq;


namespace Freengy.Common.Helpers 
{
    /// <summary>
    /// Калькулятор опыта по уровню аккаунта.
    /// </summary>
    public static class ExpirienceCalculator 
    {
        private static readonly Dictionary<uint, uint> expToLevels = new Dictionary<uint, uint>();

        /// <summary>
        /// Минимальный возможный уровень.
        /// </summary>
        public const uint MinLevel = 1;

        /// <summary>
        /// Максимальный возможный уровень.
        /// </summary>
        public const uint MaxLevel = 90;

        /// <summary>
        /// Базовый опыт за нахождение в онлайне на момент начисления опыта всем аккаунтам.
        /// </summary>
        public const uint BasicOnlineExpReward = 10;

        /// <summary>
        /// Базовый опыт за добавление друга.
        /// </summary>
        public const uint BasicAddFriendExpReward = 500;


        static ExpirienceCalculator() 
        {
            for (uint level = 2; level < MaxLevel; level++)
            {
                uint levelExp = GetExpOfLevel(level);

                expToLevels.Add(levelExp, level);
            }
        }


        public static uint GetAddFriendRewardForLevel(uint level) 
        {
            return BasicAddFriendExpReward + (uint)Math.Pow(level, 2);
        }

        public static uint GetOnlineRewardForLevel(uint level) 
        {
            return BasicOnlineExpReward + 2 * level;
        }

        /// <summary>
        /// Посчитать количество опыта, нужное для получения уровня.
        /// </summary>
        /// <param name="level">Значение уровня для подсчёта необходимого опыта.</param>
        /// <returns>Количество опыта, нужное для достижения уровня.</returns>
        public static uint GetExpOfLevel(uint level) 
        {
            if(level == 0) throw new InvalidOperationException("Level must not be lesser than 1");

            if (level == 1) return 0;
            if (level == 2) return 1614;

            uint baseLevelExp = level * 1000;
            uint exp = baseLevelExp + (uint)Math.Pow(level, 3.5) * (uint)Math.Log(baseLevelExp, 2);

            if (exp % 2 != 0)
            {
                exp += 1;
            }

            return exp;
        }

        /// <summary>
        /// Посчитать уровень, которому соответствует данное кличество опыта.
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public static uint GetLevelForExp(uint exp) 
        {
            if (expToLevels.ContainsKey(exp))
            {
                return expToLevels[exp];
            }

            uint nearestExp = expToLevels.Keys.FirstOrDefault(expValue => expValue > exp);

            uint level = expToLevels[nearestExp] - 1;

            return level;
        }
    }
}