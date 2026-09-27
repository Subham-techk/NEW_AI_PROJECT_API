using Application_Layer.Tenancy.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.Tenancy.Interfaces
{
    public interface IBranchRepository
    {
        Task<List<BranchDto>> GetAllBranchAsync(int tenantId);
        Task<BranchDto?> GetBranchByIdAsync(int branchId,int tenantId);
        Task<int> SaveBranchAsync(BranchDto branch);
        Task<bool> ActivateDeactivateBranchAsync(int branchId,int tenantId,bool isActive);
    }
}
