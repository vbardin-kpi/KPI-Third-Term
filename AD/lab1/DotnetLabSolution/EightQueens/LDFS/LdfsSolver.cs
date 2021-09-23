using System;
using System.Collections.Generic;
using System.Linq;

using EightQueens.Common;

namespace EightQueens.LDFS
{
    public class LdfsSolver
    {
        private int _depth;
        private int _totalSteps;
        private int _backtracks;

        public LdfsSolutionInfo Solve(Board board)
        {
            if (board.ValidateLite())
            {
                return new LdfsSolutionInfo()
                {
                    Depth = _depth,
                    TotalSteps = _totalSteps,
                    BackTracks = _backtracks,
                    SolvedBoard = board,
                };
            }

            var topNode = new LdfsNode()
            {
                Depth = 0,
                Board = board,
                ParentLdfsNode = null,
                PositionUpdate = null,
            };

            try
            {
                Ldfs(1000, topNode);
            }
            catch (Exception)
            {
                throw new DepthLimitReached(
                    new LdfsSolutionInfo
                    {
                        Depth = _depth,
                        TotalSteps = _totalSteps,
                        BackTracks = _backtracks,
                        SolvedBoard = topNode.Board,
                    });
            }

            return new LdfsSolutionInfo()
            {
                Depth = _depth,
                TotalSteps = _totalSteps,
                BackTracks = _backtracks,
                SolvedBoard = topNode.Board,
            };
        }

        private void Ldfs(int limit, LdfsNode node)
        {
            if (node.Board.Validate())
            {
                return;
            }

            if (node.Depth >= limit)
            {
                throw new Exception("Depth limit was reached but solutions wasn't found!");
            }

            _totalSteps++;
            _depth = node.Depth;

            if (node is { PositionUpdate: not null })
            {
                node.Board.Move(node.PositionUpdate);
                node.Board.InvalidPositions.Clear();

                if (node.Board.Validate())
                {
                    return;
                }
            }

            // If nodes wasn't generated yet for the current node, it's a time to generate some
            if (node.Nodes is null)
            {
                var subNodes = GetPossiblePositions(node);
                if (subNodes is { Count: > 0 })
                {
                    node.Nodes = subNodes;
                }
            }

            // Try to select a sub-node to check next for the given node
            if (node.Nodes is { Count: > 0 })
            {
                var nextSubNode = node.Nodes.First();
                if (nextSubNode is not null)
                {
                    Ldfs(limit, nextSubNode);
                    return;
                }
            }

            RollbackChanges(node);
            _backtracks++;
            if (node.ParentLdfsNode is null) throw new Exception("Solutions not found");

            Ldfs(limit, node.ParentLdfsNode);
        }

        // TODO: Add linking possible movements to the queen
        private List<LdfsNode> GetPossiblePositions(LdfsNode node)
        {
            var safeCells = node.Board.GetSafeCellsExt();

            var possibleStates = safeCells.Select(safeCell =>
                    new LdfsNode()
                    {
                        Depth = ++node.Depth,
                        ParentLdfsNode = node,
                        Board = node.Board,
                        PositionUpdate = new QueenPositionUpdate()
                        {
                            NewPosition = safeCell.New,
                            CurrentPosition = safeCell.Current,
                        },
                    })
                .ToList();

            return possibleStates;
        }

        private void RollbackChanges(LdfsNode node)
        {
            if (node is { ParentLdfsNode: { Nodes: { Count: > 0 } } })
            {
                node.ParentLdfsNode.Nodes.Remove(node);
            }

            if (node.PositionUpdate is null) return;
            node.Board.MoveBack(node.PositionUpdate);
        }
    }
}