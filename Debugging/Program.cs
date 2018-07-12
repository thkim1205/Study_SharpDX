#define DEBUG

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using SharpDX;
using SharpDX.DXGI;
using SharpDX.Windows;
using SharpDX.Direct3D11;

using Device  = SharpDX.Direct3D11.Device;
using Device1 = SharpDX.Direct3D11.Device1;

using Debugging;

namespace D3D11_Debugging
{
    static class Program
    {
        /// <summary>
        /// 해당 응용 프로그램의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            #region Direct3D 11.1 Initialization

            // Enables Direct3D object tracking
            SharpDX.Configuration.EnableObjectTracking = true;

            var form = new Form1();
            form.Text = "D3D11_DebuggingInfo App";
            form.Width = 1920;
            form.Height = 1080;

            Device1 device11_1;
            SharpDX.DXGI.SwapChain1 swapChain1;

            // 1. Create a regular D3D11 device
            // Explicitly excluding feature levels below 11_0 as we will be using SM5.0 and other D3D11 features
#if DEBUG
            using (var device11 = new Device(SharpDX.Direct3D.DriverType.Hardware,
                                              DeviceCreationFlags.Debug,                    // Enables debug flag
                                              new[]
                                              {
                                                   SharpDX.Direct3D.FeatureLevel.Level_11_1,
                                                   SharpDX.Direct3D.FeatureLevel.Level_11_0
                                              }))
            {
                // Query device for the device11_1 interface (ID3D11device11_1)
                device11_1 = device11.QueryInterfaceOrNull<Device1>();
                if (device11_1 == null)
                {
                    throw new NotSupportedException("SharpDX.Direct3D11.Device1 is not supported.");
                }
            }
#else
             using (var device11 = new Device(SharpDX.Direct3D.DriverType.Hardware,
                                               DeviceCreationFlags.None,
                                               new[]
                                               {
                                                   SharpDX.Direct3D.FeatureLevel.Level_11_1,
                                                   SharpDX.Direct3D.FeatureLevel.Level_11_0
                                               }))
            {
                // Query device for the device11_1 interface (ID3D11device11_1)
                device11_1 = device11.QueryInterfaceOrNull<Device1>();
                if (device11_1 == null)
                {
                    throw new NotSupportedException("SharpDX.Direct3D11.Device1 is not supported.");
                }
            }
#endif



            // 2. Initialize Swap Chain
            // To create the swap chain, we need to first get a reference to SharpDX.DXGI.Factory2 instance
            // Rather than creating a new factory, we will use the one that was initialized internally to create our device

            using (var dxgi = device11_1.QueryInterface<SharpDX.DXGI.Device2>())
            using (var adapter = dxgi.Adapter)
            using (var factory = adapter.GetParent<Factory2>())
            {
                // Describing the 
                var desc1 = new SwapChainDescription1()
                {
                    Width = form.ClientSize.Width,
                    Height = form.ClientSize.Height,
                    Format = Format.R8G8B8A8_UNorm,
                    Stereo = false,
                    SampleDescription = new SampleDescription(1, 0),
                    Usage = SharpDX.DXGI.Usage.BackBuffer | SharpDX.DXGI.Usage.RenderTargetOutput,
                    BufferCount = 1,
                    Scaling = SharpDX.DXGI.Scaling.Stretch,
                    SwapEffect = SharpDX.DXGI.SwapEffect.Discard
                };

                swapChain1 = new SwapChain1(factory,
                                            device11_1,
                                            form.Handle,
                                            ref desc1,
                                            new SwapChainFullScreenDescription()
                                            {
                                                RefreshRate = new Rational(60, 1),
                                                Scaling = DisplayModeScaling.Centered,
                                                Windowed = true
                                            },
                                            adapter.Outputs[0]);
            }

            // Retrieve references to backbuffer and rendertargetview
            var backBuffer = Texture2D.FromSwapChain<Texture2D>(swapChain1, 0);
            var renderTargetView = new RenderTargetView(device11_1, backBuffer);

            #endregion

            // Setup object debug name
            device11_1.DebugName = "The Device";
            swapChain1.DebugName = "The SwapChain";
            backBuffer.DebugName = "the BackBuffer";
            renderTargetView.DebugName = "The RenderTargetView";

            #region RenderLoop

            var clock = new System.Diagnostics.Stopwatch();
            var clockFreq = System.Diagnostics.Stopwatch.Frequency;         // Timer tick count per second
            clock.Start();

            var deltaTime = 0.0f;
            var fpsTimer = new System.Diagnostics.Stopwatch();
            fpsTimer.Start();

            var fps = 0.0;
            int fpsFrames = 0;

            SharpDX.Windows.RenderLoop.Run(form,
                                           () =>
                                           {
                                               // Time in seconds
                                               var totalSeconds = clock.ElapsedTicks / clockFreq;

                                               #region FPS and title update
                                               fpsFrames++;
                                               if (fpsTimer.ElapsedMilliseconds > 1000)
                                               {
                                                   fps = 1000.0 * fpsFrames / fpsTimer.ElapsedMilliseconds;

                                                   // Update window title with FPS once every second
                                                   form.Text = string.Format("D3DRendering D3D 11.1 - FPS: {0:F2} ({1:F2}ms/frame)", fps, (float)fpsTimer.ElapsedMilliseconds / fpsFrames);

                                                   // Restart the FPS counter
                                                   fpsTimer.Reset();
                                                   fpsTimer.Start();
                                                   fpsFrames = 0;
                                               }
                                               #endregion

                                               // Execute rendering commends here...
                                               device11_1.ImmediateContext1.ClearRenderTargetView(renderTargetView, Color.Green);
#if DEBUG
                                               // Output the current active Direct3D objects
                                               System.Diagnostics.Debug.Write(SharpDX.Diagnostics.ObjectTracker.ReportActiveObjects());
#endif                                               
                                               // Present the frame
                                               swapChain1.Present(0, PresentFlags.None, new PresentParameters());

                                               // Calculate the time it took to render the frame
                                               deltaTime = (clock.ElapsedTicks / clockFreq) - totalSeconds;
                                           });

            #endregion



            #region Cleanup Direct3D 11.1

            renderTargetView.Dispose();
            backBuffer.Dispose();
            swapChain1.Dispose();
            device11_1.Dispose();

            #endregion
            /*
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
            */
        }
    }
}
