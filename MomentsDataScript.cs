using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MomentsDataScript {

    public static float balance_streak_points;

    public static float GetStreakPoints()
    {
        return balance_streak_points;
    }

    public static void AddStreakPoint()
    {
        balance_streak_points += 1;
    }
}
