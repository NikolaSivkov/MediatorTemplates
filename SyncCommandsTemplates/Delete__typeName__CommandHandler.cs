using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.SignalR;
using Ondo.Shared.Enums;
using Ondo.Shared.Models.Messaging;
using _namespaceRoot_.Hubs;
using _namespaceRoot_.Infrastructure.DomainServices;
using _namespaceRoot_.Infrastructure.Services;
using _namespaceRoot_.Infrastructure.UnitOfWork;
using _namespaceRoot_.Model;
;

namespace OndoCloud.Infrastructure.Commands
{
    public class Delete_typeName_CommandHandler : IRequestHandler<Delete_typeName_Command, bool>
    {
        private readonly IRepository<_typeName_> _typeName_Repo;
        private readonly IRepository<IrrigationInfrastructure> irrigationInfrastructureRepo;
        private readonly ILogger<Delete_typeName_CommandHandler> logger;
        private readonly IHubContext<ComunicationHub> hubContext;
        private readonly MessageDispatcher messageDispatcher;
        private readonly SignalRConnectionMapping connectionMapping;

        public Delete_typeName_CommandHandler(
            IRepository<_typeName_> _typeName_Repo,
            IRepository<IrrigationInfrastructure> irrigationInfrastructureRepo,
            ILogger<Delete_typeName_CommandHandler> logger,
            IHubContext<ComunicationHub> hubContext,
            MessageDispatcher messageDispatcher,
            SignalRConnectionMapping connectionMapping)
        {
            this._typeName_Repo = _typeName_Repo;
            this.irrigationInfrastructureRepo = irrigationInfrastructureRepo;

            this.logger = logger;
            this.hubContext = hubContext;
            this.messageDispatcher = messageDispatcher;
            this.connectionMapping = connectionMapping;
        }

        public async Task<bool> Handle(Delete_typeName_Command request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Delete _typeName_ Command");

            var db_typeName_ = await _typeName_Repo.GetByIdAsync(request.Id);
            var irrInfra = await irrigationInfrastructureRepo.GetByIdAsync(db_typeName_.IrrigationInfrastructureId);

            var cReq = new CloudRequest<Ondo.Shared.DTO.Delete_typeName_Dto>
            {
                Message = new Ondo.Shared.DTO.Delete_typeName_Dto() { Id = request.Id }
            };

            var connectionId = connectionMapping.Get(irrInfra.ControllerId.ToString());

            await hubContext.Clients.Client(connectionId).SendAsync(CloudEndpointsEnum.Delete_typeName_, cReq, cancellationToken: cancellationToken);

            var response = await messageDispatcher.AwaitCommandResponse<bool>(cReq.Id, cancellationToken, 5000);

            if (response == null || (response.Errors?.Any() ?? false))
            {
                var errors = string.Join(";", response?.Errors ?? new List<string> { "Response is null" });
                logger.LogError(errors);
                throw new Exception(errors);
            }
            else
            {
                var _typeName_Db = await _typeName_Repo.GetByIdAsync(request.Id);

                await _typeName_Repo.DeleteAsync(_typeName_Db);

                return true;
            }
        }
    }
}