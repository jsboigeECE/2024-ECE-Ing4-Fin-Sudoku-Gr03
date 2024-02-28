from timeit import default_timer

#instance = ((0,0,0,0,9,4,0,3,0),
#           (0,0,0,5,1,0,0,0,7),
#           (0,8,9,0,0,0,0,4,0),
#           (0,0,0,0,0,0,2,0,8),
#           (0,6,0,2,0,1,0,5,0),
#           (1,0,2,0,0,0,0,0,0),
#           (0,7,0,0,0,0,5,2,0),
#           (9,0,0,0,6,5,0,0,0),
#           (0,4,0,9,7,0,0,0,0))

#start = default_timer()
if(solveSudoku(instance)):
	#print_grid(instance)
	r=instance
else:
	print ("Aucune solution trouv�e")

#execution = default_timer() - start
print("Le temps de r�solution est de : ", execution, " seconds as a floating point value")