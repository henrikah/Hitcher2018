using Hitcher2018.Services.AccountServices;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Hitcher2018.Services.ApiServices
{
    public enum HTTP
    {
        GET = 0,
        POST = 1,
        PUT = 2,
        DELETE = 3
    }
    static class Api
    {
        private static readonly string Uri = "http://localhost:4964/api/";
        public static async Task<T> QueryAsync<T>(string path, HTTP method, T objectReference = default(T))
        {
            using (var client = new HttpClient())
            {
                if(AccountService.AuthorizeHeaders() != null)
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(AccountService.AuthorizeHeaders());
                }
                HttpResponseMessage query;
                switch (method)
                {
                    case HTTP.GET:
                        query = await client.GetAsync(Uri + path);
                        break;
                    case HTTP.POST:
                        query = await client.PostAsJsonAsync(Uri + path, objectReference);
                        break;
                    case HTTP.PUT:
                        query = await client.PutAsJsonAsync(Uri + path, objectReference);
                        break;
                    case HTTP.DELETE:
                        query = await client.DeleteAsync (Uri + path);
                        break;
                    default:
                        query = default(HttpResponseMessage);
                        break;
                }
                return await ProsessAsync<T>(query);
            }
        }
        public static async Task<T> AuthenticateAsync<T>(FormUrlEncodedContent headerContent)
        {
            using (var client = new HttpClient())
            {
                return await ProsessAsync<T>(await client.PostAsync(Uri + "../token", headerContent));
            }
        }
        private static async Task<T> ProsessAsync<T>(HttpResponseMessage clientResponse)
        {
            var jsonString = await clientResponse.Content.ReadAsStringAsync();
            try
            {
                return JsonConvert.DeserializeObject<T>(jsonString);
            }
            catch (JsonSerializationException)
            {
                return default(T);
            } catch (Exception)
            {
                return default(T);
            }
        }
    }
}