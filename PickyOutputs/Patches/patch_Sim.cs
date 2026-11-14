using MonoMod;
using System;
using PickyOutputs.Components;
using Quintessential.Components;
using System.Collections.Generic;
using Quintessential;

internal class patch_Sim : Sim 
{
    [MonoModIgnore]
    private static extern bool orig_IsSameMolecule(Molecule molecule1, Molecule molecule2);

    private static bool IsSameMolecule(Molecule molecule1, Molecule molecule2)
    {
        if (!orig_IsSameMolecule(molecule1, molecule2)) return false;
        
        foreach (KeyValuePair<HexIndex, Atom> pair in molecule1.GetAtoms())
        {
            PickyOutputs.PickyOutputs.Instance.Log.Info("found atom: " + pair);
            if (!pair.Value.HasComponent(PickyComponent.Id))
            {
                continue;
            }
            PickyOutputs.PickyOutputs.Instance.Log.Info("checking atom: " + pair);

            if (!molecule2.GetAtoms().ContainsKey(pair.Key))
            {
                return false;
            }
            if (pair.Value.GetComponent(PickyComponent.Id) is not PickyComponent pickyComp)
            {
                return false;
            }

            return pickyComp.IsSatisfiedByAtom(molecule2.GetAtoms()[pair.Key]);
        }

        PickyOutputs.PickyOutputs.Instance.Log.Info("Found no matching PickyComponent");

        return true;
    }
}