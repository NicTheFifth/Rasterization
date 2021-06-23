using OpenTK;
using System.Collections.Generic;

namespace Template
{
    public class Light : Node
    {
        //RGB light
        Vector3 colour;
        public Light(string id, Node parent, Matrix4 transformMatrix, Vector3 colour) : base()
        {
            this.id = id;
            this.transformMatrix = transformMatrix;
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
    }
}
