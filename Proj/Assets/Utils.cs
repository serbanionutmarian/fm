using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Gameplay
{
	public class Utils
	{
		public static int TransfEndMonth(DateTime date1, DateTime date2)
		{
			if (date1.Month > date2.Month)
			{
				return date2.Month + 12;
			}

			return date2.Month;
		}

		public static bool IsDateBetweenDayMonth(DateTime input, DateTime date1, DateTime date2)
		{
			return ((date1.Month < input.Month || (date1.Month == input.Month && date1.Day <= input.Day)) && 
			        (input.Month < TransfEndMonth(date1, date2)/*date2.Month*/ || (date2.Month == input.Month && input.Day <= date2.Day)));

		}

		public static int GetIndexByWeight(float value, float[] weights, int numWeights)
		{
			int ls = 0, lr = numWeights - 1;
			int lastGoodIndex = numWeights - 1;

			while (ls <= lr)
			{
				int mid = (ls + lr) >> 1;
				if (weights[mid] <= value)
				{
					lastGoodIndex = mid;
					ls = mid + 1;
				}
				else
				{
					lr = mid - 1;
				}
			}

			return lastGoodIndex;
		}
	}
}

