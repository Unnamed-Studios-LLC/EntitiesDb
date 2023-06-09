﻿using System.Diagnostics.CodeAnalysis;
using EntitiesDb.Components;

namespace EntitiesDb
{
    public unsafe readonly struct EntityArchetype
    {
        private readonly ulong[]? _array;

        internal EntityArchetype(ulong[]? array)
        {
            _array = array;
        }

        public bool IsDisabled => Contains(ComponentRegistry.Type<Disabled>.Id);

        internal int Depth => _array?.Length ?? 0;

        internal ulong this[int index]
        {
            get => _array != null ? _array[index] : throw new Exception("Attempted to access an empty Archetype");
            set
            {
                if (_array == null) throw new Exception("Attempted to access an empty Archetype");
                _array[index] = value;
            }
        }

        public static EntityArchetype FromIds(IEnumerable<int> componentIds)
        {
            int maxDepth = 0;
            foreach (var id in componentIds)
            {
                var depth = id / 64 + 1;
                if (depth > maxDepth) maxDepth = depth;
            }

            if (maxDepth == 0) return default;

            var array = new ulong[maxDepth];
            foreach (var id in componentIds)
            {
                var index = id / 64;
                var bitIndex = id % 64;
                array[index] |= 1ul << bitIndex;
            }
            return new EntityArchetype(array);
        }

        public bool Contains(int componentId)
        {
            if (_array == null) return false;
            var depth = componentId / 64;
            if (depth >= Depth)
            {
                return false;
            }
            var relType = 1ul << componentId % 64;
            return (_array[depth] & relType) == relType;
        }

        public IEnumerable<int> GetIds()
        {
            if (_array == null) yield break;
            for (int i = 0; i < Depth; i++)
            {
                var relArchetype = _array[i];
                if (relArchetype == 0) continue;

                for (int j = 0; j < 64; j++)
                {
                    var relType = 1ul << j;
                    if (relType > relArchetype) break;

                    var componentId = i * 64 + j;
                    if ((relArchetype & relType) == relType)
                    {
                        yield return componentId;
                    }
                }
            }
        }

        internal void Clear()
        {
            if (_array == null) return;
            Array.Clear(_array);
        }

        internal void CopyTo(EntityArchetype archetype)
        {
            if (_array == null || archetype._array == null) return;
            Array.Copy(_array, archetype._array, Math.Min(_array.Length, archetype._array.Length));
        }

        internal bool ContainsAll(in EntityArchetype filter)
        {
            if (filter.Depth > Depth) return false;

            for (int i = 0; i < filter.Depth; i++)
            {
                var mask = filter._array![i];
                if ((_array![i] & mask) != mask) return false;
            }

            return true;
        }

        internal bool ContainsAny(in EntityArchetype filter)
        {
            var minDepth = Math.Min(Depth, filter.Depth);
            if (minDepth == 0) return true;

            for (int i = 0; i < minDepth; i++)
            {
                if ((_array![i] & filter._array![i]) != 0) return true;
            }

            return false;
        }

        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            if (obj is EntityArchetype archetype) return Equals(in archetype);
            return base.Equals(obj);
        }

        public bool Equals(in EntityArchetype other)
        {
            if (Depth != other.Depth)
            {
                return false;
            }

            for (int i = 0; i < Depth; i++)
            {
                if (_array![i] != other._array![i])
                {
                    return false;
                }
            }
            return true;
        }

        public override int GetHashCode()
        {
            if (_array == null) return 0;
            int hash = Depth * 2;
            for (int i = 0; i < Depth; i++)
            {
                var val = _array[i];
                hash = unchecked(hash * 31 + (int)val);
                hash = unchecked(hash * 31 + (int)(val >> 32));
            }
            return hash;
        }

        internal IEnumerable<long> GetIndices()
        {
            if (_array == null)
            {
                yield return 0;
                yield break;
            }

            foreach (var id in GetIds())
            {
                yield return id;
            }

            int skip = 1;
            foreach (var idA in GetIds())
            {
                int i = 0;
                foreach (var idB in GetIds())
                {
                    if (i++ < skip) continue;
                    yield return (long)idB << 32 | (uint)idA;
                }
                skip++;
            }
        }

        internal IEnumerable<int> GetNonZeroIds()
        {
            foreach (var componentId in GetIds())
            {
                if (ComponentRegistry.Get(componentId).ZeroSize) continue;
                yield return componentId;
            }
        }

        internal long GetQueryIndex()
        {
            if (_array == null) return 0;

            int idA = -1;
            int idB = -1;
            foreach (var id in GetIds())
            {
                if (idA == -1) idA = id;
                else
                {
                    idB = id;
                    break;
                }
            }

            if (idB == -1) return idA;
            return (long)idB << 32 | (uint)idA;
        }

        internal int NonZeroIndexOf(int componentId)
        {
            int i = 0;
            foreach (var id in GetNonZeroIds())
            {
                if (id == componentId) return i;
                i++;
            }
            return -1;
        }

        public static bool operator ==(EntityArchetype left, EntityArchetype right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(EntityArchetype left, EntityArchetype right)
        {
            return !(left == right);
        }
    }
}
