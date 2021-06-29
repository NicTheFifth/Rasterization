using System.Diagnostics;
using System;
using OpenTK;
using OpenTK.Input;
using OpenTK.Graphics.OpenGL;
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
		float a,b = 0;                            // teapot rotation angle
		Stopwatch timer;                        // timer for measuring frame duration
		public Shader shader;                          // shader to use for rendering
		Shader postproc;                        // shader to use for post processing
		Texture wood, stone;                           // texture to use for rendering
		RenderTarget target;                    // intermediate render target
		ScreenQuad quad;                        // screen filling quad for post processing
		bool useRenderTarget = true;
		KeyboardState keyboardstate;
		// initialize
		private Node Cameranode, Floornode,potbig, potmedium, potsmall,wipwap,wip1,wip2;
		public Light lightnode;
		public Matrix4 Tworld;
		public Vector4 currentlightpos;
		
		
		
		public SceneGraph sceneGraph;

		public Matrix4 Tcamera;
		public Matrix4 Tview;
		public bool isneg, isnegwipwap;
		public void Init()
		{
			
			Tcamera =  Matrix4.CreateFromAxisAngle(new Vector3(-1,0, 0), 0.5f)*Matrix4.CreateTranslation(new Vector3(-4, 15, 70));

			Tview = Matrix4.CreatePerspectiveFieldOfView(1.2f, 1.3f, .1f, 1000);
            System.Console.WriteLine(Tcamera);
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
			if (Keyboard[Key.Q])
			{
				Tcamera = Matrix4.CreateRotationZ(0.02f) * Tcamera;
			}
			if (Keyboard[Key.E])
			{
				Tcamera = Matrix4.CreateRotationZ(-0.02f) * Tcamera;
			}
			if(Keyboard[Key.Number8])
            {
				currentlightpos += new Vector4(0,0,1f,0);

            }
			if (Keyboard[Key.Number4])
			{
				currentlightpos += new Vector4(-1f, 0, 0, 0);

			}
			if (Keyboard[Key.Number6])
			{
				currentlightpos += new Vector4(1f, 0, 0, 0);

			}
			if (Keyboard[Key.Number5])
			{
				currentlightpos += new Vector4(0, 0, -1f, 0);

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
			b+=0.001f*frameDuration;
			if (b > 2 * PI) b -= 2 * PI;
			Tworld =  Matrix4.CreateFromAxisAngle(new Vector3(0, 1, 0), b);
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
				float c = -20 * ((float)System.Math.Sin(a * System.Math.PI));
				
				potbig.TransformMatrix *= Matrix4.CreateTranslation( c,0, c);
				potmedium.TransformMatrix *= Matrix4.CreateTranslation(-c, 0, -c);
			}
			else
			{

				float c= 20 * ((float)System.Math.Sin(a * System.Math.PI));
				potbig.TransformMatrix *= Matrix4.CreateTranslation(c ,0,c);
				potmedium.TransformMatrix *= Matrix4.CreateTranslation(-c, 0, -c);
			}
			wipper();
			Matrix3 lightMat = new Matrix3(lightnode.position.Xyz, lightnode.colour, Vector3.Zero);
			int lightMatID = GL.GetUniformLocation(shader.programID, "light");
			GL.UseProgram(shader.programID);
			GL.UniformMatrix3(lightMatID, true, ref lightMat);
			sceneGraph.render();
			
		}

		void wipper()
        {
			if(wipwap.transformMatrix.ExtractRotation().X>0.25f)
            {
				
				isnegwipwap= true;
            }
			else if(wipwap.transformMatrix.ExtractRotation().X<-0.25f)
            {
				
					isnegwipwap = false;
            }
            if (isnegwipwap) { 
				wipwap.transformMatrix *= Matrix4.CreateRotationX(-0.01f);
				wip2.transformMatrix*=  Matrix4.CreateTranslation(0,0.1f,0.05f);
				wip1.transformMatrix*= Matrix4.CreateTranslation(0,-0.1f,0.05f);	
			}
            else
            {
				wipwap.transformMatrix *= Matrix4.CreateRotationX(0.01f);
				wip2.transformMatrix*=  Matrix4.CreateTranslation(0,-0.1f,-0.05f);
				wip1.transformMatrix*= Matrix4.CreateTranslation(0,0.1f,-0.05f);
            }
        }
		void initNodeSystem()
		{ 
			Tpotmesh = new Mesh("../../assets/teapot.obj");
			floormesh = new Mesh("../../assets/floor.obj");
			
			sceneGraph = new SceneGraph(this);

			Cameranode = new Node("Camera", null, Tworld*Tcamera*Tview, null, null);
			sceneGraph.Root = Cameranode;
			
			Matrix4 floormatrix = Matrix4.CreateScale(4f) * Matrix4.CreateTranslation(0,0f,0);
			
			Floornode = new Node("Floor", Cameranode, floormatrix, floormesh, wood);

			Matrix4 bigtpotmatrix = Matrix4.CreateScale(0.5f);
			potbig = new Node("Big Teapot", Floornode, bigtpotmatrix, Tpotmesh, wood);

			Matrix4 mediumtpotmatrix = Matrix4.CreateScale(0.5f) * Matrix4.CreateTranslation(20, 0, 0) ;
			potmedium = new Node("Medium Teapot", potbig, mediumtpotmatrix, Tpotmesh, wood);
			
			
			Matrix4 smalltpotmatrix = Matrix4.CreateScale(0.5f) *Matrix4.CreateTranslation(10f,0,0);
			potsmall = new Node("Small Teapot", potmedium, smalltpotmatrix, Tpotmesh, wood);


			Matrix4 wipwapmatrix = Matrix4.CreateScale(1f) * Matrix4.CreateTranslation(0, 10, 10);
			Matrix4 wip1matrix = Matrix4.CreateScale(1)* Matrix4.CreateTranslation(0,2.5f,-6);
			Matrix4 wip2matrix =  Matrix4.CreateScale(1)*Matrix4.CreateTranslation(0,2.5f,6);
		
			
			wipwap = new Node("Wipwap On Small Teapot",potsmall,wipwapmatrix,floormesh,wood);
			wip1 = new Node("Wipper",wipwap,wip1matrix,Tpotmesh,stone);
			wip2 = new Node("Wapper",wipwap,wip2matrix,Tpotmesh,stone);

			isneg = false;
			isnegwipwap = false;
			
			Matrix4 lightmatrix = Matrix4.Identity;
			lightnode = new Light("Light", Floornode, bigtpotmatrix, new Vector3(5f, 5f, 5f), new Vector3(1,1,1));
			lightnode.position = new Vector4(0,0, 0, 0);

			showTree(sceneGraph.Root, "");
			currentlightpos = new Vector4(1, 1, 1, 0);
        }

		void showTree(Node root, string pref)
        {
			System.Console.WriteLine(pref + root.ID);
			foreach(Node child in root.Children)
            {
				showTree(child, pref + '\t');
            }
        }
		public Matrix4 TWorld
        {
            get { return Tworld; }
        }
	}
}

