using ControlInventario.Web.Api.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ControlInventario.Web.Api.Controllers.ApiKey
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiKeyController : ControllerBase
    {
        [HttpGet]
        [AllowAnonymous]
        public ActionResult<ApiKeyModel> Get(Guid apiKey)
        {
            ControlInventario.Web.Api.Model.ApiKeyModel apiKeyModel = new ControlInventario.Web.Api.Model.ApiKeyModel
            {
                HttpStatusCode = System.Net.HttpStatusCode.OK
            };

            try
            {
                if (ControlInventario.Web.Api.Security.ApiKey.Key.Equals(apiKey))
                {
                    string accessToken = ControlInventario.Web.Api.Security.AccessToken.GenerateAccessToken(apiKey);

                    apiKeyModel.Data = accessToken;

                    return this.Ok(apiKeyModel);
                }

                apiKeyModel.HttpStatusCode = System.Net.HttpStatusCode.BadRequest;

                return this.BadRequest(apiKeyModel);
            }
            catch
            {
                apiKeyModel.HttpStatusCode = System.Net.HttpStatusCode.InternalServerError;

                return this.BadRequest(apiKeyModel);
            }
        }
    }
}