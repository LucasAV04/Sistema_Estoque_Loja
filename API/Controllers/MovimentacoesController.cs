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

        /// <summary>
        /// Lista todas as movimentações
        /// </summary>
        [HttpGet]
        public IActionResult ListarTodos()
        {
            var movimentacoes = _movimentacaoService.ListarTodos();
            return Ok(movimentacoes);
        }

        /// <summary>
        /// Lista movimentações de um produto específico
        /// </summary>
        [HttpGet("produto/{produtoId:int}")]
        public IActionResult ListarPorProduto(int produtoId)
        {
            var movimentacoes = _movimentacaoService.ListarPorProduto(produtoId);
            return Ok(movimentacoes);
        }

        /// <summary>
        /// Busca uma movimentação por Id
        /// </summary>
        [HttpGet("{id:int}")]
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

        /// <summary>
        /// Registra uma nova movimentação (ENTRADA ou SAIDA)
        /// </summary>
        [HttpPost]
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
