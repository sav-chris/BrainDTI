using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using System.IO;
using System.Collections;

using MathNet;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;

using MathNetMatrix = MathNet.Numerics.LinearAlgebra.Matrix;
using XNAMatrix = Microsoft.Xna.Framework.Matrix;

namespace BrainViewer
{
    delegate float FlipType(float single);
    delegate byte ReverseType(byte binary);
    delegate byte IndexType(byte binary, int index);

    /// <summary>
    /// This is the main type for the viewer
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Camera camera;
        //WorldEntity eightBall;
        Model model;

        ComplexVector[] eigenValues;
        MathNetMatrix[] eigenVectors;
        WorldEntity[] entities;
        float[] confidence;
        int confidentValuesCount;

        readonly int X = 29, Y = 30, Z = 31;

        float[,] Data;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Convert an index into the list data to a coordinate is space
        /// The data is stored in a regular grid with regular spacing 
        /// and is in a right handed coordinate system. The grid is 
        /// traversed in the order X Y Z
        /// </summary>
        /// <param name="index"> index into list</param>
        /// <returns>[X Y Z] coordinate </returns>
        private int[] ConvertIndexToCoordinates(int index)
        {
            //inverts index = X + (xmax * Y) + (xmax * ymax * Z)
            int x = index % X;
            int y = ((index - x) % Y) / X;
            int z = ((index - x) - (X*y)) / (X*Y);
            int[] pos = new int[3];
            pos[0] = x; pos[1] = y; pos[2] = z;
            return pos;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            float aspectRatio = (float)GraphicsDevice.Viewport.Width / (float)GraphicsDevice.Viewport.Height;

            camera = new Camera(new Vector3(0, 0, -100), Vector3.Zero, Vector3.Up, MathHelper.PiOver4, aspectRatio, 1f, 30000f);

            // <Create an object>
            model = Content.Load<Model>("Models\\Colour_Cube");//"Models\\ball_08");

            //XNAMatrix stretch = XNAMatrix.CreateScale(new Vector3(0.5f, 0.5f, 0.5f));
            //XNAMatrix position = XNAMatrix.CreateTranslation(new Vector3(0, 0, 0));
            //TO DO: rotation matrix

            //eightBall = new WorldEntity(model, position, stretch);

            //Load Raw Tensor
            //string filePath = "C:\\Chris\\Math\\MAB681 Advanced Visualisation and Data analysis\\Data sets\\spiral\\dt-helix.raw";
            string filePath = "C:\\Chris\\Math\\MAB681 Advanced Visualisation and Data analysis\\Data sets\\Viz09Data\\Test\\helix-ten.raw";
            //File Dimensions
            int length = X * Y * Z;

            Data = LoadRaw(filePath, X, Y, Z, true);

            eigenValues = new ComplexVector[length];
            eigenVectors = new MathNetMatrix[length];
            confidence = new float[length];
            for (int i = 0; i < length; i++)
            {

                confidence[i] = Data[i, 0];
                float dxx = Data[i, 1];
                float dxy = Data[i, 2];
                float dxz = Data[i, 3];
                float dyy = Data[i, 4];
                float dyz = Data[i, 5];
                float dzz = Data[i, 6];

                eigenValues[i] = MathsClass.GetEigenValues(dxx, dxy, dxz, dyy, dyz, dzz);
                eigenVectors[i] = MathsClass.GetEigenVectors(dxx, dxy, dxz, dyy, dyz, dzz);
            }

            //////////Write Eigenvalues/vectors and positions to file //////////////
            #region writeEigen
            {

                StreamWriter writer = new StreamWriter("eigen.txt");

                //Positions
                writer.WriteLine("#Positions");
                for (int posIndex = 0; posIndex < confidence.Length; posIndex++)
                {
                    if (confidence[posIndex] == 1.0)
                    {
                        int[] pos = ConvertIndexToCoordinates(posIndex);
                        writer.Write(pos[0].ToString()); writer.Write(" ");
                        writer.Write(pos[1].ToString()); writer.Write(" ");
                        writer.Write(pos[2].ToString()); writer.Write(" ");
                        writer.WriteLine("");
                    }
                }

                //EigenValues
                writer.WriteLine("#EigenValues");
                for (int eigenIndex = 0; eigenIndex < eigenValues.Length; eigenIndex++)
                {
                    if (confidence[eigenIndex] == 1.0)
                    {
                        string lambda1 = eigenValues[eigenIndex][0].ToString();
                        string lambda2 = eigenValues[eigenIndex][1].ToString();
                        string lambda3 = eigenValues[eigenIndex][2].ToString();

                        writer.Write(lambda1);
                        writer.Write(" ");
                        writer.Write(lambda2);
                        writer.Write(" ");
                        writer.Write(lambda3);
                        writer.Write(" ");
                        writer.WriteLine("");
                    }
                }

                //Eigenvectors
                writer.WriteLine("#EigenVectors V1x V1y V1z V2x V2y V2z V3x V3y V3z");
                for (int eigenIndex = 0; eigenIndex < eigenVectors.Length; eigenIndex++)
                {
                    if (confidence[eigenIndex] == 1.0)
                    {
                        MathNetMatrix eigenVec = eigenVectors[eigenIndex];

                        double V1x = eigenVec[0, 0];
                        double V1y = eigenVec[1, 0];
                        double V1z = eigenVec[2, 0];

                        double V2x = eigenVec[0, 1];
                        double V2y = eigenVec[1, 1];
                        double V2z = eigenVec[2, 1];

                        double V3x = eigenVec[0, 2];
                        double V3y = eigenVec[1, 2];
                        double V3z = eigenVec[2, 2];

                        writer.Write(V1x.ToString()); writer.Write(" ");
                        writer.Write(V1y.ToString()); writer.Write(" ");
                        writer.Write(V1z.ToString()); writer.Write(" ");

                        writer.Write(V2x.ToString()); writer.Write(" ");
                        writer.Write(V2y.ToString()); writer.Write(" ");
                        writer.Write(V2z.ToString()); writer.Write(" ");

                        writer.Write(V3x.ToString()); writer.Write(" ");
                        writer.Write(V3y.ToString()); writer.Write(" ");
                        writer.Write(V3z.ToString());

                        writer.WriteLine("");
                    }
                }

                writer.Close();
            }
            #endregion

            ///////////////////Create World Entities/////////////////////
            #region Create World Entities
            {
                //Count number of 'confident' values
                int confidenceLength = 0;
                for (int j = 0; j < confidence.Length; j++)
                {
                    if (confidence[j] == 1.0)
                    {
                        confidenceLength++;
                    }
                }
                confidentValuesCount = confidenceLength;

                XNAMatrix position = new XNAMatrix();
                XNAMatrix stretch = new XNAMatrix();
                entities = new WorldEntity[confidentValuesCount];
                StreamWriter w = new StreamWriter("pos.txt");

                int index = 0;
                int listIndex;
                for (int z = 0; z < Z; z++)
                {
                    for (int y = 0; y < Y; y++)
                    {
                        for (int x = 0; x < X; x++)
                        {
                            listIndex = x + (X * y) + (X * Y * z);
                            if (confidence[listIndex] == 1.0)
                            {
                                //Write to file 
                                w.Write(x.ToString()); w.Write(" ");
                                w.Write(y.ToString()); w.Write(" ");
                                w.Write(z.ToString()); w.Write(" ");
                                w.WriteLine("");

                                //Calculate rotation matrix
                                MathNetMatrix V1 = eigenVectors[listIndex];
                                double v1x = V1[0, 0];
                                double v1y = V1[1, 0];
                                double v1z = V1[2, 0];
                                Vector3 v = new Vector3((float)v1x, (float)v1y, (float)v1z);
                                Vector3 nx = new Vector3(1.0f,0.0f,0.0f);
                                v.Normalize();
                                XNAMatrix rotate = XNAMatrix.CreateFromAxisAngle(Vector3.Cross(v, nx), (float)MathNet.Numerics.Trig.InverseCosine(Vector3.Dot(v, nx)));

                                position = XNAMatrix.CreateTranslation(new Vector3(x, y, z));
                                stretch = XNAMatrix.CreateScale(new Vector3(0.01f, 0.01f, 0.01f));

                                entities[index] = new WorldEntity(model, stretch * rotate * position, XNAMatrix.Identity);
                                index++;
                            }
                        }
                    }
                }
                w.Close();
            }
            #endregion 

            /////////////End Create World Entities/////////////////////

        }

        /// <summary>
        /// Loads Tensor Field Data in raw format 
        /// See http://www.sci.utah.edu/~gk/DTI-data/
        /// </summary>
        /// <param name="filePath"></param>        
        /// <param name="X">Length of field in X direction</param>
        /// <param name="Y">Length of field in Y direction</param>
        /// <param name="Z">Length of field in Z direction</param>
        /// <param name="bigEndian">true for big endian, false for little</param>
        /// <returns>7 X (X*Y*Z) Array of the Tensor Data</returns>
        public float[,] LoadRaw(string filePath, int X, int Y, int Z, bool bigEndian)
        {
            Stream stream = File.Open(filePath, FileMode.Open); 
            BinaryReader binReader = new BinaryReader(stream);

            int length = X * Y * Z;

            float[,] data = new float[length, 7]; //seven values for each tensor
            
            #region Bit Reversing
            IndexType getBinaryDigit = delegate(byte binary, int index)
            {
                int integer = (int)binary;
                integer = integer << index;
                byte bite = (byte) integer;

                if (bite >= 128) //if there's a one on the front
                { return (byte) 1; }
                else 
                { return (byte) 0; };
            };

            ReverseType Reverse = delegate(byte binary)
            {
                int integer = 0;
                for (int index = 0; index < 8; index++)
                {
                    integer += (int) (getBinaryDigit(binary, index) * Math.Pow(2, index));
                };
                return (byte)integer;
            };
            
            //swap between big and little endian if neccessary
            FlipType flip = delegate(float single)
            {
                byte[] bytes = BitConverter.GetBytes(single);
                if (bigEndian)
                {   //reverse the bits
                    Array.Reverse(bytes); //or System.Net.IPAddress.HostToNetworkOrder(value); ??
                    //for (int i = 0; i < bytes.Length; i++)
                    //{
                    //    bytes[i] = Reverse(bytes[i]);
                    //};
                };
                return BitConverter.ToSingle(bytes, 0);
            };
            #endregion


            for (int i = 0; i < length; i++)
            {
                float confidence = binReader.ReadSingle();
                data[i, 0] = flip(confidence);

                float dxx = binReader.ReadSingle();
                data[i, 1] = flip(dxx);

                float dxy = binReader.ReadSingle();
                data[i, 2] = flip(dxy);

                float dxz = binReader.ReadSingle();
                data[i, 3] = flip(dxz);

                float dyy = binReader.ReadSingle();
                data[i, 4] = flip(dyy);

                float dyz = binReader.ReadSingle();
                data[i, 5] = flip(dyz);

                float dzz = binReader.ReadSingle();
                data[i, 6] = flip(dzz);
            };

            return data;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.PageUp))
            {
                Vector3 shift = camera.cameraUpVector;
                camera.cameraPosition += shift ;
                camera.cameraTarget += shift;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.PageDown))
            {
                Vector3 shift = -1*camera.cameraUpVector;
                camera.cameraPosition += shift;
                camera.cameraTarget += shift;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                Vector3 to = camera.cameraTarget - camera.cameraPosition;
                to.Normalize();
                Vector3 shift = Vector3.Cross(to, camera.cameraUpVector);
                camera.cameraPosition += shift;
                camera.cameraTarget += shift;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                Vector3 to = camera.cameraTarget - camera.cameraPosition;
                to.Normalize();
                Vector3 shift = Vector3.Cross(camera.cameraUpVector, to);
                camera.cameraPosition += shift;
                camera.cameraTarget += shift;
            }

            //boom
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                Vector3 shift = camera.cameraTarget - camera.cameraPosition;
                shift.Normalize();
                camera.cameraPosition += shift;
                camera.cameraTarget += shift;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                Vector3 shift = camera.cameraPosition - camera.cameraTarget;
                shift.Normalize();
                camera.cameraPosition += shift;
                camera.cameraTarget += shift;
            }

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

            //TO DO: sort by distance from camera
            
            //camera.Draw(eightBall);

            // TODO: Add your drawing code here

            for (int i = 0; i < confidentValuesCount; i++)
            {
                camera.Draw(entities[i]);
            }

            base.Draw(gameTime);
        }
    }
}



/////////////////Grave yard //////////////////////////////////
//Rotate the binary digits once
//ShiftType Shift = delegate(byte binary)
//{
//    int digit = (int)binary;
//    int endDigit = 0;
//    if (digit >= 128)
//    {
//        endDigit = 1;
//    };
//    byte output = (byte)((digit << 1) + endDigit);
//    return output;
//};

////Rotate the binary digits numRotations times
//RotateType Rotate = delegate(byte binary, int numRotations)
//{
//    byte temp = binary;
//    for (int count = 1; count <= numRotations; count++)
//    {
//        temp = Shift(temp);
//    };
//    return temp;
//};

//int integer = (byte)binary;
//int temp = 0;
//int addAmount = 1;

//for (int shifts = 1; shifts <= 8; shifts++)
//{
//if (integer >= 128)
//{
//temp = temp + addAmount;
//};
//integer = integer << 1;
//addAmount = addAmount << 1;
//};
//return (byte)temp;

//byte m = getBinaryDigit((byte)128, 0);
//byte m1 = getBinaryDigit((byte)1, 7);
//byte m2 = getBinaryDigit((byte)2, 6);
//byte m3 = getBinaryDigit((byte)64, 1);
//byte m4 = getBinaryDigit((byte)127, 0);
//byte m5 = getBinaryDigit((byte)128, 1);

//byte m = Reverse((byte)1);
//byte m1 = Reverse((byte)2);
//byte m2 = Reverse((byte)128);
//byte m3 = Reverse((byte)3);
//byte m4 = Reverse((byte)127);
//byte m5 = Reverse((byte)64);
//byte m6 = Reverse((byte)10);
//byte m8 = Reverse((byte)1);
//Confidence
//writer.WriteLine("#Confidence");
//for (int index = 0; index < confidence.Length; index++)
//{
//    writer.Write(confidence[index].ToString());
//    writer.WriteLine("");
//}