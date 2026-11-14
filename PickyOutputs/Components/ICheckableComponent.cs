using Quintessential.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace PickyOutputs.Components
{
    internal interface ICheckableComponent : IAtomComponent
    {
        public abstract bool CheckIfMatching(ICheckableComponent otherComponent);

    }
}
