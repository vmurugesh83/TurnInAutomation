using System;
using System.Collections.Generic;
using System.Text;

using CatalogEstimating.Datasets;
using System.Data;

namespace CatalogEstimating.CustomGrids.Component.Editors
{
    public class VendorEditor : SourceGrid.Cells.Editors.ComboBox
    {
        public VendorEditor(Estimates dsEstimates, int VendorType, long? VendorID) : base(typeof(LongPair))
        {
            List<LongPair> vendors = new List<LongPair>();

            vendors.Add(new LongPair(-1, string.Empty));

            foreach (Estimates.vnd_vendorRow vr in dsEstimates.VendorsByType(VendorType, VendorID))
            {
                LongPair curVendor = new LongPair(vr.vnd_vendor_id, vr.description);
                vendors.Add(curVendor);
            }

            this.StandardValues = vendors;
            this.StandardValuesExclusive = true;
            this.Control.FormattingEnabled = true;
            this.AllowNull = true;
            this.EditableMode = SourceGrid.EditableMode.Focus | SourceGrid.EditableMode.AnyKey | SourceGrid.EditableMode.SingleClick;
        }

        public LongPair GetVendorfromDesc(string VendorDescription)
        {
            foreach (LongPair v in this.StandardValues)
                if (v.Display == VendorDescription)
                    return v;

            return null;
        }

        public LongPair GetVendorfromID(long VendorID)
        {
            foreach (LongPair v in this.StandardValues)
                if (v.Value == VendorID)
                    return v;

            return null;
        }
    }
}
