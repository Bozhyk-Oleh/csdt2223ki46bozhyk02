using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sea_Battle
{
    public static class Utils
    {
        public static Image RotateImage(Bitmap img, RotateFlipType rotateFlipType)
        {
            img.RotateFlip(rotateFlipType);
            return img;
        }
    }
}
