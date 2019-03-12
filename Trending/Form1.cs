using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Trending
{
    public partial class Form1 : Form
    {
        private Dictionary<string, double> tagsValues = new Dictionary<string, double>();
        private Dictionary<string, double[]> tagsArrays = new Dictionary<string, double[]>();

        private static ServiceReference1.ServiceTrendingClient client = null;
        private static InstanceContext context = null;
        private delegate void UpdateTrendingDelegate(string tagId, double value);
        private static UpdateTrendingDelegate UpdateTrendingHandler = null;

        public class ServiceCallback : ServiceReference1.IServiceTrendingCallback
        {
            public void RaiseAlarm(string alarm)
            {
            }

            public void SendToTrending(string tagId, double value)
            {
                if (UpdateTrendingHandler != null)
                {
                    UpdateTrendingHandler(tagId, value);
                }
            }
        }

        private Thread thread;
        private Random rnd = new Random();
        //private List<double[]> arrays = new List<double[]>();

        public Form1()
        {
            context = new InstanceContext(new ServiceCallback());
            client = new ServiceReference1.ServiceTrendingClient(context);
            UpdateTrendingHandler = new UpdateTrendingDelegate(UpdateValue);

            client.TrendingInit();

            InitializeComponent();
            thread = new Thread(new ThreadStart(this.getNewValues));
            thread.IsBackground = true;
            thread.Start();
        }

        private void UpdateValue(string tagId, double value)
        {
            
            tagsValues[tagId] = value;
 
            foreach (Series ser in chart.Series)
            {
                if (ser.Name.Equals(tagId)) {
                    return;
                }
            }

            try { 
                Legend newLegend = new Legend();
                newLegend.Name = tagId + " Legend";
                newLegend.LegendStyle = LegendStyle.Column;
                newLegend.Docking = Docking.Left;
                newLegend.DockedToChartArea = "ChartArea1";
                newLegend.IsDockedInsideChartArea = true;
                chart.Legends.Add(newLegend);
                
                Series newSeries = new Series();
                newSeries.ChartArea = "ChartArea1";
                newSeries.ChartType = SeriesChartType.Spline;
                newSeries.Legend = newLegend.Name;
                
                newSeries.Name = tagId;
                //newSeries.XValueType = ChartValueType.String;
                chart.Series.Add(newSeries);

                

                tagsArrays[tagId] = new double[60];
            } catch
            {
                return;
            }
        }

        private void getNewValues()
        {
            while (true)
            {
                //testArray[testArray.Length - 1] = 20 * Math.Sin((double)DateTime.Now.Second / 60 * Math.PI);

                foreach (KeyValuePair<string, double[]> pair in tagsArrays)
                {
                    pair.Value[pair.Value.Length - 1] = tagsValues[pair.Key];
                    Array.Copy(pair.Value, 1, pair.Value, 0, pair.Value.Length - 1);
                }

                //testArray[testArray.Length - 1] = 0;
                //Array.Copy(testArray, 1, testArray, 0, testArray.Length - 1);

                if (chart.IsHandleCreated)
                {
                    this.Invoke((MethodInvoker)delegate { UpdateChart(); });
                }
                else
                {
                    //......
                }


                Thread.Sleep(1000);
            }
        }

        private void UpdateChart()
        {
            foreach (Series ser in chart.Series)
            {
                ser.Points.Clear();

                for (int i = 0; i < tagsArrays[ser.Name].Length - 1; ++i)
                {
                    ser.Points.AddY(tagsArrays[ser.Name][i]);
                }
            }
            
        }

        

        
    }
}
