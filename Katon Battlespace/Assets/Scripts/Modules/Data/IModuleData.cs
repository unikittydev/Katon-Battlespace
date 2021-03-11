using Unity.Mathematics;

namespace Game.Modules.Data
{
    public interface IModuleData
    {
        public int3 boundsStart { get; set; }
        public int3 boundsSize { get; set; }
    }
}
