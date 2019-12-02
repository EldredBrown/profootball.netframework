using System;
using FakeItEasy;
using NUnit.Framework;

namespace EldredBrown.ProFootball.Shared.Tests
{
    [TestFixture]
    public class CalculatorTest
    {
        #region Test Cases

        [TestCase]
        public void Add()
        {
            // Arrange
            var lVal = 2d;
            var rVal = 1d;

            // Act
            var calculator = new Calculator();
            var result = calculator.Add(lVal, rVal);

            // Assert
            Assert.AreEqual(lVal + rVal, result);
        }

        [TestCase]
        public void Subtract()
        {
            // Arrange
            var lVal = 2d;
            var rVal = 1d;

            // Act
            var calculator = new Calculator();
            var result = calculator.Subtract(lVal, rVal);

            // Assert
            Assert.AreEqual(lVal - rVal, result);
        }

        [TestCase]
        public void Multiply()
        {
            // Arrange
            var lVal = 2d;
            var rVal = 1d;

            // Act
            var calculator = new Calculator();
            var result = calculator.Multiply(lVal, rVal);

            // Assert
            Assert.AreEqual(lVal * rVal, result);
        }

        [TestCase]
        public void Divide_DivByZero_ReturnsNull()
        {
            // Arrange
            var numerator = 2d;
            var denominator = 0;

            // Act
            var calculator = new Calculator();
            var result = calculator.Divide(numerator, denominator);

            // Assert
            Assert.IsNull(result);
        }

        [TestCase]
        public void Divide_NoDivByZero_ReturnsResult()
        {
            // Arrange
            var numerator = 2d;
            var denominator = 3d;

            // Act
            var calculator = new Calculator();
            var result = calculator.Divide(numerator, denominator);

            // Assert
            Assert.AreEqual(numerator / denominator, result);
        }

        [TestCase]
        public void CalculateWinningPercentage()
        {
            // Arrange
            var teamSeasonFake = A.Fake<ITeamSeason>();
            teamSeasonFake.Games = 3;
            teamSeasonFake.Wins = 2;
            teamSeasonFake.Ties = 1;

            // Act
            var calculator = new Calculator();
            var result = calculator.CalculateWinningPercentage(teamSeasonFake);

            // Assert
            Assert.AreEqual((2 * teamSeasonFake.Wins + teamSeasonFake.Ties) / (2 * teamSeasonFake.Games), result);
        }

        [TestCase]
        public void CalculatePythagoreanWinningPercentage()
        {
            // Arrange
            var teamSeasonFake = A.Fake<ITeamSeason>();
            teamSeasonFake.PointsFor = 32;
            teamSeasonFake.PointsAgainst = 16;

            // Act
            var calculator = new Calculator();
            var result = calculator.CalculatePythagoreanWinningPercentage(teamSeasonFake);

            // Assert
            var expResult = Math.Pow(teamSeasonFake.PointsFor, Calculator.Exponent) /
                (Math.Pow(teamSeasonFake.PointsFor, Calculator.Exponent) +
                Math.Pow(teamSeasonFake.PointsAgainst, Calculator.Exponent));
            Assert.AreEqual(expResult, result);
        }

        //[TestCase]
        //public void TestCase1()
        //{
        //    // Arrange

        //    // Act

        //    // Assert
        //}

        #endregion Test Cases
    }
}
