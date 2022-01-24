using MediatR;
using OndoNet.Infrastructure.Dtos;
using OndoNet.Model;

namespace OndoNet.Infrastructure.Commands
{
    public class Create_typeName_Command : Create_typeName_Dto, IRequest<_typeName_>
    {
    }
}
