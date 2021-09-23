using System;
using System.Collections.Generic;
using System.Linq;

using EightQueens.Common;

namespace EightQueens.AStar
{
    public class AStarSolver
    {
        private readonly PriorityQueue<AStarNode, int> _solutionNodes;
        private readonly List<List<Position>> _closedStates;
        private readonly DateTime _stopDateTime;

        public AStarSolver()
        {
            _solutionNodes = new PriorityQueue<AStarNode, int>(new NodesComparer());
            _closedStates = new List<List<Position>>();
            _stopDateTime = DateTime.Now.AddMinutes(30);
        }

        public AStarSolutionInfo Solve(Board board)
        {
            if (board.Validate())
            {
                return new AStarSolutionInfo()
                {
                    SolvedBoard = board,
                };
            }

            try
            {
                var solNode = FindSolutionPath(board);
                return new AStarSolutionInfo()
                {
                    OpenStates = _solutionNodes.Count,
                    ClosedStates = _closedStates.Count,
                    SolvedBoard = solNode.Board,
                };
            }
            catch (TimeoutException)
            {
                return new AStarSolutionInfo()
                {
                    OpenStates = _solutionNodes.Count,
                    ClosedStates = _closedStates.Count,
                };
            }
        }

        private AStarNode FindSolutionPath(Board board)
        {
            var node = new AStarNode()
            {
                Weight = 0,
                Board = new Board(board),
                // It's a top node that represents the beginning state of the board
                PositionUpdate = null,
            };

            _solutionNodes.Enqueue(node, node.Weight);

            while (!node.Board.Validate())
            {
                if (_stopDateTime <= DateTime.Now)
                {
                    throw new TimeoutException();
                }

                node = _solutionNodes.Dequeue();

                if (node is { PositionUpdate: not null })
                {
                    node.Board.Move(node.PositionUpdate);
                    _closedStates.Add(new List<Position>(node.Board.Positions));
                }

                // Generate and filter subNodes
                var subNodes = new List<AStarNode>();
                foreach (var subNode in GenerateSubNodes(node.Board))
                {
                    // This can be optimized with creating a hash code and saving it instead of a list of positions
                    node.Board.Move(subNode.PositionUpdate);
                    var state = new List<Position>(node.Board.Positions);
                    node.Board.MoveBack(subNode.PositionUpdate);

                    var cState = _closedStates.FirstOrDefault(x => state.All(x.Contains));
                    if (cState is null)
                    {
                        subNodes.Add(subNode);
                    }
                }

                foreach (var sNode in subNodes)
                {
                    sNode.Weight = Heuristic(sNode, node.Weight);
                    _solutionNodes.Enqueue(sNode, sNode.Weight);
                }

                if (_solutionNodes.Count is 0)
                {
                    throw new Exception("Solution wasn't found! Checked states: " + _closedStates.Count);
                }
            }

            return node;
        }

        private List<AStarNode> GenerateSubNodes(Board board)
        {
            var safeCells = board.GetSafeCellsExt();

            return safeCells.Select(x => new AStarNode()
            {
                Weight = 0,
                Board = new Board(board),
                PositionUpdate = new QueenPositionUpdate()
                {
                    CurrentPosition = x.Current,
                    NewPosition = x.New,
                },
            }).ToList();
        }

        private int Heuristic(AStarNode node, int currentWeight)
        {
            var weight = currentWeight;

            node.Board.Move(node.PositionUpdate);
            foreach (var pos in node.Board.Positions)
            {
                // -1 requires cause method returns a total number of the queens at the row, col or diagonal
                weight += node.Board.GetQueensAmountOnRow(pos.Row) - 1;
                weight += node.Board.GetQueensAmountOnColumn(pos.Column) - 1;
                weight += node.Board.GetQueensAmountOnDiagonals(pos.Row, pos.Column) - 1;
            }
            node.Board.MoveBack(node.PositionUpdate);

            return weight;
        }
    }
}