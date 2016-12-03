using Altidude.Contracts;
using Altidude.Contracts.Events;
using Serilog;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace Altidude.Infrastructure
{
    public class AzureBlobChartImageManager : AzureBlobStorage, IHandleEvent<ChartChanged>
    {
        private const string ChartImageContainerName = "chartimages";

        private static ILogger _log = Log.ForContext<AzureBlobChartImageManager>();


        public AzureBlobChartImageManager(string connectionString)
          : base(connectionString, ChartImageContainerName)
        {
        }

        private Image CreateImage(string base64String)
        {
            var data = Convert.FromBase64String(base64String);

            using (var inputStream = new MemoryStream(data, 0, data.Length))
            {
                var image = Image.FromStream(inputStream);

                if (image.Width != 1140 || image.Height != 600)
                {
                    _log.Debug("Resizing image {width}x{height} => 1140x600");

                    image = ResizeImage(image, 1140, 600);
                }

                return image;
            }

        }

        /// <summary>
        /// Resize the image to the specified width and height.
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <param name="width">The width to resize to.</param>
        /// <param name="height">The height to resize to.</param>
        /// <returns>The resized image.</returns>
        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        public void Handle(ChartChanged evt)
        {
            _log.Debug("Handling ChartChanged in AzureBlobChartImageManager");

            var image = CreateImage(evt.Base64Image);

            using (var stream = new MemoryStream())
            {
                image.Save(stream, ImageFormat.Png);

                stream.Position = 0;

                Save(stream, string.Format(@"{0}.png", evt.Id), string.Format(@"{0}.png", evt.Id), "image/png");
            }

        }
    }
}