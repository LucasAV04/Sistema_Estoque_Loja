using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProdutosController : ControllerBase
    {
        private readonly IProdutoService _produtoService;

        public ProdutosController(IProdutoService produtoService)
        {
            _produtoService = produtoService;
        }

       
        [HttpGet]
        public IActionResult Buscar([FromQuery] string? nome, [FromQuery] string? Ref)
        {
            if (nome != null || Ref != null)
            {
                var resultado = _produtoService.Buscar(nome, Ref);
                return Ok(resultado);
            }

            var produtos = _produtoService.ListarTodos();
            return Ok(produtos);
        }

        
        [HttpGet("{id:int}")]
        public IActionResult BuscarPorId(int id)
        {
            try
            {
                var produto = _produtoService.BuscarPorId(id);
                return Ok(produto);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { mensagem = ex.Message });
            }
        }

        
        [HttpGet("ref/{ref}")]
        public IActionResult BuscarPorRef(string @ref)
        {
            try
            {
                var produto = _produtoService.BuscarPorRef(@ref);
                return Ok(produto);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { mensagem = ex.Message });
            }
        }

       
        [HttpPost]
        public IActionResult Criar([FromBody] ProdutoCreateDto dto)
        {
            try
            {
                _produtoService.CriarProduto(dto);
                return Created(string.Empty, new { mensagem = "Produto criado com sucesso." });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { mensagem = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

       
        [HttpPut("{id:int}")]
        public IActionResult Atualizar(int id, [FromBody] ProdutoCreateDto dto)
        {
            try
            {
                _produtoService.AtualizarProduto(id, dto);
                return Ok(new { mensagem = "Produto atualizado com sucesso." });
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { mensagem = ex.Message });
            }
        }

        [HttpDelete("{id:int}")]
        public IActionResult Deletar(int id)
        {
            try
            {
                _produtoService.DeletarProduto(id);
                return Ok(new { mensagem = "Produto removido com sucesso." });
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { mensagem = ex.Message });
            }
        }
    }
}
