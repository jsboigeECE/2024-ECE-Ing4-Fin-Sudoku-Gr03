using GeneticSharp;
using Sudoku.GeneticAlgorithm;
using Sudoku.Shared;

namespace Sudoku.GeneticAlgorithm
{
    public class GeneticSolver : ISudokuSolver
    {
        public SudokuGrid Solve(SudokuGrid s)
        {
            var permutatedCellsChromosome = new SudokuOrderedCellsChromosome(s);

            var popSize = 400;

            var crossover = new CycleCrossover();
            
            var mutation = new TworsMutation();
           
            var sdkBoard = SudokuTestHelper.Eval(permutatedCellsChromosome, crossover, mutation, s, popSize);

            return sdkBoard;
        }
    }

    public class GeneticPermutedCellsCycleTworsSolver : ISudokuSolver
    {
        public SudokuGrid Solve(SudokuGrid s)
        {
            var permutatedCellsChromosome = new SudokuPermutatedCellsChromosome(s);

            var popSize = 400;

            var crossover = new CycleCrossover();

            var mutation = new TworsMutation();

            var sdkBoard = SudokuTestHelper.Eval(permutatedCellsChromosome, crossover, mutation, s, popSize);

            return sdkBoard;
        }
    }

    public class GeneticPermutedCellsPartiallyMappedReverseSequenceSolver : ISudokuSolver
    {
        public SudokuGrid Solve(SudokuGrid s)
        {
            var permutatedCellsChromosome = new SudokuPermutatedCellsChromosome(s);

            var popSize = 400;

            var crossover = new PartiallyMappedCrossover();

            var mutation = new ReverseSequenceMutation();

            var sdkBoard = SudokuTestHelper.Eval(permutatedCellsChromosome, crossover, mutation, s, popSize);

            return sdkBoard;
        }
    }

    public class GeneticPermutedCellsOrderedInsertionSolver : ISudokuSolver
    {
        public SudokuGrid Solve(SudokuGrid s)
        {
            var permutatedCellsChromosome = new SudokuPermutatedCellsChromosome(s);

            var popSize = 400;

            var crossover = new OrderedCrossover();

            var mutation = new InsertionMutation();

            var sdkBoard = SudokuTestHelper.Eval(permutatedCellsChromosome, crossover, mutation, s, popSize);

            return sdkBoard;
        }
    }



    public class GeneticPermutationsSolver : ISudokuSolver
    {
        public SudokuGrid Solve(SudokuGrid s)
        {

            var permutatedCellsChromosome = new SudokuPermutationsChromosome(s);


            var popSize = 400;

            var crossover = new UniformCrossover();

            var mutation = new UniformMutation();

            var sdkBoard = SudokuTestHelper.Eval(permutatedCellsChromosome, crossover, mutation, s, popSize);

            return sdkBoard;
        }
    }


    public class GeneticCellsSolver : ISudokuSolver
    {
        public SudokuGrid Solve(SudokuGrid s)
        {

            var permutatedCellsChromosome = new SudokuPermutationsChromosome(s);

            var popSize = 400;
            var crossover = new UniformCrossover();

            var mutation = new UniformMutation();

            var sdkBoard = SudokuTestHelper.Eval(permutatedCellsChromosome, crossover, mutation, s, popSize);

            return sdkBoard;

        }
    }


}