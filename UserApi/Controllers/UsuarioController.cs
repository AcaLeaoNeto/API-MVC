using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using User.Data;

namespace User.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        //Injeção do Banco
        private readonly ApiDbContext _Db;

        public UsuarioController(ApiDbContext Db)
        {
            _Db = Db;
        }

        [HttpGet]
        public async Task<ActionResult<List<Usuario>>> GetUsuarios()
        {//Retorno Apenas Usuarios Ativos
            return Ok(await _Db.Usuarios.Where(u => u.Ativo == true).ToListAsync());
        }


        private int IdadeUsuario(DateTime Nacimento)
        { //Calcula Idade Sem Relaçao Com Hora
            var Age = DateTime.Now.Year - Nacimento.Year;
            if (Nacimento > DateTime.Now.AddYears(-Age)) Age--;

            return Age;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuarioById(int id)
        {
            var user = await _Db.Usuarios.FindAsync(id);
            if (user == null) return BadRequest("Cliente não Encontrado.");
            //Avalia Entrada De Usuario Incorreto
            if (user.Ativo == false) return BadRequest("Cliente Desativado.");
            //Avalia Usuario Inativo    
            return Ok(user);
        }


        [HttpPost]
        public async Task<ActionResult> AddUsuario(Usuario user)
        {   //Averigua Validade da Data Postada
            if(user.DataDeNacimento > DateTime.Now)
            {
                return BadRequest("Data de Nacimento Inválido");
            }
            //Calcula Idade
            user.Idade = IdadeUsuario(user.DataDeNacimento);

            if(user.Idade < 18)
            {//Verifica Maioridade
                return BadRequest("Cliente Menor de idade");
            }    
            //Salva no Banco Objeto "user"
            _Db.Usuarios.Add(user);
            await _Db.SaveChangesAsync();
            //Retorna Lista Usuarios Ativos
            return Ok();
        }
        

        [HttpPut]
        public async Task<ActionResult> UpdateUsuario (Usuario request)
        {   //Deve Retorna objeto Ou Null
            var user =  await _Db.Usuarios.FindAsync(request.id);
            //Avaliar Caso Null
            if(user == null) return BadRequest("Cliente não Encontrado.");
            //Avalia Usuario Inativo 
            if (user.Ativo == false) return BadRequest("Cliente Desativado.");

            user.NomeDoContato = request.NomeDoContato;
            user.DataDeNacimento = request.DataDeNacimento;
            if(user.DataDeNacimento > DateTime.Now)
            {
                return BadRequest("Data de Nacimento Inválido");
            }
            user.Sexo = request.Sexo;
            //Idade Aparatir Possivel Nova Data
            user.Idade = IdadeUsuario(request.DataDeNacimento);
            //Salva no Banco Objeto "user"
            if(user.Idade < 18)
            {//Verifica Maioridade
                return BadRequest("Cliente Menor de idade");
            }
            await _Db.SaveChangesAsync();
            //Retorna Lista Usuarios Ativos
            return Ok();
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteUsuario (UsuarioDTO request)
        {
            var user = await _Db.Usuarios.FindAsync(request.id);
            //Retorna Usuario ou Null, Caso, Erro
            if(user == null) return BadRequest("Cliente não Encontrado.");

            _Db.Usuarios.Remove(user); //Remove Usuario
            await _Db.SaveChangesAsync();
            return Ok();
        }

        [HttpPatch]
        public async Task<ActionResult> DesativarUsuario (UsuarioDTO request)
        {
            var user = await _Db.Usuarios.FindAsync(request.id);

            if(user == null) return BadRequest("Cliente não Encontrado.");
            //Avalia Usuario Inativo
            if (user.Ativo == false) return BadRequest("Cliente Já Desativado."); 

            user.Ativo = false; //Desabilita Usuario
            
            await _Db.SaveChangesAsync();

            return Ok();
        }

    }
}