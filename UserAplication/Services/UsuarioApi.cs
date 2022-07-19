using UserAplication.Models;
using System.Text.Json;
using System.Text;
using Microsoft.AspNetCore.Mvc; 
using Newtonsoft.Json;

namespace UserAplication.Services
{
    public class UsuarioApi : IUsuarioApi 
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _Config;
        private string host;

        public UsuarioApi(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _Config = config;
            host = _Config.GetSection("UsuarioApiHost").Value;
        }    

        [HttpGet]
        public async Task<Usuario> GetId(int id)
        {   
            //Disativa comparação com case sensitive
            JsonSerializerOptions options = new JsonSerializerOptions() {PropertyNameCaseInsensitive = true};
            var response = await _httpClient.GetAsync($"{host}/{id}");

            if(response == null)
            {
                throw new HttpRequestException("Falha de Requisição");
            }
            var content = await response.Content.ReadAsStringAsync();//Converte Json para string
            var user = System.Text.Json.JsonSerializer.Deserialize<Usuario>(content, options);
            //Popula Objeto Usuario apartir do retorna string
            if(user == null)
            {
                throw new HttpRequestException();
            }
            return user;
        }

        [HttpGet]
        public async Task<List<Usuario>> Get()
        {
            //Disativa comparação com case sensitive
            JsonSerializerOptions options = new JsonSerializerOptions() {PropertyNameCaseInsensitive = true};
            var response = await _httpClient.GetAsync(host);

            if(response == null)
            {
                throw new HttpRequestException("Falha de Requisição");
            }
            var content = await response.Content.ReadAsStringAsync();//Converte Json para string
            var user = System.Text.Json.JsonSerializer.Deserialize<List<Usuario>>(content, options);
            //Popula Lista Objeto Usuario apartir do retorna string
            if(user == null)
            {
                throw new HttpRequestException();
            }
            return user;
        }

        [HttpPost]
        public async Task<IActionResult> Post(Usuario user)
        {
            //Converte Usuario para Json e Realiza Request Post
            var Response = await _httpClient.PostAsJsonAsync(host, user);
            if(!Response.IsSuccessStatusCode)
            { 
                Console.WriteLine(Response.Content.ReadAsStringAsync().GetAwaiter().GetResult());
                return new BadRequestResult();
            }
            return new OkResult();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Usuario user)
        {
            //Montagem do Delete Request
            var request = new HttpRequestMessage 
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri($"{host}"),
                Content = new StringContent
                (//Converte Objeto Usuario em Json
                    JsonConvert.SerializeObject(user),
                    Encoding.UTF8,//Padrão Enconde
                    "application/json"
                )
            };   
            var Response = await _httpClient.SendAsync(request);//Lança Request
            if(!Response.IsSuccessStatusCode)
            {
                Console.WriteLine(Response.Content.ReadAsStringAsync().GetAwaiter().GetResult());
                return new BadRequestResult();
            }
           return new NoContentResult();
        }

        [HttpPut]
        public async Task<IActionResult> Edit(Usuario user)
        {
            //Converte Usuario para Json e Realiza Request Put
            var Response = await _httpClient.PutAsJsonAsync(host, user);
            if(!Response.IsSuccessStatusCode)
            {
                Console.WriteLine(Response.Content.ReadAsStringAsync().GetAwaiter().GetResult());
                return new BadRequestResult();
            }
            return new NoContentResult();
        }

        [HttpPatch]
        public async Task<IActionResult> DisableUser(Usuario user)
        {   //Cria HttpContent para Request
            var content = new StringContent
                (   //Converte Usuario em Json
                    JsonConvert.SerializeObject(user),
                    Encoding.UTF8,//Padrão encode
                    "application/json"
                );
            var Response = await _httpClient.PatchAsync(host, content);//Envia Request Patch
            if(!Response.IsSuccessStatusCode)
            {
                Console.WriteLine(Response.Content.ReadAsStringAsync().GetAwaiter().GetResult());
                return new BadRequestResult();   
            }
            return new NoContentResult();
        }
    } 
}
