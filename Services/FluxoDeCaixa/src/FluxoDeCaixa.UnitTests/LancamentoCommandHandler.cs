using FluxoDeCaixa.Domain.Commands;
using FluxoDeCaixa.Domain.Interfaces;
using FluxoDeCaixa.Domain.Models;
using MediatR;
using Moq;
using NetDevPack.Data;

namespace FluxoDeCaixa.UnitTests
{

    public class LancamentoCommandHandlerTests
    {
        private readonly Mock<IMediator> _mediator;
        private readonly Mock<ILancamentoRepository> _lancamentoRepository;
        private readonly LancamentoCommandHandler _lancamentoCommandHandler;
        private readonly Mock<IUnitOfWork> _unitOfWork;

        public LancamentoCommandHandlerTests()
        {
            _mediator = new Mock<IMediator>();
            _lancamentoRepository = new Mock<ILancamentoRepository>();
            _lancamentoCommandHandler = new LancamentoCommandHandler(
                _lancamentoRepository.Object);
            _unitOfWork = new Mock<IUnitOfWork>();

        }

        [Fact]
        public async Task HandlerRegistrarDebitoCommand_Success_TotalValue()
        {
            // Arrange
            var entity = new Lancamento(Guid.NewGuid(), Guid.NewGuid().ToString(), 100);
            var command = new RegistrarDebitoCommand(entity.IdUsuario, entity.Valor);

            _mediator
                .Setup(s => s.Send(command, It.IsAny<CancellationToken>()));

            _lancamentoRepository
                .Setup(s => s.Add(entity));
            _lancamentoRepository.Setup(s => s.Add(entity));

            // Act
            await _lancamentoCommandHandler.Handle(command, new CancellationToken());

            // Assert

            //Assert.True(entity.TotalValue == 1000);

            Assert.True((await _lancamentoRepository.Object.FindAsync(lancamento => lancamento.Id == entity.Id)).Any());

        }
        [Fact]
        public async Task HandlerRegistrarCreditoCommand_Success_TotalValue()
        {
            // Arrange
            var entity = new Lancamento(Guid.NewGuid(), Guid.NewGuid().ToString(), 100);
            var command = new RegistrarDebitoCommand(entity.IdUsuario, entity.Valor);

            _mediator
                .Setup(s => s.Send(command, It.IsAny<CancellationToken>()));

            _lancamentoRepository
                .Setup(s => s.Add(entity));
            _lancamentoRepository.Setup(s => s.Add(entity));

            // Act
            await _lancamentoCommandHandler.Handle(command, new CancellationToken());

            // Assert

            //Assert.True(entity.TotalValue == 1000);

            Assert.True((await _lancamentoRepository.Object.FindAsync(lancamento => lancamento.Id == entity.Id)).Any());

        }
    }
}
