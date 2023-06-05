using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FluxoDeCaixa.Domain.Models;
using NetDevPack.Data;

namespace FluxoDeCaixa.Domain.Interfaces
{
    public interface ILancamentoRepository : IRepository<Lancamento>
    {
        Task<Lancamento> GetById(Guid id);
        Task<IEnumerable<Lancamento>> GetAll();
        Task<IEnumerable<Lancamento>> FindAsync(Expression<Func<Lancamento, bool>> predicate);
        void Add(Lancamento lancamento);
        void Update(Lancamento lancamento);
        void Remove(Lancamento lancamento);
    }
}