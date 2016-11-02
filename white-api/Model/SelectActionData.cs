using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace white_api.Model
{
    /// <summary>
    /// modal class for select action data
    /// </summary>
    class SelectActionData
    {
        /// <summary>
        /// locator
        /// </summary>
        public string locator { get; set; }

        /// <summary>
        /// selector
        /// </summary>
        public string selector { get; set; }
    }
}
