using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Controles
{

   public static class UIColors
    {
        /* ... under construction ... */

        // Buttons, DropDownButtons
        static internal Color BorderColor = Color.FromArgb(64, 64, 64); // 100
        //static internal Pen FocusPen = new Pen(Color.DarkGoldenrod);
        //static internal Pen FocusPen = new Pen(Color.FromArgb(169, 163, 136));
        static internal Pen FocusPen = new Pen(Color.FromArgb(179, 173, 146));

        static internal Color NormalColor1A = Color.Gray;
        static internal Color NormalColor1B = Color.FromArgb(32, 32, 32);
        static internal Color NormalColor2A = Color.Black;
        static internal Color NormalColor2B = Color.FromArgb(48, 48, 48);

        static internal Color HotColor1A = Color.FromArgb(148, 148, 148);
        static internal Color HotColor1B = Color.FromArgb(32, 32, 32);
        static internal Color HotColor2A = Color.Black;
        static internal Color HotColor2B = Color.FromArgb(60, 60, 60); //60

        static internal Color PressedColor1A = Color.FromArgb(180, 180, 180); // 164
        static internal Color PressedColor1B = Color.FromArgb(40, 40, 40); // 48
        static internal Color PressedColor2A = Color.Black; // 18
        static internal Color PressedColor2B = Color.FromArgb(72, 72, 72); // 72

        // Sliders Thumb
        static internal Pen ThumbBorderPen = new Pen(Color.FromArgb(80, 80, 80));

        static internal Color NormalThumbColor1 = Color.FromArgb(132, 132, 132);
        static internal Color NormalThumbColor2 = Color.Black;

        static internal Color HotThumbColor1 = Color.FromArgb(148, 148, 148);
        static internal Color HotThumbColor2 = Color.FromArgb(18, 18, 18);

        static internal Color PressedThumbColor1 = Color.FromArgb(180, 180, 180); // 164
        static internal Color PressedThumbColor2 = Color.FromArgb(18, 18, 18);

        // Menus
        static internal Color MenuBackgroundColor = Color.FromArgb(32, 32, 32);
        static internal Color MenuMarginColor = Color.FromArgb(48, 48, 48);
        //static internal Color MenuBorderColor = Color.DimGray;
        static internal Color MenuBorderColor = Color.FromArgb(64, 64, 64);
        //static internal Color MenuBorderColor = Color.FromArgb(56, 56, 56);

        static internal Color MenuSeparatorDarkColor = Color.FromArgb(80, 80, 80);
        static internal Color MenuSeparatorLightColor = Color.DimGray;

        static internal Color MenuHighlightColor = Color.FromArgb(64, 24, 24);
        //static internal Color MenuHighlightColor2 = Color.FromArgb(18, 18, 18);
        //static internal Color MenuHighlightColor = Color.FromArgb(48, 48, 48);
        //static internal Color MenuHighlightBorderColor = Color.FromArgb(80, 80, 80);
        static internal Color MenuHighlightBorderColor = Color.FromArgb(64, 64, 64);

        //static internal Color MenuTextEnabledColor = Color.FromArgb(169, 163, 136);
        static internal Color MenuTextEnabledColor = Color.FromArgb(179, 173, 146);
        static internal Color MenuTextDisabledColor = Color.DimGray;
    }


    public sealed class Dial : Control
    {

        // ******************************** Events / EventArgs

        #region Events / EventArgs

        /// <summary>
        /// Provides data for the Dial.ValueChanged event.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public class ValueChangedEventArgs : EventArgs
        {
            private Dial _base;
            private int _value;

            public ValueChangedEventArgs(Dial theDial)
            {
                _base = theDial;
            }

            /// <summary>
            /// Gets a numeric value that represents the position of the dial. Value 0 to 1000. 
            /// </summary>
            public int Value
            {
                get { return _value; }
                set { _value = _base._value; }
            }
        }

        /// <summary>
        /// Occurs when the position of the dial has changed.
        /// </summary>
        public event EventHandler<ValueChangedEventArgs> ValueChanged;

        #endregion

        // ******************************** Fields

        #region Fields

        #region Constants

        private const int FULL_CIRCLE = 360;
        private const int MINIMUM_VALUE = 0;
        private const float MAXIMUM_VALUE = 1000;

        #endregion

        private bool _dialing;

        private Bitmap _dialImage;
        private Size _dialSize;
        private int _dialCenter;

        private float _dialSpeed = 2.2F;   // dial speed accelerator / decelerator
        private float _dialAngle;

        private bool _hasMinMax = true;
        private float _minMaxUnits = 270 / MAXIMUM_VALUE;

        private float _dialAngleMin = -135; // depends on the 'start position' of the used image
        private float _dialAngleMax = 135;  // and the amount of total rotation (here 2 * 135 = 270)
        private float _dialAngleUnits = MAXIMUM_VALUE / 270;
        //private int[]       _dotsX              = { 9, 4, 2, 4, 9, 17, 26, 36, 44, 49, 51, 49, 44 };
        //private int[]       _dotsY              = { 44, 36, 26, 17, 9, 4, 2, 4, 9, 17, 26, 36, 44 };

        private int _oldLocationX;
        private int _oldLocationY;

        private int _value;
        private ValueChangedEventArgs _valueChangedArgs;

        private bool _disposed;

        #endregion


        // ******************************** Main - Constructor / Dispose

        #region Main - Constructor / Dispose

        public Dial()
        {
            SetStyle(ControlStyles.DoubleBuffer | ControlStyles.SupportsTransparentBackColor, true);

            //Image = Properties.Resources.Dial_Normal_2;
            //Image = Properties.Resources.Dial_Green_Gold;
            _valueChangedArgs = new ValueChangedEventArgs(this);
        }

        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                _disposed = true;
                if (disposing)
                {
                    _dialing = false;
                    _dialImage = null;
                }
            }
            base.Dispose(disposing);
        }

        #endregion

        // ******************************** Paint Dial

        #region Paint Dial

        protected override void OnPaint(PaintEventArgs e)
        {
            if (_dialImage != null)
            {
                Graphics g = e.Graphics;

                // dot indicators halo
                //SolidBrush b2 = new SolidBrush(Color.FromArgb(10, 10, 10));
                //g.FillEllipse(b2, 0, 0, 53, 53);
                //b2.Dispose();
                //// or
                ////g.FillEllipse(Brushes.Black, 0, 0, 53, 53);

                //SolidBrush b = new SolidBrush(Color.LightYellow);
                //bool changed = false;
                //for (int i = 0; i < 13; i++)
                //{
                //	if (!changed && _value < i * 83)
                //	{
                //		changed = true;
                //		b.Color = Color.Gray;
                //	}
                //	g.FillRectangle(b, _dotsX[i], _dotsY[i], 1, 1);
                //}

                SolidBrush b = new SolidBrush(Color.FromArgb(201, 195, 167));

                g.FillRectangle(b, 9, 44, 1, 1); // bottom left
                g.FillRectangle(b, 4, 36, 1, 1);

                g.FillRectangle(b, 2, 26, 1, 1); // left
                g.FillRectangle(b, 4, 17, 1, 1);

                g.FillRectangle(b, 9, 9, 1, 1); // top left
                g.FillRectangle(b, 17, 4, 1, 1);

                g.FillRectangle(b, 26, 2, 1, 1); // top

                g.FillRectangle(b, 36, 4, 1, 1);
                g.FillRectangle(b, 44, 9, 1, 1); // top right

                g.FillRectangle(b, 49, 17, 1, 1);
                g.FillRectangle(b, 51, 26, 1, 1); // right

                g.FillRectangle(b, 49, 36, 1, 1);
                g.FillRectangle(b, 44, 44, 1, 1); // bottom right

                b.Dispose();

                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;

                g.TranslateTransform(_dialCenter, _dialCenter);
                g.RotateTransform(_dialAngle);
                g.TranslateTransform(-_dialCenter, -_dialCenter);
                g.DrawImage(_dialImage, 5, 5);
            }
        }

        #endregion

        // ******************************** Properties - Image / Size / Value / Minimum / Maximum

        #region Properties - Image / Size / Value / Minimum / Maximum

        /// <summary>
        /// Get or sets the image of the dial control. The width and height of the image have to be equal.
        /// </summary>
        public Bitmap Image
        {
            get { return _dialImage; }
            set
            {
                if (value.Width == value.Height)
                {
                    _dialImage = value;
                    _dialImage.SetResolution(96, 96);

                    Width = _dialImage.Size.Width + 11;
                    Height = Width;

                    _dialSize.Width = Width;
                    _dialSize.Height = Height;

                    _dialCenter = Width / 2;

                    Invalidate();
                }
            }
        }

        public override Size MinimumSize
        {
            get { return _dialSize; }
            set { }
        }
        public override Size MaximumSize
        {
            get { return _dialSize; }
            set { }
        }

        /// <summary>
        /// Gets or sets a numeric value that represents the position of the dial. Value 0 to 1000. 
        /// </summary>
        public int Value
        {
            get { return _value; }
            set
            {
                if (SetDialValue(value) && ValueChanged != null)
                {
                    _valueChangedArgs.Value = _value;
                    ValueChanged(this, _valueChangedArgs);
                }
            }
        }

        private bool SetDialValue(int value)
        {
            bool changed = false;

            if (value < MINIMUM_VALUE)
            {
                if (_hasMinMax) value = MINIMUM_VALUE;
                else value = (int)MAXIMUM_VALUE + value;
            }
            else if (value > MAXIMUM_VALUE)
            {
                if (_hasMinMax) value = (int)MAXIMUM_VALUE;
                else value -= (int)MAXIMUM_VALUE;
            }

            if (_value != value)
            {
                _value = value;
                changed = true;

                if (_hasMinMax) _dialAngle = _dialAngleMin + (_minMaxUnits * _value);
                else _dialAngle = FULL_CIRCLE * (_value / MAXIMUM_VALUE);

                Invalidate();
            }
            return changed;
        }

        #endregion

        // ******************************** Methods - SetValue / Switch Image

        #region Methods - SetValue / Switch Image

        /// <summary>
        /// Sets the value of the dial without raising the ValueChanged event.
        /// </summary>
        /// <param name="value">The value to be set for the dial. Value 0 to 1000.</param>
        public bool SetValue(int value)
        {
            return SetDialValue(value);
        }

        // just for this application
        public void SwitchImage(bool redDial)
        {
            if (redDial) _dialImage = Properties.Resources.Dial_Red_2;
            else _dialImage = Properties.Resources.Dial_Normal_2;
            _dialImage.SetResolution(96, 96);
            Invalidate();
        }

        #endregion

        // ************************************************ Mouse Handling

        #region Mouse Handling

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (_dialImage != null && e.Button == MouseButtons.Left)
            {
                Focus();

                _oldLocationX = e.Location.X;
                _oldLocationY = e.Location.Y;
                _dialing = true;
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (_dialing)
            {
                float oldAngle = _dialAngle;

                if (_oldLocationY <= _dialCenter) _dialAngle += (e.Location.X - _oldLocationX) * _dialSpeed;
                else _dialAngle -= (e.Location.X - _oldLocationX) * _dialSpeed;

                if (_oldLocationX <= _dialCenter) _dialAngle -= (e.Location.Y - _oldLocationY) * _dialSpeed;
                else _dialAngle += (e.Location.Y - _oldLocationY) * _dialSpeed;

                if (_hasMinMax)
                {
                    if (_dialAngle <= _dialAngleMin) _dialAngle = _dialAngleMin;
                    else if (_dialAngle > _dialAngleMax) _dialAngle = _dialAngleMax;
                }

                if (_dialAngle < -FULL_CIRCLE) _dialAngle += FULL_CIRCLE;
                else if (_dialAngle > FULL_CIRCLE) _dialAngle -= FULL_CIRCLE;

                if (_hasMinMax) _value = (int)((_dialAngle - _dialAngleMin) * _dialAngleUnits);
                else
                {
                    if (_dialAngle < 0) _value = (int)(((FULL_CIRCLE + _dialAngle) / FULL_CIRCLE) * MAXIMUM_VALUE);
                    else _value = (int)((_dialAngle / FULL_CIRCLE) * MAXIMUM_VALUE);
                }

                if (_dialAngle != oldAngle)
                {
                    Invalidate();

                    if (ValueChanged != null)
                    {
                        _valueChangedArgs.Value = _value;
                        ValueChanged(this, _valueChangedArgs);
                    }
                }

                _oldLocationX = e.Location.X;
                _oldLocationY = e.Location.Y;
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            _dialing = false;
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            int value = _value - 50;
            if (e.Delta > 0)
            {
                value += 100;
                if (value > 1000) value = 1000;
            }
            else if (value < 0) value = 0;

            if (value != _value)
            {
                if (SetDialValue(value) && ValueChanged != null)
                {
                    _valueChangedArgs.Value = _value;
                    ValueChanged(this, _valueChangedArgs);
                }
            }
        }

        #endregion

        // ************************************************Key Handling

        #region Key Handling

        protected override bool IsInputKey(Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Right:
                case Keys.Left:
                case Keys.Up:
                case Keys.Down:
                    return true;
                case Keys.Shift | Keys.Right:
                case Keys.Shift | Keys.Left:
                case Keys.Shift | Keys.Up:
                case Keys.Shift | Keys.Down:
                    return true;
            }
            return base.IsInputKey(keyData);
        }

        // TODO
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            e.Handled = true;
            switch (e.KeyCode)
            {
                case Keys.Left:
                case Keys.NumPad4:
                    Value = _value - 10;
                    break;

                case Keys.Down:
                case Keys.NumPad2:
                    Value = _value - 100;
                    break;

                case Keys.Right:
                case Keys.NumPad6:
                    Value = _value + 10;
                    break;

                case Keys.Up:
                case Keys.NumPad8:
                    Value = _value + 100;
                    break;

                case Keys.PageUp:
                    Value = _value + 250;
                    break;

                case Keys.PageDown:
                    Value = _value - 250;
                    break;

                case Keys.Home:
                    Value = MINIMUM_VALUE;
                    break;

                case Keys.End:
                    Value = (int)MAXIMUM_VALUE;
                    break;

                default:
                    e.Handled = false;
                    break;
            }
        }

        #endregion
    }
    // TrackBar used with position slider
    sealed public class CustomSlider : TrackBar
    {
        // ********************************************* Fields

        #region Fields

        private Rectangle _trackRect;
        private Rectangle _tempRect; // split track area before and after thumb

        private SafeNativeMethods.RECT _thumbRECT;
        private Rectangle _thumbBorderRect;
        private Rectangle _thumbEraseRect;
        private Rectangle _thumbRect;

        private LinearGradientBrush _normalBrush;
        private LinearGradientBrush _hotBrush;
        private LinearGradientBrush _pressedBrush;

        private bool _hotThumb;
        private bool _pressedThumb;

        private LinearGradientBrush _track1Brush;
        private LinearGradientBrush _track2Brush;

        private Region _eraseRegion; // to reduce flicker
        private SolidBrush _eraseBrush;

        private Graphics _graphics;

        private bool _disposed;

        #endregion

        // ********************************************* Main

        #region Main

        public CustomSlider()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.Opaque, true);
            base.BackColor = Color.Black;
            AutoSize = false;

            // background
            _eraseRegion = new Region(ClientRectangle);
            _eraseBrush = new SolidBrush(Color.Black); // default backcolor

            // track
            _trackRect = new Rectangle(8, 8, Width - 16, 4);
            _tempRect = new Rectangle(8, 8, Width - 16, 4);
            _track1Brush = new LinearGradientBrush(_trackRect, Color.DimGray, Color.Black, LinearGradientMode.Vertical);
            _track1Brush.SetBlendTriangularShape(0.5F);
            _track2Brush = new LinearGradientBrush(_trackRect, Color.FromArgb(48, 48, 48), Color.Black, LinearGradientMode.Vertical);
            //track2Brush.SetBlendTriangularShape(0.5F);

            // thumb
            _thumbRECT = new SafeNativeMethods.RECT();
            _thumbBorderRect = new Rectangle(0, 2, 9, 14);
            _thumbEraseRect = new Rectangle(0, 2, 10, 15);
            _thumbRect = new Rectangle(0, 3, 8, 13);

            _normalBrush = new LinearGradientBrush(_thumbRect, UIColors.NormalThumbColor1, UIColors.NormalThumbColor2, LinearGradientMode.Vertical);
            _normalBrush.SetBlendTriangularShape(0.5F);
            _hotBrush = new LinearGradientBrush(_thumbRect, UIColors.HotThumbColor1, UIColors.HotThumbColor2, LinearGradientMode.Vertical);
            _hotBrush.SetBlendTriangularShape(0.5F);
            _pressedBrush = new LinearGradientBrush(_thumbRect, UIColors.PressedThumbColor1, UIColors.PressedThumbColor2, LinearGradientMode.Vertical);
            _pressedBrush.SetBlendTriangularShape(0.5F);

            _graphics = CreateGraphics();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            _trackRect.Width = Width - 16;
            _graphics.Dispose();
            _graphics = CreateGraphics();
            base.OnSizeChanged(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (_thumbBorderRect.Contains(e.Location))
            {
                if (!_hotThumb)
                {
                    _hotThumb = true;
                    Invalidate(_thumbRect);
                }
            }
            else if (_hotThumb && !_pressedThumb)
            {
                _hotThumb = false;
                Invalidate(_thumbRect);
            }

            base.OnMouseMove(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (_hotThumb && e.Button == MouseButtons.Left)
            {
                _pressedThumb = true;
                Invalidate(_thumbRect);
            }
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (_pressedThumb)
            {
                _pressedThumb = false;
                Invalidate(_thumbRect);
            }
            base.OnMouseUp(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            if (_hotThumb)
            {
                _hotThumb = _pressedThumb = false;
                Invalidate(_thumbRect);
            }
            base.OnMouseLeave(e);
        }

        [DefaultValue(typeof(Color), "Black")]
        public override Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                base.BackColor = value;
                _eraseBrush.Color = value;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _eraseRegion.Dispose();
                    _eraseBrush.Dispose();

                    _normalBrush.Dispose();
                    _hotBrush.Dispose();
                    _pressedBrush.Dispose();
                    _track1Brush.Dispose();
                    _track2Brush.Dispose();

                    _graphics.Dispose();
                }
                _disposed = true;
                base.Dispose(disposing);
            }
        }

        #endregion

        // ********************************************* OnPaint

        #region OnPaint

        protected override void OnPaint(PaintEventArgs e)
        {
            // get the thumb position first - also used with draw track
            SafeNativeMethods.SendMessage(Handle, SafeNativeMethods.TBM_GETTHUMBRECT, IntPtr.Zero, ref _thumbRECT);
            _thumbBorderRect.X = _thumbEraseRect.X = _thumbRECT.left;
            _thumbRect.X = _thumbRECT.left + 1;

            // erase background
            _eraseRegion.MakeEmpty();
            _eraseRegion.Union(ClientRectangle);
            _eraseRegion.Exclude(_trackRect);
            _eraseRegion.Exclude(_thumbEraseRect);
            _graphics.FillRegion(_eraseBrush, _eraseRegion);

            // draw track - different fill color before and after thumb position
            // before thumb
            _tempRect.X = _trackRect.X;
            //g.DrawRectangle(Pens.DimGray, trackRect);
            _tempRect.Width = _thumbBorderRect.X - _trackRect.X;
            _graphics.FillRectangle(_track1Brush, _tempRect);
            // after thumb
            _tempRect.X = _thumbBorderRect.X + _thumbBorderRect.Width;
            _tempRect.Width = _trackRect.Width - _thumbBorderRect.X;
            _graphics.FillRectangle(_track2Brush, _tempRect);

            // draw thumb
            if (_hotThumb)
            {
                _graphics.FillRectangle(_pressedThumb ? _pressedBrush : _hotBrush, _thumbRect);
            }
            else
            {
                _graphics.FillRectangle(_normalBrush, _thumbRect);
            }
            _graphics.DrawRectangle(UIColors.ThumbBorderPen, _thumbBorderRect);
        }

        #endregion
    }
    public sealed class CustomButton : Button
    {
        // ************************************ Fields

        #region Fields

        private LinearGradientBrush _normalBrush1;
        private LinearGradientBrush _normalBrush2;
        private LinearGradientBrush _hotBrush1;
        private LinearGradientBrush _hotBrush2;
        private LinearGradientBrush _pressedBrush1;
        private LinearGradientBrush _pressedBrush2;

        private Rectangle _borderRect;
        private Rectangle _buttonRect1;
        private Rectangle _buttonRect2;

        private Pen _borderPen;
        private bool _hotButton;
        private bool _pressedButton;
        private bool _notifyDefault;
        private bool _notifyDefaultDraw;

        private TextFormatFlags _textFlags;

        private bool _disposed;

        #endregion

        // ************************************ Main

        #region Main

        public CustomButton()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.Opaque, true);
            TextAlign = ContentAlignment.MiddleCenter;
            _textFlags = new TextFormatFlags();
            _textFlags = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter;

            _borderPen = new Pen(UIColors.BorderColor);
            SetButtonRectangle();
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control's focus border is drawn.
        /// </summary>
        [Category("Appearance"), Description("Indicates whether the control's focus border is drawn."), DefaultValue(false)]
        public bool FocusBorder
        {
            get { return _notifyDefaultDraw; }
            set { _notifyDefaultDraw = value; }
        }

        /// <summary>
        /// Gets or sets the control's border color.
        /// </summary>
        [Category("Appearance"), Description("The control's border color.")]
        public Color BorderColor
        {
            get { return _borderPen.Color; }
            set { _borderPen.Color = value; }
        }

        public override void NotifyDefault(bool value)
        {
            _notifyDefault = value;
            base.NotifyDefault(value);
        }

        // need this for strange spacing of WebDings chars
        [DefaultValue(ContentAlignment.MiddleCenter)]
        public override ContentAlignment TextAlign
        {
            get
            {
                return base.TextAlign;
            }
            set
            {
                switch (value)
                {
                    case ContentAlignment.MiddleLeft:
                        _textFlags = TextFormatFlags.Left | TextFormatFlags.VerticalCenter;
                        break;
                    case ContentAlignment.MiddleCenter:
                        _textFlags = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter;
                        break;
                    case ContentAlignment.MiddleRight:
                        _textFlags = TextFormatFlags.Right | TextFormatFlags.VerticalCenter;
                        break;

                    // This is only to get the pause/next/previous/stop symbols centered!
                    case ContentAlignment.BottomCenter:
                        _textFlags = TextFormatFlags.Bottom;
                        break;
                }
                base.TextAlign = value;
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            SetButtonRectangle();
        }

        protected override void OnMouseEnter(EventArgs eventargs)
        {
            _hotButton = true;
            base.OnMouseEnter(eventargs);
        }

        protected override void OnMouseLeave(EventArgs eventargs)
        {
            _hotButton = false;
            base.OnMouseLeave(eventargs);
        }

        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            _pressedButton = true;
            base.OnMouseDown(mevent);
        }

        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            _pressedButton = false;
            Invalidate();
            base.OnMouseUp(mevent);
        }

        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {

                if (disposing)
                {
                    _borderPen.Dispose();
                    _normalBrush1.Dispose();
                    _normalBrush2.Dispose();
                    _hotBrush1.Dispose();
                    _hotBrush2.Dispose();
                    _pressedBrush1.Dispose();
                    _pressedBrush2.Dispose();
                }
                base.Dispose(disposing);
                _disposed = true;
            }
        }

        #endregion

        // ************************************ OnPaint

        #region OnPaint

        protected override void OnPaint(PaintEventArgs pevent)
        {
            // erase background - dropped (dropped rounded corners)
            // pevent.Graphics.FillRectangle(eraseBrush, this.ClientRectangle);

            // draw fill
            if (_hotButton)
            {
                if (_pressedButton)
                {
                    pevent.Graphics.FillRectangle(_pressedBrush1, _buttonRect1);
                    pevent.Graphics.FillRectangle(_pressedBrush2, _buttonRect2);
                }
                else
                {
                    pevent.Graphics.FillRectangle(_hotBrush1, _buttonRect1);
                    pevent.Graphics.FillRectangle(_hotBrush2, _buttonRect2);
                }
            }
            else
            {
                pevent.Graphics.FillRectangle(_normalBrush1, _buttonRect1);
                pevent.Graphics.FillRectangle(_normalBrush2, _buttonRect2);
            }

            // draw border
            if (_notifyDefault && _notifyDefaultDraw) pevent.Graphics.DrawRectangle(UIColors.FocusPen, _borderRect);
            else pevent.Graphics.DrawRectangle(_borderPen, _borderRect);

            // draw text
            if (_textFlags == TextFormatFlags.Bottom)
            {
                // This is only to get the pause/next/previous/stop symbols centered!
                TextRenderer.DrawText(pevent.Graphics, Text, Font, new Point(5, 1),
                    Enabled ? ForeColor : Color.DimGray, Color.Transparent);
            }
            else
            {
                TextRenderer.DrawText(pevent.Graphics, Text, Font, ClientRectangle,
                    Enabled ? ForeColor : Color.DimGray, Color.Transparent, _textFlags);
            }
        }

        #endregion

        // ************************************ SetButtonRectangle

        #region SetButtonRectangle

        private void SetButtonRectangle()
        {
            // dropped rounded corners

            if (_normalBrush1 != null)
            {
                _normalBrush1.Dispose();
                _normalBrush2.Dispose();
                _hotBrush1.Dispose();
                _hotBrush2.Dispose();
                _pressedBrush1.Dispose();
                _pressedBrush2.Dispose();
            }
            _borderRect.Width = ClientRectangle.Width - 1;
            _borderRect.Height = ClientRectangle.Height - 1;

            _buttonRect1 = new Rectangle(0, 0, ClientRectangle.Width, ClientRectangle.Height / 2);
            // gradient fix with odd heights
            _buttonRect2 = _buttonRect1.Height % 2 == 0 ? new Rectangle(0, ClientRectangle.Height / 2, ClientRectangle.Width, ClientRectangle.Height / 2) : new Rectangle(0, ClientRectangle.Height / 2, ClientRectangle.Width, ClientRectangle.Height / 2 + 2);

            _normalBrush1 = new LinearGradientBrush(_buttonRect1, UIColors.NormalColor1A, UIColors.NormalColor1B, LinearGradientMode.Vertical);
            _normalBrush2 = new LinearGradientBrush(_buttonRect2, UIColors.NormalColor2A, UIColors.NormalColor2B, LinearGradientMode.Vertical);
            _hotBrush1 = new LinearGradientBrush(_buttonRect1, UIColors.HotColor1A, UIColors.HotColor1B, LinearGradientMode.Vertical);
            _hotBrush2 = new LinearGradientBrush(_buttonRect2, UIColors.HotColor2A, UIColors.HotColor2B, LinearGradientMode.Vertical);
            _pressedBrush1 = new LinearGradientBrush(_buttonRect1, UIColors.PressedColor1A, UIColors.PressedColor1B, LinearGradientMode.Vertical);
            _pressedBrush2 = new LinearGradientBrush(_buttonRect2, UIColors.PressedColor2A, UIColors.PressedColor2B, LinearGradientMode.Vertical);
        }

        #endregion
    }


}
