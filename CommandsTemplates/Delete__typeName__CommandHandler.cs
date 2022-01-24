using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using _namespaceRoot_.Model;
using _namespaceRoot_.Services.UnitOfWork;

namespace _namespaceRoot_.Infrastructure.Commands
{
    public class Delete_typeName_CommandHandler : IRequestHandler<Delete_typeName_Command, bool>
    {
        private readonly ILogger<Delete_typeName_CommandHandler> logger;
        private readonly IMapper map;
        private readonly IRepository<_typeName_> _typeName_Repo;

        public Delete_typeName_CommandHandler(
            ILogger<Delete_typeName_CommandHandler> logger,
            IMapper map,
            IRepository<_typeName_> _typeName_Repo
            )
        {
            this.logger = logger;
            this.map = map;
            this._typeName_Repo = _typeName_Repo;
        }

        public async Task<bool> Handle(Delete_typeName_Command request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Delete _typeName_ Command");

            var _typeName_ = map.Map<Delete_typeName_Command, _typeName_>(request);

            await mainPumpRepo.DeleteAsync(mainPump);
            return _typeName_;
        }
    }
}
