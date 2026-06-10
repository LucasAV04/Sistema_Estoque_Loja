using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotaFiscalController : ControllerBase
    {
        private readonly INotaFiscalService _service;
        public NotaFiscalController(INotaFiscalService service) => _service = service;

        [Authorize(Roles = "Admin,User")]
        [HttpPost("Emitir")]
        public IActionResult Emitir([FromBody] EmitirNotaFiscalDto dto)
        {
            try { return Created(string.Empty, _service.Emitir(dto)); }
            catch (ArgumentException ex) { return BadRequest(new { mensagem = ex.Message }); }
        }

        [Authorize(Roles = "Admin,User")]
        [HttpGet("Listar")]
        public IActionResult Listar() => Ok(_service.ListarTodos());

        [Authorize(Roles = "Admin,User")]
        [HttpGet("{id:int}")]
        public IActionResult BuscarPorId(int id)
        {
            try { return Ok(_service.BuscarPorId(id)); }
            catch (ArgumentException ex) { return NotFound(new { mensagem = ex.Message }); }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}/Deletar")]
        public IActionResult Deletar(int id)
        {
            try { _service.Deletar(id); return Ok(new { mensagem = "Nota excluída." }); }
            catch (ArgumentException ex) { return NotFound(new { mensagem = ex.Message }); }
        }
    }
}