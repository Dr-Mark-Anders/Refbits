using ModelEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelEngine
{
    public class ReactionPackage
    {
        public Reactions Reactions { get; set; }
        public MixedComponents mixedComponents { get; set; }
        public CalibrationFactors calibFactors = new CalibrationFactors();
        public Components cc;

        public ReactionPackage()
        {

        }
    }
}
