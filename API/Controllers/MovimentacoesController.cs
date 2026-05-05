using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MovimentacoesController : ControllerBase
    {
        private readonly IMovimentacaoEstoqueService _movimentacaoService;

        public MovimentacoesController(IMovimentacaoEstoqueService movimentacaoService)
        {
            _movimentacaoService = movimentacaoService;
        }

        [HttpGet("ListarTodos")]
        public IActionResult ListarTodos()
        {
            var movimentacoes = _movimentacaoService.ListarTodos();
            return Ok(movimentacoes);
        }

       
        [HttpGet("produto/{produtoId:int}/ListarPorProduto")]
        public IActionResult ListarPorProduto(int produtoId)
        {
            var movimentacoes = _movimentacaoService.ListarPorProduto(produtoId);
            return Ok(movimentacoes);
        }

        [HttpGet("{id:int}/BuscarPorId")]
        public IActionResult BuscarPorId(int id)
        {
            try
            {
                var movimentacao = _movimentacaoService.BuscarPorId(id);
                return Ok(movimentacao);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { mensagem = ex.Message });
            }
        }

      
        [HttpPost("Registrar")]
        public IActionResult Registrar([FromBody] MovimentacaoEstoqueDto dto)
        {
            try
            {
                _movimentacaoService.Registrar(dto);
                return Created(string.Empty, new { mensagem = "Movimentação registrada com sucesso." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }
    }
}
