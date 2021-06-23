using System;
using System.Collections.Generic;
using OpenTK;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template
{

    public class Node
    {
        //one parent max per node
        Node parent;

        //multiple children possible
        List<Node> children;

        //ID of node
        string id;

        Mesh nodeMesh;
        Texture nodeTexture;

        Matrix4 transformMatrix;


       
        

        public Node(string id, Node parent, Matrix4 transformMatrix, Mesh nodeMesh, Texture nodeTexture)
        {
            this.id = id;
            this.nodeMesh = nodeMesh;
            this.nodeTexture = nodeTexture;
            if (parent != null)
            {

                this.parent = parent;

                Parent.Children.Add(this);
            }

            this.children = new List<Node>();
            this.transformMatrix = transformMatrix;
        }

        public List<Node> Children
        {
            get { return children; }
        }

        public Matrix4 TransformMatrix
        {
            get { return transformMatrix; }
            set { transformMatrix = value; }
        }

        public Node Parent
        {
            get { return parent; }
        }

        public string ID
        {
            get { return id; }
        }


        public Mesh NodeMesh
            {
            get { return nodeMesh; }
            }
        public Texture NodeTexture
        {
            get { return nodeTexture; }
        }
    }

}