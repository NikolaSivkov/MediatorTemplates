using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using _namespaceRoot_.Model;
using _namespaceRoot_.Services.UnitOfWork;

namespace _namespaceRoot_.Infrastructure.Commands
{
    public class Create_typeName_CommandHandler : IRequestHandler<Create_typeName_Command, _typeName_>
    {
        private readonly ILogger<Create_typeName_CommandHandler> logger;
        private readonly IMapper map;
        private readonly IRepository<_typeName_> _typeName_Repo;

        public Create_typeName_CommandHandler(
            ILogger<Create_typeName_CommandHandler> logger,
            IMapper map,
            IRepository<_typeName_> _typeName_Repo
            )
        {
            this.logger = logger;
            this.map = map;
            this._typeName_Repo = _typeName_Repo;
        }

        public async Task<_typeName_> Handle(Create_typeName_Command request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Create _typeName_ Command");

            var _typeName_ = map.Map<Create_typeName_Command, _typeName_>(request);

            await _typeName_Repo.AddAsync(_typeName_);

            return _typeName_;
        }
    }
}
