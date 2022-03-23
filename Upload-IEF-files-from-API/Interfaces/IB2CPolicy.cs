using Upload_IEF_files_from_API.Models;

namespace Upload_IEF_files_from_API.Interfaces
{
    public interface IB2CPolicy
    {
        List<B2CPolicyModel> ListB2CPolicies();
        Task<bool> RemoveB2CPolicy(string trustFrameworkPolicyId);
        bool UpdateB2CPolicy(string trustFrameworkPolicyId);
    }
}
