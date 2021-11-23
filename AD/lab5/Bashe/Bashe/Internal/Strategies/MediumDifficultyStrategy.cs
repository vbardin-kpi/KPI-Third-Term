using Bashe.Abstractions;
using Bashe.Models;

namespace Bashe.Internal.Strategies;

public class MediumDifficultyStrategy : IDifficultyStrategy
{
    public int GetItemsAmount(GameConfiguration config, int itemsLeft)
    {
        return AlphaBeta(2, config.StepMin, config.StepMax, itemsLeft);
    }

    private int AlphaBeta(int depth, int alpha, int beta, int itemsLeft) {
        if (depth == 0) return GetAmount(alpha, beta, itemsLeft);
        var moves = Enumerable.Range(Math.Abs(alpha), Math.Abs(beta)).ToList();
        for(int i = 0; i < moves.Count; i++) {
            itemsLeft -= moves[i];
            var eval = -AlphaBeta(depth-1, -beta, -alpha, -itemsLeft);
            itemsLeft += moves[i];

            if(eval >= beta)
                return beta;

            if(eval > alpha) {
                alpha = eval;
                if (depth == 1) {
                    return moves[i];  
                }
            }
        }
        return alpha;
    }

    private int GetAmount(int min, int max, int itemsLeft)
    {
        if (itemsLeft <= max && itemsLeft >= min)
        {
            return itemsLeft;
        }

        var maxAmount = max > itemsLeft ? itemsLeft : max;
        for (var i = maxAmount; i >= min; i--)
        {
            if (max % i == 0)
                return i;
        }

        return min;
    }
}