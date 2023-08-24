﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ImageToDiscordRoles.HelperFunctions;
using static ImageToDiscordRoles.Discord;

namespace ImageToDiscordRoles
{
    public static class DiscordProfile
    {
        public static class INVISIBLE_CHAR
        {
            public const string RAW_CHAR = "​";
            
            // not working
            public const string HTML_REPRESENTATION = "&ZeroWidthSpace;";
        }
        public static class SPACE_CHAR
        {
            public const string RAW_CHAR = " ";
        }

        public const int MAX_IMAGE_WIDTH = 7;

        /// <summary>
        /// Expects to be navigated on the role tab.
        /// </summary>
        /// <param name="c"></param>
        public static void DrawChar(char c)
        {

        }

        /// <summary>
        /// Expects to be navigated on the role tab.
        /// Writes <paramref name="s"/> vertically
        /// </summary>
        /// <param name="s"></param>
        public static void Write(string s)
        {

        }

        /// <summary>
        /// Draw a whole line to ensure the image starts generating on
        /// a new line
        /// </summary>
        public static void DrawSpacing()
        {

        }

        /// <summary>
        /// Expects to be navigated on the role tab. 
        /// </summary>
        /// 
        /// <param name="image">
        /// Image has to be 7 pixels wide. Otherwise the final image 
        /// will be corrupted.
        /// </param>
        public static async void DrawImage(Bitmap image, string profileName)
        {
            // clear
            //Discord.DeleteEmptyRoles();

            for (int x = 0; x < image.Width;  x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    var color = HexConverter(image.GetPixel(x, y));
                    Console.WriteLine(HexConverter(image.GetPixel(x, y)));
                    Console.WriteLine(RGBConverter(image.GetPixel(x, y)));

                    await CreateRole(SPACE_CHAR.RAW_CHAR, color, "Oq_");
                    await SaveChanges();
                }
            }
        }
        private static String HexConverter(System.Drawing.Color c)
        {
            return "#" + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2");
        }

        private static String RGBConverter(System.Drawing.Color c)
        {
            return "RGB(" + c.R.ToString() + "," + c.G.ToString() + "," + c.B.ToString() + ")";
        }
    }
}
