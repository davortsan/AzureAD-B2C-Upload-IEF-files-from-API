using Microsoft.AspNetCore.Mvc;
using Upload_IEF_files_from_API.Interfaces;
using Upload_IEF_files_from_API.Models;

namespace Upload_IEF_files_from_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class B2CPolicyController : ControllerBase
    {
        #region Vars
        private IB2CPolicy B2CPolicy { get; set; }
        #endregion

        #region Public
        public B2CPolicyController(IB2CPolicy b2cPolicy) { this.B2CPolicy = b2cPolicy; }

        [HttpGet]
        public ActionResult<IEnumerable<B2CPolicyModel>> List()
        {
            try
            { return this.Ok(B2CPolicy.ListB2CPolicies()); }
            catch (Exception ex)
            { return this.BadRequest("Unable to get all custom policies - " + ex.Message); }
        }

        [HttpPost("{trustFrameworkPolicyId}")]
        public ActionResult<B2CPolicyModel> Update(string trustFrameworkPolicyId)
        {
            try
            {
                this.B2CPolicy.UpdateB2CPolicy(trustFrameworkPolicyId);

                return this.Ok("The trustFrameworkPolicy was updated successfully!");
            }
            catch (Exception ex)
            { return this.BadRequest("Unable to update the trustFrameworkPolicy - " + ex.Message); }
        }

        [HttpDelete("{trustFrameworkPolicyId}")]
        public async Task<ActionResult> Delete(string trustFrameworkPolicyId)
        {
            try
            {
                var _result = await this.B2CPolicy.RemoveB2CPolicy(trustFrameworkPolicyId);

                if (_result) { return this.Ok("The trustFrameworkPolicy was removed successfully!"); }

                return this.BadRequest("Unable to remove the trustFrameworkPolicy from the B2C tenant.");
            }
            catch (Exception ex)
            { return this.BadRequest("Unable to remove the trustFrameworkPolicy - " + ex.Message); }
        }
        #endregion

        #region Private
        #endregion
    }
}
