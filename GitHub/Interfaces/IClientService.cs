using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitHub.Interfaces
{
    public interface IClientService
    {
        Task<string> GetStringAsync(string uri);
    }
}
