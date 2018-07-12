using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX.Direct3D;
using SharpDX.Direct3D11;

namespace RenderFramework
{
    public class MyDeviceManager : SharpDX.Component
    {
        // Declare Direct3D objects
        protected SharpDX.Direct3D11.Device1        d3dDevice;
        protected SharpDX.Direct3D11.DeviceContext1 d3dContext;
        protected float _Dpi;

        // Declare Direct2D objects
        protected SharpDX.Direct2D1.Device        d2dDevice;
        protected SharpDX.Direct2D1.DeviceContext d2dContext;
        protected SharpDX.Direct2D1.Factory1 d2dFactory;

        // Declare DirectWrite & Windows Imaging Component Objects
        protected SharpDX.DirectWrite.Factory dwriteFactory;
        protected SharpDX.WIC.ImagingFactory2 wicFactory;

        /// <summary>
        /// The list of feature level to accept
        /// </summary>
        public SharpDX.Direct3D.FeatureLevel[] D3DFeatureLevel = new FeatureLevel[]
        {
            FeatureLevel.Level_11_1,
            FeatureLevel.Level_11_0,
        };

        /// <summary>
        /// Gets the Direct3D11(ver 11.1) device
        /// </summary>
        public SharpDX.Direct3D11.Device1 Direct3DDevice { get { return d3dDevice; } }

        /// <summary>
        /// Gets the Direct3D11 immediate context
        /// </summary>
        public SharpDX.Direct3D11.DeviceContext1 Direct3DContext { get { return d3dContext; } }

        /// <summary>
        /// Gets the Direct2D device
        /// </summary>
        public SharpDX.Direct2D1.Device Direct2DDevice { get { return d2dDevice; } }

        /// <summary>
        /// Gets the Direct2D context
        /// </summary>
        public SharpDX.Direct2D1.DeviceContext Direct2DContext { get { return d2dContext; } }
        
        /// <summary>
        /// Gets the Direct2D factory
        /// </summary>
        public SharpDX.Direct2D1.Factory1 Direct2DFactory { get { return d2dFactory; } }

        /// <summary>
        /// Gets the DirectWrite factory
        /// </summary>
        public SharpDX.DirectWrite.Factory DirectWriteFactory { get { return dwriteFactory; } }

        /// <summary>
        /// Gets the Windows Imageing Component factory
        /// </summary>
        public SharpDX.WIC.ImagingFactory2 WICFactory { get { return wicFactory; } }

        /// <summary>
        /// Gets/Sets the DPI
        /// </summary>
        /// <remarks>
        /// This method will fire the event <see cref="OnDpiChanged"/>
        /// if the DPI is modified
        /// </remarks>
        public virtual float Dpi
        {
            get { return _Dpi; }
            set
            {
                if(_Dpi != value)
                {
                    _Dpi = value;
                    d2dContext.DotsPerInch = new SharpDX.Size2F(_Dpi, _Dpi);

                    if(OnDpiChanged != null)
                    {
                        OnDpiChanged(this);
                    }
                }
            }
        }

        /// <summary>
        /// This event is fired when the MyDeviceManager is 
        /// initialized by the <see cref="Initialize"/> method.
        /// </summary>
        public event Action<MyDeviceManager> OnInitialize;

        /// <summary>
        /// This event is fired when the <see cref="Dpi"/> is called.
        /// </summary>
        public event Action<MyDeviceManager> OnDpiChanged;

        /// <summary>
        /// Initialize this instance. 
        /// </summary>
        /// <param name="window"> Window to receive the rendering </param>
        public virtual void Initialize(float dpi = 96.0f)
        {
            CreateInstance();

            if (OnInitialize != null)
            {
                OnInitialize(this);
            }

            Dpi = dpi;
        }
            
        /// <summary>
        /// Create device manager objects
        /// </summary>
        /// <remarks>
        /// This method is called at the initialization of this instance.
        /// </remarks>
        protected virtual void CreateInstance()
        {
            // Dispose previous references and set to null
            RemoveAndDispose(ref d3dDevice);
            RemoveAndDispose(ref d3dContext);
            RemoveAndDispose(ref d2dDevice);
            RemoveAndDispose(ref d2dContext);
            RemoveAndDispose(ref d2dFactory);
            RemoveAndDispose(ref dwriteFactory);
            RemoveAndDispose(ref wicFactory);


            #region Create Direct3D 11.1 device and retrieve device context

            // BGRA performs better especially with Direct2D software render targets
            var creationFlags = SharpDX.Direct3D11.DeviceCreationFlags.BgraSupport;
#if DEBUG
            // Enble D3D device debug layer
            creationFlags |= SharpDX.Direct3D11.DeviceCreationFlags.Debug;
#endif
            // Retrieve the Direct3D 11.1 device and device context
            using (var device = new SharpDX.Direct3D11.Device(DriverType.Hardware,
                                                             creationFlags,
                                                             D3DFeatureLevel))
            {
                d3dDevice = ToDispose(device.QueryInterface<Device1>());
            }

            // Get Direct3D 11.1 immediate context
            d3dContext = ToDispose(d3dDevice.ImmediateContext.QueryInterface<DeviceContext1>());
            #endregion

            #region Create Direct2D device and context

#if DEBUG
            var debugLevel = SharpDX.Direct2D1.DebugLevel.Information;
#endif
            // Allocate new references
            d2dFactory    = ToDispose(new SharpDX.Direct2D1.Factory1(SharpDX.Direct2D1.FactoryType.SingleThreaded, debugLevel));
            dwriteFactory = ToDispose(new SharpDX.DirectWrite.Factory(SharpDX.DirectWrite.FactoryType.Shared));
            wicFactory    = ToDispose(new SharpDX.WIC.ImagingFactory2());

            // Create Direct2D device
            using (var dxgiDevice = d3dDevice.QueryInterface<SharpDX.DXGI.Device>())
            {
                d2dDevice = ToDispose(new SharpDX.Direct2D1.Device(d2dFactory, dxgiDevice));
            }

            // Create Direct2D context
            d2dContext = ToDispose(new SharpDX.Direct2D1.DeviceContext(d2dDevice, 
                                                                       SharpDX.Direct2D1.DeviceContextOptions.None));

            #endregion
        }
    }
}
