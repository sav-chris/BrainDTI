using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Design;

namespace BrainViewer
{
    /// <summary>
    /// Camera
    /// </summary>
    public class Camera
    {

        public Vector3 cameraPosition, cameraTarget, cameraUpVector;
        public float fieldOfView, aspectRatio, nearPlaneDistance, farPlaneDistance;

        public Camera(Vector3 cameraPosition, Vector3 cameraTarget, Vector3 cameraUpVector, float fieldOfView, float aspectRatio, float nearPlaneDistance, float farPlaneDistance)
        {
            this.cameraPosition = cameraPosition;
            this.cameraTarget = cameraTarget;
            this.cameraUpVector = cameraUpVector;
            this.fieldOfView = fieldOfView;
            this.aspectRatio = aspectRatio;
            this.nearPlaneDistance = nearPlaneDistance;
            this.farPlaneDistance = farPlaneDistance;
        }

        public void Draw(WorldEntity worldEntity)
        {
            Matrix view = Matrix.CreateLookAt(cameraPosition, cameraTarget, cameraUpVector);

            Matrix projection = Matrix.CreatePerspectiveFieldOfView(
                              fieldOfView,
                              aspectRatio,
                              nearPlaneDistance,
                              farPlaneDistance);

            Model model = worldEntity.model;
            Matrix[] transforms = worldEntity.Transforms;

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.World = transforms[mesh.ParentBone.Index] * worldEntity.positionTransform * worldEntity.stretchTransform;
                    effect.View = view;
                    effect.Projection = projection;
                }

                mesh.Draw();
            }
        }

    }

}
