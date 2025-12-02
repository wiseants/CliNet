using System;

namespace Common.Tools
{
    /// <summary>
    /// From Gemini
    /// </summary>
    public class KeyTool
    {
        /// <summary>
        /// ushort 범위의 난수를 반환합니다.
        /// </summary>
        /// <param name="minValue">최소 값.</param>
        /// <param name="maxValue">최대 값.</param>
        /// <returns>생성된 난수.</returns>
        public static ushort GetRandomUShort(int minValue, int maxValue)
        {
            if (minValue < ushort.MinValue) minValue = ushort.MinValue; // 0
            if (maxValue > ushort.MaxValue + 1) maxValue = ushort.MaxValue + 1; // 65536

            Random random = new Random();

            return (ushort)random.Next(minValue, maxValue);
        }

        /// <summary>
        /// ushort 전체 범위의 난수 생성.
        /// </summary>
        /// <returns>생성된 난수.</returns>
        public static ushort GetRandomUShort()
        {
            return GetRandomUShort(ushort.MinValue, ushort.MaxValue + 1);
        }

        /// <summary>
        /// 간단한 해시 생성.
        /// </summary>
        /// <param name="input">해시 입력 값.</param>
        /// <returns>해시 출력 값.</returns>
        public static ushort GetSimpleHash(ushort input)
        {
            ushort shifted = (ushort)(input << 8 | input >> 8);

            ushort result = (ushort)(input ^ shifted ^ 0xDEAD);

            return result;
        }
    }
}
