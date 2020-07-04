using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinForm
{
    public class PolygonElement : PolygonElementClass, IElement
    {
        private int _Opacity=50;
 
        void IElement.Activate(IDisplay Display)
        {
            base.Activate(Display);
        }
 
        void IElement.Deactivate()
        {
            base.Deactivate();
        }
 
        void IElement.Draw(IDisplay Display, ITrackCancel TrackCancel)
        {
            ITransparencyDisplayFilter filter = new TransparencyDisplayFilterClass {
                Transparency = (short) ((this._Opacity * 0xff) / 100)
            };
            IDisplayFilter filter2 = null;
            if (Display.Filter != null)
            {
                filter2 = (Display.Filter as IClone).Clone() as IDisplayFilter;
            }
            Display.Filter = filter;
            base.Draw(Display, TrackCancel);
            Display.Filter = filter2;
        }
 
        bool IElement.HitTest(double x, double y, double Tolerance)
        {
            return base.HitTest(x, y, Tolerance);
        }
 
        void IElement.QueryBounds(IDisplay Display, IEnvelope Bounds)
        {
            base.QueryBounds(Display, Bounds);
        }
 
        void IElement.QueryOutline(IDisplay Display, IPolygon Outline)
        {
            base.QueryOutline(Display, Outline);
        }
 
        IGeometry IElement.Geometry
        {
            get
            {
                return base.Geometry;
            }
            set
            {
                base.Geometry = value;
            }
        }
 
        bool IElement.Locked
        {
            get
            {
                return base.Locked;
            }
            set
            {
                base.Locked = value;
            }
        }
 
        ISelectionTracker IElement.SelectionTracker
        {
            get
            {
                return base.SelectionTracker;
            }
        }
 
        public int Opacity
        {
            get
            {
                return this._Opacity;
            }
            set
            {
                this._Opacity = value;
            }
        }
    }
}
