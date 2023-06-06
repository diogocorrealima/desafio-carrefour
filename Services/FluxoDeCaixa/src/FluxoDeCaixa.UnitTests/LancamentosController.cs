using AutoMapper;
using FluxoDeCaixa.Application.Interfaces;
using FluxoDeCaixa.Application.ViewModels;
using FluxoDeCaixa.Domain.Interfaces;
using FluxoDeCaixa.Domain.Models;
using FluxoDeCaixa.Services.Api.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;


namespace FluxoDeCaixa.UnitTests
{
    public class LancamentosControllerTests
    {
        private readonly Mock<ILancamentoAppService> _lancamentoAppService;
        private readonly Mock<ILancamentoRepository> _lancamentoRepository;
        private readonly Mock<ILogger<LancamentosController>> _logger;
        private readonly Mock<IMediator> _mediator;
        private readonly Mock<IMapper> _mapper;
        private readonly LancamentosController _lancamentosController;

        public LancamentosControllerTests()
        {
            _lancamentoAppService = new Mock<ILancamentoAppService>();
            _lancamentoRepository = new Mock<ILancamentoRepository>();
            _logger = new Mock<ILogger<LancamentosController>>();
            _mediator = new Mock<IMediator>();
            _mapper = new Mock<IMapper>();

            _lancamentosController = new LancamentosController(
                _lancamentoAppService.Object
                );
        }

        [Fact]
        public async Task PostDebitoAsync_AddDebito_ReturnOk()
        {
            // Arrange
            var lancamento = new Lancamento(Guid.NewGuid(), Guid.NewGuid().ToString(), 100);
            var lancamentoVM = new LancamentoViewModel(lancamento.IdUsuario, lancamento.Valor);


            _lancamentoRepository.Setup(s => s.Add(lancamento));
            _lancamentoAppService.Setup(s => s.Debito(lancamentoVM)).ReturnsAsync(new FluentValidation.Results.ValidationResult());

            // Act
            var result = await _lancamentosController.PostDebito(lancamentoVM);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }
        [Fact]
        public async Task PostCreditoAsync_AddCredito_ReturnOk()
        {
            // Arrange
            var lancamento = new Lancamento(Guid.NewGuid(), Guid.NewGuid().ToString(), 100);
            var lancamentoVM = new LancamentoViewModel(lancamento.IdUsuario, lancamento.Valor);


            _lancamentoRepository.Setup(s => s.Add(lancamento));
            _lancamentoAppService.Setup(s => s.Credito(lancamentoVM)).ReturnsAsync(new FluentValidation.Results.ValidationResult());

            // Act
            var result = await _lancamentosController.PostDebito(lancamentoVM);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }
        [Fact]
        public async Task GetConsolidadoAsync_AddCredito_ReturnOk()
        {
            // Arrange
            var idUsuario = Guid.NewGuid().ToString();
            var lancamentos = new List<Lancamento> {
                new Lancamento(Guid.NewGuid(), idUsuario, 150),
                new Lancamento(Guid.NewGuid(), idUsuario, 150),
                new Lancamento(Guid.NewGuid(), idUsuario, 100),
                new Lancamento(Guid.NewGuid(), idUsuario, 100)
            };
            lancamentos[0].SetCredito();
            lancamentos[1].SetCredito();
            lancamentos[2].SetDebito();
            lancamentos[3].SetDebito();
            var consolidadoVM = new ConsolidadoViewModel(1, 10, new List<string> { idUsuario });


            _lancamentoRepository.Setup(s => s.FindConsolidadeAsync(consolidadoVM.IdsUsuario, consolidadoVM.Pagina, consolidadoVM.Quantidade));
            _lancamentoRepository.Setup(s => s.FindAsync(e => true).Result).Returns(lancamentos.AsEnumerable());

            // Act
            var result = await _lancamentosController.GetConsolidado(consolidadoVM);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }
    }
}
