using Auxiliar.Application.Interfaces;
using Auxiliar.Domain.Core.Bus;
using Auxiliar.Domain.Core.Notifications;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Auxiliar.Services.Api.Controllers
{
    [Route("[controller]")]
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
        [HttpPost("Guid")]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Post))]
        [ProducesResponseType(typeof(IEnumerable<Guid>), statusCode: StatusCodes.Status200OK)]
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

        #endregion POST
    }
}