using Sudoku.Shared;

namespace Sudoku.GeneticAlgorithm
{
    public class GeneticSolver : ISudokuSolver
    {
        public SudokuGrid Solve(SudokuGrid s)
        {
            return s.CloneSudoku();
        }

    }
}