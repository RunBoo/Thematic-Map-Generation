using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.DataSourcesGDB;

namespace WinForm
{
    public partial class IntensityPoint : Form
    {
        public string dataFile = null;  //记录数据库位置
        public string time = null;     //时间
        IFeatureLayer pFeatureLayerPoint = null;     //地震震中图层
        IFeatureLayer pFeatureLayer8LD = null;     //“烈度8”图层
        IFeatureLayer pFeatureLayer7LD = null;     //“烈度7”图层
        IFeatureLayer pFeatureLayer6LD = null;     //“烈度6”图层
        private static object _missing = Type.Missing;
        //生成椭圆使用
        AxPageLayoutControl _MainPageLayoutControl = null;
        //声明一个委托类型事件，用于将震中数据点传回给Form1
        public event setIPoint setFormIPoint;

        public IntensityPoint(AxMapControl axMapControl1, AxPageLayoutControl pMainPageLayoutControl, string file)
        {
            InitializeComponent();
            dataFile = file;
            //获取“地震震中”图层
            pFeatureLayerPoint = axMapControl1.Map.get_Layer(0) as IFeatureLayer;
            pFeatureLayer8LD = axMapControl1.Map.get_Layer(7) as IFeatureLayer;
            pFeatureLayer7LD = axMapControl1.Map.get_Layer(8) as IFeatureLayer;
            pFeatureLayer6LD = axMapControl1.Map.get_Layer(9) as IFeatureLayer;
            _MainPageLayoutControl = pMainPageLayoutControl;
        }

        //确定键
        private void buttonOK_Click(object sender, EventArgs e)
        {
            double dL = 0.0, dB = 0.0, dZJ = 0.0, dTransAngle = 0.0;
            bool bL = double.TryParse(textBoxLong.Text, out dL);//经度
            bool bB = double.TryParse(textBoxLat.Text, out dB);//纬度
            bool bZJ = double.TryParse(textBoxLevel.Text, out dZJ);//震级
            bool bTransAngle = double.TryParse(txtTransAngle.Text, out dTransAngle);//旋转角
            time = textBoxTime.Text;

            //开启编辑状态
            IWorkspaceFactory pWorkspaceFactory = new AccessWorkspaceFactoryClass();
            IFeatureWorkspace pFeatureWorkspace = pWorkspaceFactory.OpenFromFile(dataFile, 0) as IFeatureWorkspace;
            IWorkspaceEdit pWorkspaceEdit = pFeatureWorkspace as IWorkspaceEdit;
            pWorkspaceEdit.StartEditing(false);
            pWorkspaceEdit.StartEditOperation();
            //插入震中点数据
            IFeatureClass pFeatureClassPoint = pFeatureLayerPoint.FeatureClass;
            IFeatureClassWrite fwritePoint = pFeatureClassPoint as IFeatureClassWrite;
            IFeature pFeaturePoint = pFeatureClassPoint.CreateFeature();
            IPointCollection pointCollection = new MultipointClass();
            IPoint pPoint = new PointClass();
            IGeoDataset pGeoDataset = pFeatureClassPoint as IGeoDataset;
            //记录空间投影信息
            ISpatialReference spatialReference = pGeoDataset.SpatialReference;
            //输入经纬度
            pPoint.PutCoords(dL, dB);
            pPoint.SpatialReference = spatialReference;
            pointCollection.AddPoint(pPoint, ref _missing, ref _missing);
            pFeaturePoint.Shape = pointCollection as IGeometry;
            //设置“标注”属性值
            pFeaturePoint.set_Value(3, time);
            fwritePoint.WriteFeature(pFeaturePoint);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeaturePoint);
            //记录屏幕显示区域
            IEnvelope pEnvelope = null;

            //生成地震烈度椭圆
            //当都输入值的时候，生成椭圆
            if (bL && bB && bZJ && bTransAngle)
            {
                IGraphicsContainer graphicsContainer = _MainPageLayoutControl.ActiveView.FocusMap as IGraphicsContainer;
                string sElementName = "zjEllipticArc";
                DelectElementByName(graphicsContainer, sElementName);
                List<EllipticArcPro> pListEllipticArcPro = CalculatEllipticArc(pPoint, dZJ, dTransAngle);
                int tot = pListEllipticArcPro.Count;
                int iCount = 1;
                foreach (EllipticArcPro pEllipticArcPro in pListEllipticArcPro)
                {
                    IEllipticArc pEll = new EllipticArcClass();
                    if (tot == 3)
                    {
                        pEll.PutCoordsByAngle(pEllipticArcPro.ellipseStd, pEllipticArcPro.CenterPoint, pEllipticArcPro.FromAngle, pEllipticArcPro.CentralAngle, pEllipticArcPro.rotationAngle, pEllipticArcPro.semiMajor / 100000, pEllipticArcPro.minorMajorRatio);
                    }
                    else
                    {
                        pEll.PutCoordsByAngle(pEllipticArcPro.ellipseStd, pEllipticArcPro.CenterPoint, pEllipticArcPro.FromAngle, pEllipticArcPro.CentralAngle, pEllipticArcPro.rotationAngle, pEllipticArcPro.semiMajor / 100000, pEllipticArcPro.minorMajorRatio);
                    }
                    
                    IGeometry pGeo = EllipticArcTransPolygon(pEll);
                    pGeo.SpatialReference = spatialReference;
                    IPolygon pPloy = pGeo as IPolygon;
                    if (iCount == 1) //第一个烈度图层
                    {
                        IFeatureClass pFeatureClass8LD = pFeatureLayer8LD.FeatureClass;
                        IFeatureClassWrite fwrite8LD = pFeatureClass8LD as IFeatureClassWrite;
                        IFeature pFeature8LD = pFeatureClass8LD.CreateFeature();
                        pFeature8LD.Shape = pPloy;
                        fwrite8LD.WriteFeature(pFeature8LD);
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeature8LD);
                        pFeature8LD = null;
                        if (dZJ>5.3)
                        {
                            iCount++;
                        }
                        //更新显示区域
                        IActiveView pAV = _MainPageLayoutControl.ActiveView.FocusMap as IActiveView;
                        pEnvelope = pPloy.Envelope;
                        pEnvelope.Expand(1.8, 1.8, true);
                        pAV.Extent = pEnvelope;
                        pAV.Refresh();
                    }
                    else if (iCount == 2)
                    {
                        IFeatureClass pFeatureClass7LD = pFeatureLayer7LD.FeatureClass;
                        IFeatureClassWrite fwrite7LD = pFeatureClass7LD as IFeatureClassWrite;
                        IFeature pFeature7LD = pFeatureClass7LD.CreateFeature();
                        pFeature7LD.Shape = pPloy;
                        fwrite7LD.WriteFeature(pFeature7LD);
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeature7LD);
                        pFeature7LD = null;
                        if (tot == 3)
                        {
                            iCount++;
                        }
                    }
                    else if (iCount == 3)
                    {
                        IFeatureClass pFeatureClass6LD = pFeatureLayer6LD.FeatureClass;
                        IFeatureClassWrite fwrite6LD = pFeatureClass6LD as IFeatureClassWrite;
                        IFeature pFeature6LD = pFeatureClass6LD.CreateFeature();
                        pFeature6LD.Shape = pPloy;
                        fwrite6LD.WriteFeature(pFeature6LD);
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeature6LD);
                        pFeature6LD = null;
                        iCount++;
                    }
                }
            }
            pFeaturePoint = null;
            pWorkspaceEdit.StopEditOperation();
            pWorkspaceEdit.StopEditing(true);
            this.Hide();
            setFormIPoint(pPoint, textBoxLevel.Text,pEnvelope);
            //修改标题
            IElement pElementTitle = _MainPageLayoutControl.FindElementByName("Title");
            ITextElement pTextElementTitle = pElementTitle as ITextElement;
            string sXZQName = GetXZQName(pPoint);
            string sText = sXZQName + "M" + textBoxLevel.Text + "级地震震区水库分布图";
            pTextElementTitle.Text = sText;
            
            MessageBox.Show("操作成功！", "提示");
        }
        private string GetXZQName(IPoint pCentPoint)
        {
            string sXZQName = string.Empty;
            IFeatureWorkspace featureWorkspace = null;
            IFeatureCursor pFeatureCursor = null;
            try
            {
                AccessWorkspaceFactory accessWorkspaceFactory = new AccessWorkspaceFactoryClass();
                featureWorkspace = (IFeatureWorkspace)accessWorkspaceFactory.OpenFromFile(dataFile, 0);
                IFeatureClass featureClass = featureWorkspace.OpenFeatureClass("town_code");

                IGeoDataset pGeoDataset = featureClass as IGeoDataset;
                if (pCentPoint.SpatialReference.FactoryCode != pGeoDataset.SpatialReference.FactoryCode)
                {
                    pCentPoint.Project(pGeoDataset.SpatialReference);
                }
                ISpatialFilter spatialFilter = new ESRI.ArcGIS.Geodatabase.SpatialFilterClass();
                spatialFilter.Geometry = pCentPoint;
                spatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                pFeatureCursor = featureClass.Search(spatialFilter, true);
                IFeature pFeature = pFeatureCursor.NextFeature();
                if (pFeature != null)
                {
                    int iIndex = pFeature.Fields.FindField("NAME");
                    if (iIndex > -1)
                    {
                        object obVaue = pFeature.get_Value(iIndex);
                        if (obVaue != null)
                        {
                            sXZQName = obVaue.ToString();
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                MathComm.ReleaseCOMObject(pFeatureCursor);
                MathComm.ReleaseCOMObject(featureWorkspace);
            }
            return sXZQName;
        }
            
        //取消键
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
        private IGeometry EllipticArcTransPolygon(IEllipticArc pEll)
        {
            try
            {
                object missing = Type.Missing;
                ISegmentCollection pSegmentColl = new RingClass();
                pSegmentColl.AddSegment((ISegment)pEll, ref missing, ref missing);
                IRing pRing = (IRing)pSegmentColl;
                pRing.Close(); //得到闭合的环
                IGeometryCollection pGeometryCollection = new PolygonClass();
                pGeometryCollection.AddGeometry(pRing, ref missing, ref missing); //环转面
                IPolygon pPolygon = (IPolygon)pGeometryCollection;
                return pPolygon;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
         //<summary>
         //计算椭圆参数
         //</summary>
         //<param name="pCentPoint"></param>
         //<param name="dZJ"></param>
         //<param name="dTransAngle"></param>
         //<returns></returns>
        private List<EllipticArcPro> CalculatEllipticArc(IPoint pCentPoint, double dZJ, double dTransAngle)
        {

            try
            {
                List<EllipticArcPro> pListEllipticArcPro = new List<EllipticArcPro>();
                List<double> pListLD = new List<double>();
                pListLD.Add(6);
                pListLD.Add(7);
                pListLD.Add(8);

                int iEnd = 0;
                //如果震级小于6，生成两个椭圆震圈
                if (dZJ < 6)
                {
                    iEnd = 2;
                }
                else
                {
                    iEnd = 3;
                }

                for (int i = 0; i < iEnd; i++)
                {
                    EllipticArcPro pEllipticArcPro = new EllipticArcPro();
                    pEllipticArcPro.ellipseStd = false;
                    pEllipticArcPro.CenterPoint = pCentPoint;
                    pEllipticArcPro.FromAngle = 3;
                    pEllipticArcPro.CentralAngle = 2 * Math.PI;
                    pEllipticArcPro.rotationAngle = dTransAngle / 180 * Math.PI;
                    pEllipticArcPro.LD = pListLD[i];
                    //计算长半轴
                    pEllipticArcPro.semiMajor = (Math.Pow(10, (5.7123 + 1.3626 * dZJ - pListLD[i]) / 4.2903) - 25) * 1000;

                    double b = (Math.Pow(10, (3.6588 + 1.3626 * dZJ - pListLD[i]) / 3.5406) - 13) * 1000;
                    pEllipticArcPro.minorMajorRatio = b / pEllipticArcPro.semiMajor;
                    pListEllipticArcPro.Add(pEllipticArcPro);

                }
                return pListEllipticArcPro;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 获取颜色
        /// </summary>
        /// <param name="dLD"></param>
        /// <returns></returns>
        private static IRgbColor GetColor(double dLD)
        {
            RgbColor color = new RgbColor();

            int iLD = (int)dLD;
            switch (iLD)
            {
                case 6:
                    color.Red = 254;
                    color.Green = 224;
                    color.Blue = 222;
                    break;
                case 7:
                    color.Red = 253;
                    color.Green = 174;
                    color.Blue = 179;
                    break;
                case 8:
                    color.Red = 254;
                    color.Green = 113;
                    color.Blue = 119;
                    break;
                default:
                    color.Red = 254;
                    color.Green = 113;
                    color.Blue = 119;
                    break;
            }
            return color;
        }

        private void DelectElementByName(IGraphicsContainer graphicsContainer, string sElementName)
        {
            try
            {
                IElementProperties pElementProperties = graphicsContainer.Next() as IElementProperties;
                while (pElementProperties != null)
                {
                    if (pElementProperties.Name == sElementName)
                    {
                        graphicsContainer.DeleteElement(pElementProperties as IElement);
                    }
                    pElementProperties = graphicsContainer.Next() as IElementProperties;
                }
            }
            catch (Exception)
            {
            }
        }
        public class EllipticArcPro
        {
            /// <summary>
            /// 为False，角度是以坐标轴作为基准.为True，角度是以长半轴作为基准；并且，起点、终点的坐标都是相对于中心的点的相对坐标
            /// </summary>
            public bool ellipseStd { get; set; }

            /// <summary>
            /// 椭圆弧中心点
            /// </summary>
            public IPoint CenterPoint { get; set; }

            /// <summary>
            /// 起点角度
            /// </summary>
            public double FromAngle { get; set; }

            /// <summary>
            /// 椭圆弧圆心角
            /// </summary>
            public double CentralAngle { get; set; }

            /// <summary>
            /// 旋转角度
            /// </summary>
            public double rotationAngle { get; set; }

            /// <summary>
            /// 长半轴长度
            /// </summary>
            public double semiMajor { get; set; }

            /// <summary>
            /// 短半轴与长半轴比例
            /// </summary>
            public double minorMajorRatio { get; set; }

            /// <summary>
            /// 烈度
            /// </summary>
            public double LD { get; set; }
        }




    }
    //声明一个委托，用来将震中点传回给Form1
    public delegate void setIPoint(IPoint iPoint, string level, IEnvelope pEnvelope);
}