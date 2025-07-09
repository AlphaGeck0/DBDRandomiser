using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBDRandomiser
{
    public class CharacterSelection
    {
        public List<string> SelectedSurvivors { get; set; } = new();
        public List<string> SelectedKillers { get; set; } = new();
        public List<string> SelectedSurvivorPerks { get; set; } = new();
        public List<string> SelectedKillerPerks { get; set; } = new();
    }
}
