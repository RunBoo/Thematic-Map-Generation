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
    class Cleandata
    {
        public void cleanData(AxMapControl axMapControl1, string dataFile, int type)
        {
            //开启编辑状态
            IWorkspaceFactory pWorkspaceFactory = new AccessWorkspaceFactoryClass();
            IFeatureWorkspace pFeatureWorkspace = pWorkspaceFactory.OpenFromFile(dataFile, 0) as IFeatureWorkspace;
            IWorkspaceEdit pWorkspaceEdit = pFeatureWorkspace as IWorkspaceEdit;
            pWorkspaceEdit.StartEditing(false);
            pWorkspaceEdit.StartEditOperation();
            switch (type)
            {
                case 1:     //导入图4，删除震中及烈度图
                    IFeatureLayer pFeatureLayer1 = axMapControl1.Map.get_Layer(0) as IFeatureLayer;
                    IFeatureCursor pFeatureCursor1 = pFeatureLayer1.Search(null, true);
                    IFeature pf1 = pFeatureCursor1.NextFeature();
                    if (pf1 != null)
                    {
                        pf1.Delete();
                    }
                    IFeatureLayer pFeatureLayer2 = axMapControl1.Map.get_Layer(7) as IFeatureLayer;
                    IFeatureCursor pFeatureCursor2 = pFeatureLayer2.Search(null, true);
                    IFeature pf2 = pFeatureCursor2.NextFeature();
                    if (pf2 != null)
                    {
                        pf2.Delete();
                    }
                    IFeatureLayer pFeatureLayer3 = axMapControl1.Map.get_Layer(8) as IFeatureLayer;
                    IFeatureCursor pFeatureCursor3 = pFeatureLayer3.Search(null, true);
                    IFeature pf3 = pFeatureCursor3.NextFeature();
                    if (pf3 != null)
                    {
                        pf3.Delete();
                    }
                    IFeatureLayer pFeatureLayer4 = axMapControl1.Map.get_Layer(9) as IFeatureLayer;
                    IFeatureCursor pFeatureCursor4 = pFeatureLayer4.Search(null, true);
                    IFeature pf4 = pFeatureCursor4.NextFeature();
                    if (pf4 != null)
                    {
                        pf4.Delete();
                    }
                    break;
                case 2:    //导入图5，删除余震信息
                    IFeatureLayer pFeatureLayer5 = axMapControl1.Map.get_Layer(0) as IFeatureLayer;
                    IFeatureCursor pFeatureCursor5 = pFeatureLayer5.Search(null, true);
                    IFeature pf5 = pFeatureCursor5.NextFeature();
                    if (pf5 != null)
                    {
                        pf5.Delete();
                    }
                    break;
                case 3:     //导入图16，删除灾情信息
                    IFeatureLayer pFeatureLayer6 = axMapControl1.Map.get_Layer(0) as IFeatureLayer;
                    IFeatureCursor pFeatureCursor6 = pFeatureLayer6.Search(null, true);
                    IFeature pf6 = pFeatureCursor6.NextFeature();
                    if (pf6 != null)
                    {
                        pf6.Delete();
                    }
                    break;
                case 4:    //导入图17，删除调查信息
                    IFeatureLayer pFeatureLayer7 = axMapControl1.Map.get_Layer(0) as IFeatureLayer;
                    IFeatureCursor pFeatureCursor7 = pFeatureLayer7.Search(null, true);
                    IFeature pf7 = pFeatureCursor7.NextFeature();
                    if (pf7 != null)
                    {
                        pf7.Delete();
                    }
                    break;
                case 5:         //导入图6，删除距离分布信息
                    IFeatureLayer pFeatureLayer8 = axMapControl1.Map.get_Layer(5) as IFeatureLayer;
                    IFeatureCursor pFeatureCursor8 = pFeatureLayer8.Search(null, true);
                    IFeature pf8 = pFeatureCursor8.NextFeature();
                    if (pf8 != null)
                    {
                        pf8.Delete();
                    }
                    break;

            }
            pWorkspaceEdit.StopEditOperation();
            pWorkspaceEdit.StopEditing(true);

        }
    }
}
