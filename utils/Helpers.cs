using System.Collections.Generic;

namespace aoc.utils
{
    class Helpers
    {
        private static void swap<T>(List<T> nums, int ndx1, int ndx2) {
            var tmp = nums[ndx1];
            nums[ndx1] = nums[ndx2];
            nums[ndx2] = tmp;
        }

        private static void getPerms<T>(List<List<T>> perms, List<T> nums, int size) {
            if (size == 1) {
                var copy = new T[nums.Count];

                nums.CopyTo(copy);
                perms.Add(new List<T>(copy));

                return;
            }

            for (var ndx = 0; ndx < size; ndx += 1) {
                getPerms(perms, nums, size - 1);

                if (size % 2 == 0) {
                    swap(nums, ndx, size - 1);
                } else {
                    swap(nums, 0, size - 1);
                }
            }
        }

        public static List<List<T>> getPerms<T>(List<T> data) {
            var perms = new List<List<T>>();

            getPerms(perms, data, data.Count);

            return perms;
        }

        public static List<List<T>> getPairs<T>(List<T> data) {
            var pairs = new List<List<T>>();

            for (var first = 0; first < data.Count - 1; first += 1) {
                for (var second = first + 1; second < data.Count; second += 1) {
                    pairs.Add(new List<T>() { data[first], data[second] });
                }
            }

            return pairs;
        }

        private static (long, long) gcd_reduce(long big, long small) {
            var result = big - small;

            while (result > small) {
                result -= small;
            }

            return (small, result);
        }

        public static long gcd(long n1, long n2) {
            var big = n1 > n2 ? n1 : n2;
            var small = n1 > n2 ? n2 : n1;
            var result = small;

            while (small > 0) {
                result = small;
                (big, small) = gcd_reduce(big, small);
            }

            return result;
        }

        public static long lcm(long n1, long n2) {
            return (n1 * n2) / gcd(n1, n2);
        }
    }
}