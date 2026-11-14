using PickyOutputs.Components;
using Quintessential;
using System.Linq;

namespace PickyOutputs;

internal class PickyOutputs : QuintessentialMod, IDataMod {
    public static PickyOutputs Instance { get; }
    public override string ModId => "pickyoutputs";

    public override void Load() { }

    public override void LoadContent()
    {

        PickyComponent.Register();
    }
    public override void LoadCompatContent() { }
    public override void FinaliseContent() { }

    public override void PostLoad() 
    {

        
    }
	public override void Unload() { }
}
