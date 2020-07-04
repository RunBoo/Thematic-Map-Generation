using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Output;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.GISClient;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.DataSourcesFile;


namespace WinForm
{
    public partial class Form1 : Form
    {
        public string InputFilePath = "E:\\AE二次开发专题图\\专题图生成\\";   //定义存放所有mxd的路径
        public string OutputFileName = "E:\\photo\\";    //定义所有输出专题图的位置
        public string dataFile = "E:\\AE二次开发专题图\\专题图生成\\社服数据（修正版）.mdb";  //记录数据库位置
        public string sMxdFilePath = null;
        public IPoint CenterPoint = null;     //记录震中位置点
        public string Level = null;     //记录震级
        public IEnvelope pEnvelope = null;    //记录显示位置
        public DateTime pTateTime = DateTime.Now;    //记录当前时间

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                sMxdFilePath = InputFilePath + "4震区水库分布图-A3.mxd";
                axPageLayoutControl1.LoadMxFile(sMxdFilePath);
                updateTime();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //一键制图按钮
        private void btnExport_Click(object sender, EventArgs e)
        {
            int count = 1;
            if (count == 1)
            {
                //等待半秒钟
                Thread.Sleep(500);
                sMxdFilePath = InputFilePath + "1震中位置图-A3.mxd";
                axPageLayoutControl1.LoadMxFile(sMxdFilePath);
                updateTime();
                updateLocation("震中位置图");
                //更新显示区域
                IActiveView pAV = axPageLayoutControl1.ActiveView.FocusMap as IActiveView;
                pAV.Extent = pEnvelope;
                pAV.Refresh();
                axPageLayoutControl1.Refresh();
                string fileName = OutputFileName + "1震中位置图.png";
                OutputPhoto(fileName);
                count++;
            }
            if (count == 2)
            {
                //等待半秒钟
                Thread.Sleep(500);
                sMxdFilePath = InputFilePath + "2影响估计范围分布图-A3.mxd";
                axPageLayoutControl1.LoadMxFile(sMxdFilePath);
                updateTime();
                updateLocation("影响估计范围分布图");
                //更新显示区域
                IActiveView pAV = axPageLayoutControl1.ActiveView.FocusMap as IActiveView;
                pAV.Extent = pEnvelope;
                pAV.Refresh();
                axPageLayoutControl1.Refresh();
                string fileName = OutputFileName + "2影响估计范围分布图.png";
                OutputPhoto(fileName);
                count++;
            }
            if (count == 3)
            {
                //等待半秒钟
                Thread.Sleep(500);
                sMxdFilePath = InputFilePath + "3震区历史地震分布图-A3.mxd";
                axPageLayoutControl1.LoadMxFile(sMxdFilePath);
                updateTime();
                updateLocation("震区历史地震分布图");
                //更新显示区域
                IActiveView pAV = axPageLayoutControl1.ActiveView.FocusMap as IActiveView;
                pAV.Extent = pEnvelope;
                pAV.Refresh();
                axPageLayoutControl1.Refresh();
                string fileName = OutputFileName + "3震区历史地震分布图.png";
                OutputPhoto(fileName);
                count++;
            }
            if (count == 4)
            {
                //等待半秒钟
                Thread.Sleep(500);
                sMxdFilePath = InputFilePath + "4震区水库分布图-A3.mxd";
                axPageLayoutControl1.LoadMxFile(sMxdFilePath);
                updateTime();
                updateLocation("震区水库分布图");
                //更新显示区域
                IActiveView pAV = axPageLayoutControl1.ActiveView.FocusMap as IActiveView;
                pAV.Extent = pEnvelope;
                pAV.Refresh();
                axPageLayoutControl1.Refresh();
                string fileName = OutputFileName + "4震区水库分布图.png";
                OutputPhoto(fileName);
                count++;
            }
            if (count == 5)
            {
                //等待半秒钟
                Thread.Sleep(500);
                sMxdFilePath = InputFilePath + "5余震分布图-A3.mxd";
                axPageLayoutControl1.LoadMxFile(sMxdFilePath);
                updateTime();
                updateLocation("余震分布图截止"+ pTateTime.Day + "日" + pTateTime.Hour + "时" + pTateTime.Minute + "分");
                //更新显示区域
                IActiveView pAV = axPageLayoutControl1.ActiveView.FocusMap as IActiveView;
                pAV.Extent = pEnvelope;
                pAV.Refresh();
                axPageLayoutControl1.Refresh();
                string fileName = OutputFileName + "5余震分布图.png";
                OutputPhoto(fileName);
                count++;
            }
            if (count == 6)
            {
                //等待半秒钟
                Thread.Sleep(500);
                sMxdFilePath = InputFilePath + "6震中与主要城市距离分布图-A3.mxd";
                axPageLayoutControl1.LoadMxFile(sMxdFilePath);
                updateTime();
                updateLocation("震中与主要城市距离分布图");
                //更新显示区域
                IActiveView pAV = axPageLayoutControl1.ActiveView.FocusMap as IActiveView;
                pAV.Extent = pEnvelope;
                pAV.Refresh();
                axPageLayoutControl1.Refresh();
                string fileName = OutputFileName + "6震中与主要城市距离分布图.png";
                OutputPhoto(fileName);
                count++;
            }
            if (count == 7)
            {
                //等待半秒钟
                Thread.Sleep(500);
                sMxdFilePath = InputFilePath + "7震区交通图-A3.mxd";
                axPageLayoutControl1.LoadMxFile(sMxdFilePath);
                updateTime();
                updateLocation("震区交通图");
                //更新显示区域
                IActiveView pAV = axPageLayoutControl1.ActiveView.FocusMap as IActiveView;
                pAV.Extent = pEnvelope;
                pAV.Refresh();
                axPageLayoutControl1.Refresh();
                string fileName = OutputFileName + "7震区交通图.png";
                OutputPhoto(fileName);
                count++;
            }
            if (count == 8)
            {
                //等待半秒钟
                Thread.Sleep(500);
                sMxdFilePath = InputFilePath + "8震区学校分布图-A3.mxd";
                axPageLayoutControl1.LoadMxFile(sMxdFilePath);
                updateTime();
                updateLocation("震区学校分布图");
                //更新显示区域
                IActiveView pAV = axPageLayoutControl1.ActiveView.FocusMap as IActiveView;
                pAV.Extent = pEnvelope;
                pAV.Refresh();
                axPageLayoutControl1.Refresh();
                string fileName = OutputFileName + "8震区学校分布图.png";
                OutputPhoto(fileName);
                count++;
            }
            if (count == 9)
            {
                //等待半秒钟
                Thread.Sleep(500);
                sMxdFilePath = InputFilePath + "9震区医院分布图-A3.mxd";
                axPageLayoutControl1.LoadMxFile(sMxdFilePath);
                updateTime();
                updateLocation("震区医院分布图");
                //更新显示区域
                IActiveView pAV = axPageLayoutControl1.ActiveView.FocusMap as IActiveView;
                pAV.Extent = pEnvelope;
                pAV.Refresh();
                axPageLayoutControl1.Refresh();
                string fileName = OutputFileName + "9震区医院分布图.png";
                OutputPhoto(fileName);
                count++;
            }
            if (count == 10)
            {
                //等待半秒钟
                Thread.Sleep(500);
                sMxdFilePath = InputFilePath + "10震区潜在地质灾害分布图-A3.mxd";
                axPageLayoutControl1.LoadMxFile(sMxdFilePath);
                updateTime();
                updateLocation("震区潜在地质灾害分布图");
                //更新显示区域
                IActiveView pAV = axPageLayoutControl1.ActiveView.FocusMap as IActiveView;
                pAV.Extent = pEnvelope;
                pAV.Refresh();
                axPageLayoutControl1.Refresh();
                string fileName = OutputFileName + "10震区潜在地质灾害分布图.png";
                OutputPhoto(fileName);
                count++;
            }
            if (count == 11)
            {
                //等待半秒钟
                Thread.Sleep(500);
                sMxdFilePath = InputFilePath + "11震区危险源分布图-A3.mxd";
                axPageLayoutControl1.LoadMxFile(sMxdFilePath);
                updateTime();
                updateLocation("震区危险源分布图");
                //更新显示区域
                IActiveView pAV = axPageLayoutControl1.ActiveView.FocusMap as IActiveView;
                pAV.Extent = pEnvelope;
                pAV.Refresh();
                axPageLayoutControl1.Refresh();
                string fileName = OutputFileName + "11震区危险源分布图.png";
                OutputPhoto(fileName);
                count++;
            }
            if (count == 12)
            {
                //等待半秒钟
                Thread.Sleep(500);
                sMxdFilePath = InputFilePath + "12震区烈度区划图-A3.mxd";
                axPageLayoutControl1.LoadMxFile(sMxdFilePath);
                updateTime();
                updateLocation("震区烈度区划图");
                //更新显示区域
                IActiveView pAV = axPageLayoutControl1.ActiveView.FocusMap as IActiveView;
                pAV.Extent = pEnvelope;
                pAV.Refresh();
                axPageLayoutControl1.Refresh();
                string fileName = OutputFileName + "12震区烈度区划图.png";
                OutputPhoto(fileName);
                count++;
            }
            if (count == 13)
            {
                //等待半秒钟
                Thread.Sleep(500);
                sMxdFilePath = InputFilePath + "13震区地震动峰值加速度区划图-A3.mxd";
                axPageLayoutControl1.LoadMxFile(sMxdFilePath);
                updateTime();
                updateLocation("震区地震动峰值加速度区划图");
                //更新显示区域
                IActiveView pAV = axPageLayoutControl1.ActiveView.FocusMap as IActiveView;
                pAV.Extent = pEnvelope;
                pAV.Refresh();
                axPageLayoutControl1.Refresh();
                string fileName = OutputFileName + "13震区地震动峰值加速度区划图.png";
                OutputPhoto(fileName);
                count++;
            }
            if (count == 14)
            {
                //等待半秒钟
                Thread.Sleep(500);
                sMxdFilePath = InputFilePath + "14震区GDP图-A3.mxd";
                axPageLayoutControl1.LoadMxFile(sMxdFilePath);
                updateTime();
                updateLocation("震区GDP图");
                //更新显示区域
                IActiveView pAV = axPageLayoutControl1.ActiveView.FocusMap as IActiveView;
                pAV.Extent = pEnvelope;
                pAV.Refresh();
                axPageLayoutControl1.Refresh();
                string fileName = OutputFileName + "14震区GDP图.png";
                OutputPhoto(fileName);
                count++;
            }
            if (count == 15)
            {
                //等待半秒钟
                Thread.Sleep(500);
                sMxdFilePath = InputFilePath + "15震区人口分布图-A3.mxd";
                axPageLayoutControl1.LoadMxFile(sMxdFilePath);
                updateTime();
                updateLocation("震区人口分布图");
                //更新显示区域
                IActiveView pAV = axPageLayoutControl1.ActiveView.FocusMap as IActiveView;
                pAV.Extent = pEnvelope;
                pAV.Refresh();
                axPageLayoutControl1.Refresh();
                string fileName = OutputFileName + "15震区人口分布图.png";
                OutputPhoto(fileName);
                count++;
            }
            if (count == 16)
            {
                //等待半秒钟
                Thread.Sleep(500);
                sMxdFilePath = InputFilePath + "16灾情信息分布图-A3.mxd";
                axPageLayoutControl1.LoadMxFile(sMxdFilePath);
                updateTime();
                updateLocation("灾情信息分布图");
                //更新显示区域
                IActiveView pAV = axPageLayoutControl1.ActiveView.FocusMap as IActiveView;
                pAV.Extent = pEnvelope;
                pAV.Refresh();
                axPageLayoutControl1.Refresh();
                string fileName = OutputFileName + "16灾情信息分布图.png";
                OutputPhoto(fileName);
                count++;
            }
            if (count ==17)
            {
                //等待半秒钟
                Thread.Sleep(500);
                sMxdFilePath = InputFilePath + "17现场调查点分布图-A3.mxd";
                axPageLayoutControl1.LoadMxFile(sMxdFilePath);
                updateTime();
                updateLocation("现场调查点分布图");
                //更新显示区域
                IActiveView pAV = axPageLayoutControl1.ActiveView.FocusMap as IActiveView;
                pAV.Extent = pEnvelope;
                pAV.Refresh();
                axPageLayoutControl1.Refresh();
                string fileName = OutputFileName + "17现场调查点分布图.png";
                OutputPhoto(fileName);
                count++;
            }
            MessageBox.Show("操作成功！");
        }
        //制图输出函数
        public void OutputPhoto(string fileName)
        {
            double iScreenDispalyResolution = axPageLayoutControl1.ActiveView.ScreenDisplay.DisplayTransformation.Resolution;
            ESRI.ArcGIS.Output.IExport pExport = new ESRI.ArcGIS.Output.ExportPNGClass();
            //设置输出文件路径及名称
            pExport.ExportFileName = fileName;
            // 设置输出分辨率
            pExport.Resolution = (short)iScreenDispalyResolution;
            // 获取输出范围,获取视图框架对象，进而得到视图范围
            tagRECT deviceRect = axPageLayoutControl1.ActiveView.ScreenDisplay.DisplayTransformation.get_DeviceFrame();
            IEnvelope pDeviceEnvelop = new EnvelopeClass();
            // 设置一个边框范围
            pDeviceEnvelop.PutCoords(deviceRect.left, deviceRect.bottom, deviceRect.right, deviceRect.top);
            // 将打印像素范围设置给输出对象
            pExport.PixelBounds = pDeviceEnvelop;
            // 设置跟踪取消对象
            ITrackCancel pCancle = new CancelTrackerClass();
            // 进行视图控件的视图输出操作，设置对应参数
            axPageLayoutControl1.ActiveView.Output(pExport.StartExporting(), (int)pExport.Resolution, ref deviceRect, axPageLayoutControl1.ActiveView.Extent, pCancle);
            Application.DoEvents();
            pExport.FinishExporting();
            pExport.Cleanup();

        }
        //椭圆烈度圈及震中点要素生成
        private void PointButton_Click(object sender, EventArgs e)
        {
            //通过AxMapControl来编辑数据
            axMapControl1.LoadMxFile(sMxdFilePath);
            IntensityPoint intPoint = new IntensityPoint(axMapControl1,axPageLayoutControl1,dataFile);
            intPoint.setFormIPoint += new setIPoint(getCenter);
            intPoint.Show();
            axPageLayoutControl1.Refresh();
        }
        //获取震中点及震级
        void getCenter(IPoint iPoint, string level,IEnvelope envelope)
        {
            CenterPoint = iPoint;
            Level = level;
            pEnvelope = envelope;
        }
        //更新时间
        public void updateTime()
        {
            //修改时间
            IElement pElement = axPageLayoutControl1.FindElementByName("Time");
            ITextElement pTextElement = pElement as ITextElement;
            pTextElement.Text = "制作时间：" + pTateTime.Year + "年" + pTateTime.Month + "月" + pTateTime.Day + "日" + pTateTime.Hour + "时" + pTateTime.Minute + "分";
            axPageLayoutControl1.Refresh();
        }
        //更新地点及震级
        public void updateLocation(string type)
        {
            //修改标题
            IElement pElementTitle = axPageLayoutControl1.FindElementByName("Title");
            ITextElement pTextElementTitle = pElementTitle as ITextElement;
            string sXZQName = GetXZQName(CenterPoint);
            string sText = sXZQName + "M" + Level + "级地震" + type;
            pTextElementTitle.Text = sText;
            axPageLayoutControl1.Refresh();
        }
        //余震分布数据编辑
        private void button1_Click(object sender, EventArgs e)
        {
            //加载余震分布数据
            sMxdFilePath = InputFilePath + "5余震分布图-A3.mxd";
            axPageLayoutControl1.LoadMxFile(sMxdFilePath);
            updateTime();
            updateLocation("余震分布图截止"+ pTateTime.Day + "日" + pTateTime.Hour + "时" + pTateTime.Minute + "分");
            //更新显示区域
            IActiveView pAV = axPageLayoutControl1.ActiveView.FocusMap as IActiveView;
            pAV.Extent = pEnvelope;
            pAV.Refresh();
            axPageLayoutControl1.Refresh();
            //通过AxMapControl来编辑数据
            axMapControl1.ClearLayers();
            axMapControl1.LoadMxFile(sMxdFilePath);
            YuZhenPoint yuzhenPoint = new YuZhenPoint(axMapControl1, dataFile);
            //等待一秒弹出输入框
            Thread.Sleep(1000);
            yuzhenPoint.Show();
            axPageLayoutControl1.Refresh();
        }
        //灾情信息编辑
        private void button2_Click(object sender, EventArgs e)
        {
            //加载灾情信息数据
            sMxdFilePath = InputFilePath + "16灾情信息分布图-A3.mxd";
            axPageLayoutControl1.LoadMxFile(sMxdFilePath);
            updateTime();
            updateLocation("灾情信息分布图");
            //更新显示区域
            IActiveView pAV = axPageLayoutControl1.ActiveView.FocusMap as IActiveView;
            pAV.Extent = pEnvelope;
            pAV.Refresh();
            axPageLayoutControl1.Refresh();
            //通过AxMapControl来编辑数据
            axMapControl1.ClearLayers();
            axMapControl1.LoadMxFile(sMxdFilePath);
            ZaiQingPoint zaiqingPoint = new ZaiQingPoint(axMapControl1, dataFile);
            //等待一秒弹出输入框
            Thread.Sleep(1000);
            zaiqingPoint.Show();
            axPageLayoutControl1.Refresh();
        }
        //调查信息编辑
        private void button3_Click(object sender, EventArgs e)
        {
            //加载调查信息数据
            sMxdFilePath = InputFilePath + "17现场调查点分布图-A3.mxd";
            axPageLayoutControl1.LoadMxFile(sMxdFilePath);
            updateTime();
            updateLocation("现场调查点分布图");
            //更新显示区域
            IActiveView pAV = axPageLayoutControl1.ActiveView.FocusMap as IActiveView;
            pAV.Extent = pEnvelope;
            pAV.Refresh();
            axPageLayoutControl1.Refresh();
            //通过AxMapControl来编辑数据
            axMapControl1.ClearLayers();
            axMapControl1.LoadMxFile(sMxdFilePath);
            DiaoChaPoint diaochaPoint = new DiaoChaPoint(axMapControl1, dataFile);
            //等待一秒弹出输入框
            Thread.Sleep(1000);
            diaochaPoint.Show();
            axPageLayoutControl1.Refresh();
        }
        //震中点与主要城市距离分布图
        private void button4_Click(object sender, EventArgs e)
        {
            //加载调查信息数据
            sMxdFilePath = InputFilePath + "6震中与主要城市距离分布图-A3.mxd";
            axPageLayoutControl1.LoadMxFile(sMxdFilePath);
            updateTime();
            updateLocation("震中与主要城市距离分布图");
            //更新显示区域
            IActiveView pAV = axPageLayoutControl1.ActiveView.FocusMap as IActiveView;
            pAV.Extent = pEnvelope;
            pAV.Refresh();
            axPageLayoutControl1.Refresh();
            //通过AxMapControl来编辑数据
            axMapControl1.ClearLayers();
            axMapControl1.LoadMxFile(sMxdFilePath);
            CreateLine cLine = new CreateLine();
            cLine.creatLine(axMapControl1, dataFile,CenterPoint,pEnvelope);
            axPageLayoutControl1.Refresh();
        }
        //获取地区名，用于更新图名
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
        //一键清空数据
        private void button5_Click(object sender, EventArgs e)
        {
            int number = 1;
            if (number == 1)   //导入图4，删除震中及烈度图
            {
                sMxdFilePath = InputFilePath + "4震区水库分布图-A3.mxd";
                //通过AxMapControl来编辑数据
                axMapControl1.ClearLayers();
                axMapControl1.LoadMxFile(sMxdFilePath);
                Cleandata clean = new Cleandata();
                clean.cleanData(axMapControl1, dataFile, number);
                number += 1;
            }
            if (number == 2)   //导入图5，删除余震信息
            {
                sMxdFilePath = InputFilePath + "5余震分布图-A3.mxd";
                //通过AxMapControl来编辑数据
                axMapControl1.ClearLayers();
                axMapControl1.LoadMxFile(sMxdFilePath);
                Cleandata clean = new Cleandata();
                clean.cleanData(axMapControl1, dataFile, number);
                number += 1;
            }
            if (number == 3)   //导入图16，删除灾情信息
            {
                sMxdFilePath = InputFilePath + "16灾情信息分布图-A3.mxd";
                //通过AxMapControl来编辑数据
                axMapControl1.ClearLayers();
                axMapControl1.LoadMxFile(sMxdFilePath);
                Cleandata clean = new Cleandata();
                clean.cleanData(axMapControl1, dataFile, number);
                number += 1;
            }
            if (number == 4)   //导入图17，删除调查信息
            {
                sMxdFilePath = InputFilePath + "17现场调查点分布图-A3.mxd";
                //通过AxMapControl来编辑数据
                axMapControl1.ClearLayers();
                axMapControl1.LoadMxFile(sMxdFilePath);
                Cleandata clean = new Cleandata();
                clean.cleanData(axMapControl1, dataFile, number);
                number += 1;
            }
            if (number == 5)   //导入图6，删除距离分布信息
            {
                sMxdFilePath = InputFilePath + "6震中与主要城市距离分布图-A3.mxd";
                //通过AxMapControl来编辑数据
                axMapControl1.ClearLayers();
                axMapControl1.LoadMxFile(sMxdFilePath);
                Cleandata clean = new Cleandata();
                //距离分布数据较多，需要多删几次
                for (int j = 0; j < 10;j++ )
                {
                    clean.cleanData(axMapControl1, dataFile, number);
                }  
            }
            MessageBox.Show("清理成功！");
        }
    }
}
