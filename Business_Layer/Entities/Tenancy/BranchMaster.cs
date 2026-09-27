using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Layer.Tenancy
{
    public class BranchMaster
    {
        public int BranchId { get; set; }

        public int TenantId { get; set; }

        public string BranchName { get; set; } = string.Empty;

        public string BranchCode { get; set; } = string.Empty;

        public string? BranchEmail { get; set; }

        public string? BranchPhone { get; set; }

        public string? BranchAddress { get; set; }

        public string? BranchCity { get; set; }

        public string? BranchState { get; set; }

        public string? BranchPincode { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
