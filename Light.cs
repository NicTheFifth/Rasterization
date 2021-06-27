using OpenTK;
using System.Collections.Generic;

namespace Template
{
    public class Light : Node
    {
        //RGB light
        public Vector3 colour;
        public Vector4 position;
        public Light(string id, Node parent,Matrix4 transformMatrix,  Vector3 colour) : base()
        {
            this.transformMatrix = transformMatrix;
            this.id = id;
            
            System.Console.WriteLine(position);
            this.colour = colour;
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
