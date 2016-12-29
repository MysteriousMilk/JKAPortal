using JKAPortal.API.Core;
using System.Windows.Media.Imaging;

namespace JKAPortal.Core
{
    public class Thumbnail : IThumbnail
    {
        public BitmapImage Image
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public Thumbnail()
        {

        }
    }
}
