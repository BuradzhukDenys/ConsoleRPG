using ConsoleRPG.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRPG.Effects;

internal abstract class Effect(string newName, int newDuration)
{
    private int _duration = newDuration;
    public int Duration
    {
        get { return _duration; }
        private set
        {
            _duration = Math.Clamp(value, 0, 10);
        }
    }
    public string Name { get; private set; } = newName;
    public void Apply(Entity target)
    {
        Action(target);
        Duration--;
    }
    public abstract void InitializeMessage(Entity target);
    public abstract void Action(Entity target);
}
