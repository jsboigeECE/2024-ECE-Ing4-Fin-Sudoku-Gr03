using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Sudoku.Shared;
using Z3.LinqBinding;

namespace Sudoku.LinqToZ3
{
    public static class SudokuTheorem
    {
        public static Theorem<SudokuGrid> Create(Z3Context context)
        {
            var sudokuTheorem = context.NewTheorem<SudokuGrid>();

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    //To avoid side effects with lambdas, we copy indices to local variables
                    var i1 = i;
                    var j1 = j;
                    sudokuTheorem = sudokuTheorem.Where(sudoku => (sudoku.Cells[i1][j1] >= 1) &&
                                                        (sudoku.Cells[i1][j1] <= 9));
                }
            }

            // Row, col, and box constraints
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    var i1 = i;
                    var j1 = j;
                    var allNeighbors = SudokuGrid.CellNeighbours[i1][j1];
                    foreach (var (row, col) in allNeighbors)
                    {
                        var i2 = row;
                        var j2 = col;
                        sudokuTheorem = sudokuTheorem.Where(sudoku => sudoku.Cells[i1][j1] != sudoku.Cells[i2][j2]);
                    }
                    // Doesn't work :(
                    // sudokuTheorem = sudokuTheorem.Where(sudoku => Z3Methods.DistinctItems(allNeighbors));
                }
            }

            return sudokuTheorem;
        }
    }
}