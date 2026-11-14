using Quintessential;
using Quintessential.Components;
using Quintessential.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace PickyOutputs.Components
{
    internal class PickyComponent(List<Identifier> matchingComponents) : IAtomComponent
    {
        public static readonly Identifier Id = "pickyoutputs:picky_component";
        Identifier IComponentBase<Atom>.Id => Id;

        public Atom self;

        public List<Identifier> componentsToCheck = matchingComponents;

        public CycleEvent.CycleEventExecutionType CallbackType => CycleEvent.CycleEventExecutionType.None;
        
        private static readonly Codec<List<Identifier>> ListCodec = ListCodec<Identifier>.Create(Codecs.ID);
        private static readonly Codec<IAtomComponent> CODEC = Codec<IAtomComponent>.Create(
            ListCodec.Seal("List of Components", (IAtomComponent component) => (component as PickyComponent).componentsToCheck),
            (components) => new PickyComponent(components)
        );


        public bool IsSatisfiedByAtom(Atom other)
        {
            foreach (Identifier componentId in componentsToCheck)
            {
                if (!self.GetComponents().ContainsKey(componentId))
                {
                    throw new Exception("Picky Component could not find needed component in its own atom.");
                }

                IAtomComponent componentToCompare = self.GetComponent(componentId);
                if (componentToCompare is not ICheckableComponent comparableComponent)
                {
                    throw new Exception("Picky Component found non-checkable component with checkable component's Id in own atom");
                }

                if (!other.HasComponent(componentId)) return false;

                IAtomComponent otherComponentToCompare = other.GetComponent(componentId);

                if (otherComponentToCompare is not ICheckableComponent otherComparableComponent)
                {
                    throw new Exception("Picky Component found non-checkable component with checkable component's Id in other molecule");
                }

                if (!comparableComponent.CheckIfMatching(otherComparableComponent))
                {
                    return false;
                }
            }
            return true;
        }

        public void OnAddToMolecule(Molecule molecule, HexIndex hexPos) { }

        public void OnBind(Atom self, Molecule arg1)
        {
            this.self = self;
        }

        public void OnClone(ref Atom cloned)
        {
            cloned.AddComponent(new PickyComponent(componentsToCheck));
        }

        public void OnCycleCallback(Sim sim, CycleEvent.CycleEventExecutionType executionType) { }

        public void OnRemoveFromMolecule(Molecule molecule, HexIndex hexPos) { }

        public void OnReplace(AtomType newType) => self.RemoveComponent(Id);

        public void OnUnbind(Atom self, Molecule arg1) { }

        internal static void Register()
        { // Called by LoadContent.
          // Bind the codec to the component to use it when saving atoms with the component.
            Atom.RegisterComponent(Id, CODEC);
        }
    }
}
