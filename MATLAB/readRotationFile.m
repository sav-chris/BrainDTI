function [Data] = readRotationFile(filename)


% Count the Data
fid=fopen(filename,'r');

if fid == -1,
    error('Unable to open specified file');
end

counter = 0;
while 1
    tline = fgetl(fid);
    if ~ischar(tline)
        break; % For end of file recognition
    end
    temp = fscanf(fid, '%f %f %f %f %f %f %f %f %f', [1,9]);
    counter = counter +1;
end

% Create the dataset
Data = zeros(counter, 9);
fclose(fid);

% Read the Data
fid=fopen(filename,'r');
if fid == -1,
    error('Unable to open specified file');
end

counter = 0;


while 1
    tline = fgetl(fid);
    if ~ischar(tline)
        break; % For end of file recognition
    end
    temp = textscan(tline, '%f %f %f %f %f %f %f %f %f');
    counter = counter +1;
    if (size(temp,1) > 0)
        
        Data(counter,1) = temp{1};
        Data(counter,2) = temp{2};
        Data(counter,3) = temp{3};
        Data(counter,4) = temp{4};
        Data(counter,5) = temp{5};
        Data(counter,6) = temp{6};
        Data(counter,7) = temp{7};
        Data(counter,8) = temp{8};
        Data(counter,9) = temp{9};
    else
        fprintf(1, 'Problem\n');
    end
end
fclose(fid);
