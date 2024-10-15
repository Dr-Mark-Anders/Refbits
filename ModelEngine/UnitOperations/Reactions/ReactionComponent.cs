using MathNet.Numerics.Optimization.ObjectiveFunctions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;

namespace ModelEngine
{
    public class ReactionComponent
    {
        private string name; private int stoich; private double power; private double moleFraction;
        private double tempMoleFlow = 0;
        CompType type= CompType.Normal;
        bool isForward =true;

        YieldPattern yieldpattern = null;

        public ReactionComponent(string name, int stoich)
        {
            Name = name;
            Stoich = stoich;
            this.power = 0;
        }

        public ReactionComponent(string name, int stoich, double power) : this(name, stoich)
        {
            this.power = power;
        }

        public string Name { get => name; set => name = value; }
        public int Stoich { get => stoich; set => stoich = value; }
        public double Power { get => power; set => power = value; }
        public double MoleFraction { get => moleFraction; set => moleFraction = value; }
        public double TempMoleFlow { get => tempMoleFlow; set => tempMoleFlow = value; }
        public CompType Type { get => type; set => type = value; }
        public YieldPattern Yieldpattern { get => yieldpattern; set => yieldpattern = value; }
        public bool IsForward { get => isForward; set => isForward = value; }

        public override string ToString()
        {
                return this.name;
        }
    }

    public class ReactionComponents
    {
        private Dictionary<string, ReactionComponent> ForwardComponents = new(StringComparer.OrdinalIgnoreCase);
        private Dictionary<string, ReactionComponent> ReverseComponents = new(StringComparer.OrdinalIgnoreCase);

        public List<ReactionComponent> AllComponents()
        {
            List<ReactionComponent> res = new();
            foreach (var component in ForwardComponents)
            {
                res.Add(component.Value);
            }

            foreach (var component in ReverseComponents)
            {
                res.Add(component.Value);
            }
            return res;
        }

        public List<string> Names
        {
            get
            {
                List<string> res = new List<string>();
                res.AddRange(ForwardComponents.Keys.ToArray());
                res.AddRange(ReverseComponents.Keys.ToArray());
                return res;
            }
        }

        public Dictionary<string, ReactionComponent> FComps { get => ForwardComponents; set => ForwardComponents = value; }
        public Dictionary<string, ReactionComponent> RComps { get => ReverseComponents; set => ReverseComponents = value; }

        
        public void Add(ReactionComponent item)
        {
            if (item.Stoich < 0)
            {
                this.FComps.Add(item.Name,item);
                item.IsForward = true;
            }
            else
            {
                this.RComps.Add(item.Name, item);
                item.IsForward = false;
            }

            //ReverseComponents.Add(item.Name, item);
        }
    }
}