using Sudoku.Shared;
using Z3.LinqBinding;

namespace Sudoku.LinqToZ3
{
    public class LinqToZ3Solver : ISudokuSolver
    {
        public SudokuGrid Solve(SudokuGrid s)
        {
            using (var ctx = new Z3Context())
			{
                // ctx.Log = Console.Out; // see internal logging

                var theorem = SudokuTheorem.Create(ctx);

                for (int row = 0; row < 9; row++)
                {
                    for (int col = 0; col < 9; col++)
                    {
                        var i1 = row;
                        var j1 = col;
                        var v1 = s.Cells[i1][j1];
                        if (v1 == 0) continue;
                        theorem = from t in theorem
                                    where t.Cells[i1][j1] == v1
                                    select t;
                    }
                }
                
				var result = theorem.Solve();
				if (result is not null)
                    return result.CloneSudoku();
                else return s;
			}
        }
    }
}

