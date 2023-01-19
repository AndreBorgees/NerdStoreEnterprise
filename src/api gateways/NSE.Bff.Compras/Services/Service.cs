using NSE.Core.Comunication;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NSE.Bff.Compras.Services
{
    public abstract class Service
    {
        protected StringContent GetContent(object data)
        {
            return new StringContent(
                JsonSerializer.Serialize(data),
                Encoding.UTF8,
                "application/json");
        }

        protected async Task<T> DeserializeObjectResponse<T>(HttpResponseMessage responseMessage)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            return JsonSerializer.Deserialize<T>(await responseMessage.Content.ReadAsStringAsync(), options);
        }

        protected bool ProcessErrorsResponse(HttpResponseMessage responseMessage)
        {
            if(responseMessage.StatusCode == HttpStatusCode.BadRequest) return false;

            responseMessage.EnsureSuccessStatusCode();
            return true;
        }

        protected ResponseResult ReturnOk()
        {
            return new ResponseResult();
        }
    }
}
