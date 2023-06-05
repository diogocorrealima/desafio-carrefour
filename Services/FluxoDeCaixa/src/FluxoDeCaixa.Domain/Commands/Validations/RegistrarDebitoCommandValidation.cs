namespace FluxoDeCaixa.Domain.Commands.Validations
{
    public class RegistrarDebitoCommandValidation : LancamentoValidation<RegistrarDebitoCommand>
    {
        public RegistrarDebitoCommandValidation()
        {
            ValidarIdUsuario();
            ValidarValor();
        }
    }
   
}