//Maya ASCII 2015 scene
//Name: DoorWall.ma
//Last modified: Thu, Oct 06, 2016 10:30:47 PM
//Codeset: 1252
requires maya "2015";
currentUnit -l centimeter -a degree -t film;
fileInfo "application" "maya";
fileInfo "product" "Maya 2015";
fileInfo "version" "2015";
fileInfo "cutIdentifier" "201503261530-955654";
fileInfo "osv" "Microsoft Windows 8 Business Edition, 64-bit  (Build 9200)\n";
createNode transform -n "pCube7";
	setAttr ".r" -type "double3" 0 -90 0 ;
	setAttr ".rp" -type "double3" 2.2057209014892578 4.1433650220233051 -0.71189594268798828 ;
	setAttr ".sp" -type "double3" 2.2057209014892578 4.1433650220233051 -0.71189594268798828 ;
createNode mesh -n "pCube7Shape" -p "pCube7";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".pv" -type "double2" 0.81172838807106018 0.43727175891399384 ;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 49 ".pt";
	setAttr ".pt[156]" -type "float3" -0.071004592 2.1538327e-014 -5.3845817e-015 ;
	setAttr ".pt[157]" -type "float3" -0.071004592 2.1538327e-014 2.1538327e-014 ;
	setAttr ".pt[158]" -type "float3" -0.071004592 8.6153307e-014 -5.3845817e-015 ;
	setAttr ".pt[159]" -type "float3" -0.071004592 8.6153307e-014 2.1538327e-014 ;
	setAttr ".pt[160]" -type "float3" 0.070397601 8.6153307e-014 2.1538327e-014 ;
	setAttr ".pt[161]" -type "float3" 0.070397601 8.6153307e-014 -5.3845817e-015 ;
	setAttr ".pt[162]" -type "float3" 0.070397601 2.1538327e-014 -5.3845817e-015 ;
	setAttr ".pt[163]" -type "float3" 0.070397601 2.1538327e-014 2.1538327e-014 ;
	setAttr ".pt[164]" -type "float3" 0.070397601 8.6153307e-014 2.1538327e-014 ;
	setAttr ".pt[165]" -type "float3" 0.070397601 8.6153307e-014 -5.3845817e-015 ;
	setAttr ".pt[166]" -type "float3" 0.070397601 8.6153307e-014 -5.3845817e-015 ;
	setAttr ".pt[167]" -type "float3" 0.070397601 8.6153307e-014 2.1538327e-014 ;
	setAttr ".pt[168]" -type "float3" 0.070397601 1.7230661e-013 2.1538327e-014 ;
	setAttr ".pt[169]" -type "float3" 0.070397601 1.7230661e-013 -5.3845817e-015 ;
	setAttr ".pt[170]" -type "float3" 0.070397601 1.7230661e-013 -5.3845817e-015 ;
	setAttr ".pt[171]" -type "float3" 0.070397601 1.7230661e-013 2.1538327e-014 ;
	setAttr ".pt[172]" -type "float3" -0.071004592 1.7230661e-013 -5.3845817e-015 ;
	setAttr ".pt[173]" -type "float3" -0.071004592 1.7230661e-013 2.1538327e-014 ;
	setAttr ".pt[174]" -type "float3" -0.071004592 1.7230661e-013 -5.3845817e-015 ;
	setAttr ".pt[175]" -type "float3" -0.071004592 1.7230661e-013 2.1538327e-014 ;
	setAttr ".pt[176]" -type "float3" -0.071004592 8.6153307e-014 2.1538327e-014 ;
	setAttr ".pt[177]" -type "float3" -0.071004592 8.6153307e-014 2.1538327e-014 ;
	setAttr ".pt[178]" -type "float3" -0.071004592 8.6153307e-014 -5.3845817e-015 ;
	setAttr ".pt[179]" -type "float3" -0.071004592 8.6153307e-014 -5.3845817e-015 ;
	setAttr ".pt[180]" -type "float3" -0.071004592 2.1538327e-014 4.3076653e-014 ;
	setAttr ".pt[181]" -type "float3" -0.071004592 2.1538327e-014 8.6153307e-014 ;
	setAttr ".pt[182]" -type "float3" -0.071004592 8.6153307e-014 4.3076653e-014 ;
	setAttr ".pt[183]" -type "float3" -0.071004592 8.6153307e-014 8.6153307e-014 ;
	setAttr ".pt[184]" -type "float3" 0.070397601 8.6153307e-014 8.6153307e-014 ;
	setAttr ".pt[185]" -type "float3" 0.070397601 8.6153307e-014 4.3076653e-014 ;
	setAttr ".pt[186]" -type "float3" 0.070397601 2.1538327e-014 4.3076653e-014 ;
	setAttr ".pt[187]" -type "float3" 0.070397601 2.1538327e-014 8.6153307e-014 ;
	setAttr ".pt[188]" -type "float3" 0.070397601 8.6153307e-014 8.6153307e-014 ;
	setAttr ".pt[189]" -type "float3" 0.070397601 8.6153307e-014 4.3076653e-014 ;
	setAttr ".pt[190]" -type "float3" 0.070397601 8.6153307e-014 4.3076653e-014 ;
	setAttr ".pt[191]" -type "float3" 0.070397601 8.6153307e-014 8.6153307e-014 ;
	setAttr ".pt[192]" -type "float3" 0.070397601 1.7230661e-013 8.6153307e-014 ;
	setAttr ".pt[193]" -type "float3" 0.070397601 1.7230661e-013 4.3076653e-014 ;
	setAttr ".pt[194]" -type "float3" 0.070397601 1.7230661e-013 4.3076653e-014 ;
	setAttr ".pt[195]" -type "float3" 0.070397601 1.7230661e-013 8.6153307e-014 ;
	setAttr ".pt[196]" -type "float3" -0.071004592 1.7230661e-013 4.3076653e-014 ;
	setAttr ".pt[197]" -type "float3" -0.071004592 1.7230661e-013 8.6153307e-014 ;
	setAttr ".pt[198]" -type "float3" -0.071004592 1.7230661e-013 4.3076653e-014 ;
	setAttr ".pt[199]" -type "float3" -0.071004592 1.7230661e-013 8.6153307e-014 ;
	setAttr ".pt[200]" -type "float3" -0.071004592 8.6153307e-014 8.6153307e-014 ;
	setAttr ".pt[201]" -type "float3" -0.071004592 8.6153307e-014 8.6153307e-014 ;
	setAttr ".pt[202]" -type "float3" -0.071004592 8.6153307e-014 4.3076653e-014 ;
	setAttr ".pt[203]" -type "float3" -0.071004592 8.6153307e-014 4.3076653e-014 ;
createNode mesh -n "polySurfaceShape11" -p "pCube7";
	setAttr -k off ".v";
	setAttr ".io" yes;
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".pv" -type "double2" 0.79852968454360962 0.42410871386528015 ;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 94 ".uvst[0].uvsp[0:93]" -type "float2" 0.63431728 0.26489204
		 0.79852974 0.26489204 0.79852974 0.58687466 0.6343174 0.58687478 0.98913956 0.60543704
		 0.82492721 0.60543704 0.82492721 0.58687466 0.98913956 0.58687466 0.82492721 0.26489204
		 0.98913944 0.26489204 0.82492721 0.24278051 0.98913944 0.24278039 0.60791981 0.26489204
		 0.60791993 0.58687478 0.17557576 0.48343134 0.17557582 0.49332017 0.041567713 0.49332017
		 0.041567668 0.48343128 0.17557576 0.43691421 0.18546459 0.42330295 0.18546465 0.48343134
		 0.031678841 0.42330295 0.041567624 0.43691421 0.031678841 0.48343134 0.17311919 0.55918121
		 0.1730206 0.56617516 0.03895326 0.56517035 0.03905189 0.55817652 0.17289248 0.57526773
		 0.038825095 0.57426298 0.17276421 0.58436042 0.038696915 0.58335572 0.17263606 0.59345293
		 0.038568705 0.59244817 0.17250785 0.60254532 0.038440526 0.60154068 0.17834055 0.56798697
		 0.18532309 0.56757683 0.18682039 0.58001107 0.17800528 0.59179151 0.18661314 0.59472328
		 0.19530037 0.59203517 0.20074844 0.58475441 0.2008763 0.57566208 0.19563547 0.56823081
		 0.026616141 0.56622475 0.033584416 0.56683153 0.024768919 0.57861179 0.033248425
		 0.59063607 0.024561375 0.59332407 0.015953451 0.59039217 0.010712743 0.58296061 0.010841176
		 0.57386839 0.016289502 0.56658769 0.17557576 0.48343134 0.17557582 0.49332017 0.041567713
		 0.49332017 0.041567668 0.48343128 0.17557576 0.43691421 0.18546459 0.42330295 0.18546465
		 0.48343134 0.031678841 0.42330295 0.041567624 0.43691421 0.031678841 0.48343134 0.17311919
		 0.55918121 0.1730206 0.56617516 0.03895326 0.56517035 0.03905189 0.55817652 0.17289248
		 0.57526773 0.038825095 0.57426298 0.17276421 0.58436042 0.038696915 0.58335572 0.17263606
		 0.59345293 0.038568705 0.59244817 0.17250785 0.60254532 0.038440526 0.60154068 0.17834055
		 0.56798697 0.18532309 0.56757683 0.18682039 0.58001107 0.17800528 0.59179151 0.18661314
		 0.59472328 0.19530037 0.59203517 0.20074844 0.58475441 0.2008763 0.57566208 0.19563547
		 0.56823081 0.026616141 0.56622475 0.033584416 0.56683153 0.024768919 0.57861179 0.033248425
		 0.59063607 0.024561375 0.59332407 0.015953451 0.59039217 0.010712743 0.58296061 0.010841176
		 0.57386839 0.016289502 0.56658769;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 68 ".vt[0:67]"  2.2057209 0.0023722649 3.75230742 2.2057209 0.0023722649 -0.71189594
		 2.2057209 8.16412926 3.75230742 2.2057209 8.16412926 -0.71189594 1.99352884 8.16412926 3.75230742
		 1.99352884 8.16412926 -0.71189594 1.99352884 0.0023722649 3.75230742 1.99352884 0.0023722649 -0.71189594
		 2.067747593 6.21105862 -0.7226935 2.067747593 6.21105862 -0.71189594 2.067747593 6.92490721 -0.71189594
		 2.067747593 6.92490721 -0.7226935 2.15195084 6.21105862 -0.7226935 2.1668129 6.21105862 -0.71189594
		 2.1668129 6.92490721 -0.71189594 2.15195084 6.92490721 -0.7226935 2.19429755 6.21105862 -0.75763553
		 2.18518305 6.21105862 -0.76843315 2.1668129 6.21105862 -0.76843315 2.15195084 6.21105862 -0.75763553
		 2.14627504 6.21105862 -0.74016458 2.15195084 6.21105862 -0.7226935 2.1668129 6.21105862 -0.71189594
		 2.18518305 6.21105862 -0.71189594 2.20004416 6.21105862 -0.7226935 2.2057209 6.21105862 -0.74016458
		 2.19429755 6.92490721 -0.75763553 2.18518305 6.92490721 -0.76843315 2.1668129 6.92490721 -0.76843315
		 2.15195084 6.92490721 -0.75763553 2.14627504 6.92490721 -0.74016458 2.15195084 6.92490721 -0.7226935
		 2.1668129 6.92490721 -0.71189594 2.18518305 6.92490721 -0.71189594 2.20004416 6.92490721 -0.7226935
		 2.2057209 6.92490721 -0.74016458 2.17599797 6.21105862 -0.74016458 2.17599797 6.92490721 -0.74016458
		 2.067747593 1.18325639 -0.7226935 2.067747593 1.18325639 -0.71189594 2.067747593 1.89710486 -0.71189594
		 2.067747593 1.89710486 -0.7226935 2.15195084 1.18325639 -0.7226935 2.1668129 1.18325639 -0.71189594
		 2.1668129 1.89710486 -0.71189594 2.15195084 1.89710486 -0.7226935 2.19429755 1.18325639 -0.75763553
		 2.18518305 1.18325639 -0.76843315 2.1668129 1.18325639 -0.76843315 2.15195084 1.18325639 -0.75763553
		 2.14627504 1.18325639 -0.74016458 2.15195084 1.18325639 -0.7226935 2.1668129 1.18325639 -0.71189594
		 2.18518305 1.18325639 -0.71189594 2.20004416 1.18325639 -0.7226935 2.2057209 1.18325639 -0.74016458
		 2.19429755 1.89710486 -0.75763553 2.18518305 1.89710486 -0.76843315 2.1668129 1.89710486 -0.76843315
		 2.15195084 1.89710486 -0.75763553 2.14627504 1.89710486 -0.74016458 2.15195084 1.89710486 -0.7226935
		 2.1668129 1.89710486 -0.71189594 2.18518305 1.89710486 -0.71189594 2.20004416 1.89710486 -0.7226935
		 2.2057209 1.89710486 -0.74016458 2.17599797 1.18325639 -0.74016458 2.17599797 1.89710486 -0.74016458;
	setAttr -s 126 ".ed[0:125]"  0 1 0 2 3 0 4 5 0 6 7 0 0 2 0 1 3 0 2 4 0
		 3 5 0 4 6 0 5 7 0 6 0 0 7 1 0 12 8 0 13 9 0 8 9 0 14 10 0 9 10 0 15 11 0 11 10 0
		 8 11 0 12 13 0 15 14 0 12 15 0 16 17 0 17 18 0 18 19 0 19 20 0 20 21 0 21 22 0 22 23 0
		 23 24 0 24 25 0 25 16 0 26 27 0 27 28 0 28 29 0 29 30 0 30 31 0 31 32 0 32 33 0 33 34 0
		 34 35 0 35 26 0 16 26 0 17 27 1 18 28 1 19 29 1 20 30 1 21 31 0 36 16 0 36 17 0 36 18 0
		 36 19 0 36 20 0 36 21 0 36 22 0 36 23 0 36 24 0 36 25 0 26 37 0 27 37 0 28 37 0 29 37 0
		 30 37 0 31 37 0 32 37 0 33 37 0 34 37 0 35 37 0 42 38 0 43 39 0 38 39 0 44 40 0 39 40 0
		 45 41 0 41 40 0 38 41 0 42 43 0 45 44 0 42 45 0 46 47 0 47 48 0 48 49 0 49 50 0 50 51 0
		 51 52 0 52 53 0 53 54 0 54 55 0 55 46 0 56 57 0 57 58 0 58 59 0 59 60 0 60 61 0 61 62 0
		 62 63 0 63 64 0 64 65 0 65 56 0 46 56 0 47 57 1 48 58 1 49 59 1 50 60 1 51 61 0 66 46 0
		 66 47 0 66 48 0 66 49 0 66 50 0 66 51 0 66 52 0 66 53 0 66 54 0 66 55 0 56 67 0 57 67 0
		 58 67 0 59 67 0 60 67 0 61 67 0 62 67 0 63 67 0 64 67 0 65 67 0;
	setAttr -s 64 -ch 216 ".fc[0:63]" -type "polyFaces" 
		f 4 0 5 -2 -5
		mu 0 4 0 1 2 3
		f 4 1 7 -3 -7
		mu 0 4 4 5 6 7
		f 4 2 9 -4 -9
		mu 0 4 7 6 8 9
		f 4 3 11 -1 -11
		mu 0 4 9 8 10 11
		f 4 -12 -10 -8 -6
		mu 0 4 1 8 6 2
		f 4 10 4 6 8
		mu 0 4 12 0 3 13
		f 4 14 16 -19 -20
		mu 0 4 14 15 16 17
		f 4 20 13 -15 -13
		mu 0 4 18 19 20 14
		f 4 -22 17 18 -16
		mu 0 4 21 22 17 23
		f 4 -23 12 19 -18
		mu 0 4 22 18 14 17
		f 4 23 44 -34 -44
		mu 0 4 24 25 26 27
		f 4 24 45 -35 -45
		mu 0 4 25 28 29 26
		f 4 25 46 -36 -46
		mu 0 4 28 30 31 29
		f 4 26 47 -37 -47
		mu 0 4 30 32 33 31
		f 4 27 48 -38 -48
		mu 0 4 32 34 35 33
		f 3 -24 -50 50
		mu 0 3 36 37 38
		f 3 -25 -51 51
		mu 0 3 28 36 38
		f 3 -26 -52 52
		mu 0 3 30 28 38
		f 3 -27 -53 53
		mu 0 3 39 30 38
		f 3 -28 -54 54
		mu 0 3 40 39 38
		f 3 -29 -55 55
		mu 0 3 41 40 38
		f 3 -30 -56 56
		mu 0 3 42 41 38
		f 3 -31 -57 57
		mu 0 3 43 42 38
		f 3 -32 -58 58
		mu 0 3 44 43 38
		f 3 -33 -59 49
		mu 0 3 37 44 38
		f 3 33 60 -60
		mu 0 3 45 46 47
		f 3 34 61 -61
		mu 0 3 46 29 47
		f 3 35 62 -62
		mu 0 3 29 31 47
		f 3 36 63 -63
		mu 0 3 31 48 47
		f 3 37 64 -64
		mu 0 3 48 49 47
		f 3 38 65 -65
		mu 0 3 49 50 47
		f 3 39 66 -66
		mu 0 3 50 51 47
		f 3 40 67 -67
		mu 0 3 51 52 47
		f 3 41 68 -68
		mu 0 3 52 53 47
		f 3 42 59 -69
		mu 0 3 53 45 47
		f 4 71 73 -76 -77
		mu 0 4 54 55 56 57
		f 4 77 70 -72 -70
		mu 0 4 58 59 60 54
		f 4 -79 74 75 -73
		mu 0 4 61 62 57 63
		f 4 -80 69 76 -75
		mu 0 4 62 58 54 57
		f 4 80 101 -91 -101
		mu 0 4 64 65 66 67
		f 4 81 102 -92 -102
		mu 0 4 65 68 69 66
		f 4 82 103 -93 -103
		mu 0 4 68 70 71 69
		f 4 83 104 -94 -104
		mu 0 4 70 72 73 71
		f 4 84 105 -95 -105
		mu 0 4 72 74 75 73
		f 3 -81 -107 107
		mu 0 3 76 77 78
		f 3 -82 -108 108
		mu 0 3 68 76 78
		f 3 -83 -109 109
		mu 0 3 70 68 78
		f 3 -84 -110 110
		mu 0 3 79 70 78
		f 3 -85 -111 111
		mu 0 3 80 79 78
		f 3 -86 -112 112
		mu 0 3 81 80 78
		f 3 -87 -113 113
		mu 0 3 82 81 78
		f 3 -88 -114 114
		mu 0 3 83 82 78
		f 3 -89 -115 115
		mu 0 3 84 83 78
		f 3 -90 -116 106
		mu 0 3 77 84 78
		f 3 90 117 -117
		mu 0 3 85 86 87
		f 3 91 118 -118
		mu 0 3 86 69 87
		f 3 92 119 -119
		mu 0 3 69 71 87
		f 3 93 120 -120
		mu 0 3 71 88 87
		f 3 94 121 -121
		mu 0 3 88 89 87
		f 3 95 122 -122
		mu 0 3 89 90 87
		f 3 96 123 -123
		mu 0 3 90 91 87
		f 3 97 124 -124
		mu 0 3 91 92 87
		f 3 98 125 -125
		mu 0 3 92 93 87
		f 3 99 116 -126
		mu 0 3 93 85 87;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
createNode transform -n "polySurface7";
createNode mesh -n "polySurface7Shape" -p "|polySurface7";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".pv" -type "double2" 0.49877576529979706 0.61929357051849365 ;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 50 ".uvst[0].uvsp[0:49]" -type "float2" 0.039040044 0.55626535
		 0.039040044 0.54580599 0.173013 0.54580599 0.173013 0.55626529 0.028580666 0.49057281
		 0.039040044 0.50656915 0.18347242 0.54580587 0.028580636 0.54580587 0.18347242 0.49057281
		 0.173013 0.50656915 0.039040044 0.55626535 0.039040044 0.54580599 0.173013 0.54580599
		 0.173013 0.55626529 0.028580666 0.49057281 0.039040044 0.50656915 0.028580636 0.54580587
		 0.173013 0.50656915 0.18347242 0.49057281 0.18347242 0.54580587 0.19700733 0.61919159
		 0.007304877 0.61919165 0.0073054135 0.99218166 0.19700775 0.99218136 0.21407971 0.99218136
		 0.40378192 0.9921813 0.40378186 0.61919159 0.21407941 0.61919159 0.42085373 0.61919159
		 0.42085382 0.9921813 0.59350663 0.93602693 0.59350652 0.99317884 0.4037593 0.99317884
		 0.40375918 0.93602705 0.4037593 0.99317884 0.40375918 0.93602705 0.59350663 0.93602693
		 0.59350652 0.99317884 0.59350652 0.91773534 0.40375924 0.91773534 0.99024665 0.61939555
		 0.80054426 0.61939555 0.80054402 0.99408233 0.99024642 0.99408239 0.59376985 0.99408215
		 0.78347206 0.99408233 0.78347242 0.61939555 0.59377009 0.61939555 0.57669818 0.61939555
		 0.57669777 0.99408215;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 40 ".vt[0:39]"  2.2057209 6.21105862 -0.91624111 2.19429755 6.21105862 -0.91624123
		 2.19429755 6.92490721 -0.91624123 2.2057209 6.92490721 -0.91624111 2.2057209 6.21105862 -0.74016458
		 2.19429755 6.21105862 -0.75763553 2.19429755 6.92490721 -0.75763553 2.2057209 6.92490721 -0.74016458
		 2.2057209 1.18325639 -0.91624111 2.19429755 1.18325639 -0.91624123 2.19429755 1.89710486 -0.91624123
		 2.2057209 1.89710486 -0.91624111 2.2057209 1.18325639 -0.74016458 2.19429755 1.18325639 -0.75763553
		 2.19429755 1.89710486 -0.75763553 2.2057209 1.89710486 -0.74016458 -7.053760529 0.0023715496 -0.59897059
		 -2.25848269 0.0023715496 -0.59897059 -7.053760529 9.54111481 -0.59897059 -2.25848269 9.54111481 -0.59897059
		 -7.053760529 9.54111481 -1.030514717 -2.25848269 9.54111481 -1.030514717 -7.053760529 0.0023715496 -1.030514717
		 -2.25848269 0.0023715496 -1.030514717 2.20572066 8.16412926 -0.59897059 2.20572066 9.54111481 -0.59897059
		 -2.25848269 8.16412926 -0.59897059 -2.25848269 9.54111481 -0.59897059 -2.25848269 8.16412926 -1.030514717
		 -2.25848269 9.54111481 -1.030514717 2.20572066 8.16412926 -1.030514717 2.20572066 9.54111481 -1.030514717
		 2.20572066 0.0023715496 -0.59897131 7.00099849701 0.0023715496 -0.59897131 2.20572066 9.54112053 -0.59897012
		 7.00099849701 9.54112053 -0.59897012 2.20572066 9.54111195 -1.030514359 7.00099849701 9.54111195 -1.030514359
		 2.20572066 0.0023701191 -1.030514002 7.00099849701 0.0023701191 -1.030514002;
	setAttr -s 56 ".ed[0:55]"  4 0 0 5 1 0 0 1 0 6 2 0 1 2 0 7 3 0 3 2 0
		 0 3 0 4 5 0 5 6 0 7 6 0 12 8 0 13 9 0 8 9 0 14 10 0 9 10 0 15 11 0 11 10 0 8 11 0
		 12 13 0 13 14 0 15 14 0 16 17 0 18 19 0 20 21 0 22 23 0 16 18 0 17 19 0 18 20 0 19 21 0
		 20 22 0 21 23 0 22 16 0 23 17 0 24 25 0 26 27 0 28 29 0 30 31 0 24 26 0 25 27 0 26 28 0
		 28 30 0 29 31 0 30 24 0 32 33 0 34 35 0 36 37 0 38 39 0 32 34 0 33 35 0 34 36 0 35 37 0
		 36 38 0 37 39 0 38 32 0 39 33 0;
	setAttr -s 19 -ch 76 ".fc[0:18]" -type "polyFaces" 
		f 4 2 4 -7 -8
		mu 0 4 0 1 2 3
		f 4 8 1 -3 -1
		mu 0 4 4 5 1 7
		f 4 9 3 -5 -2
		mu 0 4 5 9 2 1
		f 4 -11 5 6 -4
		mu 0 4 9 8 6 2
		f 4 13 15 -18 -19
		mu 0 4 10 11 12 13
		f 4 19 12 -14 -12
		mu 0 4 14 15 11 16
		f 4 20 14 -16 -13
		mu 0 4 15 17 12 11
		f 4 -22 16 17 -15
		mu 0 4 17 18 19 12
		f 4 22 27 -24 -27
		mu 0 4 20 21 22 23
		f 4 24 31 -26 -31
		mu 0 4 24 25 26 27
		f 4 -34 -32 -30 -28
		mu 0 4 28 26 25 29
		f 4 32 26 28 30
		mu 0 4 27 20 23 24
		f 4 34 39 -36 -39
		mu 0 4 30 31 32 33
		f 4 36 42 -38 -42
		mu 0 4 34 35 36 37
		f 4 43 38 40 41
		mu 0 4 38 30 33 39
		f 4 44 49 -46 -49
		mu 0 4 40 41 42 43
		f 4 46 53 -48 -53
		mu 0 4 44 45 46 47
		f 4 -56 -54 -52 -50
		mu 0 4 41 46 45 42
		f 4 54 48 50 52
		mu 0 4 47 48 49 44;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
createNode polyExtrudeFace -n "polyExtrudeFace1";
	setAttr ".ics" -type "componentList" 12 "f[89]" "f[93]" "f[95]" "f[97]" "f[101]" "f[103]" "f[121]" "f[125]" "f[127]" "f[129]" "f[133]" "f[135]";
	setAttr ".ix" -type "matrix" 2.2204460492503131e-016 0 1 0 -0 1 0 0 -1 -0 2.2204460492503131e-016 0
		 1.4938249588012691 0 -2.9176168441772461 1;
	setAttr ".ws" yes;
	setAttr ".pvt" -type "float3" -0.023761064 4.3719277 -0.81799197 ;
	setAttr ".rs" 54664;
	setAttr ".c[0]"  0 1 1;
	setAttr ".cbn" -type "double3" -1.8130245208740234 1.0064889192581177 -0.9240880012512207 ;
	setAttr ".cbx" -type "double3" 1.765502393245697 7.7373661994934082 -0.71189594268798739 ;
createNode polySplitRing -n "polySplitRing10";
	setAttr ".uopa" yes;
	setAttr ".ics" -type "componentList" 12 "e[0:3]" "e[132]" "e[140]" "e[148]" "e[156]" "e[164]" "e[172]" "e[238:239]" "e[241]" "e[263]" "e[265]" "e[267]";
	setAttr ".ix" -type "matrix" 2.2204460492503131e-016 0 1 0 -0 1 0 0 -1 -0 2.2204460492503131e-016 0
		 1.4938249588012691 0 -2.9176168441772461 1;
	setAttr ".wt" 0.77877283096313477;
	setAttr ".dr" no;
	setAttr ".re" 238;
	setAttr ".sma" 29.999999999999996;
	setAttr ".p[0]"  0 0 1;
	setAttr ".fq" yes;
createNode polySplitRing -n "polySplitRing9";
	setAttr ".uopa" yes;
	setAttr ".ics" -type "componentList" 12 "e[0:3]" "e[132]" "e[140]" "e[148]" "e[156]" "e[164]" "e[172]" "e[206:207]" "e[209]" "e[231]" "e[233]" "e[235]";
	setAttr ".ix" -type "matrix" 2.2204460492503131e-016 0 1 0 -0 1 0 0 -1 -0 2.2204460492503131e-016 0
		 1.4938249588012691 0 -2.9176168441772461 1;
	setAttr ".wt" 0.18009522557258606;
	setAttr ".re" 206;
	setAttr ".sma" 29.999999999999996;
	setAttr ".p[0]"  0 0 1;
	setAttr ".fq" yes;
createNode polySplitRing -n "polySplitRing8";
	setAttr ".uopa" yes;
	setAttr ".ics" -type "componentList" 12 "e[0:3]" "e[132]" "e[140]" "e[148]" "e[156]" "e[164]" "e[172]" "e[174:175]" "e[177]" "e[199]" "e[201]" "e[203]";
	setAttr ".ix" -type "matrix" 2.2204460492503131e-016 0 1 0 -0 1 0 0 -1 -0 2.2204460492503131e-016 0
		 1.4938249588012691 0 -2.9176168441772461 1;
	setAttr ".wt" 0.38969296216964722;
	setAttr ".re" 174;
	setAttr ".sma" 29.999999999999996;
	setAttr ".p[0]"  0 0 1;
	setAttr ".fq" yes;
createNode polySplitRing -n "polySplitRing7";
	setAttr ".uopa" yes;
	setAttr ".ics" -type "componentList" 13 "e[0:3]" "e[128]" "e[132]" "e[136]" "e[140]" "e[144]" "e[148]" "e[152]" "e[156]" "e[160]" "e[164]" "e[168]" "e[172]";
	setAttr ".ix" -type "matrix" 2.2204460492503131e-016 0 1 0 -0 1 0 0 -1 -0 2.2204460492503131e-016 0
		 1.4938249588012691 0 -2.9176168441772461 1;
	setAttr ".wt" 0.098610781133174896;
	setAttr ".re" 152;
	setAttr ".sma" 29.999999999999996;
	setAttr ".p[0]"  0 0 1;
	setAttr ".fq" yes;
createNode polySplitRing -n "polySplitRing6";
	setAttr ".uopa" yes;
	setAttr ".ics" -type "componentList" 3 "e[4:5]" "e[161]" "e[163]";
	setAttr ".ix" -type "matrix" 2.2204460492503131e-016 0 1 0 -0 1 0 0 -1 -0 2.2204460492503131e-016 0
		 1.4938249588012691 0 -2.9176168441772461 1;
	setAttr ".wt" 0.32951590418815613;
	setAttr ".re" 5;
	setAttr ".sma" 29.999999999999996;
	setAttr ".p[0]"  0 0 1;
	setAttr ".fq" yes;
createNode polySplitRing -n "polySplitRing5";
	setAttr ".uopa" yes;
	setAttr ".ics" -type "componentList" 3 "e[4:5]" "e[153]" "e[155]";
	setAttr ".ix" -type "matrix" 2.2204460492503131e-016 0 1 0 -0 1 0 0 -1 -0 2.2204460492503131e-016 0
		 1.4938249588012691 0 -2.9176168441772461 1;
	setAttr ".wt" 0.78580522537231445;
	setAttr ".dr" no;
	setAttr ".re" 5;
	setAttr ".sma" 29.999999999999996;
	setAttr ".p[0]"  0 0 1;
	setAttr ".fq" yes;
createNode polySplitRing -n "polySplitRing4";
	setAttr ".uopa" yes;
	setAttr ".ics" -type "componentList" 3 "e[4:5]" "e[145]" "e[147]";
	setAttr ".ix" -type "matrix" 2.2204460492503131e-016 0 1 0 -0 1 0 0 -1 -0 2.2204460492503131e-016 0
		 1.4938249588012691 0 -2.9176168441772461 1;
	setAttr ".wt" 0.69174295663833618;
	setAttr ".dr" no;
	setAttr ".re" 5;
	setAttr ".sma" 29.999999999999996;
	setAttr ".p[0]"  0 0 1;
	setAttr ".fq" yes;
createNode polySplitRing -n "polySplitRing3";
	setAttr ".uopa" yes;
	setAttr ".ics" -type "componentList" 3 "e[4:5]" "e[137]" "e[139]";
	setAttr ".ix" -type "matrix" 2.2204460492503131e-016 0 1 0 -0 1 0 0 -1 -0 2.2204460492503131e-016 0
		 1.4938249588012691 0 -2.9176168441772461 1;
	setAttr ".wt" 0.92881685495376587;
	setAttr ".dr" no;
	setAttr ".re" 5;
	setAttr ".sma" 29.999999999999996;
	setAttr ".p[0]"  0 0 1;
	setAttr ".fq" yes;
createNode polySplitRing -n "polySplitRing2";
	setAttr ".uopa" yes;
	setAttr ".ics" -type "componentList" 3 "e[4:5]" "e[129]" "e[131]";
	setAttr ".ix" -type "matrix" 2.2204460492503131e-016 0 1 0 -0 1 0 0 -1 -0 2.2204460492503131e-016 0
		 1.4938249588012691 0 -2.9176168441772461 1;
	setAttr ".wt" 0.78029364347457886;
	setAttr ".dr" no;
	setAttr ".re" 5;
	setAttr ".sma" 29.999999999999996;
	setAttr ".p[0]"  0 0 1;
	setAttr ".fq" yes;
createNode polySplitRing -n "polySplitRing1";
	setAttr ".uopa" yes;
	setAttr ".ics" -type "componentList" 2 "e[4:5]" "e[8:9]";
	setAttr ".ix" -type "matrix" 2.2204460492503131e-016 0 1 0 -0 1 0 0 -1 -0 2.2204460492503131e-016 0
		 1.4938249588012691 0 -2.9176168441772461 1;
	setAttr ".wt" 0.94771188497543335;
	setAttr ".dr" no;
	setAttr ".re" 5;
	setAttr ".sma" 29.999999999999996;
	setAttr ".p[0]"  0 0 1;
	setAttr ".fq" yes;
createNode materialInfo -n "materialInfo1";
createNode shadingEngine -n "lambert2SG";
	setAttr ".ihi" 0;
	setAttr -s 2 ".dsm";
	setAttr ".ro" yes;
createNode lambert -n "WallDoor";
createNode file -n "file1";
	setAttr ".ftn" -type "string" "C:/Users/LL/Desktop/DoorWall.jpg";
createNode place2dTexture -n "place2dTexture1";
createNode lightLinker -s -n "lightLinker1";
	setAttr -s 3 ".lnk";
	setAttr -s 3 ".slnk";
select -ne :time1;
	setAttr ".o" 1;
	setAttr ".unw" 1;
select -ne :renderPartition;
	setAttr -s 3 ".st";
select -ne :renderGlobalsList1;
select -ne :defaultShaderList1;
	setAttr -s 3 ".s";
select -ne :postProcessList1;
	setAttr -s 2 ".p";
select -ne :defaultRenderUtilityList1;
select -ne :defaultRenderingList1;
select -ne :defaultTextureList1;
select -ne :initialShadingGroup;
	setAttr ".ro" yes;
select -ne :initialParticleSE;
	setAttr ".ro" yes;
select -ne :defaultResolution;
	setAttr ".pa" 1;
select -ne :hardwareRenderGlobals;
	setAttr ".ctrs" 256;
	setAttr ".btrs" 512;
select -ne :hardwareRenderingGlobals;
	setAttr ".otfna" -type "stringArray" 22 "NURBS Curves" "NURBS Surfaces" "Polygons" "Subdiv Surface" "Particles" "Particle Instance" "Fluids" "Strokes" "Image Planes" "UI" "Lights" "Cameras" "Locators" "Joints" "IK Handles" "Deformers" "Motion Trails" "Components" "Hair Systems" "Follicles" "Misc. UI" "Ornaments"  ;
	setAttr ".otfva" -type "Int32Array" 22 0 1 1 1 1 1
		 1 1 1 0 0 0 0 0 0 0 0 0
		 0 0 0 0 ;
select -ne :defaultHardwareRenderGlobals;
	setAttr ".res" -type "string" "ntsc_4d 646 485 1.333";
select -ne :ikSystem;
	setAttr -s 4 ".sol";
connectAttr "polyExtrudeFace1.out" "pCube7Shape.i";
connectAttr "polySplitRing10.out" "polyExtrudeFace1.ip";
connectAttr "pCube7Shape.wm" "polyExtrudeFace1.mp";
connectAttr "polySplitRing9.out" "polySplitRing10.ip";
connectAttr "pCube7Shape.wm" "polySplitRing10.mp";
connectAttr "polySplitRing8.out" "polySplitRing9.ip";
connectAttr "pCube7Shape.wm" "polySplitRing9.mp";
connectAttr "polySplitRing7.out" "polySplitRing8.ip";
connectAttr "pCube7Shape.wm" "polySplitRing8.mp";
connectAttr "polySplitRing6.out" "polySplitRing7.ip";
connectAttr "pCube7Shape.wm" "polySplitRing7.mp";
connectAttr "polySplitRing5.out" "polySplitRing6.ip";
connectAttr "pCube7Shape.wm" "polySplitRing6.mp";
connectAttr "polySplitRing4.out" "polySplitRing5.ip";
connectAttr "pCube7Shape.wm" "polySplitRing5.mp";
connectAttr "polySplitRing3.out" "polySplitRing4.ip";
connectAttr "pCube7Shape.wm" "polySplitRing4.mp";
connectAttr "polySplitRing2.out" "polySplitRing3.ip";
connectAttr "pCube7Shape.wm" "polySplitRing3.mp";
connectAttr "polySplitRing1.out" "polySplitRing2.ip";
connectAttr "pCube7Shape.wm" "polySplitRing2.mp";
connectAttr "polySurfaceShape11.o" "polySplitRing1.ip";
connectAttr "pCube7Shape.wm" "polySplitRing1.mp";
connectAttr "lambert2SG.msg" "materialInfo1.sg";
connectAttr "WallDoor.msg" "materialInfo1.m";
connectAttr "file1.msg" "materialInfo1.t" -na;
connectAttr "WallDoor.oc" "lambert2SG.ss";
connectAttr "polySurface7Shape.iog" "lambert2SG.dsm" -na;
connectAttr "pCube7Shape.iog" "lambert2SG.dsm" -na;
connectAttr "file1.oc" "WallDoor.c";
connectAttr "place2dTexture1.c" "file1.c";
connectAttr "place2dTexture1.tf" "file1.tf";
connectAttr "place2dTexture1.rf" "file1.rf";
connectAttr "place2dTexture1.mu" "file1.mu";
connectAttr "place2dTexture1.mv" "file1.mv";
connectAttr "place2dTexture1.s" "file1.s";
connectAttr "place2dTexture1.wu" "file1.wu";
connectAttr "place2dTexture1.wv" "file1.wv";
connectAttr "place2dTexture1.re" "file1.re";
connectAttr "place2dTexture1.of" "file1.of";
connectAttr "place2dTexture1.r" "file1.ro";
connectAttr "place2dTexture1.n" "file1.n";
connectAttr "place2dTexture1.vt1" "file1.vt1";
connectAttr "place2dTexture1.vt2" "file1.vt2";
connectAttr "place2dTexture1.vt3" "file1.vt3";
connectAttr "place2dTexture1.vc1" "file1.vc1";
connectAttr "place2dTexture1.o" "file1.uv";
connectAttr "place2dTexture1.ofs" "file1.fs";
relationship "link" ":lightLinker1" ":initialShadingGroup.message" ":defaultLightSet.message";
relationship "link" ":lightLinker1" ":initialParticleSE.message" ":defaultLightSet.message";
relationship "link" ":lightLinker1" "lambert2SG.message" ":defaultLightSet.message";
relationship "shadowLink" ":lightLinker1" ":initialShadingGroup.message" ":defaultLightSet.message";
relationship "shadowLink" ":lightLinker1" ":initialParticleSE.message" ":defaultLightSet.message";
relationship "shadowLink" ":lightLinker1" "lambert2SG.message" ":defaultLightSet.message";
connectAttr "lambert2SG.pa" ":renderPartition.st" -na;
connectAttr "WallDoor.msg" ":defaultShaderList1.s" -na;
connectAttr "place2dTexture1.msg" ":defaultRenderUtilityList1.u" -na;
connectAttr "file1.msg" ":defaultTextureList1.tx" -na;
dataStructure -fmt "raw" -as "name=externalContentTable:string=node:string=key:string=upath:uint32=upathcrc:string=rpath:string=roles";
applyMetadata -fmt "raw" -v "channel\nname externalContentTable\nstream\nname v1.0\nindexType numeric\nstructure externalContentTable\n0\n\"file1\" \"fileTextureName\" \"C:/Users/LL/Desktop/DoorWall.jpg\" 2905309849 \"C:/Users/LL/Desktop/DoorWall.jpg\" \"sourceImages\"\nendStream\nendChannel\nendAssociations\n" 
		-scn;
// End of DoorWall.ma
