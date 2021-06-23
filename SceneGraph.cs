﻿using System;
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
            unpackChildren(Root, app.Tcamera.Inverted() * app.Tview);

        }
        public void unpackChildren(Node node, Matrix4 transformation,bool debug = false)
        {
            Matrix4 T = node.TransformMatrix * transformation;
            if (node.Parent != null)
                node.NodeMesh.Render(app.shader, T, node.NodeTexture);
            else
                T = transformation;
            
            
            //if(debug==true)
            //	Console.WriteLine(node.ID);
            if( debug == true)
            {
                Console.WriteLine(node.ID);
            }
            
            
                foreach (Node child in node.Children)
                {

                    if (child.Children != null)
                    {
                        if (debug == true)
                            unpackChildren(child,T, true);
                        else
                            unpackChildren(child,T);
                    }

                    if (debug == true)
                    {
                        Console.WriteLine("wtf");
                        Console.WriteLine(child.ID);
                        Console.WriteLine(child.TransformMatrix);
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
