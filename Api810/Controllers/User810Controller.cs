using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api810.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class User810Controller : ControllerBase
    {
        [HttpPost("CreatePasswordInitial")]
        public IActionResult CreatePasswordInitial(Models.DTOs.User810DTO user)
        {
            if (user == null)
            {
                return BadRequest("Debe ingresar todos los datos.");
            }

            if (string.IsNullOrEmpty(user.UserBirthDate))
            {
                return BadRequest("La fecha de nacimiento es obligatoria.");
            }

            if (!DateTime.TryParse(user.UserBirthDate, out DateTime userBirthDate))
            {
                return BadRequest("Formato de fecha inválido. Use dd/MM/yyyy o yyyy-MM-dd.");
            }

            if (userBirthDate >= DateTime.Today)
            {
                return BadRequest("La fecha de nacimiento debe ser anterior a la fecha actual.");
            }

            if (user.UserCUI.ToString().Length != 13)
            {
                return BadRequest("El CUI debe tener exactamente 13 dígitos.");
            }

            if (string.IsNullOrWhiteSpace(user.UserName) || user.UserName.Length <= 5)
            {
                return BadRequest("El nombre de usuario debe tener al menos 5 caracteres.");
            }

            var userName = string.Concat(user.UserName.Select((c, i) => i == 0 ? char.ToUpper(c) : char.ToLower(c)));
            string dayMonth = userBirthDate.ToString("ddMM");
            string postalCode  = user.UserCUI.ToString().Substring(user.UserCUI.ToString().Length - 3);

            string password = $"{userName}{dayMonth}@{postalCode}";

            return Ok(new { PasswordInicial = password });
        }
    }
}
