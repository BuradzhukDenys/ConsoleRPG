using ConsoleRPG.Entities.Enemies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRPG.Tests
{
    public class EntityTests
    {
        [Fact]
        public void EntityHealth_HealthChange_EqualmaxHealthIfGreaterThanMaxHealth()
        {
            Slime slime = new();
            slime.Health = 1000;
            Assert.Equal(slime.MaxHealth, slime.Health);
        }

        [Fact]
        public void EntityHealth_HealthChange_GreaterThanZero()
        {
            Slime slime = new();
            slime.Health = -1000;
            Assert.Equal(0, slime.Health);
        }

        [Fact]
        public void EntityHealth_HealthIsZero_IsDeadEqualTrue()
        {
            Slime slime = new();
            slime.Health = 0;
            Assert.True(slime.IsDead);
        }

        [Fact]
        public void DamageReduction_ChangeOverMaxReduction_EqualMaxReduction()
        {
            Slime slime = new();
            slime.DamageReduction = 1;
            Assert.Equal(0.5, slime.DamageReduction);
        }

        [Fact]
        public void DamageReduction_ChangeUnderMinReduction_EqualZero()
        {
            Slime slime = new();
            slime.DamageReduction = -12;
            Assert.Equal(0, slime.DamageReduction);
        }

        [Fact]
        public void TotalDamage_AddDamageMultiplier_CorrectExpression()
        {
            Slime slime = new();
            slime.AddDamageMultiplier(0.25);
            Assert.Equal(13, slime.TotalDamage);
        }

        [Fact]
        public void TotalDamage_RemoveDamageMultiplier_NotGiveUpUnderOne()
        {
            Slime slime = new();
            slime.AddDamageMultiplier(0.1);
            slime.RemoveDamageMultiplier(0.5);
            Assert.Equal(1, slime.DamageMultiplier);
        }

        [Fact]
        public void MaxHealth_IfZero_ThrowException()
        {
            Slime slime = new();
            Assert.Throws<ArgumentOutOfRangeException>(() => slime.MaxHealth = 0);
        }

        [Fact]
        public void AddDamageMultiplier_WithNegativeValue_ThrowException()
        {
            Slime slime = new();
            Assert.Throws<ArgumentException>(() => slime.AddDamageMultiplier(-1));
        }
    }
}
