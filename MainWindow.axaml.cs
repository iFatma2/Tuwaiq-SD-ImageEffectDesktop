using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ImageEffectDesktop
{
    public partial class MainWindow : Window
    {
        private Bitmap? _originalImage;
        private bool _isAutoRunning = false;
        private CancellationTokenSource? _cts;
        private Random _rnd = new Random();
        private readonly string[] Effects = { "Grayscale", "Sepia", "Invert", "OilPaint", "Pixelate", "Vignette", "Glow", "Polaroid" };


        private int _manualEffectIndex = 0; 

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void OnUploadButtonClick(object? sender, RoutedEventArgs e)
        {
            var options = new FilePickerOpenOptions
            {
                Title = "Select an image",
                AllowMultiple = false,
                FileTypeFilter = new[]
                {
                    new FilePickerFileType("Images")
                    {
                        Patterns = new[] { "*.png", "*.jpg", "*.jpeg", "*.bmp" }
                    }
                }
            };

            var files = await this.StorageProvider.OpenFilePickerAsync(options);
            if (files != null && files.Count > 0)
            {
                var file = files[0];
                using var stream = await file.OpenReadAsync();
                _originalImage = new Bitmap(stream);
                ManualImage.Source = _originalImage;
                AutoImage.Source = _originalImage;
                _manualEffectIndex = 0; 
            }
        }

        // Manual Effects:
        private void OnManualButtonClick(object? sender, RoutedEventArgs e)
        {
            if (_originalImage == null) return;

            string effect = Effects[_manualEffectIndex];
            ManualImage.Source = ApplyEffect(_originalImage, effect);

            _manualEffectIndex = (_manualEffectIndex + 1) % Effects.Length;
        }

        // Auto Effects
        private async void OnAutoButtonClick(object? sender, RoutedEventArgs e)
        {
            if (_isAutoRunning)
            {
                _cts?.Cancel();
                _isAutoRunning = false;
                AutoButton.Content = "Start Auto Effects";
            }
            else
            {
                if (_originalImage == null) return;

                _isAutoRunning = true;
                AutoButton.Content = "Stop";
                _cts = new CancellationTokenSource();

                try
                {
                    while (!_cts.Token.IsCancellationRequested)
                    {
                        string effect = Effects[_rnd.Next(Effects.Length)];
                        AutoImage.Source = ApplyEffect(_originalImage, effect);
                        await Task.Delay(3000, _cts.Token);
                        //Thread.Sleep(3000);

                    }
                }
                catch (TaskCanceledException) { }
            }
        }

        // Apply effect using ImageSharp
        private Bitmap ApplyEffect(Bitmap bitmap, string effect)
        {
            if (bitmap == null) return null!;

            using var ms = new MemoryStream();
            bitmap.Save(ms);
            ms.Position = 0;

            using var image = SixLabors.ImageSharp.Image.Load<Rgba32>(ms);

            switch (effect)
            {
                case "Grayscale":
                    image.Mutate(x => x.Grayscale());
                    break;
                case "Sepia":
                    image.Mutate(x => x.Sepia());
                    break;
                case "Invert":
                    image.Mutate(x => x.Invert());
                    break;
                case "OilPaint":
                    image.Mutate(x => x.OilPaint(10, 20));
                    break;
               case "Pixelate":
                    image.Mutate(x => x.Pixelate(10));  
                    break;
                case "Vignette":
                    image.Mutate(x => x.Vignette());
                    break;
                case "Glow":
                    image.Mutate(x => x.Glow(0.5f));
                    break;
                case "Polaroid":
                    image.Mutate(x => x.Polaroid());
                    break;

            }

            var outStream = new MemoryStream();
            image.SaveAsPng(outStream);
            outStream.Position = 0;
            return new Bitmap(outStream);
        }
    }
}
