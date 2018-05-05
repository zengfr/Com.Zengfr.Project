using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmartQuant.Charting;
using SmartQuant.Series;
using System.Drawing;
using SmartQuant.Indicators;
using SmartQuant.Data;
using Castle.ActiveRecord;
using NHibernate.Criterion;
using ConsoleApplication3.Stock;
using System.Linq;
using ConsoleControlSample;
namespace ConsoleApplication3
{
    public class MyCanvas : Canvas
    {
        public MyCanvas():base("Graph Canvas", "Pad range demo canvas", 800, 600)
        {
            Init();
        }
        private void Init()
        {
            Chart.GroupZoomEnabled = true;
            this.Clear();

            this.AddPad(0, 0, 1, 0.5);//.ForeColor = Color.Black;
            this.AddPad(0, 0.5, 1, 0.75);//.ForeColor = Color.Black;
            this.AddPad(0, 0.75, 1, 1);
           
            

            //Chart.Pad = Canvas.cd(0);


            //Chart.Pad.AxisBottom.GridEnabled = true;
            //Chart.Pad.AxisLeft.GridEnabled = true;

            //Chart.Pad.SetRange(-100, 100, -200, 200);
            //Chart.Pad.Title.Text = "Pad with x = [-100:100], y = [-200:200] range";

            //TLine Line = new TLine(-50, -100, 50, 100);
            //Line.Draw();

            //TText Text;
            //Text = new TText("(-50, -100) point", -50, -100);
            //Text.Draw();

            //Text = new TText("(50, 100) point", 50, 100);
            //Text.Draw();


            var s = BuildDailySeries("sh600150");


            var pad = this.cd(1);
            pad.XGridEnabled = false;
            pad.YGridEnabled = false;
            pad.MouseWheelMode = EMouseWheelMode.ZoomX;
            pad.XAxisLabelFormat = "yyyy-MM-dd";
            pad.YAxisType = EAxisType.DateTime;
            //pad.ForeColor = Color.Black;
            //pad.MouseContextMenuEnabled = true;
             

           s.Draw();
           DrawEMA(s);
            
            //TLine Line = new TLine(DateTime.Today, 100, DateTime.Today.AddMonths(4), 500);
            //Line.Color = Color.Blue;
            //Line.Draw();
            
            var pad2= this.cd(2);
            pad2.XGridEnabled = false;
            pad2.YGridEnabled = false;
            pad2.XAxisType = EAxisType.DateTime;
            pad2.XAxisLabelFormat = "yyyy-MM-dd";
            DrawHistogram(s);
           

           var pad3= this.cd(3);
              pad3.XGridEnabled = false;
            pad3.YGridEnabled = false;

            // var sf = new K_Fast(s, 14, Color.Aqua); sf.Draw();
           // var ss = new K_Slow(s, 14, 10, Color.LightYellow); ss.Draw();

            var macd = new MACD(s, 6, 21); 
            macd.DrawStyle=EDrawStyle.Bar;
           // macd.Option=BarData.
            macd.Draw();

            // var vOSC = new VOSC(s, 14, 10);
            // vOSC.Draw();


           // var sar = new SAR(s, 0.2, 0.001, 0.02);
           // sar.Draw();

           // var cci = new CCI(s, 21, Color.Blue);
           // cci.Draw();

           // var adx = new ADX(s, 6, Color.Red);
           // adx.Draw();

            var f = new FormConsoleControlSample();
            
            f.TopLevel = false;

 
            this.Controls.Add(f); f.Show();
           
        }
        private static void DrawHistogram(DailySeries s)
        {
            var minDate = s.GetMinDateTime();
            var maxDate = s.GetMaxDateTime();
             var d = (int)((maxDate - minDate).TotalDays);
            var Hist = new Histogram(d, minDate.Ticks, maxDate.Ticks);
            Hist.FillColor = Color.Red;
            Hist.ToolTipEnabled=true;
            //Hist. = Color.Green;
            //Random r = new Random();

            var Hist2 = new Histogram(d, minDate.Ticks, maxDate.Ticks);
            Hist2.FillColor = Color.Green;
            Hist2.ToolTipEnabled = true;

            foreach (Daily item in s)
            {
                if (item.Close > item.Open)
                {
                    Hist.Add(item.DateTime.Ticks, item.Volume / 10000 );
                }
            }
             
            foreach (Daily item in s)
            {
                if (item.Close < item.Open)
                {
                    Hist2.Add(item.DateTime.Ticks, item.Volume / 10000);
                }
            }
            Hist.Draw();
            Hist2.Draw();
            
        }
        private static void DrawEMA(DailySeries s)
        {
            var days=BuildDaysList();
            foreach (int i in days)
            {
                var ema1 = new EMA(s, i, Color.FromArgb(2*i,0,0));

                ema1.Draw();
            }
        }
        private static int[] BuildDaysList()
        {
            int[] days = new int[] { 3, 6, 14, 21,36,72, 5, 10, 15, 20, 25, 30, 45, 60 };

            return days;
        }
        private static DailySeries BuildDailySeries(string stockCode)
        {
            var items = ActiveRecordMediator<StockFuQuanMarketHistory>.FindAll(
                      Restrictions.Eq("StockCode", stockCode)
                      );
            var s = new DailySeries("DELL - CSCO");
            s.Color = Color.Red;
            Random r = new Random();
            
                
                foreach (var item in items)
                {

                    var d = new Daily(item.EffectDate.Value, (double)item.开盘价, (double)item.最高价, (double)item.最低价, (double)item.收盘价
                        , (long)item.交易量);
                    //var d2 = d.Clone() as Daily;
                    //d2.DateTime = d.DateTime.AddDays(-25);
                    s.Add(d);// s.Add(d2);
                    d.BarType = BarType.Time;
                    
              }
                s.ChartStyle = ChartStyle.Candle;
                s.CandleWidth = 10;
            s.CandleWidthStyle = EWidthStyle.Pixel;
            
            s.CandleBlackColor = Color.Green;
            s.CandleWhiteColor = Color.Red;
            return s;
        }
    }
    
}
