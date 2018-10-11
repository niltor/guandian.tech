using System;

namespace MSDev.DB.Tools
{
    public class StringTools
    {

        /// <summary>
        /// 计算两字符串相似度
        /// <param name="source">原字符串</param>
        /// <param name="target">对比字符串</param>
        /// <returns>返回0-1.0</returns>
        /// </summary>
        public static Double Similarity(string source, string target)
        {
            if ((source == null) || (target == null))
			{
				return 0.0;
			}

			if ((source.Length == 0) || (target.Length == 0))
			{
				return 0.0;
			}

			if (source == target)
			{
				return 1.0;
			}

			int stepsToSame = ComputeLevenshteinDistance(source, target);
            return (1.0 - ((Double)stepsToSame / (Double)Math.Max(source.Length, target.Length)));
        }

        /// <summary>
        /// 计算两字符串转变距离
        /// </summary>
        public static int ComputeLevenshteinDistance(string source, string target)
        {
            if ((source == null) || (target == null))
			{
				return 0;
			}

			if ((source.Length == 0) || (target.Length == 0))
			{
				return 0;
			}

			if (source == target)
			{
				return source.Length;
			}

			int sourceWordCount = source.Length;
			int targetWordCount = target.Length;

            // Step 1
            if (sourceWordCount == 0)
			{
				return targetWordCount;
			}

			if (targetWordCount == 0)
			{
				return sourceWordCount;
			}

			int[,] distance = new int[sourceWordCount + 1, targetWordCount + 1];

            // Step 2
            for (int i = 0; i <= sourceWordCount; distance[i, 0] = i++)
			{
				;
			}

			for (int j = 0; j <= targetWordCount; distance[0, j] = j++)
			{
				;
			}

			for (int i = 1; i <= sourceWordCount; i++)
            {
                for (int j = 1; j <= targetWordCount; j++)
                {
					// Step 3
					int cost = (target[j - 1] == source[i - 1]) ? 0 : 1;

                    // Step 4
                    distance[i, j] = Math.Min(Math.Min(distance[i - 1, j] + 1, distance[i, j - 1] + 1), distance[i - 1, j - 1] + cost);
                }
            }

            return distance[sourceWordCount, targetWordCount];
        }
    }
}
