using System;
using System.Runtime.InteropServices;
using System.Text;

using System.Threading.Tasks;
using SFML;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace QuartRenderPlayground
{
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
        public static extern int initQuartRender();
        [DllImport("QuartRender.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr createLogger();
        [DllImport("QuartRender.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr createRenderer(IntPtr errorLog, uint sizex, uint sizey, uint rendererType);
        [DllImport("QuartRender.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int renderImage(IntPtr renderer, IntPtr errorLog);
        [DllImport("QuartRender.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern void getRenderImage(IntPtr renderer, byte**imgbug, uint *sizex, uint *sizey);
        [DllImport("QuartRender.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern int getAndAllowClose(IntPtr renderer, IntPtr errorLog, bool *val);
        [DllImport("QuartRender.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int exitQuartRender();


        //TESTS!!
        [DllImport("QuartRender.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int drawTest(IntPtr renderer, IntPtr errorLog, string name, float posx, float posy);
        [DllImport("QuartRender.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int destroyAllDrawTests();
        [DllImport("QuartRender.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int testFunc(string arg);

        [DllImport("QuartRender.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void windowTest();

        [DllImport("QuartRender.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern void imageTest(byte**imgbuf,uint*sizex, uint*sizey);

        [DllImport("QuartRender.dll", CallingConvention = CallingConvention.Cdecl)]
        unsafe public static extern int renderImageTest(byte** imgbuf, uint *sizex, uint *sizey);

        [DllImport("QuartRender.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int startTestRenderer(uint sizex, uint sizey);

        [DllImport("QuartRender.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int stopTestRenderer();
        //TESTS!!

        unsafe static Image getRenderImageTest()
        {
            if (startTestRenderer(1000, 1000) != 0)
            {
                Console.WriteLine("test renderer could not start!");
            }
            else { 
                Console.WriteLine("test renderer could start");
            }
            byte* buf;
            uint sizex;
            uint sizey;
            if (renderImageTest(&buf, &sizex, &sizey) != 0)
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
            stopTestRenderer();
            return new Image(sizex,sizey,finalbuf);
        }

        unsafe static Image getImage(string whichFunc = "")
        {
            byte* buf;
            uint sizex;
            uint sizey;
            if (whichFunc == "imageTest")
            {
                imageTest(&buf, &sizex, &sizey);
            }
            else
            {
                //Console.WriteLine("choosed render image!");
                getRenderImage(m_renderer, &buf, &sizex, &sizey);
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
            getAndAllowClose(renderer,errorLog,&retval);
            return retval;
        }

        static void Main(string[] args)
        {
            initQuartRender();
            m_errorLog = createLogger();
            m_renderer = createRenderer(m_errorLog,1000,1000, (uint)RendererTypes.onscreenRendererIMGUI);


            IntPtr tempErrorLog = createLogger();
            IntPtr tempRenderer = createRenderer(tempErrorLog, 1000, 1000, (uint)RendererTypes.onscreenRenderer);

            Console.WriteLine("renderer call finished.");
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
            window.SetFramerateLimit(0);
            window.SetActive();





            //sprite.Position = new Vector2f(100,100);
            double i = 0;
            window.Closed += OnClose;
            window.Resized += OnResize;
            Time time;
            Clock clock = new Clock();
            while (window.IsOpen)
            {
                time = clock.Restart();

                Console.WriteLine(1/time.AsSeconds());

                i -= Math.Truncate(i);
                i += time.AsSeconds();

                if (!safeGetAndAllowClose(m_renderer, m_errorLog))
                {

                    drawTest(m_renderer, m_errorLog, "asd", (float)i, 0);
                    drawTest(m_renderer, m_errorLog, "asd2", 0, (float)i);
                    renderImage(m_renderer, m_errorLog);
                    //destroyAllDrawTests();
                }

                if (!safeGetAndAllowClose(tempRenderer, tempErrorLog))
                {
                    drawTest(tempRenderer, tempErrorLog, "asd",0,0);
                    renderImage(tempRenderer, tempErrorLog);
                }





                window.Clear();
                window.DispatchEvents();
                window.Draw(cs);
                //window.Draw(sprite);

                window.Display();
            }

            //windowTest();
            //Console.WriteLine(testFunc("asdasd"));
            exitQuartRender();
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
