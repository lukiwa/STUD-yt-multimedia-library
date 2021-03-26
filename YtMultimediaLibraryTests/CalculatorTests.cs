using NUnit.Framework;


namespace YtMultimediaLibraryTests {
    public class CalculatorTests {
        [TestCase(2, 2, 4)]
        public void Add_ReturnsAdditionResult(int a, int b, int expected) {
            var calc = new YtMultimediaLibrary.Calculator();
            var result = calc.Add(a, b);
            Assert.AreEqual(expected, result);
        }
    }
}