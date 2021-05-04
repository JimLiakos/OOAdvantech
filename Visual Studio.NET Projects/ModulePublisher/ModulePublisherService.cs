using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModulePublisher
{
    /// <MetaDataID>{d3e1cf40-f4c1-4dc9-9b40-a54d17655cd7}</MetaDataID>
    public class ModulePublisherService:IModulePublisher
    {
        

        public int ExecudeModulePublishCommand(string fileName, string arguments)
        {
            System.Diagnostics.Process process = System.Diagnostics.Process.Start(fileName,arguments);
            int Count = 0;
            while (!process.HasExited && Count < 40)
            {
                Count++;
                System.Threading.Thread.Sleep(200);
            }
            if (process.HasExited)
                return process.ExitCode;
            else
                return 1;
        }

       
    }
}
