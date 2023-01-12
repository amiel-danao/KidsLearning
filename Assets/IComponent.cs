using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidsLearning
{
    public interface IComponent
    {
        public IComponent DependentTo { get; set; }
        public Action DoneInitialization { get; set; }
        void Initialize();

    }
}
