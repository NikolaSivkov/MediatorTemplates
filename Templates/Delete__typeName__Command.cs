using MediatR;
using _namespaceRoot_.Infrastructure.Dtos;
using _namespaceRoot_.Model;

namespace _namespaceRoot_.Infrastructure.Commands
{
    public class Delete_typeName_Command : Create_typeName_Dto, IRequest<bool>
    {
    }
}
