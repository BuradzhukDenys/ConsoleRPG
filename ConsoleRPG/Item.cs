using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRPG
{
    internal abstract class Item
    {
        private int _count;
        private int _maxCount;
        private string? _name;

        public string? Name
        {
            get { return _name; }
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Item name can't be empty or null!");
                }

                _name = value;
            }
        }
        public int Count
        {
            get { return _count; }
            set
            {
                _count = Math.Clamp(value, 1, MaxCount);
            }
        }

        protected int MaxCount
        {
            get { return _maxCount; }
            private set
            {
                if (value == 1)
                {
                    CanStack = false;
                }
                else
                {
                    CanStack = true;
                }

                _maxCount = Math.Clamp(value, 1, 100);
            }
        }
        public bool CanStack { get; private set; } = false;

        public Item(string name, int maxCount, int count)
        {
            Name = name;
            MaxCount = maxCount;
            Count = count;
        }
    }
}
