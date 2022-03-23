using Microsoft.Extensions.Options;
using Microsoft.Graph.Auth;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using Upload_IEF_files_from_API.Interfaces;
using Upload_IEF_files_from_API.Models;

namespace Upload_IEF_files_from_API.Services
{
    public class B2CPolicy : IB2CPolicy
    {
        #region Vars
        private readonly GraphServiceClient graphClient;
        private readonly B2CTenantSettings b2cTenantSettings;
        #endregion

        #region Public
        public B2CPolicy(IOptions<B2CTenantSettings> userSettings)
        {
            this.b2cTenantSettings = userSettings.Value;

            IConfidentialClientApplication confidentialClientApplication = ConfidentialClientApplicationBuilder
                .Create(b2cTenantSettings.ClientId)
                .WithTenantId(b2cTenantSettings.TenantId)
                .WithClientSecret(b2cTenantSettings.ClientSecret)
                .Build();

            ClientCredentialProvider _authProvider = new ClientCredentialProvider(confidentialClientApplication);
            GraphServiceClient _graphClient = new GraphServiceClient(_authProvider);

            graphClient = _graphClient;
        }
        public bool UpdateB2CPolicy(string trustFrameworkPolicyId)
        {
            try
            {
                var _policyToChange = graphClient.TrustFramework.Policies[trustFrameworkPolicyId];
                var url = _policyToChange.AppendSegmentToRequestUrl("$value");

                var _request = new TrustFrameworkPolicyContentRequestBuilder(url, graphClient);

                // Read a policy
                _request.Request().GetAsync().Wait();

                string path = @"<newXMLFilePath>"; //Example: C:\temp\new_B2C_1A_SIGNUP_SIGNIN.XML

                _request.Request().PutAsync(new StreamReader(path).BaseStream).Wait();

                return true;
            }
            catch (Exception ex) { return false; }
        }

        public List<B2CPolicyModel> ListB2CPolicies()
        {
            try
            {
                var _lstB2CPolicies = new List<B2CPolicyModel>();

                var _result = this.graphClient.TrustFramework.Policies
                                    .Request()
                                    .GetAsync().Result;

                _lstB2CPolicies = ConvertToList(_result.CurrentPage);

                if (_result.CurrentPage != null) { return _lstB2CPolicies; }

                return null;
            }
            catch (Exception ex) { return null; }
        }
        public async Task<bool> RemoveB2CPolicy(string trustFrameworkPolicyId)
        {
            try
            {
                // Delete user by object ID
                await graphClient.TrustFramework.Policies[trustFrameworkPolicyId]
                        .Request()
                        .DeleteAsync();

                return true;
            }
            catch (Exception ex) { return false; }
        }
        #endregion

        #region Private
        public List<B2CPolicyModel> ConvertToList(IList<TrustFrameworkPolicy> policies)
        {
            List<B2CPolicyModel> _return = new List<B2CPolicyModel>();

            foreach (TrustFrameworkPolicy _current in policies)
            {
                B2CPolicyModel _b2cPolicy = new B2CPolicyModel();

                _b2cPolicy.Id = _current.Id;

                _return.Add(_b2cPolicy);

                _b2cPolicy = null;
            }

            return _return;
        }
        #endregion
    }
}
