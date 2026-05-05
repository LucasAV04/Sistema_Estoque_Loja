using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EstoqueController : ControllerBase
    {
        private readonly IEstoqueService _estoqueService;

        public EstoqueController(IEstoqueService estoqueService)
        {
            _estoqueService = estoqueService;
        }

        [HttpGet("{produtoId:int}/ObterPorProduto")]
        public IActionResult ObterPorProduto(int produtoId)
        {
            var estoque = _estoqueService.ObterPorProduto(produtoId);
            return Ok(estoque);
        }

     
        [HttpPost("Entrada")]
        public IActionResult Entrada([FromBody] EntradaEstoqueDto dto)
        {
            try
            {
                _estoqueService.Entrada(dto);
                return Ok(new { mensagem = "Entrada registrada com sucesso." });
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

      
        [HttpPost("Saida")]
        public IActionResult Saida([FromBody] SaidaEstoqueDto dto)
        {
            try
            {
                _estoqueService.Saida(dto);
                return Ok(new { mensagem = "Saída registrada com sucesso." });
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }
    }
}
