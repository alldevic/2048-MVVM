﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace _2048.Core.Model
{
    public class GameModel
    {
        public int Score { get; private set; }

        public int RowCount { get; private set; }
        public int ColumnCount { get; private set; }

        public Cell[][] Cells { get; private set; }

        public IEnumerable<Cell> CellsIterator()
        {
            for (var x = 0; x < ColumnCount; ++x)
            {
                for (var y = 0; y < RowCount; ++y)
                {
                    yield return Cells[x][y];
                }
            }
        }

        public GameModel(int rowCount, int columnCount)
        {
            RowCount = rowCount;
            ColumnCount = columnCount;
            Reset();
        }

        public bool PerformMove(MoveDirection direction)
        {
            if (!PackAndMerge(direction)) return false;
            var newTile = GetRandomEmptyTile();

            if (newTile == null) return false;
            Cells[newTile.Item1][newTile.Item2].Value = GetRandomStartingNumber();
            Cells[newTile.Item1][newTile.Item2].WasCreated = true;
            return true;
        }


        private bool PackAndMerge(MoveDirection direction)
        {
            var changed = false;
            switch (direction)
            {
                case MoveDirection.Up:
                    // For each column
                    for (var x = 0; x < ColumnCount; ++x)
                    {
                        // Look at tiles in the column from bottom to top
                        for (var y = 1; y < RowCount; ++y)
                        {
                            if (Cells[x][y].IsEmpty())
                            {
                                continue;
                            }

                            var destinationY = y;
                            while (destinationY - 1 >= 0 &&
                                   (Cells[x][destinationY - 1].IsEmpty() ||
                                    (Cells[x][destinationY - 1].Value == Cells[x][y].Value &&
                                     !Cells[x][destinationY - 1].WasMerged)))
                            {
                                --destinationY;
                            }

                            if (destinationY == y) continue;
                            MergeCells(Cells[x][y], Cells[x][destinationY]);
                            changed = true;
                        }
                    }

                    break;
                case MoveDirection.Down:
                    // For each column
                    for (var x = 0; x < ColumnCount; ++x)
                    {
                        // Look at tiles in the column from bottom to top
                        for (var y = RowCount - 2; y >= 0; --y)
                        {
                            if (Cells[x][y].IsEmpty())
                            {
                                continue;
                            }

                            var destinationY = y;
                            while (destinationY + 1 < RowCount &&
                                   (Cells[x][destinationY + 1].IsEmpty() ||
                                    (Cells[x][destinationY + 1].Value == Cells[x][y].Value &&
                                     !Cells[x][destinationY + 1].WasMerged)))
                            {
                                ++destinationY;
                            }

                            if (destinationY == y) continue;
                            MergeCells(Cells[x][y], Cells[x][destinationY]);
                            changed = true;
                        }
                    }

                    break;
                case MoveDirection.Left:
                    for (var y = 0; y < RowCount; ++y)
                    {
                        // Look at tiles in the column from bottom to top
                        for (var x = 1; x < ColumnCount; ++x)
                        {
                            if (Cells[x][y].IsEmpty())
                            {
                                continue;
                            }

                            var destinationX = x;
                            while (destinationX - 1 >= 0 &&
                                   (Cells[destinationX - 1][y].IsEmpty() ||
                                    (Cells[destinationX - 1][y].Value == Cells[x][y].Value &&
                                     !Cells[destinationX - 1][y].WasMerged)))
                            {
                                --destinationX;
                            }

                            if (destinationX == x) continue;
                            MergeCells(Cells[x][y], Cells[destinationX][y]);
                            changed = true;
                        }
                    }

                    break;
                case MoveDirection.Right:
                    for (var y = 0; y < RowCount; ++y)
                    {
                        // Look at tiles in the column from bottom to top
                        for (var x = ColumnCount - 2; x >= 0; --x)
                        {
                            if (Cells[x][y].IsEmpty())
                            {
                                continue;
                            }

                            var destinationX = x;
                            while (destinationX + 1 < ColumnCount &&
                                   (Cells[destinationX + 1][y].IsEmpty() ||
                                    (Cells[destinationX + 1][y].Value == Cells[x][y].Value &&
                                     !Cells[destinationX + 1][y].WasMerged)))
                            {
                                ++destinationX;
                            }

                            if (destinationX == x) continue;
                            MergeCells(Cells[x][y], Cells[destinationX][y]);
                            changed = true;
                        }
                    }

                    break;
            }

            return changed;
        }

        private void MergeCells(Cell sourceCell, Cell destinationCell)
        {
            // Assumes that an appropriate merge CAN definitely be done.
            if (!(sourceCell.X == destinationCell.X ^ sourceCell.Y == destinationCell.Y))
            {
                throw new InvalidOperationException(
                    "Cells to be merged must share either a row or column but not both");
            }

            if (destinationCell.IsEmpty())
            {
                // This is the last available empty cell so take it!
                destinationCell.Value = sourceCell.Value;
                destinationCell.PreviousPosition =
                    sourceCell.PreviousPosition ?? new Coordinate(sourceCell.X, sourceCell.Y);
                sourceCell.Value = 0;
                sourceCell.PreviousPosition = null;
            }
            else
            {
                if (destinationCell.WasMerged)
                {
                    throw new InvalidOperationException("Destination cell has already been merged");
                }
                else if (sourceCell.Value != destinationCell.Value)
                {
                    throw new InvalidOperationException("Source and destination cells must have the same value");
                }

                // The next available cell has the same value and hasn't yet
                // been merged, so lets merge them!
                destinationCell.Value *= 2;
                destinationCell.WasMerged = true;
                destinationCell.PreviousPosition =
                    sourceCell.PreviousPosition ?? new Coordinate(sourceCell.X, sourceCell.Y);
                sourceCell.Value = 0;
                sourceCell.PreviousPosition = null;

                // Update the score
                Score += destinationCell.Value;
            }
        }

        public void Reset()
        {
            Score = 0;

            Cells = new Cell[ColumnCount][];
            for (var i = 0; i < ColumnCount; ++i)
            {
                Cells[i] = new Cell[RowCount];
            }

            for (var y = 0; y < RowCount; ++y)
            {
                for (var x = 0; x < ColumnCount; ++x)
                {
                    Cells[x][y] = new Cell(x, y);
                }
            }
        }

        private Random rnd = new Random();

        private Tuple<int, int> GetRandomEmptyTile()
        {
            var emptyIndices =
                (from cell in CellsIterator() where cell.IsEmpty() select new Tuple<int, int>(cell.X, cell.Y)).ToList();

            if (emptyIndices.Count == 0)
            {
                return null;
            }

            var next = rnd.Next(0, emptyIndices.Count - 1);
            return emptyIndices[next];
        }

        private int GetRandomStartingNumber()
        {
            return rnd.NextDouble() < 0.9 ? 2 : 4;
        }
    }
}