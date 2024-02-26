using Google.OrTools.Sat;
using Sudoku.Shared;

namespace GeneticAlgorithmSolver
{
    public class ORToolsSolver : ISudokuSolver
    {
        public SudokuGrid Solve(SudokuGrid grid)
        {
            CpModel model = new CpModel();

            // Définir les variables du modèle
            IntVar[,] cells = new IntVar[9, 9];
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    // Les variables représentent les chiffres dans chaque cellule (de 1 à 9)
                    cells[i, j] = model.NewIntVar(1, 9, $"Cell_{i}_{j}");
                }
            }

            // Ajouter les contraintes du Sudoku
            for (int i = 0; i < 9; i++)
            {
                // Contrainte pour chaque ligne
                IntVar[] rowVariables = new IntVar[9];
                for (int j = 0; j < 9; j++)
                {
                    rowVariables[j] = cells[i, j];
                }
                model.AddAllDifferent(rowVariables);

                // Contrainte pour chaque colonne
                IntVar[] colVariables = new IntVar[9];
                for (int j = 0; j < 9; j++)
                {
                    colVariables[j] = cells[j, i];
                }
                model.AddAllDifferent(colVariables);
            }

            // Contrainte pour chaque bloc 3x3
            for (int i = 0; i < 9; i += 3)
            {
                for (int j = 0; j < 9; j += 3)
                {
                    IntVar[] blockVariables = new IntVar[9];
                    int index = 0;
                    for (int bi = 0; bi < 3; bi++)
                    {
                        for (int bj = 0; bj < 3; bj++)
                        {
                            blockVariables[index++] = cells[i + bi, j + bj];
                        }
                    }
                    model.AddAllDifferent(blockVariables);
                }
            }

            // Ajouter les contraintes initiales basées sur la grille donnée
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (grid.GetElement(i, j) != 0)
                    {
                        model.Add(cells[i, j] == grid.GetElement(i, j));
                    }
                }
            }

            // Créer le solveur
            CpSolver solver = new CpSolver();

            // Résoudre le modèle
            CpSolverStatus status = solver.Solve(model);

            if (status == CpSolverStatus.Feasible || status == CpSolverStatus.Optimal)
            {
                // Construire la grille résolue à partir des valeurs des variables
                SudokuGrid solvedGrid = new SudokuGrid();
                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        solvedGrid.SetElement(i, j, (int)solver.Value(cells[i, j]));
                    }
                }

                return solvedGrid;
            }
            else
            {
                // La résolution a échoué, renvoyer la grille d'origine
                return grid;
            }
        }
    }
}
