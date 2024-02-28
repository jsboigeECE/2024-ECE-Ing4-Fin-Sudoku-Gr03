using System.Collections.Generic;
using System.Linq.Expressions;
using Z3.LinqBinding;

namespace Sudoku.LinqToZ3
{
    public class SudokuTheoremRewriter : ITheoremGlobalRewriter
    {
        public IEnumerable<LambdaExpression> Rewrite(IEnumerable<LambdaExpression> constraints)
        {
            return constraints;
        }
    }
}