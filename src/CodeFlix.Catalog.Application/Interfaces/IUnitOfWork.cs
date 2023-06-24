using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CodeFlix.Catalog.Application.Interfaces
{
    public interface IUnitOfWork
    {
        public Task Commit(CancellationToken cancellationToken);
    }
}
