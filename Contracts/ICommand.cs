using System;

namespace Altidude.Contracts
{
    public interface ICommand
    {
        Guid Id { get; }
    }
}
