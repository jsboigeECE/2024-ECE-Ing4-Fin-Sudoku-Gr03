using GeneticSharp;
using Sudoku.GeneticAlgorithm;
using Sudoku.Shared;
using System;
using System.Diagnostics;
using System.Linq;


namespace Sudoku.GeneticAlgorithm
{
    public static class SudokuTestHelper
    {
 
        public static SudokuGrid Eval(IChromosome sudokuChromosome, ICrossover crossover, IMutation mutation, SudokuGrid sudokuBoard, int populationSize)
        {

            var fitnessThreshold = 0;
            int stableGenerationNb = 20;

            SudokuFitness fitness = new SudokuFitness(sudokuBoard);
            EliteSelection selection = new EliteSelection();

            var termination = new OrTermination(new ITermination[]
            {
                new FitnessThresholdTermination(fitnessThreshold),
                new FitnessStagnationTermination(stableGenerationNb),
            });


            var nbErrors = 0;
            SudokuGrid bestSudoku;
            var sw = Stopwatch.StartNew();
            var lastTime = sw.Elapsed;
            do
            {
                Population population = new Population(populationSize, populationSize, sudokuChromosome);


                var ga = new GeneticSharp.GeneticAlgorithm(population, fitness, selection, crossover, mutation)
                {
                    Termination = termination
                };
                //Ajout d'opérateurs de parallélisation
                ga.OperatorsStrategy = new TplOperatorsStrategy();
                ga.TaskExecutor = new TplTaskExecutor();
                ga.GenerationRan += (sender, args) =>
                {
                    var bestIndividual = (ISudokuChromosome)ga.Population.BestChromosome;
                    var solutions = bestIndividual.GetSudokus();
                    bestSudoku = solutions[0];
                    nbErrors = bestSudoku.NbErrors(sudokuBoard);
                    Console.WriteLine($"Generation {ga.GenerationsNumber}, population {ga.Population.CurrentGeneration.Chromosomes.Count}, nbErrors {nbErrors} Elapsed since initial Gen {sw.Elapsed}");
                    lastTime = sw.Elapsed;
                };

                ga.Start();
                ISudokuChromosome bestIndividual = (ISudokuChromosome)ga.Population.BestChromosome;
                IList<SudokuGrid> solutions = bestIndividual.GetSudokus();
                bestSudoku = solutions[0];
                nbErrors = bestSudoku.NbErrors(sudokuBoard);
                populationSize *= 2;
            } while (nbErrors > 0);

            return bestSudoku;
        }
    }
}