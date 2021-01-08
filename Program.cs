using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

using System.Threading.Tasks;
using SFML;
using SFML.Graphics;
using SFML.System;
using SFML.Window;






namespace QuartRenderPlayground
{

    [StructLayout(LayoutKind.Sequential)]
    struct PlanetCharacteristics
    {
        public double radius;
    }


    [StructLayout(LayoutKind.Sequential)]
    struct KeyboardInput
    {
        public int key;
        public int scancode;
        public int action;
        public int mods;
        public bool capturedByIMGUI;
        public bool isValid;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct CursorPosition
    {
        public double xpos;
        public double ypos;
        public bool capturedByIMGUI;

        public string getDisplayString()
        {
            string retval = "";
            retval += "{x position: " + xpos.ToString() + ", y position: " + ypos.ToString() + ", is captured by imgui?: " + capturedByIMGUI.ToString() + "}";
            return retval;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    struct ScrollInput
    {
        public double xoffset;
        public double yoffset;
        public CursorPosition cursorPosition;
        public bool isValid;

        public string getDisplayString()
        {
            string retval = "";
            retval += "{x offset: " + xoffset.ToString() +", y offset: "+ yoffset.ToString()+", cursorPosition: "+cursorPosition.getDisplayString()+", is valid?: "+isValid.ToString() +"}" ;
            return retval;
        }
    };


    [StructLayout(LayoutKind.Sequential)]
    struct testStruct
    {
        int ID;
        UInt16 data16;
        byte data8;

        public void display()
        {
            Console.WriteLine(ID);
            Console.WriteLine(data16);
            Console.WriteLine(data8);
        }
    }
    class Program
    {
        static private RenderWindow window;

        static private IntPtr m_errorLog;
        static private IntPtr m_renderer;

        enum RendererTypes:uint
        {
            onscreenRenderer, onscreenRendererIMGUI, offscreenRenderer, ENUM_MAX
        };

        [DllImport("QuartRender.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int quartRender_initQuartRender();
        [DllImport("QuartRender.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr quartRender_createLogger();
        [DllImport("QuartRender.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr quartRender_createRenderer(IntPtr errorLog, uint sizex, uint sizey, uint rendererType);
        [DllImport("QuartRender.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int quartRender_renderImage(IntPtr renderer, IntPtr errorLog);
        [DllImport("QuartRender.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern void quartRender_getRenderImage(IntPtr renderer, byte**imgbug, uint *sizex, uint *sizey);
        [DllImport("QuartRender.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern int quartRender_getAndAllowClose(IntPtr renderer, IntPtr errorLog, bool *val);
        [DllImport("QuartRender.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int quartRender_exitQuartRender();
        [DllImport("QuartRender.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern void quartRender_getLogString(IntPtr errorLog, StringBuilder str, uint* len);

        //input
        [DllImport("QuartRender.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern int quartRender_getAndPopLastKeyboardInput(IntPtr renderer, IntPtr errorLog, KeyboardInput *keyboardInput);
        [DllImport("QuartRender.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern int quartRender_getCurrentCursorPosition(IntPtr renderer, IntPtr errorLog, CursorPosition *cursorPosition);
        [DllImport("QuartRender.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern int quartRender_getAndPopLastScrollInput(IntPtr renderer, IntPtr errorLog, ScrollInput *scrollInput);

        //~input

        //drawing
        [DllImport("QuartRender.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int quartRender_createPlanet(IntPtr errorlog, string planetClassName, PlanetCharacteristics planetCharacteristics);
        [DllImport("QuartRender.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int quartRender_drawPlanet(IntPtr renderer, IntPtr errorLog, string planetClassName, string planetName, double posx, double posy);
        //~drawing


        //camara
        [DllImport("QuartRender.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int quartRender_zoom(IntPtr renderer, IntPtr errorLog, double delta);
        [DllImport("QuartRender.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double quartRender_getZoom(IntPtr renderer);
        [DllImport("QuartRender.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int quartRender_setZoom(IntPtr renderer, IntPtr errorLog, double value);
        [DllImport("QuartRender.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int quartRender_displace(IntPtr renderer, IntPtr errorLog, double displacex, double displacey, double displacez);
        //~camara

        //imgui


        //TESTS!!
        //newer tests


            [DllImport("QuartRender.dll", CallingConvention = CallingConvention.Cdecl)]
            public static extern int quartRender_drawTest(IntPtr renderer, IntPtr errorLog, string name, float posx, float posy);
            [DllImport("QuartRender.dll", CallingConvention = CallingConvention.Cdecl)]
            public static extern int quartRender_drawCubeTest(IntPtr renderer, IntPtr errorLog, double posx, double posy, double posz, double anglex, double angley, double anglez);
        //weird tests
            [DllImport("QuartRender.dll", CallingConvention = CallingConvention.Cdecl)]
            public static extern int quartRender_addTestError(IntPtr errorLog);
            [DllImport("QuartRender.dll", CallingConvention = CallingConvention.Cdecl)]
            public static extern string quartRender_runTests();

            [DllImport("QuartRender.dll", CallingConvention = CallingConvention.Cdecl)]
            public static extern int quartRender_destroyAllDrawTests();
            [DllImport("QuartRender.dll", CallingConvention = CallingConvention.Cdecl)]
            public static extern int quartRender_testFunc(string arg);



            [DllImport("QuartRender.dll", CallingConvention = CallingConvention.Cdecl)]
            unsafe public static extern void quartRender_imageTest(byte**imgbuf,uint*sizex, uint*sizey);

            [DllImport("QuartRender.dll", CallingConvention = CallingConvention.Cdecl)]
            unsafe public static extern int quartRender_renderImageTest(byte** imgbuf, uint *sizex, uint *sizey);

            [DllImport("QuartRender.dll", CallingConvention = CallingConvention.Cdecl)]
            public static extern int quartRender_startTestRenderer(uint sizex, uint sizey);

            [DllImport("QuartRender.dll", CallingConvention = CallingConvention.Cdecl)]
            public static extern int quartRender_stopTestRenderer();
            [DllImport("QuartRender.dll", CallingConvention = CallingConvention.Cdecl)]
            unsafe public static extern void quartRender_testStringFunc(StringBuilder str, uint *len);
            [DllImport("QuartRender.dll", CallingConvention = CallingConvention.Cdecl)]
            unsafe public static extern int quartRender_structPassTest(testStruct *tststrc);
        //full tests(meant to be used standalone)
        [DllImport("QuartRender.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void quartRender_windowTest();
        [DllImport("QuartRender.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void quartRender_full3DWindowTest();

        //TESTS!!



        unsafe static ScrollInput safeGetScrollInput(IntPtr renderer, IntPtr errorLog)
        {
            ScrollInput retval;
            quartRender_getAndPopLastScrollInput(renderer, errorLog,&retval);
            return retval;
        }
        unsafe static CursorPosition safeGetCursorPosition(IntPtr renderer, IntPtr errorLog)
        {
            CursorPosition retval;
            quartRender_getCurrentCursorPosition(renderer, errorLog, &retval);
            return retval;
        }

        unsafe static KeyboardInput safeGetLastKeyboardInput(IntPtr renderer, IntPtr errorLog)
        {
            KeyboardInput retval;
            quartRender_getAndPopLastKeyboardInput(renderer, errorLog, &retval);
            return retval;
        }

        unsafe static testStruct safeStructPassTest()
        {
            testStruct tst;
            quartRender_structPassTest(&tst);
            return tst;
        }


        unsafe static string safeGetTestString()
        {
            uint len;
            quartRender_testStringFunc(null, &len);
            StringBuilder ret = new StringBuilder((int)len);

            quartRender_testStringFunc(ret, &len);
            return ret.ToString();
        }

        unsafe static string safeGetLogString(IntPtr errorLog)
        {
            uint len;
            quartRender_getLogString(errorLog,null,&len);
            StringBuilder ret = new StringBuilder((int)len);
            quartRender_getLogString(errorLog,ret,&len);
            return ret.ToString();
        }
        

        unsafe static Image getRenderImageTest()
        {
            if (quartRender_startTestRenderer(1000, 1000) != 0)
            {
                Console.WriteLine("test renderer could not start!");
            }
            else { 
                Console.WriteLine("test renderer could start");
            }
            byte* buf;
            uint sizex;
            uint sizey;
            if (quartRender_renderImageTest(&buf, &sizex, &sizey) != 0)
            {
                Console.WriteLine("render image test failed!");
                return new Image(1,1);
            }
            else
            {
                Console.WriteLine("render image test succeeded");
            }

            byte[] finalbuf = new byte[sizex*sizey*4];
            for (int i = 0; i < (sizex * sizey * 4); i++)
            {
                finalbuf[i] = *(buf + i);
            }
            quartRender_stopTestRenderer();
            return new Image(sizex,sizey,finalbuf);
        }

        unsafe static Image getImage(string whichFunc = "")
        {
            byte* buf;
            uint sizex;
            uint sizey;
            if (whichFunc == "imageTest")
            {
                quartRender_imageTest(&buf, &sizex, &sizey);
            }
            else
            {
                //Console.WriteLine("choosed render image!");
                quartRender_getRenderImage(m_renderer, &buf, &sizex, &sizey);
            }
            //Console.WriteLine("x and y dim: ");
            //Console.WriteLine(sizex);
            //Console.WriteLine(sizey);
            byte[] finalbuf = new byte[sizex * sizey * 4];
            for (int i = 0; i < (sizex * sizey * 4); i++)
            {
                finalbuf[i] = *(buf + i);
            }
            return new Image(sizex, sizey, finalbuf);
        }
        unsafe static Image getImageTest()
        {

            return getImage("imageTest");
        }


        unsafe static bool safeGetAndAllowClose(IntPtr renderer, IntPtr errorLog)
        {
            bool retval;
            quartRender_getAndAllowClose(renderer,errorLog,&retval);
            return retval;
        }

        static void Main(string[] args)
        {
            //quartRender_full3DWindowTest();
            //safeStructPassTest().display();
            Console.WriteLine(quartRender_runTests());
            quartRender_initQuartRender();
            m_errorLog = quartRender_createLogger();
            m_renderer = quartRender_createRenderer(m_errorLog,1000,1000, (uint)RendererTypes.onscreenRendererIMGUI);

            PlanetCharacteristics tempPlanetCharacteristics;
            tempPlanetCharacteristics.radius = 1000;
            quartRender_createPlanet(m_errorLog,"earth", tempPlanetCharacteristics);


            //IntPtr tempErrorLog = quartRender_createLogger();
            //IntPtr tempRenderer = quartRender_createRenderer(tempErrorLog, 1000, 1000, (uint)RendererTypes.onscreenRenderer);

            if (m_renderer == null)
            {
                Console.WriteLine("failed to create renderer!");
            }





            SFML.Window.VideoMode mode = new SFML.Window.VideoMode();
            mode.Height = 1000;
            mode.Width = 1000;
            window = new RenderWindow(mode, "test");
            CircleShape cs = new CircleShape(100.0f);
            cs.FillColor = Color.Green;
            window.SetFramerateLimit(60);
            window.SetActive();



            bool dHeld = false;
            bool aHeld = false;
            bool spaceHeld = false;

            double angleAcumulator = 0;

            //sprite.Position = new Vector2f(100,100);
            double i = 0;
            window.Closed += OnClose;
            window.Resized += OnResize;
            Time time;
            Clock clock = new Clock();
            while (window.IsOpen)
            {
                time = clock.Restart();
                


                i -= Math.Truncate(i);
                i += time.AsSeconds();
                double delta_time = time.AsSeconds();

                
                if (!safeGetAndAllowClose(m_renderer, m_errorLog))
                {

                    //drawTest(m_renderer, m_errorLog, "asd", (float)i, 0);
                    //quartRender_drawTest(m_renderer, m_errorLog, "asd2", 0, (float)i);
                    if (true)
                    {

                    
                    if(quartRender_drawPlanet(m_renderer, m_errorLog, "earth", "sun", 0,0) == -1)
                    {
                        Console.WriteLine(safeGetLogString(m_errorLog));
                        Console.WriteLine("PLANET WAS NOT DRAWN DUE TO ERROR!");
                    }

                    if (quartRender_drawPlanet(m_renderer, m_errorLog, "earth", "earth", 30000, 0) == -1)
                    {
                        Console.WriteLine("PLANET WAS NOT DRAWN DUE TO ERROR!");
                    }

                    if (false)
                    {

                        
                    if (quartRender_drawPlanet(m_renderer, m_errorLog, "earth", "moon", 1+(384400/(float)149600000), 0) == -1)
                    {
                        Console.WriteLine("PLANET WAS NOT DRAWN DUE TO ERROR!");
                    }
                    }



                    }
                    if (false) {
                        if (!spaceHeld)
                        {
                            angleAcumulator += delta_time * 80;
                        }
                    
                    if(quartRender_drawCubeTest(m_renderer,m_errorLog,0.0f,0.0f,0.0f, angleAcumulator, angleAcumulator, angleAcumulator) == -1)
                    {
                        Console.WriteLine("cube was not drawn!");
                        Console.WriteLine(safeGetLogString(m_errorLog));
                    }
                    }

                    ScrollInput scrollInput;

                    while ((scrollInput = safeGetScrollInput(m_renderer,m_errorLog)).isValid)
                    {
                        //Console.WriteLine(scrollInput.getDisplayString());
                        if (!scrollInput.cursorPosition.capturedByIMGUI)
                        {
                            double zoomMultiplyChange = 1.1;
                            double currentZoomMultiplier = quartRender_getZoom(m_renderer);

                            quartRender_setZoom(m_renderer, m_errorLog, currentZoomMultiplier*Math.Pow(zoomMultiplyChange,scrollInput.yoffset));
                            if (scrollInput.yoffset > 0)
                            {
                                float displacex = (float)scrollInput.cursorPosition.xpos, displacey = (float)scrollInput.cursorPosition.ypos;
                                displacex = -(displacex - 500) / (500);
                                displacey = (displacey - 500) / (500);
                                
                                //quartRender_displace(m_renderer,m_errorLog,displacex,displacey,0);
                            }
                            Console.WriteLine(currentZoomMultiplier);
                        }
 
                        //Console.WriteLine(scrollInput.yoffset);
                    }

                    bool temp = false;

                    if(!temp) { 
                        ImGuiNET.ImGui.ShowDemoWindow(ref temp);
                    }


                    quartRender_renderImage(m_renderer, m_errorLog);


                    CursorPosition cursorPos = safeGetCursorPosition(m_renderer, m_errorLog);
                    //Console.WriteLine("pos x: {0}, pos y: {1}, captured by IMGUI: {2}",cursorPos.xpos,cursorPos.ypos,cursorPos.capturedByIMGUI);


                    double displaceFactor = delta_time / quartRender_getZoom(m_renderer);

                    if (dHeld)
                    {
                        //i += time.AsSeconds();
                        quartRender_displace(m_renderer, m_errorLog, -displaceFactor, 0, 0);
                    }

                    if (aHeld)
                    {
                        //i += time.AsSeconds();
                        quartRender_displace(m_renderer, m_errorLog, displaceFactor, 0, 0);
                    }


                    KeyboardInput input;
                    while ((input = safeGetLastKeyboardInput(m_renderer, m_errorLog)).isValid)
                    {
                        if (input.capturedByIMGUI)
                        {
                            continue;
                        }
                        else if (input.key == Encoding.ASCII.GetBytes("D")[0] && input.action == 1)
                        {
                            dHeld = true;
                            //Console.WriteLine("W");

                        }
                        else if (input.key == Encoding.ASCII.GetBytes("D")[0] && input.action == 0)
                        {
                            dHeld = false;
                        }

                        else if (input.key == Encoding.ASCII.GetBytes("A")[0] && input.action == 1)
                        {
                            aHeld = true;
                            //Console.WriteLine("W");
                        }
                        else if (input.key == Encoding.ASCII.GetBytes("A")[0] && input.action == 0)
                        {
                            aHeld = false;
                        }
                        else if (input.key == Encoding.ASCII.GetBytes(" ")[0] && input.action == 1)
                        {
                            spaceHeld = true;
                        }
                        else if (input.key == Encoding.ASCII.GetBytes(" ")[0] && input.action == 0)
                        {
                            spaceHeld = false;
                        }
                    }

                }

                //Console.WriteLine(1 / time.AsSeconds());
                //quartRender_addTestError(m_errorLog);
                //Console.WriteLine(safeGetLogString(m_errorLog));

                /*
                if (!safeGetAndAllowClose(tempRenderer, tempErrorLog))
                {
                    quartRender_drawTest(tempRenderer, tempErrorLog, "asd",0,0);
                    quartRender_renderImage(tempRenderer, tempErrorLog);
                }
                */




                window.Clear();
                window.DispatchEvents();
                window.Draw(cs);
                //window.Draw(sprite);

                window.Display();
            }

            //windowTest();
            //Console.WriteLine(testFunc("asdasd"));
            quartRender_exitQuartRender();
        }

        static private void OnResize(object sender, SizeEventArgs e)
        {
            Console.WriteLine(e.Height);
            Console.WriteLine(e.Width);
        }

        static private void OnClose(object sender, EventArgs e)
        {
            
            window.Close();
        }
    }
}
