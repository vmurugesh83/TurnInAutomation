using System;

namespace CatalogEstimating
{
    public delegate void ControlActivatedHandler( object sender, ControlActivatedArgs e );

    public class ControlActivatedArgs : EventArgs
    {
        public ControlActivatedArgs( UserControlPanel deactivated, UserControlPanel activated )
        {
            Deactivated = deactivated;
            Activated   = activated;
        }

        public UserControlPanel Deactivated;
        public UserControlPanel Activated;
    }
}