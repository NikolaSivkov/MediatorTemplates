using MediatR;
using _namespaceRoot_.Infrastructure.Dtos;
using _namespaceRoot_.Model;

namespace _namespaceRoot_.Infrastructure.Commands
{
    public class Create_typeName_Command : Create_typeName_Dto, IRequest<_typeName_>
    {
    }
}
