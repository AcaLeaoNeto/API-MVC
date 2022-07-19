using UserAplication.Models;
using Microsoft.AspNetCore.Mvc; 

namespace UserAplication.Services
{

    public interface IUsuarioApi
    {
        Task<Usuario> GetId(int id);
        Task<List<Usuario>> Get();
        Task<IActionResult> Post(Usuario user);
        Task<IActionResult> Delete(Usuario user);
        Task<IActionResult> Edit(Usuario user);
        Task<IActionResult> DisableUser(Usuario user);
    }
}