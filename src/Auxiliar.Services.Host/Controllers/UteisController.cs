using Auxiliar.Application.Interfaces;
using Auxiliar.Domain.Core.Bus;
using Auxiliar.Domain.Core.Enum;
using Auxiliar.Domain.Core.Notifications;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auxiliar.Services.Api.Controllers
{
    [Authorize, Route("[controller]")]
    public class UteisController : ApiController
    {
        #region Campos Privados

        private readonly IUteisAppService _uteisAppService;

        #endregion Campos Privados

        #region Construtores Publicos

        public UteisController(IUteisAppService uteisAppService,
                               INotificationHandler<DomainNotification> notifications,
                               IMediatorHandler mediator)
           : base(mediator, notifications)
        {
            _uteisAppService = uteisAppService;
        }

        #endregion Construtores Publicos

        #region POST

        /// <summary>
        /// Gerar GUID's.
        /// </summary>
        /// <param name="quantidadeGuids">Quantidade de GUID's a ser gerada</param>
        /// <returns></returns>
        [AllowAnonymous, HttpPost("guid")]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Post))]
        [ProducesResponseType(typeof(IEnumerable<Guid>), statusCode: StatusCodes.Status200OK)]
        [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
        [ProducesResponseType(statusCode: StatusCodes.Status500InternalServerError)]
        public IActionResult GerarGuid([FromQuery] int quantidadeGuids)
        {
            List<Guid> _guids = new();

            for (int itens = 0; itens < quantidadeGuids; itens++)
            {
                _guids.Add(Guid.NewGuid());
            }

            return Response(_guids);
        }

        /// <summary>
        /// Gerar CPF fake.
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous, HttpPost("gerar/cpf")]
        [ProducesResponseType(typeof(string), statusCode: StatusCodes.Status200OK)]
        [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
        [ProducesResponseType(statusCode: StatusCodes.Status500InternalServerError)]
        public IActionResult GerarCpf()
        {
            return Response(_uteisAppService.GerarCpf());
        }

        /// <summary>
        /// Validar Cpf ou Cnpj.
        /// </summary>
        /// <param name="documento">Dados do documento</param>
        /// <param name="tipoDocumento">Selecione o tipo do documento (0 = CPF e 1 = CNPJ) </param>
        /// <returns></returns>
        [AllowAnonymous, HttpPost("validar/documento")]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Post))]
        [ProducesResponseType(typeof(bool), statusCode: StatusCodes.Status200OK)]
        [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
        [ProducesResponseType(statusCode: StatusCodes.Status500InternalServerError)]
        public IActionResult Validar(TipoDocumentoEnum tipoDocumento, [FromBody] string documento)
        {
            bool _documentoValidado;

            switch (tipoDocumento)
            {
                case TipoDocumentoEnum.CPF:
                    _documentoValidado = _uteisAppService.ValidarCpf(cpf: documento);
                    break;

                default:
                    _documentoValidado = _uteisAppService.ValidarCnpj(cnpj: documento);
                    break;
            };

            return Response(_documentoValidado);
        }

        /// <summary>
        /// Gerar senha forte.
        /// </summary>
        /// <param name="quantidadeCaracteres">Quantidade de carecteres desejado para a senha a ser gerada</param>
        /// <returns></returns>
        [AllowAnonymous, HttpPost("gerar/Senha")]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Post))]
        [ProducesResponseType(typeof(IEnumerable<string>), statusCode: StatusCodes.Status200OK)]
        [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
        [ProducesResponseType(statusCode: StatusCodes.Status500InternalServerError)]
        public IActionResult GerarSenhaForte([FromQuery] int quantidadeCaracteres)
        {
            return Response(_uteisAppService.GerarSenhaForte(quantidadeCaracteres: quantidadeCaracteres));
        }

        #endregion POST
    }
}