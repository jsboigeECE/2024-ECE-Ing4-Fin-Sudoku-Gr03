using System;
using System.Collections.Generic;

namespace Sudoku.GeneticAlgorithm
{
    public class SudokuChromosome
    {
        //Taille standard d'un sudoku
        public const int SudokuSize = 9;
        //Nombre de cellules remplies au début
        private const int FilledCells = 17;
        //Valeurs possibles dans les cellules
        private static readonly List<int> PossibleValues = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        //Tableau qui représente les gènes des chromosomes du sudoku
        private readonly int[] _genes;

        //Constructeur de la classe SudokuChromosome
        public SudokuChromosome()
        {
            //Initialisation du tableau de gènes
            _genes = new int[SudokuSize * SudokuSize];
            //Initialisation du sudoku
            InitializeSudoku();
        }

        //Méthode pour obtenir le gène d'un index donné
        public int GetGene(int index)
        {
            return _genes[index];
        }

        //Méthode pour définir le gène d'un index donné
        public void SetGene(int index, int value)
        {
            _genes[index] = value;
        }

        //Méthode pour obtenir la représentation d'un sudoku
        public int[,] GetSudokuRepresentation()
        {
            var sudoku = new int[SudokuSize, SudokuSize];
            //Remplissage du sudoku par les gènes
            for (int row = 0; row < SudokuSize; row++)
            {
                for (int col = 0; col < SudokuSize; col++)
                {
                    sudoku[row, col] = _genes[row * SudokuSize + col];
                }
            }
            return sudoku;
        }

        //Méthode pour initialiser le sudoku
        private void InitializeSudoku()
        {
            var random = new Random();
            var filledCellsIndexes = new HashSet<int>();

            // Remplissage aléatoire des cellules
            while (filledCellsIndexes.Count < FilledCells)
            {
                filledCellsIndexes.Add(random.Next(0, _genes.Length));
            }

            // Attribuer des valeurs dans chaque cellule remplie
            foreach (var index in filledCellsIndexes)
            {
                var value = PossibleValues[random.Next(0, PossibleValues.Count)];
                _genes[index] = value;
            }

            // Vérification et correction de la grille initiale
            if (!IsValidSudoku())
            {
                // Réinitialisation de la grille
                for (int i = 0; i < _genes.Length; i++)
                {
                    _genes[i] = 0;
                }
                // Recréation de la grille jusqu'à ce qu'elle soit valide
                InitializeSudoku();
            }
        }


        // Méthode pour vérifier la validité de la grille initiale
        private bool IsValidSudoku()
        {
            var sudoku = GetSudokuRepresentation();

            // Vérification des lignes et des colonnes
            for (int i = 0; i < SudokuSize; i++)
            {
                var rowSet = new HashSet<int>();
                var colSet = new HashSet<int>();
                for (int j = 0; j < SudokuSize; j++)
                {
                    if (rowSet.Contains(sudoku[i, j]) && sudoku[i, j] != 0)
                        return false;
                    rowSet.Add(sudoku[i, j]);

                    if (colSet.Contains(sudoku[j, i]) && sudoku[j, i] != 0)
                        return false;
                    colSet.Add(sudoku[j, i]);
                }
            }

            // Vérification des blocs 3x3
            for (int blockRow = 0; blockRow < SudokuSize; blockRow += 3)
            {
                for (int blockCol = 0; blockCol < SudokuSize; blockCol += 3)
                {
                    var blockSet = new HashSet<int>();
                    for (int i = blockRow; i < blockRow + 3; i++)
                    {
                        for (int j = blockCol; j < blockCol + 3; j++)
                        {
                            if (blockSet.Contains(sudoku[i, j]) && sudoku[i, j] != 0)
                                return false;
                            blockSet.Add(sudoku[i, j]);
                        }
                    }
                }
            }

            return true;
        }


    }
}
