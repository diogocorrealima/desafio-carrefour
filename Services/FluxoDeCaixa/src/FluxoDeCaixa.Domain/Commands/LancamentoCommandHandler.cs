using System;
using System.Threading;
using System.Threading.Tasks;
using FluxoDeCaixa.Domain.Events;
using FluxoDeCaixa.Domain.Interfaces;
using FluxoDeCaixa.Domain.Models;
using FluentValidation.Results;
using MediatR;
using NetDevPack.Messaging;

namespace FluxoDeCaixa.Domain.Commands
{
    public class LancamentoCommandHandler : CommandHandler,
        IRequestHandler<RegistrarDebitoCommand, ValidationResult>,
        IRequestHandler<RegistrarCreditoCommand, ValidationResult>,
        IRequestHandler<RemoveLancamentoCommand, ValidationResult>
    {
        private readonly ILancamentoRepository _lancamentoRepository;

        public LancamentoCommandHandler(ILancamentoRepository lancamentoRepository)
        {
            _lancamentoRepository = lancamentoRepository;
        }

        public async Task<ValidationResult> Handle(RegistrarDebitoCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid()) return message.ValidationResult;

            var lancamento = new Lancamento(Guid.NewGuid(), message.IdUsuario, message.Valor);
            lancamento.SetDebito();


            lancamento.AddDomainEvent(new DebitoRegisteredEvent(lancamento.Id, lancamento.IdUsuario, lancamento.Valor, lancamento.Tipo));

            _lancamentoRepository.Add(lancamento);

            return await Commit(_lancamentoRepository.UnitOfWork);
        }

        public async Task<ValidationResult> Handle(RegistrarCreditoCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid()) return message.ValidationResult;

            var lancamento = new Lancamento(Guid.NewGuid(), message.IdUsuario, message.Valor);
            lancamento.SetCredito();

            lancamento.AddDomainEvent(new CreditoRegisteredEvent(lancamento.Id, lancamento.IdUsuario, lancamento.Valor, lancamento.Tipo));

            _lancamentoRepository.Add(lancamento);

            return await Commit(_lancamentoRepository.UnitOfWork);
        }

        public async Task<ValidationResult> Handle(RemoveLancamentoCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid()) return message.ValidationResult;

            var lancamento = await _lancamentoRepository.GetById(message.Id);

            if (lancamento is null)
            {
                AddError("The lancamento doesn't exists.");
                return ValidationResult;
            }

            lancamento.AddDomainEvent(new LancamentoRemovedEvent(message.Id));

            _lancamentoRepository.Remove(lancamento);

            return await Commit(_lancamentoRepository.UnitOfWork);
        }

        public void Dispose()
        {
            _lancamentoRepository.Dispose();
        }
    }
}