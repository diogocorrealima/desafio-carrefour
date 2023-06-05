using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using FluxoDeCaixa.Application.EventSourcedNormalizers;
using FluxoDeCaixa.Application.Interfaces;
using FluxoDeCaixa.Application.ViewModels;
using FluxoDeCaixa.Domain.Commands;
using FluxoDeCaixa.Domain.Interfaces;
using FluxoDeCaixa.Infra.Data.Repository.EventSourcing;
using FluentValidation.Results;
using NetDevPack.Mediator;

namespace FluxoDeCaixa.Application.Services
{
    public class LancamentoAppService : ILancamentoAppService
    {
        private readonly IMapper _mapper;
        private readonly IMediatorHandler _mediator;

        public LancamentoAppService(IMapper mapper,
                                  ILancamentoRepository lancamentoRepository,
                                  IMediatorHandler mediator,
                                  IEventStoreRepository eventStoreRepository)
        {
            _mapper = mapper;
            _mediator = mediator;
        }

       
        public async Task<ValidationResult> Debito(LancamentoViewModel lancamentoViewModel)
        {
            var registerCommand = _mapper.Map<RegistrarDebitoCommand>(lancamentoViewModel);
            return await _mediator.SendCommand(registerCommand);
        }
        public async Task<ValidationResult> Credito(LancamentoViewModel lancamentoViewModel)
        {
            var registerCommand = _mapper.Map<RegistrarCreditoCommand>(lancamentoViewModel);
            return await _mediator.SendCommand(registerCommand);
        }
        

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
