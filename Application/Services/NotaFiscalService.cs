using Application.DTOs;
using Application.Interfaces;
using Domain;
using Infrastructure.Repositories.Interfaces;

namespace Application.Services
{
    public class NotaFiscalService:INotaFiscalService
    {
        private readonly INotaFiscalRepository _nfRepo;
        private readonly IVendaRepository _vendaRepo;

        public NotaFiscalService(INotaFiscalRepository nfRepo, IVendaRepository vendaRepo)
        {
            _nfRepo = nfRepo;
            _vendaRepo = vendaRepo;
        }

        public NotaFiscalResponseDto Emitir(EmitirNotaFiscalDto dto)
        {
            var venda = _vendaRepo.BuscarPorId(dto.VendaId)
                ?? throw new ArgumentException("Venda não encontrada.");

            var subtotal = venda.Itens.Sum(i => i.ValorTotal);

            var nota = new NotaFiscal
            {
                VendaId = dto.VendaId,
                Numero = _nfRepo.ProximoNumero(),
                Serie = "001",
                DataEmissao = DateTime.Now,
                NomeCliente = dto.NomeCliente,
                CpfCnpjCliente = dto.CpfCnpjCliente,
                EnderecoCliente = dto.EnderecoCliente,
                BairroCliente = dto.BairroCliente,
                MunicipioCliente = dto.MunicipioCliente,
                UfCliente = dto.UfCliente,
                CepCliente = dto.CepCliente,
                TelefoneCliente = dto.TelefoneCliente,
                NaturezaOperacao = dto.NaturezaOperacao,
                FormaPagamento = dto.FormaPagamento,
                Desconto = dto.Desconto,
                ValorTotal = subtotal - dto.Desconto,
                Vendedor = dto.Vendedor,
                Observacoes = dto.Observacoes
            };

            nota.Id = _nfRepo.Inserir(nota);
            return Mapear(nota);
        }

        public IEnumerable<NotaFiscalResponseDto> ListarTodos()
            => _nfRepo.ListarTodos().Select(Mapear);

        public NotaFiscalResponseDto BuscarPorId(int id)
        {
            var nota = _nfRepo.BuscarPorId(id)
                ?? throw new ArgumentException("Nota fiscal não encontrada.");
            return Mapear(nota);
        }

        public void Deletar(int id)
        {
            _ = _nfRepo.BuscarPorId(id)
                ?? throw new ArgumentException("Nota fiscal não encontrada.");
            _nfRepo.Deletar(id);
        }

        private static NotaFiscalResponseDto Mapear(NotaFiscal n) => new()
        {
            Id = n.Id,
            VendaId = n.VendaId,
            Numero = n.Numero,
            Serie = n.Serie,
            DataEmissao = n.DataEmissao,
            NomeCliente = n.NomeCliente,
            CpfCnpjCliente = n.CpfCnpjCliente,
            EnderecoCliente = n.EnderecoCliente,
            BairroCliente = n.BairroCliente,
            MunicipioCliente = n.MunicipioCliente,
            UfCliente = n.UfCliente,
            CepCliente = n.CepCliente,
            TelefoneCliente = n.TelefoneCliente,
            NaturezaOperacao = n.NaturezaOperacao,
            FormaPagamento = n.FormaPagamento,
            Desconto = n.Desconto,
            ValorTotal = n.ValorTotal,
            Vendedor = n.Vendedor,
            Observacoes = n.Observacoes
        };
    }
}
