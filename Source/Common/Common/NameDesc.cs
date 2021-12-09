using System;
using System.Collections.Generic;
using System.Text;

namespace BioRad.Common
{
    /// <summary>
    /// Creates static look up information for user management roles.
    /// </summary>
    public sealed class NameDesc
    {
        /// <summary>
        /// Name accessor.
        /// </summary>
        public readonly string Name;

        /// <summary>
        /// Name accessor.
        /// </summary>
        public readonly string CultureVaiantName;
        /// <summary>
        /// Description accessor.
        /// </summary>
        public readonly string Description;
        /// <summary>
        /// Constructor.
        /// </summary>
        public NameDesc(string name, string cultureVaiantName, string description)
        {
            Name = name;
            CultureVaiantName = cultureVaiantName;
            Description = description;
        }
    }
}
