using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
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
            int[,] sudokuArray = new int[SudokuGrid.NeighbourIndices.Count, SudokuGrid.NeighbourIndices.Count];

            // Convertit la grille Sudoku en un tableau 2D
            for (int row = 0; row < SudokuGrid.NeighbourIndices.Count; row++)
            {
                for (int col = 0; col < SudokuGrid.NeighbourIndices.Count; col++)
                {
                    //Utilisation de la propriété Cells pour accéder aux valeurs de la grille
                    sudokuArray[row, col] = s.Cells[row][col];
                }
            }
            var sudokufinal = SolveSudoku(sudokuArray);
            // Utilisation de SolveSudoku pour résoudre le Sudoku
            return sudokufinal;
        }



        //------------------------------------------RESOLUTION SUDOKU-----------------------------------------------
        //Méthode de résolution du sudoku à partir d'un tableau 2D
        public SudokuGrid SolveSudoku(int[,] targetSudoku)
        {
            //Initialisation de la population de chromosomes
            var population = InitializePopulation(50, targetSudoku);
            //Utilise la classe SudokuFitness pour évaluer la qualité des chromosomes
            var fitness = new SudokuFitness(targetSudoku);

            var generation = 0;
            var maxGenerations = 1000;

            //Génère des générations jusqu'à ce que la solution soit trouvée ou que le nombre maximal de générations soit atteint
            while (generation < maxGenerations && !population.Any(chromosome => fitness.Evaluate(chromosome) == 0))
            {
                population = NextGeneration(population, fitness);
                generation++;
            }

            //Sélectionne le meilleur chromosome
            var bestChromosome = population.OrderBy(chromosome => fitness.Evaluate(chromosome)).First();
            //
            var sudokuChromosome = (SudokuChromosome)bestChromosome; // Cast to SudokuChromosome
            //Crée une nouvelle grille de sudoku
            var sudokuGrid = new SudokuGrid(); // Create a new SudokuGrid
            //Récupère la représentation du nouveau sudoku
            var sudokuRepresentation = sudokuChromosome.GetSudokuRepresentation(); // Get the Sudoku representation

            // Remplit la grille avec la représentation obtenue  
            for (int row = 0; row < SudokuGrid.NeighbourIndices.Count; row++)
            {
                for (int col = 0; col < SudokuGrid.NeighbourIndices.Count; col++)
                {
                    sudokuGrid.Cells[row][col] = sudokuRepresentation[row, col];
                }
            }
            return sudokuGrid;


        }

        //--------------------------------------------INITIALISATION DE POPULATION---------------------------------------
        //Méthode pour initialiser une population de chromosomes à l'aide de la classe SudokuChromosome
        private static List<SudokuChromosome> InitializePopulation(int populationSize, int[,] targetSudoku)
        {
            var population = new List<SudokuChromosome>();
            var fitness = new SudokuFitness(targetSudoku);
            var random = new Random();
            int maxAttempts = 1000; // Limite de tentatives pour générer un chromosome valide

            for (int i = 0; i < populationSize; i++)
            {
                var chromosome = new SudokuChromosome();
                int attempts = 0;

                do
                {
                    // Génère un chromosome 
                    for (int j = 0; j < SudokuChromosome.SudokuSize * SudokuChromosome.SudokuSize; j++)
                    {
                        chromosome.SetGene(j, random.Next(1, 10));
                    }
                    attempts++;

                    // Vérifie si le chromosome est valide et sort de la boucle si c'est le cas
                } while (fitness.Evaluate(chromosome) != 0 && attempts < maxAttempts);

                if (attempts < maxAttempts)
                {
                    // Ajoute le chromosome valide à la population
                    population.Add(chromosome);
                }
                else
                {
                    // Générer un message d'avertissement ou gérer la situation d'échec autrement
                    Console.WriteLine("Impossible de générer un chromosome valide après " + maxAttempts + " tentatives.");
                }
            }

            return population;
        }


        //------------------------------------------PROCHAINE GENERATION------------------------------------------------------
        //Méthode qui génère la prochaine génération de chromosomes en effectuant des mutations
        private static List<SudokuChromosome> NextGeneration(List<SudokuChromosome> currentPopulation, SudokuFitness fitness)
        {
            //Création d'une liste de chromosomes vide
            var nextGeneration = new List<SudokuChromosome>();
            var random = new Random();
            //Boucle pour chaque chromosome de la population actuelle
            foreach (var chromosome in currentPopulation)
            {
                //Utilisation de la méthode Mutate pour muter chaque chromosome
                var offspring = Mutate(chromosome, random);
                //Ajout du chromosome dans la nouvelle génération
                nextGeneration.Add(offspring);
            }

            return nextGeneration;
        }


        //-----------------------------------------MUTATION------------------------------------------------------------------
        //Méthode pour effectuer une mutation sur un chromosome
        private static SudokuChromosome Mutate(SudokuChromosome chromosome, Random random)
        {
            //Initialise un nouveau chromosome
            var mutatedChromosome = new SudokuChromosome();

            //Boucle sur chaque gène du chromosome
            for (int i = 0; i < SudokuChromosome.SudokuSize * SudokuChromosome.SudokuSize; i++)
            {
                if (random.NextDouble() < 0.1) // Mutation probability
                {
                    mutatedChromosome.SetGene(i, chromosome.GetGene(i)); // Copier le gène du chromosome parent
                }
                else
                {
                    mutatedChromosome.SetGene(i, random.Next(1, 10)); // Générer un gène aléatoire
                }
            }

            return mutatedChromosome;
        }

        //-------------------------------------------CONVERSION DU TABLEAU EN SUDOKUGRID----------------------------------------
        //Méthode pour convertir un tableau 2D en un SudokuGrid
        private static SudokuGrid ConvertToSudokuGrid(List<int[]> cells)
        {
            //Création d'un nouveau tableau 2D
            var sudoku = new int[SudokuChromosome.SudokuSize][];

            //Boucle sur chaque ligne du sudoku
            for (int i = 0; i < SudokuChromosome.SudokuSize; i++)
            {
                //Remplissage du SudokuGrid
                sudoku[i] = cells[i];
            }

            return new SudokuGrid { Cells = sudoku };
        }
    }
}
