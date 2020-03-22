function [Block, U, V, W] = ReadNHDR(dim, numx, numy, numz, filename)
% [Block, U, V, W] = ReadNHDR(13, 29, 30, 31, 'helix-dwi.raw');
 
 
% Assume datatype is float
 
fid = fopen(filename, 'rb');
 
Data = zeros(numx*numy*numz, dim);
 
for i = 1:numx*numy*numz
                % Read float
                %Data(i,:) = fread(fid, dim, 'float');
                % Read Unsigned short
                Data(i,:) = fread(fid, dim, 'float');
end
 
 
 
Block = zeros(numx,numy,numz);
 
U = zeros(numx,numy,numz);
V = zeros(numx,numy,numz);
W = zeros(numx,numy,numz);
 
for k = 1:numz
                for j = 1:numy
                                for i = 1:numx
                                                Block(i,j,k) = Data( (k-1)*numx*numy + (j-1)*numx + i ,1);           
                                                U(i,j,k)  = Data( (k-1)*numx*numy + (j-1)*numx + i ,2); 
                                                V(i,j,k)  = Data( (k-1)*numx*numy + (j-1)*numx + i ,3); 
                                                W(i,j,k)                  = Data( (k-1)*numx*numy + (j-1)*numx + i ,4); 
                                end
                end
end
 
%slice(Block, 15,15,13); colormap (flipud(jet(24)))
%streamtube(U,V,W,15,15,15);
%imagesc(Block(:,:,1)); colormap(gray(256));
 
fclose(fid);
 
 
Origin = [-96.551724 -96.666667 -96.774194];
Spacing = [6.896552 6.666667 6.451613];
 
Points = zeros(numx*numy*numz,3);
 
Counter = 0;
xOffset = Origin(1 , 1)- Spacing(1,1)*numx/2;
yOffset = Origin(1 , 2)- Spacing(1,2)*numy/2;
zOffset = Origin(1 , 3)- Spacing(1,3)*numz/2;
for k = 1:numz
                for j = 1:numy
                                for i = 1:numx
                                                Counter = Counter + 1;
                                                Points(Counter, 1) = xOffset + (i-1)*Spacing(1,1);
                                                Points(Counter, 2) = yOffset + (j-1)*Spacing(1,2);
                                                Points(Counter, 3) = zOffset + (k-1)*Spacing(1,3);
                                end
                end
end