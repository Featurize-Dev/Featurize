using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Featurize.SortFeatures;
public interface IDependsOn<TFeature>
    where TFeature : IFeature
{
}
