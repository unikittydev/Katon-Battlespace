using Game.Modules.Creation;
using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace Game.Modules
{
    public class ModuleContainer
    {
        private Ship ship { get; }

        private Dictionary<Type, List<Module>> modules { get; }

        private NativeArray<bool> _mask;
        public NativeArray<bool> mask => _mask;

        public event Action<Module> onAddModule;

        public event Action<Module> onRemoveModule;

        public ModuleContainer(Type[] moduleTypes, Ship ship)
        {
            this.ship = ship;
            modules = new Dictionary<Type, List<Module>>();
            for (int i = 0; i < moduleTypes.Length; i++)
                modules.Add(moduleTypes[i], new List<Module>());
        }

        public int Count<T>() => modules[typeof(T)].Count;

        public int Count(Type type) => modules[type].Count;

        public T Get<T>(int index) where T : Module
        {
            return modules[typeof(T)][index] as T;
        }

        public Module Get(Type type, int index)
        {
            return modules[type][index];
        }

        public void Add(in Module value)
        {
            modules[value.GetType()].Add(value);
            onAddModule?.Invoke(value);
        }

        public bool Remove(in Module item)
        {
            bool remove = modules[item.GetType()].Remove(item);
            onRemoveModule?.Invoke(item);
            return remove;
        }

        public void Update()
        {
            Clear();
            ModuleTools.FillMaskValue(mask, int3.zero, ship.component.world.size, ship.component.world.mul, false);
            ModuleFinder.FindModules(ship);
        }

        public void Clear()
        {
            foreach (var kv in modules)
                for (int i = kv.Value.Count - 1; i >= 0; i--)
                {
                    if (kv.Value[i] is IActionModule actionModule)
                        actionModule.Dispose();
                    Remove(kv.Value[i]);
                }
        }

        public void ResizeMask(in int3 size)
        {
            if (_mask.IsCreated)
                _mask.Dispose();
            _mask = new NativeArray<bool>(size.x * size.y * size.z, Allocator.Persistent);
        }

        public void OnGizmosTest(in Vector3 offset)
        {
            Gizmos.color = Color.red;
            foreach (var kv in modules)
                for (int i = 0; i < kv.Value.Count; i++)
                {
                    Vector3 center = offset + (Vector3)(kv.Value[i].boundsStart + (float3)kv.Value[i].boundsSize * .5f);
                    Vector3 size = (float3)kv.Value[i].boundsSize;
                    Gizmos.DrawWireCube(center, size);
                }
        }
    }
}
