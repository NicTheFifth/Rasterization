using System.Diagnostics;
using OpenTK;
using OpenTK.Input;
namespace Template
{
	public class MyApplication
	{
	public KeyboardState Keyboard
	{
		get { return keyboardstate; }
		set { keyboardstate = value; }
	}
		// member variables
		public Surface screen;                  // background surface for printing etc.
		Mesh Tpotmesh, floormesh;                       // a mesh to draw using OpenGL
		const float PI = 3.1415926535f;         // PI
		float a = 0;                            // teapot rotation angle
		Stopwatch timer;                        // timer for measuring frame duration
		public Shader shader;                          // shader to use for rendering
		Shader postproc;                        // shader to use for post processing
		Texture wood;                           // texture to use for rendering
		RenderTarget target;                    // intermediate render target
		ScreenQuad quad;                        // screen filling quad for post processing
		bool useRenderTarget = true;
		KeyboardState keyboardstate;
		// initialize
		public Node Tpotnode1;
		public Node Floornode1;

		public Node Tpotnode2;
		public Node Floornode2;
		public Node Cameranode;
		public SceneGraph sceneGraph;

		public Matrix4 Tcamera;
		public Matrix4 Tview;
		public void Init()
		{
			float angle90degrees = PI / 2;
			Tcamera = Matrix4.CreateTranslation(new Vector3(0, -14.5f, 0)) * Matrix4.CreateFromAxisAngle(new Vector3(1, 0, 0), angle90degrees);
			Tview = Matrix4.CreatePerspectiveFieldOfView(1.2f, 1.3f, .1f, 1000);

			// load teapot
			
			// initialize stopwatch
			timer = new Stopwatch();
			timer.Reset();
			timer.Start();
			// create shaders
			shader = new Shader( "../../shaders/vs.glsl", "../../shaders/fs.glsl" );
			postproc = new Shader( "../../shaders/vs_post.glsl", "../../shaders/fs_post.glsl" );
			// load a texture
			wood = new Texture( "../../assets/wood.jpg" );
			// create the render target
			target = new RenderTarget( screen.width, screen.height );
			quad = new ScreenQuad();
			initNodeSystem();
		}

		// tick for background surface
		public void Tick()
		{
			Inp();
			screen.Clear( 0 );
			float frameDuration = timer.ElapsedMilliseconds;
			timer.Reset();
			timer.Start();
			
			a = 0.001f  ;
			
			//Tpotnode1.TransformMatrix = Matrix4.CreateRotationY( a);
			Tpotnode2.TransformMatrix *= Matrix4.CreateRotationY(a);
		}
		void Inp()
        {
			if (Keyboard[Key.Up])
            {
                //Console.WriteLine("UP");
                Tcamera = Matrix4.CreateTranslation(0, 0, -0.5f) * Tcamera;
                
            }
            else if (Keyboard[Key.Down])
            {
                Tcamera = Matrix4.CreateTranslation(0, 0, 0.5f) * Tcamera;
            }
            else if (Keyboard[Key.Left])
            {
                Tcamera = Matrix4.CreateTranslation(-0.5f, 0, 0) * Tcamera;
            }
            else if (Keyboard[Key.Right])
            {
                Tcamera = Matrix4.CreateTranslation(0.5f, 0, 0) * Tcamera;
            }
            else if (Keyboard[Key.Space])
            {
                Tcamera = Matrix4.CreateTranslation(0, 0.5f, 0) * Tcamera;
            }
            else if (Keyboard[Key.LShift] || Keyboard[Key.RShift])
            {
                Tcamera = Matrix4.CreateTranslation(0, -0.5f, 0) * Tcamera;
            }
            else if (Keyboard[Key.W])
            {
                Tcamera = Matrix4.CreateRotationX(0.01f) * Tcamera;
            }
            else if (Keyboard[Key.S])
            {
                Tcamera = Matrix4.CreateRotationX(-0.01f) * Tcamera;
            }
            else if (Keyboard[Key.A])
            {
                Tcamera = Matrix4.CreateRotationY(0.01f) * Tcamera;
            }
            else if (Keyboard[Key.D])
            {
                Tcamera = Matrix4.CreateRotationY(-0.01f) * Tcamera;
            }
			else if (Keyboard[Key.H])
			{
				sceneGraph.unpackChildren(sceneGraph.Root, true);
			}
		}
		// tick for OpenGL rendering code
		public void RenderGL()
		{
			sceneGraph.render();
    //        // measure frame duration
    //        float frameDuration = timer.ElapsedMilliseconds;
    //        timer.Reset();
    //        timer.Start();

    //        // prepare matrix for vertex shader


    //        Matrix4 Tpot = Matrix4.CreateScale(0.5f) * Matrix4.CreateFromAxisAngle(new Vector3(0, 1, 0), a);
    //        Matrix4 Tfloor = Matrix4.CreateScale(4.0f) * Matrix4.CreateFromAxisAngle(new Vector3(0, 1, 0), a);

    //        // update rotation


    //        if (a > 2 * PI) a -= 2 * PI;

    //        if (useRenderTarget)
    //        {
    //            // enable render target
    //            target.Bind();

    //            // render scene to render target
    //            Tpotmesh.Render(shader, Tpot * Tcamera * Tview, wood);
    //            floormesh.Render(shader, Tfloor * Tcamera * Tview, wood);

    //            // render quad
    //            target.Unbind();
    //            quad.Render(postproc, target.GetTextureID());
    //        }
    //        else
    //        {
				//// render scene directly to the screen
				//Tpotmesh.Render(shader, Tpot * Tcamera * Tview, wood);
				//floormesh.Render(shader, Tfloor * Tcamera * Tview, wood);
    //        }
        }

		void initNodeSystem()
		{



			Matrix4 Tpotmatrix2 = Matrix4.CreateScale(0.5f) * Matrix4.CreateFromAxisAngle(new Vector3(0, 1, 0), 0);

			Matrix4 floormatrix2 = Matrix4.CreateScale(4.0f) * Matrix4.CreateFromAxisAngle(new Vector3(0, 1, 0), 0);
			Tpotmesh = new Mesh("../../assets/teapot.obj");
			floormesh = new Mesh("../../assets/floor.obj");
			sceneGraph = new SceneGraph(this);
			Cameranode = new Node("Camera", null, Tcamera*Tview, null, null, sceneGraph);
			
			Floornode2 = new Node("Floor", Cameranode, floormatrix2, floormesh, wood, sceneGraph);
			Tpotnode2 = new Node("Tpot", Floornode2, Tpotmatrix2, Tpotmesh, wood, sceneGraph);

			Matrix4 Tpotmatrix1 = Matrix4.CreateScale(0.5f)*Matrix4.CreateTranslation(2,0,0) * Matrix4.CreateFromAxisAngle(new Vector3(0, 1, 0), 0);
			
			Matrix4 floormatrix1 = Matrix4.CreateScale(4.0f) * Matrix4.CreateFromAxisAngle(new Vector3(0, 1, 0), 0);
            Floornode1 = new Node("Floor", Cameranode, floormatrix1, floormesh, wood, sceneGraph);
            Tpotnode1 = new Node("Tpot", Tpotnode2, Tpotmatrix1, Tpotmesh, wood, sceneGraph);

			//Tpotnode1 = new Node("Tpot", Cameranode, Tpotmatrix, Tpotmesh, wood, sceneGraph);
			//Floornode1 = new Node("Floor", Tpotnode1, floormatrix, floormesh, wood, sceneGraph);

			//Floornode2 = new Node("Floor", Floornode1, floormatrix, floormesh, wood, sceneGraph);
			//Tpotnode2 = new Node("Tpot", Floornode2, Tpotmatrix, Tpotmesh, wood, sceneGraph);
			showTree(sceneGraph.Root, "");
        }

		void showTree(Node root, string pref)
        {
			System.Console.WriteLine(pref + root.ID);
			foreach(Node child in root.Children)
            {
				showTree(child, pref += '\t');
            }
        }
	}
}

