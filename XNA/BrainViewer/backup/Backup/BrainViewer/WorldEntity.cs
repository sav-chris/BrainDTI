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


namespace BrainViewer
{
    /// <summary>
    /// An entitiy in the world (model and position and orientation transforms)
    /// </summary>
    public class WorldEntity
    {
        public Model model;
        public Matrix positionTransform;
        public Matrix stretchTransform;

        public WorldEntity(Model model, Matrix positionTransform, Matrix stretchTransform)
        {
            this.model = model;
            this.positionTransform = positionTransform;
            this.stretchTransform = stretchTransform;
        }

        public Matrix[] Transforms
        {
            get
            {
                // Copy any parent transforms.
                Matrix[] transforms = new Matrix[model.Bones.Count];
                model.CopyAbsoluteBoneTransformsTo(transforms);
                return transforms;
            }
        }

    }
}
