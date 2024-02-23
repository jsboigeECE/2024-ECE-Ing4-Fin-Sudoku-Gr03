using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using Antlr.Runtime;
using GeneticSharp;
using GeneticSharp.Extensions;
using Sudoku.Shared;


namespace Sudoku.GeneticAlgorithm
{
    public class GeneticSolver : ISudokuSolver
    {
        //Implémente la méthode Solve de ISudokuSolver
        public SudokuGrid Solve(SudokuGrid s)
        {
            // Créez une instance de fitness pour évaluer les chromosomes Sudoku
            var fitness = new SudokuFitness(s);

            var selection = new EliteSelection();

            var crossover = new UniformCrossover();

            var mutation = new UniformMutation();

            var termination = new GenerationNumberTermination(1000);

            // Créez une population pour le solveur génétique
            var population = new Population(50, 50, new SudokuPermutationsChromosome(s));

            // Créez un solveur génétique avec les opérateurs de sélection, crossover et mutation appropriés
            var ga = new GeneticAlgorithm(population, fitness, selection, crossover, mutation, termination);

            // Exécutez le solveur génétique
            ga.Start();

            // Récupérez le meilleur chromosome après la résolution
            var bestChromosome = ga.BestChromosome as ISudokuChromosome;

            // Récupérez la meilleure grille Sudoku à partir du meilleur chromosome
            var bestSudokuBoard = bestChromosome.GetSudokus().First();

            // Retournez la grille Sudoku résolue
            return bestSudokuBoard;

        }
    }
}



