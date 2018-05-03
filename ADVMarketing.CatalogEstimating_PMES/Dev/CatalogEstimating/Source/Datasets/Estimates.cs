using System.Collections.Generic;

namespace CatalogEstimating.Datasets {


    partial class Estimates
    {
        partial class est_assemdistriboptionsDataTable
        {
        }
    
        partial class est_componentDataTable
        {
        }
    
        partial class est_assemdistriboptionsRow
        {
            public bool IsValid()
            {
                return ( !Isinsertfreightvendor_idNull() &&
                         !Ispst_postalscenario_idNull() &&
                         !Ismailhouse_idNull() );
            }
        }
        
        partial class est_componentRow
        {
            public decimal CalculateWeight()
            {
                if ( ppr_paperweightRow != null )
                    return width * height / 950000M * pagecount * ppr_paperweightRow.weight * 1.03M;
                else
                    return 0;
            }
        }
    
        public List<vnd_vendorRow> VendorsByType(int vnd_vendortype_id, long? VendorID)
        {
            List<vnd_vendorRow> vendorList = new List<vnd_vendorRow>();

            foreach (vnd_vendorRow vr in tablevnd_vendor.Select("", "description"))
                if ((VendorID.HasValue && vr.vnd_vendor_id == VendorID.Value) || (vr.active && tablevnd_vendorvendortype_map.Select("vnd_vendor_id = " + vr.vnd_vendor_id.ToString() + " and vnd_vendortype_id = " + vnd_vendortype_id.ToString()).Length > 0))
                    vendorList.Add(vr);

            return vendorList;
        }
    }
}
