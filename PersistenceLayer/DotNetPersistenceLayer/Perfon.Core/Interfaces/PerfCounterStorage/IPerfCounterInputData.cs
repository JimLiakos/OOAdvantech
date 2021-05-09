using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Perfon.Interfaces.PerfCounterStorage
{
    /// <summary>
    /// Performance Counter Data for passing to storage.
    /// Immutable.
    /// </summary>
    /// <MetaDataID>{08328500-8b54-46db-900e-c7b310da9b23}</MetaDataID>
    public interface IPerfCounterInputData
    {
        /// <summary>
        /// Name of Perf. Counter which generate this data block
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Value of Perf. Counter
        /// </summary>
        float Value { get; }

        /// <summary>
        /// Optional.
        /// String Formatted Value of perf. Counter Data
        /// Used for CVS Storage for example
        /// </summary>
        string FormattedValue { get; }
    }
}
