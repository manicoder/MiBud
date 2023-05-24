using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Essentials;
using System;

namespace MiBud.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ClusterPage : ContentPage
    {
        SKSurface surface;
        SKCanvas canvas;
        float height, width = 0;
        int strokeWidth = 25;
        double speed = 0;


        public ClusterPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            MessagingCenter.Send(this, "AllowLandscape");
            MessagingCenter.Unsubscribe<MainPage>(this, "AllowLandscape");

            //Device.StartTimer(TimeSpan.FromSeconds(1f / 60), () =>
            //{
            //    canvasView.InvalidateSurface();
            //    return true;
            //});
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Send(this, "PreventLandscape");
            MessagingCenter.Unsubscribe<MainPage>(this, "PreventLandscape");
        }

        SKPaint textPaint = new SKPaint
        {
            Color = SKColors.Black,
            TextSize = 75,
            TextAlign = SKTextAlign.Center
        };

        SKPaint vWhitePaint = new SKPaint
        {
            Color = SkiaSharp.SKColor.Parse("#000000"),
            Style = SKPaintStyle.Stroke,
            StrokeWidth = 10,
            StrokeCap = SKStrokeCap.Round,
            IsAntialias = true,
        };
        // MeterLine
        SKPaint vMeterLine = new SKPaint
        {
            Color = SkiaSharp.SKColor.Parse("#000000"),
            Style = SKPaintStyle.Stroke,
            StrokeWidth = 10,
            StrokeCap = SKStrokeCap.Round,
            IsAntialias = true
        };
        private void canvasView_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            SKSurface surface = e.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();

            DateTime dateTime = DateTime.Now;

            // Translate to center of canvas
            canvas.Translate(e.Info.Width / 2, e.Info.Height / 2);
            //canvas.Scale(width / 200f);
            // Rotate around center of canvas
            //canvas.RotateDegrees(revolveDegrees);

            // Translate horizontally
            float radius = Math.Min(e.Info.Width, e.Info.Height) / 3;
            //canvas.Translate(radius, 0);

            canvas.DrawCircle(0,0,230, vWhitePaint);
            canvas.Save();
            
            //float seconds = dateTime.Second + dateTime.Millisecond / 1000f;
            canvas.RotateDegrees(Convert.ToInt32(20 * speed));
            txtSpeed.Text = $"{Convert.ToInt32(20* speed)}";
            Console.WriteLine($"Calculate = 6*{speed} : {6 * speed}");
            canvas.DrawLine(0, 0,0, 190, vMeterLine);
            //canvas.Restore();
        }

        private void canvasView_SizeChanged(object sender, System.EventArgs e)
        {

        }

        private void Slider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            speed = e.NewValue;
            canvasView.InvalidateSurface();
        }

        private void ContentPage_SizeChanged(object sender, System.EventArgs e)
        {
            try
            {
                var obj = (ContentPage)sender;
                //height = (float)obj.Height;
                //width= (float)obj.Width;
            }
            catch (System.Exception ex)
            {
            }
        }
    }
}