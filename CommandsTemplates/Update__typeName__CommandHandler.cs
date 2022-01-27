using MediatR;
using Microsoft.Extensions.Logging;
using _namespaceRoot_.Model;
using _namespaceRoot_.Services.UnitOfWork;

namespace _namespaceRoot_.Infrastructure.Commands
{
    public class Update_typeName_CommandHandler : IRequestHandler<Update_typeName_Command, bool>
    {
        private readonly ILogger<Update_typeName_CommandHandler> logger;
        private readonly IRepository<_typeName_> _typeName_Repo;

        public Update_typeName_CommandHandler(
            ILogger<Update_typeName_CommandHandler> logger,
            IRepository<_typeName_> _typeName_Repo
        )
        {
            this.logger = logger;
            this._typeName_Repo = _typeName_Repo;
        }

        public async Task<bool> Handle(Update_typeName_Command request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Update _typeName_ Command");
            var _typeName_ = await _typeName_Repo.GetByIdAsync(request.Id);

            await _typeName_Repo.UpdateAsync(_typeName_);

            return true;
        }
    }
}
