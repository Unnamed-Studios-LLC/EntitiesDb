using System;
using System.Collections.Generic;

namespace EntitiesDb
{
    internal static class Mask
    {
        private const int BitCount = sizeof(ulong) * 8;

        public static int BitAnd(Span<ulong> masksA, Span<ulong> masksB, Span<ulong> result)
        {
            var min = Math.Min(masksA.Length, masksB.Length);
            int count = 0;
            for (int i = 0; i < min; i++)
            {
                var value = masksA[i] & masksB[i];
                result[i] = value;
                if (value == 0) continue;
                count = i + 1;
            }
            return count;
        }

        public static int BitOr(Span<ulong> masksA, Span<ulong> masksB, Span<ulong> result)
        {
            // reorder so masksA is the largest
            if (masksB.Length > masksA.Length)
            {
                var tmp = masksA;
                masksA = masksB;
                masksB = tmp;
            }

            int count = 0;
            for (int i = 0; i < masksA.Length; i++)
            {
                var valueB = i < masksB.Length ? masksB[i] : 0;
                var value = masksA[i] | valueB;
                result[i] = value;
                if (value == 0) continue;
                count = i + 1;
            }
            return count;
        }

        public static int GetHashCode(Span<ulong> masks)
		{
            if (masks.Length == 0) return 0;
            int hash = masks.Length * 2;
            foreach (ref var mask in masks)
            {
                hash = unchecked(hash * 31 + (int)mask);
                hash = unchecked(hash * 31 + (int)(mask >> 32));
            }
            return hash;
        }

        public static int GetIds(Span<ulong> masks, Span<int> idResult, int maxCount = int.MaxValue)
        {
            if (maxCount <= 0) return 0;
            int count = 0;

            for (int i = 0; i < masks.Length; i++)
            {
                var mask = masks[i];
                var boundary = 1ul;
                for (int j = 0; j < BitCount; j++)
                {
                    if (mask < boundary) break;
                    if ((mask & boundary) == boundary)
                    {
                        idResult[count++] = i * BitCount + j;
                        if (count >= maxCount) return count;
                    }
                    boundary <<= 1;
                }
            }
            return count;
        }

        public static int GetIndices(Span<ulong> masks, Span<int> idResult, ref long[] results)
        {
            int resultCount = 0;
            var idCount = GetIds(masks, idResult);
            var maxSize = idCount + idCount * idCount;
            while (results.Length < maxSize)
            {
                Array.Resize(ref results, results.Length * 2);
            }

            // index for each id
            for (int i = 0; i < idCount; i++)
            {
                results[resultCount++] = idResult[i] + 1;
            }

            // index for each id pair
            for (int i = 0; i < idCount; i++)
            {
                for (int j = i + 1; j < idCount; j++)
                {
                    results[resultCount++] = (long)(idResult[j] + 1) << 32 | (uint)(idResult[i] + 1);
                }
            }
            return resultCount;
        }

        public static long GetQueryIndex(Span<ulong> masks)
        {
            Span<int> idResult = stackalloc int[2];
            var idCount = GetIds(masks, idResult, 2);
            return idCount switch
            {
                2 => (long)(idResult[1] + 1) << 32 | (uint)(idResult[0] + 1),
                1 => idResult[0] + 1,
                _ => 0
            };
        }

        public static int IdAdd(Span<ulong> masks, int id, Span<ulong> result)
        {
            var index = id / BitCount;
            var value = 1ul << (id % BitCount);
            result[index] = masks[index] | value;
            return index + 1;
        }

        public static int IdRemove(Span<ulong> masks, int id, Span<ulong> result)
        {
            var index = id / BitCount;
            var value = 1ul << (id % BitCount);
            result[index] = masks[index] & ~value;
            return index + 1;
        }

        public static bool Match(Span<ulong> mask, Span<ulong> with, Span<ulong> no, Span<ulong> any)
        {
            static bool allFilter(in Span<ulong> mask, in Span<ulong> filter)
            {
                if (filter.Length > mask.Length) return false;
                for (int i = 0; i < filter.Length; i++)
                {
                    var value = filter[i];
                    if ((mask[i] & value) != value) return false;
                }
                return true;
            }

            static bool anyFilter(in Span<ulong> mask, in Span<ulong> filter)
            {
                if (filter.Length == 0) return true;
                var min = Math.Min(mask.Length, filter.Length);
                for (int i = 0; i < min; i++)
                {
                    if ((mask[i] & filter[i]) != 0) return true;
                }
                return false;
            }

            return allFilter(in mask, in with) &&
                (no.Length == 0 || !anyFilter(in mask, in no)) &&
                anyFilter(in mask, in any);
        }

        public static Span<ulong> Trim(Span<ulong> masks)
        {
            int count = 0;
            int index = 0;
            foreach (ref var mask in masks)
            {
                index++;
                if (mask != 0) count = index;
            }
            return masks.Slice(0, count);
        }
    }
}

