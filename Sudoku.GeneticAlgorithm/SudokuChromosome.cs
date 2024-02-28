
using GeneticSharp;
using Sudoku.Shared;

namespace Sudoku.GeneticAlgorithm;
public interface ISudokuChromosome
{
    IList<SudokuGrid> GetSudokus();
}

public class SudokuFitness : IFitness
{
    private readonly SudokuGrid _targetSudokuGrid;

    public SudokuFitness(SudokuGrid targetSudokuGrid)
    {
        _targetSudokuGrid = targetSudokuGrid;
    }

    
    /// <param name="chromosome"></param>
    public double Evaluate(IChromosome chromosome)
    {
        return Evaluate((ISudokuChromosome)chromosome);
    }

    /// <param name="chromosome">a Chromosome that can build Sudokus</param>
    public double Evaluate(ISudokuChromosome chromosome)
    {
        List<double> scores = new List<double>();

        var sudokus = chromosome.GetSudokus();
        foreach (var sudoku in sudokus)
        {
            scores.Add(Evaluate(sudoku));
        }

        return scores.Sum();
    }

    /// <param name="testSudokuGrid">the board to evaluate</param>
        public double Evaluate(SudokuGrid testSudokuGrid)
    {
        var toReturn = -testSudokuGrid.NbErrors(_targetSudokuGrid);
        return toReturn;
    }


}
