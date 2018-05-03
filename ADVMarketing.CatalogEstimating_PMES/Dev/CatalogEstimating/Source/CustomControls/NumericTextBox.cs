#region Using Directives

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.ComponentModel;
using System.Drawing;

#endregion

namespace CatalogEstimating.CustomControls
{
    public class DecimalTextBox : NumericTextBox<decimal>
    {
        #region Construction

        public DecimalTextBox()
        {
            BuildFormatStrings();
        }

        #endregion

        #region Public Properties

        private int _decimalPrecision = 2;
        [DefaultValue( 2 )]
        public int DecimalPrecision
        {
            get { return _decimalPrecision;  }
            set 
            { 
                _decimalPrecision = value;
                BuildFormatStrings();
            }
        }

        #endregion

        #region Protected Overrides 

        protected override void BuildFormatStrings()
        {
            _editFormatString = "{0:F" + DecimalPrecision.ToString() + "}";
            if ( !ThousandsSeperator )
                _displayFormatString = _editFormatString;
            else
                _displayFormatString = "{0:#" + CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator + "##0." + new string( '0', DecimalPrecision ) + "}";
        }

        protected override bool TryPaste( string str )
        {
            // Block negative sign if appropriate
            if ( !AllowNegative && str.IndexOf( CultureInfo.CurrentCulture.NumberFormat.NegativeSign ) >= 0 )
                return false;

            string clean = str.Replace( CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator, "" );

            // Check to see if resulting value has too many decimal precision points
            int dotLoc = clean.IndexOf( CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator );
            if ( dotLoc >= 0 && ( clean.Length - dotLoc - 1 > DecimalPrecision ) )
                return false;

            decimal pasteVal;
            bool success = decimal.TryParse( clean, out pasteVal );
            if ( success )
                Value = pasteVal;

            return success;
        }

        protected override void OnKeyPress( KeyPressEventArgs e )
        {
            // Filter out invalid chars first, and return if one is found
            base.OnKeyPress( e );
            if ( e.Handled )
                return;

            // Is there a period?
            int dotPosition = Text.IndexOf( CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator );
            if ( dotPosition == -1 )
                return;  

            // This Key Press is to the right of the period and they're typing a number
            if ( SelectionStart > dotPosition && Char.IsDigit( e.KeyChar ) )
            {
                // Current number of decimal places
                int decimalPlaces = Text.Length - dotPosition - 1;
                if ( ( decimalPlaces >= DecimalPrecision ) && ( SelectionLength == 0 ) )
                {
                    // They're doing beyond the allowed precision
                    e.Handled = true;
                    Error();
                }
            }
        }

        #endregion
    }

    public class IntegerTextBox : NumericTextBox<int>
    {
        #region Construction

        public IntegerTextBox()
        {
            AllowDecimalPoint = false;
            BuildFormatStrings();
        }

        #endregion

        #region Protected Overrides

        protected override void BuildFormatStrings()
        {
            _editFormatString = "{0:G}";
            if ( ThousandsSeperator )
                _displayFormatString = "{0:#" + CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator + "##0}";
            else
                _displayFormatString = _editFormatString;
        }

        protected override bool TryPaste( string str )
        {
            // Block negative sign if appropriate
            if ( !AllowNegative && str.IndexOf( CultureInfo.CurrentCulture.NumberFormat.NegativeSign ) >= 0 )
                return false;

            int pasteVal;
            string clean = str.Replace( CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator, "" );
            bool success = int.TryParse( clean, out pasteVal );
            if ( success )
                Value = pasteVal;

            return success;
        }

        #endregion
    }

    public class NumericTextBox<T> : TextBox
        where T : struct
    {
        #region Private Variables

        private Timer _flashTimer = new Timer();
        private Color _oldBackColor;
        private bool  _bSwitchingFormats = false;

        private const int WM_PASTE = 0x302;

        #endregion

        #region Construction

        public NumericTextBox()
        {
            // Default to Right Align
            this.TextAlign = HorizontalAlignment.Right;

            // Flash red for 5 seconds
            _flashTimer.Interval = 500;
            _flashTimer.Tick +=new EventHandler( _flashTimer_Tick );
            _flashTimer.Enabled = false;
        }

        #endregion

        #region Properties

        private Color _flashColor = Color.Red;
        [DefaultValue( "Red" )]
        public Color FlashColor
        {
            get { return _flashColor;  }
            set { _flashColor = value; }
        }

        private bool _beep = false;
        [DefaultValue( false )]
        public bool Beep
        {
            get { return _beep;  }
            set { _beep = value; }
        }

        private bool _flash = false;
        [DefaultValue( false )]
        public bool Flash
        {
            get { return _flash;  }
            set { _flash = value; }
        }

        protected string _editFormatString = null;
        [Browsable( false )]
        public string EditFormatString
        {
            get { return _editFormatString; }
        }

        protected string _displayFormatString = null;
        [Browsable( false )]
        public string DisplayFormatString
        {
            get { return _displayFormatString; }
        }

        public T? Value
        {
            get 
            {
                if ( string.IsNullOrEmpty( Text ) )
                    return null;
                else
                    return Utilities.Convert<T>( Text );
            }
            set
            {
                if ( value == null )
                    Text = string.Empty;
                else if ( _displayFormatString == null )
                    Text = value.ToString();
                else
                    Text = string.Format( _displayFormatString, value );
            }
        }

        private bool _allowDecimalPoint = true;
        protected bool AllowDecimalPoint
        {
            get { return _allowDecimalPoint;  }
            set { _allowDecimalPoint = value; }
        }

        private bool _allowNegative = true;
        [DefaultValue( true )]
        public bool AllowNegative
        {
            get { return _allowNegative;  }
            set { _allowNegative = value; }
        }

        private bool _thousandsSep = true;
        [DefaultValue( true )]
        public bool ThousandsSeperator
        {
            get { return _thousandsSep; }
            set
            {
                _thousandsSep = value;
                BuildFormatStrings();
            }
        }

        #endregion

        #region Protected Overrides

        protected virtual void BuildFormatStrings()
        {
            throw new NotImplementedException();
        }

        protected virtual bool TryPaste( string str )
        {
            throw new NotImplementedException();
        }

        protected virtual void Error()
        {
            if ( Beep )
                System.Media.SystemSounds.Beep.Play();

            if ( Flash && !_flashTimer.Enabled )
            {
                _oldBackColor = BackColor;
                BackColor = FlashColor;
                _flashTimer.Enabled = true;
            }
        }

        protected override void OnKeyPress( KeyPressEventArgs e )
        {
            base.OnKeyPress( e );

            string decimalSep   = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            string negativeSign = CultureInfo.CurrentCulture.NumberFormat.NegativeSign;
            string keyInput     = e.KeyChar.ToString();

            bool valid = false;

            if ( Char.IsDigit( e.KeyChar ) || e.KeyChar == '\b' )
            {
                // Digits and the backspace key are both always valid
                valid = true;
            }
            else if ( AllowDecimalPoint && keyInput.Equals( decimalSep ) )
            {
                // Only one of these characters is allowed.  Be sure to catch the case where the 
                // selected text includes the . and they want to replace it with another dot
                if ( Text.IndexOf( e.KeyChar ) == -1 || SelectedText.IndexOf( e.KeyChar ) > -1 )
                    valid = true;
            }
            else if ( AllowNegative && keyInput.Equals( negativeSign ) )
            {
                // Only one of these characters is allowed, and it must be the first character
                // Be sure to catch the case where the selected text includes the - and they want 
                // to replace it with another minus sign
                if ( ( Text.IndexOf( e.KeyChar ) == -1 && SelectionStart == 0 ) || SelectedText.IndexOf( e.KeyChar ) > -1 )
                    valid = true;
            }

            // Swallow this invalid key and beep
            if ( !valid )
            {
                e.Handled = true;
                Error();
            }
        }

        protected override void OnEnter( EventArgs e )
        {
            T? val = Value;
            if ( val != null )
            {
                _bSwitchingFormats = true;
                Text = string.Format( _editFormatString, val.Value );
                _bSwitchingFormats = false;
            }

            base.OnEnter( e );
        }

        protected override void OnLeave( EventArgs e )
        {
            T? val = Value;
            if ( val != null )
            {
                _bSwitchingFormats = true;
                Text = string.Format( _displayFormatString, val.Value );
                _bSwitchingFormats = false;
            }

            base.OnLeave( e );
        }

        protected override void OnTextChanged( EventArgs e )
        {
            // Swallow the text changed event if doing an OnEnter/OnLeave
            if ( !_bSwitchingFormats )
                base.OnTextChanged( e );
        }

        protected override void WndProc( ref Message m )
        {
            if ( m.Msg == WM_PASTE )
            {
                if ( Clipboard.GetDataObject().GetDataPresent( DataFormats.Text ) )
                {
                    string clipboard = Clipboard.GetDataObject().GetData( DataFormats.Text ).ToString();
                    string newString = Text;
                    if ( SelectionLength >= 0 )
                        newString = Text.Remove( SelectionStart, SelectionLength );

                    newString = newString.Insert( SelectionStart, clipboard );

                    TryPaste( newString );
                }

                // Don't pass this on to the base class since I handled it
                return;
            }

            base.WndProc( ref m );
        }

        #endregion

        #region Event Handlers

        private void _flashTimer_Tick( object sender, EventArgs e )
        {
            _flashTimer.Enabled = false;
            BackColor = _oldBackColor;
        }

        #endregion
    }
}