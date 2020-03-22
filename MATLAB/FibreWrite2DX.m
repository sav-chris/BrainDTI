function FibreWrite2DX()

load('./data/Particle.txt')
load('./data/ParticleConections.txt')
numberOfPoints = size(Particle,1);
numberOfPointsCon = size(ParticleConections,1);


fid = fopen('FibrePatientA1.dx', 'w');
fprintf(fid, 'object 1 class array type float rank 1 shape 3 items %d data follows\n', numberOfPoints);
for i=1:numberOfPoints
	fprintf(fid, '%f %f %f\n', Particle(i,1), Particle(i,2), Particle(i,3));
end
fprintf(fid, 'object 2 class array type int rank 1 shape 2 items %d data follows\n', numberOfPointsCon);
for i = 1:numberOfPointsCon
		fprintf(fid, '%d %d\n', ParticleConections(i,1), ParticleConections(i,2));
end
fprintf(fid, 'attribute "element type" string "lines"\n');

fprintf(fid, 'object "stuff" class field\n');
fprintf(fid, 'component "positions" value 1\n');
fprintf(fid, 'component "connections" value 2\n');
fprintf(fid, 'end\n');
fclose(fid);

