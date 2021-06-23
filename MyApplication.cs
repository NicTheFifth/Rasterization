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
		Texture wood, stone;                           // texture to use for rendering
		RenderTarget target;                    // intermediate render target
		ScreenQuad quad;                        // screen filling quad for post processing
		bool useRenderTarget = true;
		KeyboardState keyboardstate;
		// initialize
		private Node Cameranode, Floornode,potbig, potmedium, potsmall;
		

		
		
		
		public SceneGraph sceneGraph;

		public Matrix4 Tcamera;
		public Matrix4 Tview;
		public bool isneg;
		public void Init()
		{
			float angle = PI / 3;
			Tcamera = Matrix4.CreateTranslation(new Vector3(-20, 14.5f, 20)) * Matrix4.CreateFromAxisAngle(new Vector3(-1,0, 0), angle);
			Tview = Matrix4.CreatePerspectiveFieldOfView(1.2f, 1.3f, .1f, 1000);

			// initialize stopwatch
			timer = new Stopwatch();
			timer.Reset();
			timer.Start();
			// create shaders
			shader = new Shader( "../../shaders/vs.glsl", "../../shaders/fs.glsl" );
			postproc = new Shader( "../../shaders/vs_post.glsl", "../../shaders/fs_post.glsl" );
			// load a texture
			wood = new Texture( "../../assets/wood.jpg" );
			stone = new Texture("../../assets/stone.png");
			// create the render target
			target = new RenderTarget( screen.width, screen.height );
			quad = new ScreenQuad();
			initNodeSystem();
		}

		// tick for background surface
		public void Tick()
		{
		}
		void Inp()
        {
			if (Keyboard[Key.Up])
            {
                Tcamera = Matrix4.CreateTranslation(0, 0, -0.5f) * Tcamera;
                
            }
            if (Keyboard[Key.Down])
            {
                Tcamera = Matrix4.CreateTranslation(0, 0, 0.5f) * Tcamera;
            }
            if (Keyboard[Key.Left])
            {
                Tcamera = Matrix4.CreateTranslation(-0.5f, 0, 0) * Tcamera;
            }
            if (Keyboard[Key.Right])
            {
                Tcamera = Matrix4.CreateTranslation(0.5f, 0, 0) * Tcamera;
            }
            if (Keyboard[Key.Space])
            {
                Tcamera = Matrix4.CreateTranslation(0, 0.5f, 0) * Tcamera;
            }
            if (Keyboard[Key.LShift] || Keyboard[Key.RShift])
            {
                Tcamera = Matrix4.CreateTranslation(0, -0.5f, 0) * Tcamera;
            }
            if (Keyboard[Key.W])
            {
                Tcamera = Matrix4.CreateRotationX(0.01f) * Tcamera;
            }
            if (Keyboard[Key.S])
            {
                Tcamera = Matrix4.CreateRotationX(-0.01f) * Tcamera;
            }
            if (Keyboard[Key.A])
            {
                Tcamera = Matrix4.CreateRotationY(0.01f) * Tcamera;
            }
            if (Keyboard[Key.D])
            {
                Tcamera = Matrix4.CreateRotationY(-0.01f) * Tcamera;
            }
			if (Keyboard[Key.H])
			{
				sceneGraph.unpackChildren(sceneGraph.Root, Tcamera.Inverted() * Tview, true);
			}
		}

		public void RenderGL()
		{
			Inp();
			screen.Clear( 0 );
			float frameDuration = timer.ElapsedMilliseconds;
			timer.Reset();
			timer.Start();
			
			a = 0.001f  ;
			
			potbig.TransformMatrix *= Matrix4.CreateRotationY( -20*a);
			potsmall.TransformMatrix *= Matrix4.CreateRotationY(a);
			potmedium.TransformMatrix *= Matrix4.CreateRotationY(a * 10);
			if (potbig.TransformMatrix.ExtractTranslation().X> 10)
			{
           
				isneg = true;
			}
			else if (potbig.TransformMatrix.ExtractTranslation().X < 0)
			{

				isneg = false;
			}
			if (isneg)
			{
				float b = -20 * ((float)System.Math.Sin(a * System.Math.PI));
				
				potbig.TransformMatrix *= Matrix4.CreateTranslation( b,0, b);
				potmedium.TransformMatrix *= Matrix4.CreateTranslation(-b, 0, -b);
			}
			else
			{
				float b = 20 * ((float)System.Math.Sin(a * System.Math.PI));
				potbig.TransformMatrix *= Matrix4.CreateTranslation(b ,0,b);
				potmedium.TransformMatrix *= Matrix4.CreateTranslation(-b, 0, -b);
			}
			
			sceneGraph.render();

        }

		void initNodeSystem()
		{



			
			Tpotmesh = new Mesh("../../assets/teapot.obj");
			floormesh = new Mesh("../../assets/floor.obj");
			
			sceneGraph = new SceneGraph(this);

			Cameranode = new Node("Camera", null, Tcamera*Tview, null, null, sceneGraph);
			
			
			Matrix4 floormatrix = Matrix4.CreateScale(20f) * Matrix4.CreateFromAxisAngle(new Vector3(0, 1, 0), 0);
			Floornode = new Node("Floor", Cameranode, floormatrix, floormesh, stone, sceneGraph);


			Matrix4 bigtpotmatrix = Matrix4.CreateScale(0.5f)*Matrix4.CreateTranslation(0,0,0) * Matrix4.CreateFromAxisAngle(new Vector3(0, 1, 0), 0);
			potbig = new Node("coftpotcoffloor", Floornode, bigtpotmatrix, Tpotmesh, wood, sceneGraph);

			Matrix4 mediumtpotmatrix = Matrix4.CreateScale(0.5f) * Matrix4.CreateTranslation(20, 0, 0) * Matrix4.CreateFromAxisAngle(new Vector3(0, 1, 0), 0);
			potmedium = new Node("coftpotcoffloor", potbig, mediumtpotmatrix, Tpotmesh, wood, sceneGraph);
			
			Matrix4 smalltpotmatrix = Matrix4.CreateScale(0.5f) *Matrix4.CreateTranslation(10f,0,0)* Matrix4.CreateFromAxisAngle(new Vector3(0, 1, 0), 0);
			potsmall = new Node("", potmedium, smalltpotmatrix, Tpotmesh, wood, sceneGraph);
			isneg = false;
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

