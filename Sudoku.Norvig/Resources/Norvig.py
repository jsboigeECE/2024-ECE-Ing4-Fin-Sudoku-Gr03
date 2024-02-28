from timeit import default_timer
 #Définition de la fonction qui combine chaque élément de A avec chaque élément de B
def cross(A, B):
    return [a+b for a in A for b in B]

digits = '123456789' # Les chiffres possibles dans une grille de Sudoku
rows = 'ABCDEFGHI' # Les lignes de la grille
cols = digits # Les colonnes de la grille
squares = cross(rows, cols)# Les cases de la grille de Sudoku
# La liste des unités, qui sont les rangées, colonnes et blocs
unitlist = ([cross(rows, c) for c in cols] + [cross(r, cols) for r in rows] +
            [cross(rs, cs) for rs in ('ABC','DEF','GHI') for cs in ('123','456','789')])
# La création d'un dictionnaire où chaque case (square) est associée à ses unités (groupes)
units = dict((s, [u for u in unitlist if s in u]) for s in squares)
# La création d'un dictionnaire où chaque case (square) est associée à ses pairs (peers)
peers = dict((s, set(sum(units[s], []))-set([s])) for s in squares)

# Fonction pour analyser la grille et l'initialiser avec les valeurs possibles
def parse_grid(grid):
    values = dict((s, digits) for s in squares)# Initialise toutes les cases avec tous les chiffres possibles
    for s,d in grid_values(grid).items():# Attribue les valeurs déjà données dans la grille
        if d in digits and not assign(values, s, d):
            return False  # Retourne False si une contradiction est détectée
    return values


# Fonction pour convertir la grille en un dictionnaire de valeurs
def grid_values(grid):
    chars = [c if c in digits else '0' for c in grid] # Remplace les cases vides par '0'
    assert len(chars) == 81# Vérifie que la grille a la bonne taille
    return dict(zip(squares, chars))# Crée un dictionnaire avec les cases et les caractères correspondants

#Propagation de contrainte
def assign(values, s, d):
    other_values = values[s].replace(d, '')  # Enlève le chiffre d des autres valeurs possibles dans la case s
    if all(eliminate(values, s, d2) for d2 in other_values):
        return values # Retourne les valeurs si aucune contradiction n'est détectée
    else:
        return False

def eliminate(values, s, d):
    if d not in values[s]:
        return values # Si le chiffre d n'est pas dans la case s, aucune modification n'est nécessaire
    values[s] = values[s].replace(d, '') # Enlève le chiffre d des valeurs possibles dans la case s
    if len(values[s]) == 0:
        return False # Retourne False si la case s n'a plus de valeurs possibles
    elif len(values[s]) == 1:
        d2 = values[s]
        if not all(eliminate(values, s2, d2) for s2 in peers[s]):
            return False
    for u in units[s]:
        dplaces = [s for s in u if d in values[s]]  # Trouve les cases où le chiffre d est possible dans l'unité
        if len(dplaces) == 1:
            if not assign(values, dplaces[0], d):
                return False
    return values # Retourne les valeurs après l'élimination

def display(values):
    width = 1+max(len(values[s]) for s in squares) # Calcule la largeur des colonnes
    line = '+'.join(['-'*(width*3)]*3)
    for r in rows:
        print(''.join(values[r+c].center(width)+('|' if c in '36' else '')
                      for c in cols)) # Affiche chaque rangée de la grille
        if r in 'CF': print(line)
    print()

#Recherche
def solve(grid):
    return search(parse_grid(grid))

def search(values):
    if values is False:
        return False# Retourne False si la grille est vide (aucune solution possible)
    if all(len(values[s]) == 1 for s in squares):
        return values # Retourne les valeurs si la grille est résolue
    n, s = min((len(values[s]), s) for s in squares if len(values[s]) > 1)
    return some(search(assign(values.copy(), s, d)) for d in values[s])

def some(seq):
    for e in seq:
        if e:
            return e
    return False

# Fonction principale pour résoudre la grille de Sudoku   
def sudoku_solver(csharp_grid):
    start_time = default_timer()
    grid = ''.join(str(c) for row in csharp_grid for c in row)# Convertit la grille en une chaîne de caractères
    result = solve(grid)# Résout la grille
    end_time = default_timer()
    elapsed_time = end_time - start_time
    print(f"Temps d'execution : {elapsed_time} secondes")
    if result:
        solved_grid = [[int(result[s]) for s in cross(r, cols)] for r in rows]
        return solved_grid
    else:
        return None # Retourne None si aucune solution n'est trouvée


# Exemple d'utilisation
example_grid = '....7..2.8.......6.1.2.5...9.54....8.........3....85.1...3.2.8.4.......9.7..6....'

solved_grid = sudoku_solver(example_grid)

# Afficher la grille résolue ou effectuer d'autres opérations avec solved_grid
if solved_grid:
    print("Grille résolue :")
    for row in solved_grid:
        print(row)
        
else:
    print("Aucune solution trouvée")




