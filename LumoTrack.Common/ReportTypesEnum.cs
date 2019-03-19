using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace LumoTrack.Common
{
    public enum ReportTypesEnum
    {
        [Description("No pasó el camión")]
        DontPass = 0,
        [Description("Comportamiento inadecuado")]
        Misbehavior = 1
    }
}
