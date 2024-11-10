using api.Dtos.Stock;
using api.Interfaces;
using api.Models;
using AutoMapper;
using Newtonsoft.Json;

namespace api.Service
{
    public class FMPService : IFMPService
    {
        private HttpClient _httpClient;
        private IConfiguration _config;
        private IMapper _mapper;
        public FMPService(HttpClient httpClient, IConfiguration config ,IMapper mapper)
        {
            _httpClient = httpClient;
            _config = config;
            _mapper = mapper;
        }
        public async Task<Stock> FindStockBySymbolAsync(string symbol)
        {
            try
            {
                var result = await _httpClient.GetAsync($"https://financialmodelingprep.com/api/v3/profile/{symbol}?apikey={_config["FMPKey"]}");
                if (result.IsSuccessStatusCode)
                {
                    var content = await result.Content.ReadAsStringAsync();
                    var tasks = JsonConvert.DeserializeObject<FMPStock[]>(content);
                    var fmpStock = tasks[0];
                    if (fmpStock != null)
                    {
                        return _mapper.Map<Stock>(fmpStock);
                    }
                    return null;
                }
                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
    }
}
