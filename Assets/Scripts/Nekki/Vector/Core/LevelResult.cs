using System;

namespace Nekki.Vector.Core
{
    public class LevelResult
    {
        public static LevelResult LastLevelResult;

        public TimeSpan timeSpan
        {
            get;
            private set;
        }

        public int pointsCollected
        {
            get;
            private set;
        }

        public int pointsMax
        {
            get;
            private set;
        }

        public int bonusCollected
        {
            get;
            private set;
        }

        public int bonusMax
        {
            get;
            private set;
        }

        public int trickCollected
        {
            get;
            private set;
        }

        public int trickMax
        {
            get;
            private set;
        }

        public LevelResult(Location.Location location, uint frameCount)
        {
            if (location != null)
            {
                var model = location.GetUserModel();
                timeSpan = TimeSpan.FromMilliseconds(frameCount * 16.666666);
                pointsMax = location.Sets.totalPoints;
                pointsCollected = model.CollectedPoints;
                bonusMax = location.Sets.totalBonus;
                bonusCollected = model.collectedBonusesCount;
                trickMax = location.Sets.totalTricks;
                trickCollected = model.collectedTricksCount;

            }
        }

        public LevelResult(Location.Location location, int cheatStars)
        {
            if (location != null)
            {
                var model = location.GetUserModel();
                var TimeSpan = new TimeSpan(0, 0, 1, 10);
                pointsMax = location.Sets.totalPoints;
                bonusMax = location.Sets.totalBonus;
                trickMax = location.Sets.totalTricks;
                if (cheatStars == 1)
                {
                    pointsCollected = 0;
                    bonusCollected = 0;
                    trickCollected = 0;
                    return;
                }
                if (cheatStars == 2)
                {
                    bonusCollected = location.Sets.totalBonus;
                    pointsCollected = location.Sets.totalBonus * 100;
                    trickCollected = 0;
                    return;
                }
                if (cheatStars == 3)
                {
                    bonusCollected = location.Sets.totalBonus;
                    pointsCollected = location.Sets.totalPoints;
                    trickCollected = location.Sets.totalTricks;
                }
            }
        }
    }
}
