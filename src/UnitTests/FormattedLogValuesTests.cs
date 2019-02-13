using Microsoft.Extensions.Logging.Internal;
using RedBear.Extensions.Logging.Filtering;
using System.Linq;
using Xunit;

namespace UnitTests
{
    public class FormattedLogValuesTests
    {
        [Fact]
        public void ClonesSimpleSuccessfully()
        {
            var original = new FormattedLogValues("Foo");
            var cloned = original.Clone();

            Assert.Equal(original.ToString(), cloned.ToString());
        }

        [Fact]
        public void ClonesComplexSuccessfully()
        {
            var original = new FormattedLogValues("The first number is {first} and the second is {second}", 10.ToString(), 20.ToString());
            var cloned = original.Clone();

            Assert.Equal(original.ToString(), cloned.ToString());
            Assert.True(original.GetValues().SequenceEqual(cloned.GetValues()));
        }

        [Fact]
        public void GetsOriginalValueSuccessfully()
        {
            var original = new FormattedLogValues("The first number is {first} and the second is {second}", 10.ToString(), 20.ToString());
            var originalValue = original.GetOriginalValue();

            Assert.Equal("The first number is {first} and the second is {second}", originalValue);
        }

        [Fact]
        public void UpdatesValuesSuccessfully()
        {
            var original = new FormattedLogValues("The first number is {first} and the second is {second}", 10.ToString(), 20.ToString());
            var updated = original.SetValues(20.ToString(), 30.ToString());

            Assert.Equal("The first number is 20 and the second is 30", updated.ToString());
        }

        [Fact]
        public void SetSimpleValueSuccessfully()
        {
            var original = new FormattedLogValues("The first number is {first} and the second is {second}", 10.ToString(), 20.ToString());
            var updated = original.SetSimpleValue("Changed");

            Assert.Equal("Changed", updated.ToString());
        }
    }
}
