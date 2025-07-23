using SixLabors.ImageSharp;
using Watermark.Net.src.WatermarkNet.Enums;

namespace Watermark.Net.src.WatermarkNet.Types
{
    public class WatermarkImageBase
    {
        protected ImagePosition _postion;
        protected float _scale;
        protected int _rotateAngle;
        protected bool _pave;
        protected Color? _backround;

        public ImagePosition Position 
        {
            get {  return _postion; } 
            set 
            { 
                _postion= value;
            }
        }

        public float Scale
        {
            get { return _scale; }
            set 
            {
                if(value <= 0) { throw new ArgumentOutOfRangeException("Scale", "Image scale can not be less or equal zero."); }
                _scale= value;
            }
        }

        public int RotateAngle
        {
            get { return _rotateAngle; }
            set
            {
                if (Math.Abs(value) > 180) { throw new ArgumentOutOfRangeException("RotateAngle", "Image rotate angle can not be larger than 180°."); }
                _rotateAngle= value;
            }
        }

        public bool Pave
        {
            get { return _pave; }
            set { _pave = value; }
        }

        public Color? BackroundColor
        {
            get { return _backround; }
            set { _backround = value; }
        }

        public WatermarkImageBase() 
        { 
            _scale= 1.0f;
        }
    }
}
