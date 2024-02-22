using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku.GeneticAlgorithm
{
    public class SudokuFitness
    {
        //Déclaration d'une variable pour stocker le sudoku cible
        private readonly int[,] _targetSudoku;

        //Constructeur de la classe SudokuFitness prenant en argument le sudoku cible
        public SudokuFitness(int[,] targetSudoku)
        {
            _targetSudoku = targetSudoku;
        }

        //Méthode qui évalue la qualité du chromosome en fonction du nombre d'erreurs
        public double Evaluate(SudokuChromosome chromosome)
        {
            //Récuperation de la représentation du sudoku du chromosome
            var sudokuGrid = chromosome.GetSudokuRepresentation();
            //Décompte du nombre d'erreurs
            int errorsCount = CountErrors(sudokuGrid);

            // Renvoie l'opposé du nombre d'erreurs
            return -errorsCount;
        }

        //Méthode qui compte le nombre d'erreurs au sein du sudoku
        private int CountErrors(int[,] sudoku)
        {
            int errors = 0;

            // Vérifier les lignes puis les colinnes
            for (int row = 0; row < SudokuChromosome.SudokuSize; row++)
            {
                for (int col = 0; col < SudokuChromosome.SudokuSize; col++)
                {
                    int value = sudoku[row, col];
                    if (value != 0 && (CountInRow(sudoku, row, value) > 1 || CountInColumn(sudoku, col, value) > 1 || CountInBlock(sudoku, row, col, value) > 1))
                    {
                        errors++;
                    }
                }
            }

            return errors;
        }

        //Méthode qui compte le nombre d'occurences d'un chiffre au sein d'une ligne
        private int CountInRow(int[,] sudoku, int row, int value)
        {
            int count = 0;
            for (int col = 0; col < SudokuChromosome.SudokuSize; col++)
            {
                if (sudoku[row, col] == value)
                {
                    count++;
                }
            }
            return count;
        }
        //Méthode qui compte le nombre d'occurences d'un chiffre au sein d'une colonne
        private int CountInColumn(int[,] sudoku, int col, int value)
        {
            int count = 0;
            for (int row = 0; row < SudokuChromosome.SudokuSize; row++)
            {
                if (sudoku[row, col] == value)
                {
                    count++;
                }
            }
            return count;
        }

        //Méthode qui compte le nombre d'occurences d'un chiffre dans un bloc 
        private int CountInBlock(int[,] sudoku, int row, int col, int value)
        {
            int count = 0;
            int blockRow = row / 3 * 3;
            int blockCol = col / 3 * 3;
            for (int r = blockRow; r < blockRow + 3; r++)
            {
                for (int c = blockCol; c < blockCol + 3; c++)
                {
                    if (sudoku[r, c] == value)
                    {
                        count++;
                    }
                }
            }
            return count;
        }

    }
}
