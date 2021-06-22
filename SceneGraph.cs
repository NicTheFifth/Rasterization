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
            if (Root != null && Root.Children != null)
            {
				foreach(Node child in Root.Children)
                {
					child.NodeMesh.Render(app.shader, child.TransformMatrix * app.Tcamera * app.Tview, child.NodeTexture);
                }
            }
		}
	}
}