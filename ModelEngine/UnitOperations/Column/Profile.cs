using Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Units;
using static gv;

namespace ModelEngine
{
    public class ProfileObj
    {
        public string name;
        public UnitOperation parent;
        public List<StreamProperty> props;
        public ePropID type;
        public string typeName;

        public ProfileObj(UnitOperation parent, ePropID type, string name = "Profile")
        {
            this.parent = parent;
            this.name = name;
            this.props = new List<StreamProperty>();
            this.typeName = type.ToString();
            this.type = type;
        }

        public new object GetType()
        {
            return this.type;
        }

        // Clean up
        public virtual void CleanUp()
        {
            this.parent = null;
            this.type = ePropID.NullUnits;
            for (int i = 0; i < props.Count(); i++)
            {
                //props[i].CleanUp();
            }
            //map(ProfileProperty.CleanUp, this.props);
        }

        // Dimensions the profile. Just appends or deletes to thed end of the profile
        public virtual void SetSize(int size)
        {
            var currSize = this.props.Count;
            //Delete if needed
            for (int i = 0; i < this.props.Count; i++)
            {
                //  this.props[i].CleanUp();
            }
            //  this.props[size].CleanUp();
            this.props.RemoveAt(size);
            for (var i = currSize; i < size - currSize; i++)
            {
                this.AddProperty();
            }
            //Re assign all the indexes
            for (int i = 0; i < size; i++)
            {
                //  props[i].CleanUp();
            }
            //map(ProfileProperty.SetIndex, this.props, Enumerable.Range(0, size));
        }

        public virtual int GetSize()
        {
            return this.props.Count;
        }

        // Add a property in the given index, if idx=-1, then append at the end
        public virtual void AddProperty(int idx = -1)
        {
            int size;
            if (idx == -1)
            {
                this.props.Add(new StreamProperty(this.type, double.NaN));
                size = this.props.Count;
            }
            else
            {
                this.props.Insert(idx, new StreamProperty(this.type, double.NaN));
                //Re assign all the indexes
                size = this.props.Count;
                //map(ProfileProperty.SetIndex, this.props[idx], Enumerable.Range( ));
            }
        }

        // Remove a property in the given index, if idx=-1, then remove using   the end
        public virtual void RemoveProperty(int idx = -1)
        {
            if (idx == -1)
            {
                var prop = this.props.pop();
                //  prop.CleanUp();
            }
            else
            {
                // this.props[idx].CleanUp();
                this.props.RemoveAt(idx);
                //Re assign all the indexes
                var size = this.props.Count;
                for (int i = idx; i < size - idx; i++)
                {
                    // this.props[i].SetIndex(i);
                }
                //map(ProfileProperty.SetIndex, this.props[idx], Enumerable.Range( ));
            }
        }

        // Remove properties by a range of indexes
        public virtual void RemoveProperties(int usingIdx, int toIdx)
        {
            for (var i = usingIdx; i < toIdx + 1 - usingIdx; i++)
            {
                //   this.props[i].CleanUp();
            }

            this.props.RemoveRange(usingIdx, toIdx + 1 - usingIdx);

            //Re assign all the indexes
            var size = this.props.Count;
            for (int i = usingIdx; i < size - usingIdx; i++)
            {
                //    this.props[i].SetIndex(i);
            }
            // map(ProfileProperty.SetIndex, this.props[using Idx], Enumerable.Range( ));
        }

        public virtual object GetName()
        {
            return this.name;
        }

        public virtual UnitOperation GetParent()
        {
            return this.parent;
        }

        public virtual string GetPath()
        {
            return String.Format(this.parent.GetPath() + "." + this.name);
        }

        // return   the object  that holds all the properties
        public virtual List<StreamProperty> GetProperties()
        {
            return this.props;
        }

        // Set the values of the whoel profile. status applies to all the values
        public virtual void SetValues(List<double> values, dynamic status = null)
        {
            if (status is null)
                status = CALCULATED_V;

            //Make status a list in case  it is an int eger
            if (status is int)
            {
                var s = this.props.Count;
                status = Ext.ArrayInt(s, (int)status);
            }
            //Loop with map
            for (int i = 0; i < this.props.Count(); i++)
            {
                //  this.props[i].SetValue(values[i], status[i]);
            }
            //map(ProfileProperty.SetValue, this.props, values, status);
        }

        // Get all the values in one single vector
        public virtual double?[] GetValues()
        {
            var values = new List<double?>();
            for (int i = 0; i < this.props.Count; i++)
            {
                //   values.Add(this.props[i].GetValue());
            }
            //var values = map(ProfileProperty.GetValue, this.props);
            return values.ToArray();
        }

        // Call th forget of each property
        public virtual void Forget()
        {
            for (int i = 0; i < this.props.Count(); i++)
            {
                //  this.props[i].Forget(null);
            }
            // map(ProfileProperty.Forget, this.props);
        }

        public virtual object GetObject(string desc)
        {
            if (desc.Slice(0, 4) == "Item")
            {
                try
                {
                    var idx = Convert.ToInt32(desc[4]);
                    return this.props[idx];
                }
                catch
                {
                    return null;
                }
            }
            else if (desc == "Size")
            {
                return this.props.Count;
            }
            else if (desc == "Values")
            {
                return this.GetValues();
            }
            else if (desc == "Values")
            {
                return this.GetValues();
            }
            else if (desc == "UnitType")
            {
                //   return   this.type.unitType;
            }
            else if (desc == "Details")
            {
                //Just pas a list of Tuples withe value, status and type id
                //  return   Tuple.Create(this.props.getvalue(), this.props.getc);
                List<dynamic> rs = new List<dynamic>();
                for (int i = 0; i < this.props.Count(); i++)
                {
                    //  rs.Add(DetailedTuple(this.props[i]));
                }
                return rs;
            }
            return null;
        }

        private Func<StreamProperty, Tuple<double, SourceEnum>> DetailedTuple = prop =>
        {
            return Tuple.Create<double, SourceEnum>(prop.BaseValue, prop.origin);
        };

        // This call is used by the cli and is intended to set the value of the prop as None
        public virtual void DeleteObject(StreamProperty prop)
        {
            prop.ForceSetValue(double.NaN, SourceEnum.Empty);
        }

        public virtual void CloneContents(ProfileObj clone)
        {
            if (this.GetSize() != clone.GetSize())
            {
                clone.SetSize(this.GetSize());
            }
            var idx = 0;
            foreach (var prop in this.props)
            {
                var propClone = clone.props[idx];
                if (prop.origin == SourceEnum.UnitOpCalcResult)
                {
                    propClone.origin = prop.origin;
                    propClone.BaseValue = prop.BaseValue;
                }
                idx += 1;
            }
        }
    }
}