using MediatR;

namespace _namespaceRoot_.Infrastructure.Commands
{
    public class Delete_typeName_Command : IRequest<bool>
    {
        public Guid Id { get; set; }
    }
}
