using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.SignalR;
using _namespaceRoot_.Shared.Enums;
using _namespaceRoot_.Shared.Models.Messaging;
using _namespaceRoot_.Hubs;
using _namespaceRoot_.Infrastructure.DomainServices;
using _namespaceRoot_.Infrastructure.Services;
using _namespaceRoot_.Infrastructure.UnitOfWork;
using _namespaceRoot_.Model;

namespace _namespaceRoot_.Infrastructure.Commands
{
    public class Create_typeName_CommandHandler : IRequestHandler<Create_typeName_Command, _typeName_>
    {
        private readonly IMapper mapper;
        private readonly IRepository<_typeName_> _typeName_Repo;
        private readonly IRepository<IrrigationInfrastructure> irrigationInfrastructureRepo;
        private readonly IInputOutputService ioSvc;
        private readonly ILogger<Create_typeName_CommandHandler> logger;
        private readonly IHubContext<ComunicationHub> hubContext;
        private readonly MessageDispatcher messageDispatcher;
        private readonly SignalRConnectionMapping connectionMapping;

        public Create_typeName_CommandHandler(
            IMapper map,
            IRepository<_typeName_> _typeName_Repo,
            IRepository<IrrigationInfrastructure> IrrigationInfrastructureRepo,
            ILogger<Create_typeName_CommandHandler> logger,
            IHubContext<ComunicationHub> hubContext,
            MessageDispatcher messageDispatcher,
            SignalRConnectionMapping connectionMapping)
        {
            this.mapper = map;
            this._typeName_Repo = _typeName_Repo;
            this.irrigationInfrastructureRepo = IrrigationInfrastructureRepo;
            this.logger = logger;
            this.hubContext = hubContext;
            this.messageDispatcher = messageDispatcher;
            this.connectionMapping = connectionMapping;
        }
        public async Task<_typeName_> Handle(Create_typeName_Command request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Create _typeName_ Command");

            var _typeName_ = map.Map<Create_typeName_Command, _typeName_>(request);

            var irrInfra = await irrigationInfrastructureRepo.GetByIdAsync(request.IrrigationInfrastructureId);

            var cReq = new CloudRequest<_namespaceRoot_.Shared.DTO.Create_typeName_Dto>
            {
                Message = mapper.Map<Create_typeName_Command, _namespaceRoot_.Shared.DTO.Create_typeName_Dto>(request)
            };

            var connectionId = connectionMapping.Get(irrInfra.ControllerId.ToString());

            await hubContext.Clients.Client(connectionId).SendAsync(CloudEndpointsEnum.Create_typeName_, cReq, cancellationToken: cancellationToken);

            var response = await messageDispatcher.AwaitCommandResponse<_namespaceRoot_.Shared.DTO._typeName_Dto>(cReq.Id, cancellationToken, 5000);

            if (response == null || (response.Errors?.Any() ?? false))
            {
                var errors = string.Join(";", response?.Errors ?? new List<string> { "Response is null" });
                logger.LogError(errors);
                throw new Exception(errors);
            }
            else
            {
                await _typeName_Repo.AddAsync(_typeName_);

                return _typeName_;
            }
        }
    }
}
