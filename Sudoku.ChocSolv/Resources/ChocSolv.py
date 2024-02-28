"""from timeit import default_timer

#instance = ((0,0,0,0,9,4,0,3,0),
#           (0,0,0,5,1,0,0,0,7),
#           (0,8,9,0,0,0,0,4,0),
#           (0,0,0,0,0,0,2,0,8),
#           (0,6,0,2,0,1,0,5,0),
#           (1,0,2,0,0,0,0,0,0),
#           (0,7,0,0,0,0,5,2,0),
#           (9,0,0,0,6,5,0,0,0),
#           (0,4,0,9,7,0,0,0,0))

def findNextCellToFill(grid, i, j):
        for x in range(i,9):
                for y in range(j,9):
                        if grid[x][y] == 0:
                                return x,y
        for x in range(0,9):
                for y in range(0,9):
                        if grid[x][y] == 0:
                                return x,y
        return -1,-1

def isValid(grid, i, j, e):
        rowOk = all([e != grid[i][x] for x in range(9)])
        if rowOk:
                columnOk = all([e != grid[x][j] for x in range(9)])
                if columnOk:
                        # finding the top left x,y co-ordinates of the section containing the i,j cell
                        secTopX, secTopY = 3 *(i//3), 3 *(j//3) #floored quotient should be used here. 
                        for x in range(secTopX, secTopX+3):
                                for y in range(secTopY, secTopY+3):
                                        if grid[x][y] == e:
                                                return False
                        return True
        return False

def solveSudoku(grid, i=0, j=0):
        i,j = findNextCellToFill(grid, i, j)
        if i == -1:
                return True
        for e in range(1,10):
                if isValid(grid,i,j,e):
                        grid[i][j] = e
                        if solveSudoku(grid, i, j):
                                return True
                        # Undo the current cell for backtracking
                        grid[i][j] = 0
        return False

#start = default_timer()
if(solveSudoku(instance)):
	print("eeeee")
	r=instance
else:
	print ("Aucune solution trouv�e")

#execution = default_timer() - start
#print("Le temps de r�solution est de : ", execution, " seconds as a floating point value")
"""        
from timeit import default_timer

# instance = [[0,0,0,0,9,4,0,3,0],
# [0,0,0,5,1,0,0,0,7],
# [0,8,9,0,0,0,0,4,0],
# [0,0,0,0,0,0,2,0,8],
# [0,6,0,2,0,1,0,5,0],
# [1,0,2,0,0,0,0,0,0],
# [0,7,0,0,0,0,5,2,0],
# [9,0,0,0,6,5,0,0,0],
# [0,4,0,9,7,0,0,0,0]]


from pychoco import Model


def solveSudokuWithChoco(grid):
    model = Model("Sudoku Solver")
    # Création des variables : une grille 9x9 de variables avec des domaines de 1 à 9
    cells = [[model.intvar(1, 9) for _ in range(9)] for _ in range(9)]
    # Ajout des contraintes
    for i in range(9):
        # Unicité dans les lignes et les colonnes
        model.all_different([cells[i][j] for j in range(9)]).post()
        model.all_different([cells[j][i] for j in range(9)]).post()
    # Unicité dans les blocs 3x3
    for block_row in range(3):
        for block_col in range(3):
            model.all_different([cells[block_row*3 + i][block_col*3 + j] for i in range(3) for j in range(3)]).post()
    # Fixation des valeurs pré-remplies du puzzle
    for i in range(9):
        for j in range(9):
            if grid[i][j] != 0:
                model.arithm(cells[i][j], "=", grid[i][j]).post()
    # Résolution
    if model.get_solver().solve():
        # Mise à jour de la grille avec les solutions trouvées
        for i in range(9):
            for j in range(9):
                grid[i][j] = cells[i][j].get_value()
        return True
    else:
        return False


"""def print_grid(grid):
    for i in range(9):
        if i % 3 == 0 and i != 0:
            print("- - - - - - - - - - - - -")
    for j in range(9):
        if j % 3 == 0 and j != 0:
            print(" | ", end="")
        if j == 8: # À la fin de la ligne
            print(grid[i][j])
        else:
            print(str(grid[i][j]) + " ", end="")"""

# start = default_timer()
if solveSudokuWithChoco(instance):
    # print_grid(instance)
    r = instance
else:
    print("Aucune solution trouvée")
# execution = default_timer() - start
# print("Le temps de résolution est de : ", execution, " seconds as a floating point value")
