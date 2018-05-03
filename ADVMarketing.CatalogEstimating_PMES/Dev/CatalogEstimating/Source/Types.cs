using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace CatalogEstimating
{
    public static class Utilities
    {
        public static T Convert<T>( string s )
        {
            string clean = s.Replace( CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator, "" );
            return (T)System.Convert.ChangeType( clean, typeof( T ) );
        }
    }

    public class LongPair
    {
        private long _Value;
        private string _Display;

        public LongPair(long value, string display)
        {
            _Value = value;
            _Display = display;
        }

        public long Value
        {
            get { return _Value; }
        }

        public string Display
        {
            get { return _Display; }
        }

        public override string ToString()
        {
            return _Display;
        }

        public override bool Equals(object obj)
        {
            LongPair lp = obj as LongPair;

            if (lp != null)
                return _Value == lp.Value && _Display == lp.Display;
            else
                return false;
        }

        public override int GetHashCode()
        {
            string compositeValue = _Value.ToString() + " - " + _Display;
            return compositeValue.GetHashCode();
        }
    }

    public class IntPair
    {
        private int _Value;
        private string _Display;

        public IntPair(int value, string display)
        {
            _Value = value;
            _Display = display;
        }

        public int Value
        {
            get { return _Value; }
        }

        public string Display
        {
            get { return _Display; }
        }

        public override string ToString()
        {
            return _Display;
        }

        public override bool Equals(object obj)
        {
            IntPair ip = obj as IntPair;

            if (ip != null)
                return _Value == ip.Value && _Display == ip.Display;
            else
                return false;
        }

        public override int GetHashCode()
        {
            string compositeValue = _Value.ToString() + " - " + _Display;
            return compositeValue.GetHashCode();
        }
    }
}
