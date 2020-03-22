function [Data] = readDataFile(filename)


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
    temp = fscanf(fid, '%f %f %f', [1,3]);
    counter = counter +1;
end

% Create the dataset
Data = zeros(counter, 3);
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
    temp = textscan(tline, '%f %f %f');
    counter = counter +1;
    if (size(temp,1) > 0)
        
        Data(counter,1) = temp{1};
        Data(counter,2) = temp{2};
        Data(counter,3) = temp{3};
    else
        fprintf(1, 'Problem\n');
    end
end
fclose(fid);
