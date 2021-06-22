using System;
using OpenTK;
namespace Template
{
    public class SceneGraph
    {
        Node root;

        MyApplication app;
        public SceneGraph(MyApplication app)
        {
            this.app = app;


        }
        public Node Root
        {
            get { return root; }
            set { root = value; }
        }

        public void render()
        {
            unpackChildren(Root);

        }
        public void unpackChildren(Node node, bool debug = false)
        {
            Matrix4 T;
            if(node.Parent!= null)
            {
                T = node.TransformMatrix * node.Parent.TransformMatrix;
               
            }
            else
            {
                //root so :
                T = app.Tcamera.Inverted() *app.Tview;
                node.TransformMatrix = T;
            }
            //if(debug==true)
            //	Console.WriteLine(node.ID);
            if( debug == true)
            {
                Console.WriteLine(node.ID);
            }

            if (node != null )
            {
                foreach (Node child in node.Children)
                {

                    if (child.Children != null)
                        unpackChildren(child);
                    if (debug == true)
                    {
                        Console.WriteLine(child.ID);
                        Console.WriteLine(child.TransformMatrix);
                    }
                    child.NodeMesh.Render(app.shader, child.TransformMatrix * T, child.NodeTexture);
                }

            }
            //if (node.NodeMesh != null && node.NodeTexture != null)
            //{
            //    if (debug == true)
            //    {
            //        Console.WriteLine(node.ID);
            //        Console.WriteLine(node.TransformMatrix);
            //    }
            //node.NodeMesh.Render(app.shader, node.TransformMatrix * app.Tcamera * app.Tview, node.NodeTexture);
            //}
        }
    }
}
