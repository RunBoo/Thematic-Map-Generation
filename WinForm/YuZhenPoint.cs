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
    public partial class YuZhenPoint : Form
    {
        public string dataFile = null;  //记录数据库位置
        IFeatureLayer pFeatureLayerPoint = null;     //余震图层
        public string time = null;     //时间
        private static object _missing = Type.Missing;

        public YuZhenPoint(AxMapControl axMapControl1, string file)
        {
            InitializeComponent();
            dataFile = file;
            pFeatureLayerPoint = axMapControl1.Map.get_Layer(0) as IFeatureLayer;   //获取余震图层
        }
        //确定键
        private void buttonOK_Click(object sender, EventArgs e)
        {
            double dL = 0.0, dB = 0.0, dZJ = 0.0;
            bool bL = double.TryParse(textBoxLong.Text, out dL);//经度
            bool bB = double.TryParse(textBoxLat.Text, out dB);//纬度
            bool bZJ = double.TryParse(textBoxLevel.Text, out dZJ);//震级
            time = textBoxTime.Text;  //时间

            //开启编辑状态
            IWorkspaceFactory pWorkspaceFactory = new AccessWorkspaceFactoryClass();
            IFeatureWorkspace pFeatureWorkspace = pWorkspaceFactory.OpenFromFile(dataFile, 0) as IFeatureWorkspace;
            IWorkspaceEdit pWorkspaceEdit = pFeatureWorkspace as IWorkspaceEdit;
            pWorkspaceEdit.StartEditing(false);
            pWorkspaceEdit.StartEditOperation();
            //插入余震点数据
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
            //设置属性值
            pFeaturePoint.set_Value(3, dZJ);
            pFeaturePoint.set_Value(5, time + "," + textBoxLevel.Text);
            fwritePoint.WriteFeature(pFeaturePoint);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeaturePoint);

            pFeaturePoint = null;
            pWorkspaceEdit.StopEditOperation();
            pWorkspaceEdit.StopEditing(true);
            this.Hide();
            MessageBox.Show("操作成功！", "提示");
        }
        //取消键
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
