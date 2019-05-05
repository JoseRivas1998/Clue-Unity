using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PossibleBoardLocations
{

    public static List<int[]> GetBoardLocations(int r, int c, int diceRoll, System.Func<int[], bool> predicate = null)
    {
        List<int[]> results = new List<int[]>();
        int highestVertical = diceRoll;
        for (int verticalMovement = -highestVertical; verticalMovement <= highestVertical; verticalMovement++)
        {
            int highestHorizontal = diceRoll - System.Math.Abs(verticalMovement);
            for (int horizontalMovement = -highestHorizontal; horizontalMovement <= highestHorizontal; horizontalMovement++)
            {
                results.Add(new int[] { r + verticalMovement, c + horizontalMovement });
            }
        }
        if(predicate != null)
        {
            return results.Where(predicate).ToList(); ;
        }
        return results;
    }

}
