Initialisation : Tout d'abord, nous initialisons une grille de sudoku avec les chiffres connus et les possibilités pour les cases vides. Chaque case vide contient initialement les chiffres de 1 à 9 comme possibilités. Avec comme indice des chiffres pour les colonnes et des lettres pour les lignes

Propagation des contraintes : Nous appliquons les règles du sudoku pour éliminer les possibilités. Cela signifie que pour chaque case remplie, nous éliminons ce chiffre comme possibilité des cases dans la même ligne, colonne et région. Avec les deux règles suivante : (1) Si un carré n'a qu'une seule valeur possible, éliminez cette valeur des pairs du carré.
(2) Si une unité n'a qu'une seule place possible pour une valeur, alors mettez-y la valeur. Cette étape réduit le nombre de possibilités pour chaque case vide.

Choix et résolution des cases les moins contraintes : Nous choisissons une case vide avec le moins de possibilités restantes. Cela maximise nos chances de faire un choix correct. Ensuite, nous tentons chaque possibilité restante pour cette case et répétons le processus de propagation des contraintes à partir de là.



