#region Using Directives

using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using CatalogEstimating.Properties;

#endregion

namespace CatalogEstimating
{
    public static class PerformanceLog
    {
        #region PInvoke Declarations

        [DllImport( "kernel32.dll", SetLastError = true )]
        private static extern bool QueryPerformanceFrequency( out long frequency );

        [DllImport( "kernel32.dll", SetLastError = true )]
        static extern bool QueryPerformanceCounter( out long lpPerformanceCount );

        #endregion

        #region Private Variables

        private static bool _loggingOn = false;
        private static Dictionary<string, PerformanceEvent> _dictTicks = new Dictionary<string, PerformanceEvent>();
        private static long _frequency;
        private static TextWriter _writer;

        #endregion

        #region Construction

        static PerformanceLog()
        {
            // Read the Configuration File to see if logging is turned on
            if ( Settings.Default.PerformanceLogging )
                _loggingOn = true;
            
            if ( _loggingOn )
            {
                // Get the performance counter frequency
                QueryPerformanceFrequency( out _frequency );

                string filename = string.Format( "PerformanceLog_{0}.log", MainForm.WorkingDatabase.FriendlyName );
                _writer = new StreamWriter( filename, true );
            }
        }

        #endregion

        #region Public Methods

        public static void Start( string eventName )
        {
            if ( _loggingOn )
            {
                // Create the event details class and add to the dictionary before recording
                // the tick count so that the dictionary overhead is not included in the time
                PerformanceEvent newEvent = new PerformanceEvent( eventName );
                _dictTicks[eventName] = newEvent;

                QueryPerformanceCounter( out newEvent.StartCount );
            }
        }

        public static void End( string eventName )
        {
            if ( _loggingOn )
            {
                long stop;
                QueryPerformanceCounter( out stop );

                PerformanceEvent perfEvent = _dictTicks[eventName];
                double seconds = (double)( stop - perfEvent.StartCount ) / _frequency;

                _writer.WriteLine( string.Format( "{0}: {1} - {2:0.#####} seconds", DateTime.Now, eventName, seconds ) );
                _writer.Flush();

                _dictTicks.Remove(eventName);
            }
        }

        public static void Close()
        {
            if ( _loggingOn )
            {
                _writer.Close();
                _writer.Dispose();
            }
        }

        #endregion

        #region PerformanceEvent Class

        private class PerformanceEvent
        {
            public PerformanceEvent( string eventName )
            {
                EventName = eventName;
            }

            public string EventName;
            public long   StartCount;
        }

        #endregion
    }
}