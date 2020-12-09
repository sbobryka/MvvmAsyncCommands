using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MvvmAsyncCommands.Services
{
    internal class Operations
    {
        public void SomeOperation()
        {
            Thread.Sleep(10);
        }
    }
}
