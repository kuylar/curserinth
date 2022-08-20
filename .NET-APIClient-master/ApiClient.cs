using CurseForge.APIClient.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CurseForge.APIClient
{
    public partial class ApiClient : IDisposable
    {
        private IServiceCollection _serviceCollection;
        private IServiceProvider _serviceProvider;
        private const string curseForgeApiBaseUrl = "https://api.curseforge.com";

        private readonly string _apiKey;
        private readonly long _partnerId;
        private readonly string _contactEmail;

        private void InitHttpClientIfMissing()
        {
            if (string.IsNullOrWhiteSpace(_apiKey))
            {
                throw new MissingApiKeyException("You need to provide an API key to be able to call the API");
            }

            if (string.IsNullOrWhiteSpace(_contactEmail))
            {
                throw new MissingContactEmailException("You need to provide an email to be contacted on, if needed.");
            }

            BootstrapDependencyInjection();
        }

        private void BootstrapDependencyInjection()
        {
            if (_serviceCollection == null)
            {
                _serviceCollection = new ServiceCollection();
            }

            _serviceCollection.AddHttpClient("curseForgeClient", _httpClient =>
            {
                _httpClient.BaseAddress = new Uri(curseForgeApiBaseUrl);

                var cfUserAgent = new StringBuilder();

                cfUserAgent.Append("CurseForgeApiClient/" + Assembly.GetExecutingAssembly().GetName().Version);

                if (_partnerId > 0 || !string.IsNullOrWhiteSpace(_contactEmail))
                {
                    cfUserAgent.Append(" (");
                    if (_partnerId > 0)
                    {
                        cfUserAgent.Append(_partnerId);
                    }

                    if (!string.IsNullOrWhiteSpace(_contactEmail))
                    {
                        if (_partnerId > 0)
                        {
                            cfUserAgent.Append(";");
                        }

                        cfUserAgent.Append(_contactEmail);
                    }

                    cfUserAgent.Append(")");
                }

                _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", cfUserAgent.ToString());
                _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("x-api-key", _apiKey);
                _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "application/json");
            });

            _serviceProvider = _serviceCollection.BuildServiceProvider();
        }

        public ApiClient(string apiKey, long partnerId, string contactEmail)
        {
            _apiKey = apiKey;
            _partnerId = partnerId;
            _contactEmail = contactEmail;

            InitHttpClientIfMissing();
        }

        public ApiClient(string apiKey, string contactEmail)
        {
            _apiKey = apiKey;
            _partnerId = -1;
            _contactEmail = contactEmail;

            InitHttpClientIfMissing();
        }

        internal string GetQuerystring(params (string Key, object Value)[] queryParameters)
        {
            return queryParameters.Count(k => k.Value != null) > 0 ? "?" +
                string.Join("&",
                    queryParameters
                        .Where(k => k.Value != null)
                        .Select(k => k.Value is Enum
                            ? $"{System.Net.WebUtility.UrlEncode(k.Key)}={System.Net.WebUtility.UrlEncode(((int)k.Value).ToString())}"
                            : $"{System.Net.WebUtility.UrlEncode(k.Key)}={System.Net.WebUtility.UrlEncode(k.Value.ToString())}")) : string.Empty;
        }

        internal async Task<T> GET<T>(string endpoint, params (string Key, object Value)[] queryParameters)
        {
            var _httpClientFactory = _serviceProvider.GetService<IHttpClientFactory>();
            var _httpClient = _httpClientFactory.CreateClient("curseForgeClient");

            return await HandleResponseMessage<T>(
                await _httpClient.GetAsync(
                    endpoint + GetQuerystring(queryParameters)
                )
            );
        }

        internal async Task<T> POST<T>(string endpoint, object body)
        {
            var _httpClientFactory = _serviceProvider.GetService<IHttpClientFactory>();
            var _httpClient = _httpClientFactory.CreateClient("curseForgeClient");
            return await HandleResponseMessage<T>(
                await _httpClient.PostAsync(
                    endpoint,
                    new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json")
                )
            );
        }

        internal async Task<T> HandleResponseMessage<T>(HttpResponseMessage result)
        {
            if (!result.IsSuccessStatusCode)
            {
                throw new Exception(await result.Content.ReadAsStringAsync());
            }

            return JsonConvert.DeserializeObject<T>(await result.Content.ReadAsStringAsync());
        }

        public void Dispose()
        {
            // Empty because of legacy
        }
    }
}
