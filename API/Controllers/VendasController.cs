using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VendasController:ControllerBase
    {
        private readonly IVendaService _vendaService;

        public VendasController(IVendaService vendaService)
        {
            _vendaService = vendaService;
        }

        [Authorize(Roles = "Admin,User")]
        [HttpPost("Finalizar")]
        public IActionResult Finalizar([FromBody] FinalizarVendaDto dto)
        {
            try
            {
                var resultado = _vendaService.FinalizarVenda(dto);
                return Created(string.Empty, resultado);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        [Authorize(Roles = "Admin,User")]
        [HttpGet("Listar")]
        public IActionResult Listar()
        {
            var vendas = _vendaService.ListarTodos();
            return Ok(vendas);
        }

        [Authorize(Roles = "Admin,User")]
        [HttpGet("{id:int}/BuscarPorId")]
        public IActionResult BuscarPorId(int id)
        {
            try
            {
                var venda = _vendaService.BuscarPorId(id);
                return Ok(venda);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { mensagem = ex.Message });
            }
        }
    }
}
