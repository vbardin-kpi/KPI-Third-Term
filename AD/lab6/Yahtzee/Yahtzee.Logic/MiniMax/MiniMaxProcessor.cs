namespace Yahtzee.Logic.MiniMax;

public class MiniMaxProcessor
{
    public MiniMaxNode GetBestNode(int depth, MiniMaxNode node, bool isMaximazing)
    {
        if (depth == 0) return node;

        if (isMaximazing)
        {
            var bestNode = node;

            foreach (var receivedNode in node.Children
                         .Select(child => GetBestNode(depth - 1, child, !isMaximazing))
                         .Where(receivedNode => bestNode.NodeWeight < receivedNode.NodeWeight))
            {
                bestNode = receivedNode;
            }

            return bestNode;
        }
        else
        {
            var bestNode = node;

            foreach (var receivedNode in node.Children
                         .Select(child => GetBestNode(depth - 1, child, !isMaximazing))
                         .Where(receivedNode => bestNode.NodeWeight > receivedNode.NodeWeight))
            {
                bestNode = receivedNode;
            }

            return bestNode;
        }
    }
}