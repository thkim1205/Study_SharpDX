using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX.Windows;

using Device1 = SharpDX.Direct3D11.Device1;
using Buffer  = SharpDX.Direct3D11.Buffer;

namespace RenderFramework
{
    public abstract class D3DAppBase : SharpDX.Component
    {
        MyDeviceManager           _deviceManager;

        // There presentation capabilities consist of specifying dirty rectangles and scroll rectangle to optimize presentation 
        SharpDX.DXGI.SwapChain1 _swapChain1;

        SharpDX.Direct3D11.RenderTargetView _renderTargetView;
        SharpDX.Direct3D11.DepthStencilView _depthStencilView;
        SharpDX.Direct3D11.Texture2D        _backBuffer;
        SharpDX.Direct3D11.Texture2D        _depthBuffer;

        // A bitmap can be used as a surface for SharpDX.Direct2D1.DeviceContext
        // or mapped into system memory, and can contain additional color context information
        SharpDX.Direct2D1.Bitmap1           _bitmapTarget;
        
        /// <summary>
        /// Gets the configured bounds of the control used to render to 
        /// </summary>
        public SharpDX.Rectangle Bounds { get; protected set; }

        /// <summary>
        /// Gets the configured bounds of the control used to render to 
        /// </summary>
        public abstract SharpDX.Rectangle CurrentBounds { get; }

        /// <summary>
        /// Gets the <see cref="MyDeviceManager"/> attached to this instance
        /// </summary>
        public MyDeviceManager DeviceManager { get { return _deviceManager; } }

        /// <summary>
        /// Gets the <see cref="SharpDX.DXGI.SwapChain1"/> created by this instance
        /// </summary>
        public SharpDX.DXGI.SwapChain1 SwapChain1 { get { return _swapChain1; } }

        /// <summary>
        /// Provides access to the list of available display modes.
        /// </summary>
        public SharpDX.DXGI.ModeDescription[] DisplayModeList { get; private set; }

        /// <summary>
        /// Gets/sets whether the swap chain will wait for the 
        /// next vertical sync before presenting.
        /// </summary>
        /// <remarks>
        /// Changes the behavior of the <see cref="D3DAppBase.Present"/>method
        /// </remarks>
        public bool VSync { get; set; }

        /// <summary>
        /// Height of the swap chain buffers
        /// </summary>
        public virtual int Width
        {
            get
            {
                return (int)(Bounds.Width * DeviceManager.Dpi / 96.0);
            }
        }

        /// <summary>
        /// Height of the swap chain buffers
        /// </summary>
        public virtual int Height
        {
            get
            {
                return (int)(Bounds.Height * DeviceManager.Dpi / 96.0);
            }
        }

        // get; set; -> auto implemented property
        protected SharpDX.ViewportF Viewport { get; set; }
        protected SharpDX.Rectangle RenderTargetBounds { get; set; }
        protected SharpDX.Size2     RenderTargetSize { get { return new SharpDX.Size2(RenderTargetBounds.Width, RenderTargetBounds.Height); } }

        public SharpDX.Direct3D11.RenderTargetView RenderTargetView
        {
            get
            {
                return _renderTargetView;
            }

            protected set
            {
                if(_renderTargetView != value)
                {
                    RemoveAndDispose(ref _renderTargetView);
                    _renderTargetView = value;
                }
            }
        }

        /// <summaray>
        /// Gets or sets the back buffer used by this app.
        /// </summaray>
        public SharpDX.Direct3D11.Texture2D BackBuffer
        {
            get
            {
                return _backBuffer;
            }

            protected set
            {
                if(_backBuffer != value)
                {
                    RemoveAndDispose(ref _backBuffer);
                    _backBuffer = value;
                }
            }
        }

        /// <summaray> 
        /// Gets or sets the depthbuffer used by this app
        /// </summaray>
        public SharpDX.Direct3D11.Texture2D DepthBuffer
        {
            get
            {
                return _depthBuffer;
            }

            protected set
            {
                if(_depthBuffer != value)
                {
                    RemoveAndDispose(ref _depthBuffer);
                    _depthBuffer = value;
                }
            }
        }

        /// <summaray>
        /// Gets the Direct3D DepthStencilView used by this app
        /// </summaray>
        public SharpDX.Direct3D11.DepthStencilView DepthStencilView
        {
            get
            {
                return _depthStencilView;
            }
            protected set
            {
                if(_depthStencilView != value)
                {
                    RemoveAndDispose(ref _depthStencilView);
                    _depthStencilView = value;
                }
            }
        }

        /// <summary>
        /// Gets the Direct2D RenderTarget used by this app.
        /// </summary>
        public SharpDX.Direct2D1.Bitmap1 BitmapTarget2D
        {
            get { return _bitmapTarget; }
            protected set
            {
                if (_bitmapTarget != value)
                {
                    RemoveAndDispose(ref _bitmapTarget);
                    _bitmapTarget = value;
                }
            }
        }

        /// <summaray>
        /// Default Constructor
        /// </summaray>
        public D3DAppBase()
        {
            // Create our device manager instance.
            // This encapsulates the creation of Direct3D and Direct2D devices
            _deviceManager = ToDispose(new MyDeviceManager());

            // If the device needs to be reinitialized, make sure we
            // are able to recreate our device dependent resources.
            //DeviceManager.OnInitialize += 
        }

        ///<summaray>
        /// Trigger the OnSizeChanged event if the width and height of the <see cref="CurrentBounds"/>
        /// differs to the last call to SizeChanged
        /// </summaray>
        protected void SizeChanged(bool force = false)
        {
            var newBounds = CurrentBounds;

            // Ignore invalid sizes, 
        }

        /// <summary>
        /// Create device dependent resources
        /// </summary>
        /// <param name="deviceManager"></param>
        protected virtual void CreateDeviceDependentResources(MyDeviceManager deviceManager)
        {
            if(_swapChain1 != null)
            {
                // Release the swap chain
                RemoveAndDispose(ref _swapChain1);

                // Force reinitialize size dependent resources
                SizeChanged(true);
            }
        }

        /// <summaray>
        /// Create size dependent resoruces, in this case the swap chain and render targets
        /// </summaray>
        /// <param name = "app"></param>
        protected virtual void CreateSizeDependentResources(D3DAppBase app)
        {
        
        }








    }
}
