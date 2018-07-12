using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

// Provides access to all the common enumerations and structures that have
// been generated in SharpDX from Direct3D SDK header files, along with a 
// number of base classes and helpers
using SharpDX;

// Contains the implementation for 'RenderLoop' and also a System.Windows.Form descendent
// that provides some helpful events for Direct3D app
using SharpDX.Windows;

// Provides access to the DXGI API(where we get outr SwapChain)
using SharpDX.DXGI;

// Provides us with access to the Direct3D 11 types
using SharpDX.Direct3D11;

// Resolve name conflicts by explicitly stating the class to use
//'Device' class in the namespaces SharpDX.DXGI and SharpDX.Direct3D11
// Explicitly state which type should be used with a device using an alias directives
using Device = SharpDX.Direct3D11.Device;


namespace EmptyWinForm
{
    static class Program
    {
        /// <summary>
        /// 해당 응용 프로그램의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            #region Direct3D Initialization
            /// <summary>
            /// Initialize Device and SwapChain
            /// </summary>

            // Create the window to render to
            Form1 form  = new Form1();
            form.Text   = "D3DRendering - emptyproject";
            form.Width  = 1920;
            form.Height = 1080;

            // Declare the device and swapChain vars
            Device                 device;                      // SharpDX.Direct3D11.Device
            SharpDX.DXGI.SwapChain swapChain;

            // Create the device and swapchain
            Device.CreateWithSwapChain(SharpDX.Direct3D.DriverType.Hardware,
                                       DeviceCreationFlags.None,                    // Natively D3D11_CREATE_DEVICE_FLAG
                                       new[]
                                       {
                                            SharpDX.Direct3D.FeatureLevel.Level_11_1,
                                            SharpDX.Direct3D.FeatureLevel.Level_11_0,
                                            SharpDX.Direct3D.FeatureLevel.Level_10_1,
                                            SharpDX.Direct3D.FeatureLevel.Level_10_0,
                                       },                                           // Implicitly-Typed Array

                                       // Describing a back buffer
                                       new SwapChainDescription()                   // Natively DXGI_SWAP_CHAIN_DESC
                                       {
                                            // The feature level also impacts the formats that can be used
                                            ModeDescription = new ModeDescription(form.ClientSize.Width, form.ClientSize.Height,
                                                                                  new Rational(60, 1),       // Refresh rate 60Hz
                                                                                  Format.R8G8B8A8_UNorm),    // Per-pixel 32-bits consisting of four 8-bit unsigned normalized values
                                            // Default sampler mode for no-AA, that is, one sample and a quality of zero
                                            SampleDescription = new SampleDescription(1, 0),
                                            Usage             = SharpDX.DXGI.Usage.BackBuffer | SharpDX.DXGI.Usage.RenderTargetOutput,
                                            BufferCount       = 1,
                                            Flags             = SwapChainFlags.None,
                                            IsWindowed        = true,
                                            OutputHandle      = form.Handle,
                                            // Back buffer contents being discarded after each 'swapChain.Present'
                                            SwapEffect        = SwapEffect.Discard
                                       }, // Use object initializer syntax
                                          // Object initializers let you assign values to any accessible fields or properties of an 
                                          // object at creation time without having to invoke a constructor

                                        out device,
                                        out swapChain
                                      );

            // Retrieve referecences for backBuffer and renderTargetView
            var backBuffer       = SharpDX.Direct3D11.Texture2D.FromSwapChain<Texture2D>(swapChain, 0);         // Gets a swap chain back buffer
            var renderTargetView = new RenderTargetView(device, backBuffer);

            #endregion


            #region Renderloop

            // Create and run the render loop

            /// <summary>
            /// SharpDX.Windows.RenderLoop       : RenderLoop provides a rendering loop infrastructure
            /// SharpDX.Windows.RenderLoop.Run(Control form, RenderLoop.RenderCallback renderCallback) : Runs the specified main loop for the specified windows form 
            /// </summary>
            SharpDX.Windows.RenderLoop.Run(form, 
                                           () => 
                                           {
                                               // Clear the render target with light blue
                                               device.ImmediateContext.ClearRenderTargetView(renderTargetView, Color.Black);

                                               // Execute rendering commands here...

                                               // Present the frame
                                               swapChain.Present(0, PresentFlags.None);
                                           }); // RenderLoop.RenderCallback renderCallback에 lambda expression을 사용

            #endregion


            #region Cleanup D3D

            renderTargetView.Dispose();
            backBuffer.Dispose();
            swapChain.Dispose();
            device.Dispose();
            
            #endregion

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
