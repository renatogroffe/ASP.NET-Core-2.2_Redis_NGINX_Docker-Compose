using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using SiteImagemNASA.Models;

namespace SiteImagemNASA.Pages
{
    public class IndexModel : PageModel
    {
        public void OnGet(
            [FromServices]IDistributedCache cache,
            [FromServices]APINasaClient client)
        {
            ImagemNASA imagemNASA = null;
            string valorJSON = cache.GetString("DadosImagemNASA");
            if (valorJSON == null)
            {
                imagemNASA = client.ObterDadosImagem();

                DistributedCacheEntryOptions opcoesCache =
                    new DistributedCacheEntryOptions();
                opcoesCache.SetAbsoluteExpiration(
                    TimeSpan.FromMinutes(1));

                valorJSON = JsonConvert.SerializeObject(imagemNASA);
                cache.SetString("DadosImagemNASA", valorJSON, opcoesCache);
            }

            if (imagemNASA == null && valorJSON != null)
            {
                imagemNASA = JsonConvert
                    .DeserializeObject<ImagemNASA>(valorJSON);
            }

            TempData["ImagemNASA"] = imagemNASA;
        }
    }
}