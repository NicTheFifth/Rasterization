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

        /* misschien niet nodig?  behalve als verschillende groepen van objecten anders van elkaar bewegen, anders gewoon een matrix doorgeven van de camera?*/
        // transformation list of children
        //Dictionary<string, Matrix4> transfromationDict;

        public Node(string id, Node parent, Matrix4 transformMatrix, Mesh nodeMesh, Texture nodeTexture)
        {
            this.id = id;
            this.nodeMesh = nodeMesh;
            this.nodeTexture = nodeTexture;
            if (parent != null)
            {

                this.parent = parent;
                //Parent.transfromationDict.Add(ID, transformMatrix);
                Parent.Children.Add(this);
            }
            //this.transfromationDict = new Dictionary<string, Matrix4>();
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

       /// public Dictionary<string, Matrix4> TransformationDict
       // {
       //     get { return transfromationDict; }
       // }
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