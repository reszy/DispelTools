using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Threading.Tasks;

namespace DispelTools.ImageProcessing.Filters
{
    public interface IPerPixelFilter
    {
        Color Apply(Color color);
    }
}
