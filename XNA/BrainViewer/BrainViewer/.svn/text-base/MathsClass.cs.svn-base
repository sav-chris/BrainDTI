using System;
using System.Collections.Generic;
using System.Text;
using MathNet;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;

namespace BrainViewer
{
    /// <summary>
    /// A static class for usefull maths functions
    /// </summary>
    public static class MathsClass
    {
        /// <summary>
        /// Gets the eigenvalues(solves Ax = lambda.x) of a 3x3 symetric (A^T = A) matrix
        /// [ dxx dxy dxz ]
        /// [ dxy dyy dyz ]
        /// [ dxz dyz dzz ]
        /// </summary>
        /// <param name="dxx">dxx</param>
        /// <param name="dxy">dxy</param>
        /// <param name="dxz">dxz</param>
        /// <param name="dyy">dyy</param>
        /// <param name="dyz">dyz</param>
        /// <param name="dzz">dzz</param>
        /// <returns>Eigenvalues (lambda)</returns>
        public static ComplexVector GetEigenValues(float dxx, float dxy, float dxz, float dyy, float dyz, float dzz)
        {

            Matrix tensor = new Matrix(3, 3);
            tensor[0, 0] = dxx;
            tensor[0, 1] = dxy;
            tensor[0, 2] = dxz;
            tensor[1, 0] = dxy;
            tensor[1, 1] = dyy;
            tensor[1, 2] = dyz;
            tensor[2, 0] = dxz;
            tensor[2, 1] = dyz;
            tensor[2, 2] = dzz;

            EigenvalueDecomposition e = new EigenvalueDecomposition(tensor);

            return e.EigenValues;
        }

        /// <summary>
        /// Gets the eigenvectors(solves Ax = lambda.x) of a 3x3 symetric (A^T = A) matrix
        /// [ dxx dxy dxz ]
        /// [ dxy dyy dyz ]
        /// [ dxz dyz dzz ]
        /// </summary>
        /// <param name="dxx">dxx</param>
        /// <param name="dxy">dxy</param>
        /// <param name="dxz">dxz</param>
        /// <param name="dyy">dyy</param>
        /// <param name="dyz">dyz</param>
        /// <param name="dzz">dzz</param>
        /// <returns>Eigenvectors (x)</returns>
        public static Matrix GetEigenVectors(float dxx, float dxy, float dxz, float dyy, float dyz, float dzz)
        {

            Matrix tensor = new Matrix(3, 3);
            tensor[0, 0] = dxx;
            tensor[0, 1] = dxy;
            tensor[0, 2] = dxz;
            tensor[1, 0] = dxy;
            tensor[1, 1] = dyy;
            tensor[1, 2] = dyz;
            tensor[2, 0] = dxz;
            tensor[2, 1] = dyz;
            tensor[2, 2] = dzz;

            EigenvalueDecomposition e = new EigenvalueDecomposition(tensor);

            return e.EigenVectors;

        }

    }
}
