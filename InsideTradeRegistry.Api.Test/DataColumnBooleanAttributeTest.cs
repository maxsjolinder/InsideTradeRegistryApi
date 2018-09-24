using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;

namespace InsideTradeRegistry.Api.Test
{
    [TestCategory("Unit test")]
    [TestClass]
    public class DataColumnBooleanAttributeTest
    {
        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void WhenConvertingNonBooleanValueThenExceptionIsThrown()
        {
            try
            {
                var ba = new DataColumnBooleanAttribute();
                ba.ConvertStringToType("dummy", typeof(int), Thread.CurrentThread.CurrentCulture);
            }
            catch (InvalidCastException e)
            {
                Assert.AreEqual("DataColumnBooleanAttribute can only be used on attributes of type System.Boolean.", e.Message);
                throw;
            }
        }

        [TestMethod]
        public void WhenNoConfigurationIsDoneThenRegularBoolConversionIsUsed()
        {
            var ba = new DataColumnBooleanAttribute();
            var falseObject = ba.ConvertStringToType("False", typeof(bool), Thread.CurrentThread.CurrentCulture);
            Assert.IsFalse((bool)falseObject);

            var trueObject = ba.ConvertStringToType("True", typeof(bool), Thread.CurrentThread.CurrentCulture);
            Assert.IsTrue((bool)trueObject);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void WhenNoConfigurationIsDoneAndInvalidStringIsConvertedThenExceptionIsThrown()
        {
            var ba = new DataColumnBooleanAttribute();
            ba.ConvertStringToType("nonsense", typeof(bool), Thread.CurrentThread.CurrentCulture);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void GivenEqualFalseAndTrueStringConfigurationWhenConvertingThenExceptionIsThrown()
        {
            try
            {
                var ba = new DataColumnBooleanAttribute();
                ba.TrueStrings = new string[] { "hey" };
                ba.FalseStrings = new string[] { "hey" };
                ba.ConvertStringToType("hey", typeof(bool), Thread.CurrentThread.CurrentCulture);
            }
            catch (InvalidCastException e)
            {
                Assert.AreEqual("Ambiguous configuration: Both true and false string contains the value hey.", e.Message);
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void GivenBothFalseAndTrueStringConfiguredWhenConvertingAndNoMatchThenExceptionIsThrown()
        {
            try
            {
                var ba = new DataColumnBooleanAttribute();
                ba.TrueStrings = new string[] { "yes" };
                ba.FalseStrings = new string[] { "no" };
                ba.ConvertStringToType("nonsense", typeof(bool), Thread.CurrentThread.CurrentCulture);
            }
            catch (InvalidCastException e)
            {
                Assert.AreEqual("No matching conversion was found for value nonsense.", e.Message);
                throw;
            }
        }

        [TestMethod]
        public void TestConversionWhenOnlyTrueStringsAreConfigured()
        {
            var ba = new DataColumnBooleanAttribute();
            ba.TrueStrings = new string[] { "yes", "yupp", "y" };
            var o = ba.ConvertStringToType("nonsense", typeof(bool), Thread.CurrentThread.CurrentCulture);
            Assert.IsFalse((bool)o);
            o = ba.ConvertStringToType("yes", typeof(bool), Thread.CurrentThread.CurrentCulture);
            Assert.IsTrue((bool)o);
            o = ba.ConvertStringToType("yUpp", typeof(bool), Thread.CurrentThread.CurrentCulture);
            Assert.IsTrue((bool)o);
            o = ba.ConvertStringToType("Y", typeof(bool), Thread.CurrentThread.CurrentCulture);
            Assert.IsTrue((bool)o);
        }

        [TestMethod]
        public void TestConversionWhenOnlyFalseStringsAreConfigured()
        {
            var ba = new DataColumnBooleanAttribute();
            ba.FalseStrings = new string[] { "no", "never", "n" };
            var o = ba.ConvertStringToType("nonsense", typeof(bool), Thread.CurrentThread.CurrentCulture);
            Assert.IsTrue((bool)o);
            o = ba.ConvertStringToType("NO", typeof(bool), Thread.CurrentThread.CurrentCulture);
            Assert.IsFalse((bool)o);
            o = ba.ConvertStringToType("never", typeof(bool), Thread.CurrentThread.CurrentCulture);
            Assert.IsFalse((bool)o);
            o = ba.ConvertStringToType("n", typeof(bool), Thread.CurrentThread.CurrentCulture);
            Assert.IsFalse((bool)o);
        }

        [TestMethod]
        public void TestConversionWhenBothTrueAndFalseStringsAreConfigured()
        {
            var ba = new DataColumnBooleanAttribute();
            ba.FalseStrings = new string[] { "no", "n" };
            ba.TrueStrings = new string[] { "yes", "y" };

            var o = ba.ConvertStringToType("NO", typeof(bool), Thread.CurrentThread.CurrentCulture);
            Assert.IsFalse((bool)o);
            o = ba.ConvertStringToType("n", typeof(bool), Thread.CurrentThread.CurrentCulture);
            Assert.IsFalse((bool)o);
            o = ba.ConvertStringToType("Y", typeof(bool), Thread.CurrentThread.CurrentCulture);
            Assert.IsTrue((bool)o);
            o = ba.ConvertStringToType("yes", typeof(bool), Thread.CurrentThread.CurrentCulture);
            Assert.IsTrue((bool)o);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void GivenBothTrueAndFalseStringsConfiguredWhenConvertingAndNoMatchThenExceptionIsThrown()
        {
            try
            {
                var ba = new DataColumnBooleanAttribute();
                ba.TrueStrings = new string[] { "yes", "yupp", "y" };
                ba.FalseStrings = new string[] { "no", "never", "n" };
                ba.ConvertStringToType("nonsense", typeof(bool), Thread.CurrentThread.CurrentCulture);
            }
            catch (InvalidCastException e)
            {
                Assert.AreEqual("No matching conversion was found for value nonsense.", e.Message);
                throw;
            }
        }
    }
}
