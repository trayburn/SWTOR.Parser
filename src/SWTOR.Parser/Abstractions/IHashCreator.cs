using System;
using System.Linq;
using System.Collections.Generic;

namespace SWTOR.Parser
{
    public interface IHashCreator
    {
        string CreateHash(string data);
    }
}
