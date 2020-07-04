using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    class CreateLine
    {
        public void creatLine(AxMapControl axMapControl1, string dataFile, IPoint pPointCen, IEnvelope iBound)
        {
            //开启编辑状态
            IWorkspaceFactory pWorkspaceFactory = new AccessWorkspaceFactoryClass();
            IFeatureWorkspace pFeatureWorkspace = pWorkspaceFactory.OpenFromFile(dataFile, 0) as IFeatureWorkspace;
            IWorkspaceEdit pWorkspaceEdit = pFeatureWorkspace as IWorkspaceEdit;
            pWorkspaceEdit.StartEditing(false);
            pWorkspaceEdit.StartEditOperation();
            //获取线图层
            IFeatureLayer pFeatureLayerLine = axMapControl1.Map.get_Layer(5) as IFeatureLayer;
            IFeatureClass pFeatureClassLine = pFeatureLayerLine.FeatureClass;
            IFeatureClassWrite fwriteLine = pFeatureClassLine as IFeatureClassWrite;
            

            //获取地级市数据点，包含两部分：市政府和区县政府
            IFeatureLayer pFeatureLayer1 = null;  //市政府图层
            pFeatureLayer1 = axMapControl1.Map.get_Layer(1) as IFeatureLayer;
            IFeatureClass pFeatureClass1 = pFeatureLayer1.FeatureClass;
            for (int i = 1; i < 15; i++)
            {
                IFeature pFeature1 = pFeatureClass1.GetFeature(i);
                string name1 = Convert.ToString(pFeature1.get_Value(3)); //获取地级市名
                IPointCollection pColl1 = pFeature1.Shape as IPointCollection;
                IPoint pPoint1 = pColl1.get_Point(pColl1.PointCount - 1);
                //判断这个地级市是否在显示范围内
                if (pPoint1.X > iBound.XMin && pPoint1.X < iBound.XMax && pPoint1.Y > iBound.YMin && pPoint1.Y < iBound.YMax)
                {
                    IFeature pFeatureLine = pFeatureClassLine.CreateFeature();
                    //构建新的线要素
                    IPolyline new_line = new PolylineClass();
                    new_line.SpatialReference = pPointCen.SpatialReference;
                    new_line.FromPoint = pPointCen;
                    new_line.ToPoint = pPoint1;
                    pFeatureLine.Shape = new_line;
                    //设置属性值
                    pFeatureLine.set_Value(4, Convert.ToInt32(new_line.Length*100));
                    pFeatureLine.set_Value(3, name1);
                    fwriteLine.WriteFeature(pFeatureLine);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeatureLine);
                    pFeatureLine = null;
                }
            }
            IFeatureLayer pFeatureLayer2 = null;  //区县政府图层
            pFeatureLayer2 = axMapControl1.Map.get_Layer(2) as IFeatureLayer;
            IFeatureClass pFeatureClass2 = pFeatureLayer2.FeatureClass;
            //获取属性值
            IFields pFields2 = pFeatureClass2.Fields;
            for (int i = 1; i < 51; i++)
            {
                IFeature pFeature2 = pFeatureClass2.GetFeature(i);
                string name2 = Convert.ToString(pFeature2.get_Value(4)); //获取地级市名
                IPointCollection pColl2 = pFeature2.Shape as IPointCollection;
                IPoint pPoint2 = pColl2.get_Point(pColl2.PointCount - 1);
                //判断这个地级市是否在显示范围内
                if (pPoint2.X > iBound.XMin && pPoint2.X < iBound.XMax && pPoint2.Y > iBound.YMin && pPoint2.Y < iBound.YMax)
                {
                    IFeature pFeatureLine = pFeatureClassLine.CreateFeature();
                    //构建新的线要素
                    IPolyline new_line = new PolylineClass();
                    new_line.SpatialReference = pPointCen.SpatialReference;
                    new_line.FromPoint = pPointCen;
                    new_line.ToPoint = pPoint2;
                    pFeatureLine.Shape = new_line;
                    //设置属性值
                    pFeatureLine.set_Value(4, Convert.ToInt32(new_line.Length * 100));
                    pFeatureLine.set_Value(3, name2);
                    fwriteLine.WriteFeature(pFeatureLine);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeatureLine);
                    pFeatureLine = null;
                }
            }
            pWorkspaceEdit.StopEditOperation();
            pWorkspaceEdit.StopEditing(true);
        }
    }
}