using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowPositionSavingSample
{
    public class WindowPositionModel
    {
        private int _screenIndex;
        public int ScreenIndex
        {
            get => _screenIndex;
            set => _screenIndex = value;
        }

        private long _screenPtr;
        /// <summary>
        /// 雖然OS重開會改變，但還是先留著
        /// </summary>
        public long ScreenPtr
        {
            get => _screenPtr;
            set => _screenPtr = value;
        }

        private double _scaleFactor;
        public double ScaleFactor
        {
            get => _scaleFactor;
            set => _scaleFactor = value;
        }

        private double _scaledWidth;
        public double ScaledWidth
        {
            get => _scaledWidth;
            set => _scaledWidth = value;
        }

        private double _scaledHeight;
        public double ScaledHeight
        {
            get => _scaledHeight;
            set => _scaledHeight = value;
        }

        private double _top;

        /// <summary>
        /// 相對於螢幕零點的 Y 座標
        /// </summary>
        public double Top
        {
            get => _top;
            set => _top = value;
        }


        private double _left;
        /// <summary>
        /// 相對於螢幕零點的 X 座標
        /// </summary>
        public double Left
        {
            get => _left;
            set => _left = value;
        }
    }
}
