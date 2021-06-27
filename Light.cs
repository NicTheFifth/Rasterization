using OpenTK;
using System.Collections.Generic;

namespace Template
{
    public class Light : Node
    {
        //RGB light
        public Vector3 colour, position, specularcolour;
        public Light(string id, Node parent,Matrix4 transformMatrix,  Vector3 colour, Vector3 specularcolour) : base()
        {
            this.id = id;
            this.position = transformMatrix.ExtractTranslation();
            this.colour = colour;
            this.specularcolour = specularcolour;
            this.parent = null;
            children = new List<Node>();
            nodeMesh = null;
            nodeTexture = null;

            if (parent != null)
            {
                this.parent = parent;
                Parent.Children.Add(this);
            }
        }
        public Vector3 Colour
        { get { return colour; } }
        public Vector3 Position
        { get { return colour; } }
    }
}
