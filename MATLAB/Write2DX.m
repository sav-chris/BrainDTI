function Write2DX(AllPoints, QuadBase, numberOfEllipsoids, AllPointData, RGB)

numberOfPoints = size(AllPoints, 1);
numberOfQuadBase = size(QuadBase, 1);
numberOfColour = size(RGB, 1);


disp(numberOfEllipsoids);
temp = numberOfPoints/numberOfEllipsoids;

fid = fopen('PatientA1.dx', 'w');
fprintf(fid, 'object 1 class array type float rank 1 shape 3 items %d data follows\n', numberOfPoints);
for i=1:numberOfPoints
	fprintf(fid, '%f %f %f\n', AllPoints(i,1), AllPoints(i,2), AllPoints(i,3));
end
fprintf(fid, 'object 2 class array type int rank 1 shape 4 items %d data follows\n', numberOfEllipsoids*numberOfQuadBase);
for i = 1:numberOfEllipsoids
	for j = 1:numberOfQuadBase
		fprintf(fid, '%d %d %d %d\n', QuadBase(j,1)+(i-1)*temp, QuadBase(j,2)+(i-1)*temp, QuadBase(j,3)+(i-1)*temp, QuadBase(j,4)+(i-1)*temp);
	end
end
fprintf(fid, 'attribute "element type" string "quads"\n');
fprintf(fid, 'attribute "ref" string "positions"\n\n');
fprintf(fid, 'object 3 class array type float rank 1 shape 3 items %d data follows\n', numberOfColour);
for i=1:numberOfPoints
	fprintf(fid, '%f %f %f\n', RGB(i,1), RGB(i,2), RGB(i,3));
end
fprintf(fid, 'attribute "dep" string "positions"\n\n');

fprintf(fid, 'object "stuff" class field\n');
fprintf(fid, 'component "positions" value 1\n');
fprintf(fid, 'component "connections" value 2\n');
fprintf(fid, 'component "colors" value 3\n');
fprintf(fid, 'end\n');
fclose(fid);

