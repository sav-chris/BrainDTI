function WriteToVTKFile(Points, Mesh)


numberOfPoints = size(Points, 1);
numberOfCells = size(Mesh, 1);

fid = fopen('OptimisedVTK.vtk','w');

fprintf(fid, '# vtk DataFile Version 3.0\n');
fprintf(fid, 'vtk output\n');
fprintf(fid, 'ASCII\n');
fprintf(fid, 'DATASET UNSTRUCTURED_GRID\n');


%fprintf(fid, 'POINTS %d float\n', numberOfPoints);
%for i=1:numberOfPoints
%	fprintf(fid, '%f %f %f\n', Points(i,1), Points(i,2), Points(i,3));      
%end
fprintf(fid, 'POINTS %d float\n', numberOfPoints);
for i=1:numberOfPoints
	fprintf(fid, '%f %f %f\n', Points(i,1), Points(i,2), Points(i,3));      
end



fprintf(fid, '\nCELLS %d %d\n', numberOfCells, numberOfCells*5);
for k=1:numberOfCells
	fprintf(fid, '4 ');
	for i = 1:4
		fprintf(fid, '%d ', Mesh(k,i));
	end
	fprintf(fid, '\n');
end
fprintf(fid, '\n');

fprintf(fid, 'CELL_TYPES %d\n', numberOfCells);
for k=1:numberOfCells
	fprintf(fid, '9\n');
end
fprintf(fid, '\n');

fprintf(fid, '\n\nPOINT_DATA %d\n', numberOfPoints);
fprintf(fid, 'SCALARS scalars float 1\n');
fprintf(fid, 'LOOKUP_TABLE default\n');
for k=1:numberOfPoints
	if (mod(k,6) == 0)
		fprintf(fid, '\n');
	end
	fprintf(fid, '%f ', 0.0 );
end
fprintf(fid, '\n');




%fprintf(fid, '\nCELLS %d %d\n', numberOfCells, numberOfCells*5);
%for k=1:numberOfCells
%	fprintf(fid, '4 ');
%	for i = 1:4
%		fprintf(fid, '%d ', Mesh(k,i)-1);
%	end
%	fprintf(fid, '\n');
%end
%fprintf(fid, '\n');

%fprintf(fid, 'CELL_TYPES %d\n', numberOfCells);
%for k=1:numberOfCells
%	fprintf(fid, '9\n');
%end
%fprintf(fid, '\n');

fclose(fid);


