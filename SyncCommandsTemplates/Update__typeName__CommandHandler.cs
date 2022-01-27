using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Ondo.Shared.Enums;
using Ondo.Shared.Models.Messaging;
using OndoCloud.Hubs;
using OndoCloud.Infrastructure.DomainServices;
using OndoCloud.Infrastructure.Services;
using OndoCloud.Infrastructure.UnitOfWork;
using OndoCloud.Model;

namespace OndoCloud.Infrastructure.Commands
{
    internal class Edit_typeName_CommandHandler : IRequestHandler<Edit_typeName_Command, bool>
    {
        private readonly IMapper mapper;
        private readonly IRepository<_typeName_> _typeName_Repo;
        private readonly IRepository<IrrigationInfrastructure> irrigationInfrastructureRepo;
        private readonly IInputOutputService ioSvc;
        private readonly ILogger<Edit_typeName_CommandHandler> logger;
        private readonly IHubContext<ComunicationHub> hubContext;
        private readonly MessageDispatcher messageDispatcher;
        private readonly SignalRConnectionMapping connectionMapping;

        public Edit_typeName_CommandHandler(
            IMapper map,
            IRepository<_typeName_> _typeName_Repo,
            IRepository<IrrigationInfrastructure> irrigationInfrastructureRepo,
            IInputOutputService ioSvc,
            ILogger<Edit_typeName_CommandHandler> logger,
            IHubContext<ComunicationHub> hubContext,
            MessageDispatcher messageDispatcher,
            SignalRConnectionMapping connectionMapping)
        {
            this.mapper = map;
            this._typeName_Repo = _typeName_Repo;
            this.irrigationInfrastructureRepo = irrigationInfrastructureRepo;
            this.ioSvc = ioSvc;
            this.logger = logger;
            this.hubContext = hubContext;
            this.messageDispatcher = messageDispatcher;
            this.connectionMapping = connectionMapping;
        }

        public async Task<bool> Handle(Edit_typeName_Command request, CancellationToken cancellationToken)
        {
            var _typeName_Db = await _typeName_Repo.GetByIdAsync(request.Id);

            if (_typeName_Db.OutputId != request.OutputId)
            {
                var validationResult = await ioSvc.ValidateUsageAsync(request.OutputId);
                if (!validationResult.IsValid)
                {
                    return false;
                }
            }

            var irrInfra = await irrigationInfrastructureRepo.GetByIdAsync(_typeName_Db.IrrigationInfrastructureId);

            var cReq = new CloudRequest<Ondo.Shared.DTO.Edit_typeName_Dto>
            {
                Message = mapper.Map<Edit_typeName_Command, Ondo.Shared.DTO.Edit_typeName_Dto>(request)
            };

            var connectionId = connectionMapping.Get(irrInfra.ControllerId.ToString());

            await hubContext.Clients.Client(connectionId).SendAsync(CloudEndpointsEnum.Update_typeName_, cReq, cancellationToken: cancellationToken);

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

                _typeName_Db.Name = request.Name;
                _typeName_Db.FlowRate = request.FlowRate;
                _typeName_Db.OpenDelayTime = request.OpenDelayTime;
                _typeName_Db.CloseDelayTime = request.CloseDelayTime;
                _typeName_Db.Pressure = request.Pressure;
                _typeName_Db.Notes = request.Notes;
                _typeName_Db.OutputId = request.OutputId;
                _typeName_Db.UpdatedAt = DateTime.UtcNow;

                await _typeName_Repo.UpdateAsync(_typeName_Db);

                return true;
            }
        }
    }
}