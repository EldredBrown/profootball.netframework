using System;

namespace EldredBrown.ProFootball.Shared
{
    public interface ICalculator
    {
        double Add(double lVal, double rVal);
        double? CalculatePythagoreanWinningPercentage(ITeamSeason teamSeason);
        double? CalculateWinningPercentage(ITeamSeason teamSeason);
        double? Divide(double numerator, double denominator);
        double Multiply(double lVal, double rVal);
        double Subtract(double lVal, double rVal);
    }

    public delegate double Operation(double lVal, double rVal);

    /// <summary>
    /// Class to represent a basic arithmetic calculator. This class wraps arithmetic functions inside methods that can
    /// then be passed via delegates. This class also makes available a couple of more advanced computational methods
    /// unique to this application.
    /// </summary>
    public class Calculator : ICalculator
    {
        public const double Exponent = 2.37;

        /// <summary>
        /// Adds two values
        /// </summary>
        /// <param name="lVal"></param>
        /// <param name="rVal"></param>
        /// <returns></returns>
        public virtual double Add(double lVal, double rVal)
        {
			return lVal + rVal;
		}

        /// <summary>
        /// Subtracts the second value from the first
        /// </summary>
        /// <param name="lVal"></param>
        /// <param name="rVal"></param>
        /// <returns></returns>
		public virtual double Subtract(double lVal, double rVal)
        {
            return lVal - rVal;
        }

        /// <summary>
        /// Multiplies two values
        /// </summary>
        /// <param name="lVal"></param>
        /// <param name="rVal"></param>
        /// <returns></returns>
		public virtual double Multiply(double lVal, double rVal)
        {
            return lVal * rVal;
        }

        /// <summary>
        /// Divides the first value by the second value
        /// </summary>
        /// <param name="numerator"></param>
        /// <param name="denominator"></param>
        /// <returns></returns>
        public virtual double? Divide(double numerator, double denominator)
        {
            // Rather than throw an error for division by zero, 
            // this will return a default result of null if division by zero occurs.
            double? result = null;

            if (denominator != 0)
            {
                result = numerator / denominator;
            }

            return result;
        }

        /// <summary>
        /// Calculates a team's winning percentage for the selected season
        /// </summary>
        /// <param name="teamSeason"></param>
        /// <returns></returns>
        public virtual double? CalculateWinningPercentage(ITeamSeason teamSeason)
        {
            var result = Divide((2 * teamSeason.Wins + teamSeason.Ties), (2 * teamSeason.Games));
            return result;
        }

        /// <summary>
        /// Calculates a team's Pythagorean Winning Percentage
        /// </summary>
        /// <param name = "pointsFor"></param>
        /// <param name = "pointsAgainst"></param>
        /// <returns></returns>
        public virtual double? CalculatePythagoreanWinningPercentage(ITeamSeason teamSeason)
        {
            var numerator = Math.Pow(teamSeason.PointsFor, Exponent);
            var denominator = Add(Math.Pow(teamSeason.PointsFor, Exponent), Math.Pow(teamSeason.PointsAgainst,
                Exponent));
            var pct = Divide(numerator, denominator);
            return pct;
        }
    }
}
