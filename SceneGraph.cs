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
            unpackChildren(Root, app.Tworld * app.Tcamera.Inverted() * app.Tview);

        }
        public void unpackChildren(Node node, Matrix4 transformation, bool debug = false)
        {
            Matrix4 T;
            
            T = node.TransformMatrix * transformation;

            if (node.Parent != null && node.GetType() != typeof(Light))
                node.NodeMesh.Render(app.shader, T,app.Tworld, node.NodeTexture);
            else
                T = transformation;



            foreach (Node child in node.Children)
            {
                if (child.GetType() == typeof(Light))
                    unpackaslight((Light)child, T);
                if (child.Children != null)
                {

                    unpackChildren(child, T);
                }



            }



        }

        private void unpackaslight(Light light, Matrix4 T)
        {
            light.position = T * new Vector4(1, 1, 1, 1);
        }
    }
}
