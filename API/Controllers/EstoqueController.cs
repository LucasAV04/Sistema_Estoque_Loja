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

        /// <summary>
        /// Consulta o estoque atual de um produto
        /// </summary>
        [HttpGet("{produtoId:int}")]
        public IActionResult ObterPorProduto(int produtoId)
        {
            var estoque = _estoqueService.ObterPorProduto(produtoId);
            return Ok(estoque);
        }

        /// <summary>
        /// Registra entrada de itens no estoque
        /// </summary>
        [HttpPost("entrada")]
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

        /// <summary>
        /// Registra saída de itens do estoque
        /// </summary>
        [HttpPost("saida")]
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
