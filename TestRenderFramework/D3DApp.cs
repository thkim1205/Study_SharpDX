using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX.D3DCompiler;

using RenderFramework;

using Buffer = SharpDX.Direct3D11.Buffer;

namespace RenderPrimitives
{
    public class D3DApp : D3DApplicationDesktop
    {
        // The vertex shader
        SharpDX.D3DCompiler.ShaderBytecode vertexShaderBytecode;
        SharpDX.Direct3D11.VertexShader    vertexShader;

        // The pixel shader
        SharpDX.D3DCompiler.ShaderBytecode pixelShaderBytecode;
        SharpDX.Direct3D11.PixelShader     pixelShader;

        // A Vertex shader that gives depth info to pixel shader
        SharpDX.D3DCompiler.ShaderBytecode depthVertexShaderBytecode;
        SharpDX.Direct3D11.VertexShader    depthVertexShader;







       public D3DApp(System.Windows.Forms.Form window) : base(window)
        {
        }

        protected override SwapChainDescription1 CreateSwapChainDescription()
        {
            return base.CreateSwapChainDescription();
        }

        protected override void CreateDeviceDependentResources(DeviceManager deviceManager)
        {
            base.CreateDeviceDependentResources(deviceManager);

            // Release all resources


            // Get a reference to the Device1 instance and immediate context

            #region Compile and create the shaders
            // Compile and create the vertex shader

            // Comopile and create the pixel shader

            // Compile and create the depth vertex and pixel shaders
            // These shaders are for checking what the depth buffer should look like
            #endregion


            #region 

            // Layout from VertexShader input signature

            // Create the constant buffer that will store our worldViewProjection matrix

            // Configure the depth buffer to discard pixels that are further than the current pixel.

            // Tell the IA what the vertices will look like 
            // in this case two 4-component 32bit floats
            // (32 bytes in total)

            // Set our constant buffer ( to store worldViewProjection )

            // Set the vertex shader to run

            // Set the pixel shader to run

            // Set our depth stencil state
        }

        protected override void CreateSizeDependentResources(D3DApplicationBase app)
        {
            base.CreateSizeDependentResources(app);
        }

        public override void Run()
        {

            #region Create renderers

            // Note : the renderers take care of creating their own device resources 
            //        and listen for DeviceManager.OnInitialize

            // Create and initialize the axis lines renderer

            // Create and initialize the triangle renderer

            // Create and initialize the quad renderer

            // Create and initialize a Direct2D FPS text renderer

            // Create and initialize a general purpose Direct2D text renderer
            // This will display some instructions and the current view and rotation offsets

            #endregion

            #region Set up a WVP transformation matrix
            // Initialize the world matrix


            // Set the camera position slightly to the right (x), above(y) and behind (-z)s


            // Prepare space transformation matrices
            // Create the view matrix from our camera position, look target and up direction


            // Create the projection matrix
            // FOV 60deg = Pi/3 radians
            // Aspect ratio (based on window size), near clip, far clip


            // Maintain the correct aspect ratio on resize

            #endregion




            #region Window event handlers
            #endregion


            #region Set up a rotation matrix
            #endregion

            #region Render loop
            #endregion






            throw new NotImplementedException();
        }



    }
}
