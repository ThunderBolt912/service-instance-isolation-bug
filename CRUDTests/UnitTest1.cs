namespace CRUDTests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {

            // Arrange : The Inputs + Creating an object of the class meant to be tested

            MyMath m1 = new MyMath();

            int input1 = 12, input2 = 55;

            int expected = 67;
            // Act : Calling the actual Method

           int actual = m1.Add(input1, input2);

            // Assert : Compare Actual Value with the expected output of the method to verify method correctness

            Assert.Equal(expected, actual);
        }
    }
}
