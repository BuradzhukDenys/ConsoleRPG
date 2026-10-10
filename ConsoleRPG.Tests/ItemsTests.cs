using ConsoleRPG.Items;
using ConsoleRPG.Items.Consumables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRPG.Tests
{
    public class ItemsTests
    {
        [Fact]
        public void CountNotOverMaxCount()
        {
            Item item = new HealingPotion();
            item.Count = 1000;
            Assert.Equal(item.MaxCount, item.Count);
        }

        [Fact]
        public void CountNotUnderZero()
        {
            Item item = new HealingPotion();
            item.Count = -1000;
            Assert.Equal(0, item.Count);
        }

        [Fact]
        public void Clone_CreateIndependentCopy()
        {
            Item potion = new HealingPotion();
            Item clone = potion.Clone();
            clone.Count = 2;
            Assert.NotEqual(potion.Count, clone.Count);
        }
    }
}
