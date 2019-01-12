using System;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;
using SiteImagemNASA.Models;

namespace SiteImagemNASA
{
    public class APINasaClient
    {
        private HttpClient _client;
        private IConfiguration _configuration;

        public APINasaClient(
            HttpClient client, IConfiguration configuration)
        {
            _client = client;
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            _configuration = configuration;
        }

        public ImagemNASA ObterDadosImagem()
        {
            DateTime dataBase = DateTime.Now.Date.AddDays(
                new Random().Next(0, 7) * -1);

            string baseURL =
                _configuration.GetSection("NASA_OpenAPIs:BaseURL").Value;
            string key =
                _configuration.GetSection("NASA_OpenAPIs:Key").Value;
            var response = _client.GetAsync(
                baseURL + $"apod?api_key={key}" +
                $"&date={dataBase.ToString("yyyy-MM-dd")}").Result;

            response.EnsureSuccessStatusCode();
            string conteudo =
                response.Content.ReadAsStringAsync().Result;
            dynamic resultado =
                JsonConvert.DeserializeObject(conteudo);

            ImagemNASA imagem = new ImagemNASA();
            imagem.Data = dataBase;
            imagem.Titulo = resultado.title;
            imagem.Descricao = resultado.explanation;
            imagem.Url = resultado.url;
            imagem.MediaType = resultado.media_type;

            return imagem;
        }
    }
}