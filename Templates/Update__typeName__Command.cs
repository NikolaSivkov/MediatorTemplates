using MediatR;
using _namespaceRoot_.Infrastructure.Dtos;
using _namespaceRoot_.Model;
using System;

namespace _namespaceRoot_.Infrastructure.Commands
{
    public class Update_typeName_Command : _typeName_Dto, IRequest<bool>
    {
        public Guid Id { get; set; }
    }
}
