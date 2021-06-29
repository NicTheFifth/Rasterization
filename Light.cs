using OpenTK;
using System.Collections.Generic;

namespace Template
{
    public class Light : Node
    {
        //RGB light
       
        public Vector4 position;
       
        public Vector3 colour, specularcolour;
        public Light(string id, Node parent,Matrix4 transformMatrix,  Vector3 colour, Vector3 specularcolour) : base()
        {
            this.transformMatrix = transformMatrix;
            this.id = id;
            
            System.Console.WriteLine(position);
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
        public Vector4 Position
        { get { return position; }
            set { position = value; } }
    }
}
