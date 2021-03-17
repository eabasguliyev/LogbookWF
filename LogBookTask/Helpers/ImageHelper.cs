using System;
using System.Drawing;
using System.IO;

namespace LogBookTask.Helpers
{
    public static class ImageHelper
    {
        public static Byte[] ConvertImageToBytes(Image image)
        {
            var ms = new MemoryStream();
            image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            var bytes = ms.ToArray();

            return bytes;
        }

        public static Image ConvertBytesToImage(Byte[] bytes)
        {
            var imageMemoryStream = new MemoryStream(bytes);

            var image = Image.FromStream(imageMemoryStream);

            return image;
        }
    }
}