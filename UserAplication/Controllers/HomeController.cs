using Microsoft.AspNetCore.Mvc;
using UserAplication.Models;
using UserAplication.Services;

namespace UserAplication.Controllers;

public class HomeController : Controller
{
    private readonly IUsuarioApi _UsuarioApi;

    public HomeController(IUsuarioApi usuarioApi)
    {
        _UsuarioApi = usuarioApi;
    }

    private string DateInvalid(DateTime Data)
    {   //Verificar Data Futura
        if(Data > DateTime.Now) return "Data Invalida";
        //Verifica Maioridade
        if(Data > DateTime.Now.AddYears(-18)) return "Cliente Precisa ser maior de idade";
        //Retorna Vazio caso nenhum erro
        return string.Empty;
    }

    [HttpGet]
    public async Task<ActionResult<Usuario>> Detail(int Id)
    {   //Apenas Id apartir de 1
        if(Id <= 0)
        { 
            throw new ArgumentOutOfRangeException("Id Invalida");
        }
        return View(await _UsuarioApi.GetId(Id));
    }

    [HttpGet]
    public async Task<ActionResult<Usuario>> Index()
    {
        return View(await _UsuarioApi.Get());
    }

    [HttpPost]
    public async Task<IActionResult> Create(Usuario user)
    {   //Espera usuario <nome data sexo>
        if (ModelState.IsValid)
        {   
            var Erro = DateInvalid(user.DataDeNacimento);
            //Espera String Vazia
            if(String.IsNullOrEmpty(Erro))
            {
                var ok = await _UsuarioApi.Post(user);
            }
            else
            {   //Envia Erro para a View
                TempData["ErrorMessage"] = Erro;
                return RedirectToAction("Registrar");   
            }
        } 
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Registrar()
    {
        return View();
    }


    [HttpPost]
    public async Task<ActionResult> Delete(Usuario user)
    {   //Usuario apenas campo <Id>
        if(user.Id > 0)
        {   
            var ok = await _UsuarioApi.Delete(user);
        }
        else 
        {
            throw new ArgumentOutOfRangeException("Id Invalida");
        }
        return RedirectToAction("Index");
    }

    [HttpGet]   
    public async Task<ActionResult<Usuario>> ConfirmDelete(int id)
    { 
        var user = await _UsuarioApi.GetId(id);
        return View(user);// Busca e redirecionamento de pagina
    }

    [HttpGet]
    public async Task<ActionResult<Usuario>> Editar(int id)
    {   
        var user = await _UsuarioApi.GetId(id);
        return View(user);// Busca e redirecionamento de pagina
    }

    [HttpPost]
    public async Task<ActionResult> Edit(Usuario user)
    {   //Usuario apenas campo <Id>
        if (ModelState.IsValid)
        {
            var Erro = DateInvalid(user.DataDeNacimento);
            //Espera String Vazia
            if(String.IsNullOrEmpty(Erro))
            {
                var ok = await _UsuarioApi.Edit(user);
            }
            else
            {   //Envia Erro para View
                TempData["ErrorMessage"] = Erro;
                return RedirectToAction("Editar", new { id = user.Id });   
            }
            
        }
        return RedirectToAction("Index");
    }

    [HttpGet]   
    public async Task<ActionResult<Usuario>> Desativar(int id)
    {   
        var user = await _UsuarioApi.GetId(id);
        return View(user);// Busca e redirecionamento de pagina
    }

    [HttpPost]
    public async Task<ActionResult> Disable(Usuario user)
    {   //Usuario apenas campo <Id>
        if(user.Id != 0)
        {             
            var ok = await _UsuarioApi.DisableUser(user);
        }
        else 
        {
            throw new ArgumentOutOfRangeException("Id Invalida");
        }
        return RedirectToAction("Index");
    }
}
