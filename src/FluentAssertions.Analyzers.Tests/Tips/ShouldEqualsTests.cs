﻿using FluentAssertions.Analyzers.Tips;
using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentAssertions.Analyzers.Tests.Tips
{
    [TestClass]
    public class ShouldEqualsTests
    {
        [TestMethod]
        [Implemented]
        public void ShouldEquals_TestAnalyzer()
            => VerifyCSharpDiagnosticExpressionBody("actual.Should().Equals(expected);");

        [TestMethod]
        [Implemented]
        public void ShouldEquals_ShouldBe_ObjectType_TestCodeFix()
        {
            var oldSource = GenerateCode.ObjectStatement("actual.Should().Equals(expected);");
            var newSource = GenerateCode.ObjectStatement("actual.Should().Be(expected);");

            DiagnosticVerifier.VerifyCSharpFix<ShouldEqualsCodeFix, ShouldEqualsAnalyzer>(oldSource, newSource);
        }

        [TestMethod]
        [Implemented]
        public void ShouldEquals_NestedInsideIfBlock_TestAnalyzer()
            => VerifyCSharpDiagnosticExpressionBody("if(true) { actual.Should().Equals(expected); }", 10, 24);

        [TestMethod]
        [Implemented]
        public void ShouldEquals_NestedInsideIfBlock_ShouldBe_ObjectType_TestCodeFix()
        {
            var oldSource = GenerateCode.ObjectStatement("if(true) { actual.Should().Equals(expected); }");
            var newSource = GenerateCode.ObjectStatement("if(true) { actual.Should().Be(expected); }");

            DiagnosticVerifier.VerifyCSharpFix<ShouldEqualsCodeFix, ShouldEqualsAnalyzer>(oldSource, newSource);
        }

        [TestMethod]
        [Implemented]
        public void ShouldEquals_NestedInsideWhileBlock_TestAnalyzer()
            => VerifyCSharpDiagnosticExpressionBody("while(true) { actual.Should().Equals(expected); }", 10, 27);

        [TestMethod]
        [Implemented]
        public void ShouldEquals_NestedInsideWhileBlock_ShouldBe_ObjectType_TestCodeFix()
        {
            var oldSource = GenerateCode.ObjectStatement("while(true) { actual.Should().Equals(expected); }");
            var newSource = GenerateCode.ObjectStatement("while(true) { actual.Should().Be(expected); }");

            DiagnosticVerifier.VerifyCSharpFix<ShouldEqualsCodeFix, ShouldEqualsAnalyzer>(oldSource, newSource);
        }

        [TestMethod]
        [Implemented]
        public void ShouldEquals_ActualIsMethodInvoaction_TestAnalyzer()
            => VerifyCSharpDiagnosticExpressionBody("object ResultSupplier() { return null; } \n" 
            + "ResultSupplier().Should().Equals(expected);", 11, 0);

        [TestMethod]
        [Implemented]
        public void ShouldEquals_ActualIsMethodInvoaction_ShouldBe_ObjectType_TestCodeFix()
        {
            const string methodInvocation = "object ResultSupplier() { return null; } \n";
            var oldSource = GenerateCode.ObjectStatement(methodInvocation + "ResultSupplier().Should().Equals(expected);");
            var newSource = GenerateCode.ObjectStatement(methodInvocation + "ResultSupplier().Should().Be(expected);");

            DiagnosticVerifier.VerifyCSharpFix<ShouldEqualsCodeFix, ShouldEqualsAnalyzer>(oldSource, newSource);
        }

        [TestMethod]
        [Implemented]
        public void ShouldEquals_ShouldBe_NumberType_TestCodeFix()
        {
            var oldSource = GenerateCode.DoubleAssertion("actual.Should().Equals(expected);");
            var newSource = GenerateCode.DoubleAssertion("actual.Should().Be(expected);");

            DiagnosticVerifier.VerifyCSharpFix<ShouldEqualsCodeFix, ShouldEqualsAnalyzer>(oldSource, newSource);
        }

        [TestMethod]
        [Implemented]
        public void ShouldEquals_ShouldBe_StringType_TestCodeFix()
        {
            var oldSource = GenerateCode.StringAssertion("actual.Should().Equals(expected);");
            var newSource = GenerateCode.StringAssertion("actual.Should().Be(expected);");

            DiagnosticVerifier.VerifyCSharpFix<ShouldEqualsCodeFix, ShouldEqualsAnalyzer>(oldSource, newSource);
        }

        [TestMethod]
        [Implemented]
        public void ShouldEquals_ShouldEqual_EnumerableType_TestCodeFix()
        {
            var oldSource = GenerateCode.GenericIListCodeBlockAssertion("actual.Should().Equals(expected);");
            var newSource = GenerateCode.GenericIListCodeBlockAssertion("actual.Should().Equal(expected);");

            DiagnosticVerifier.VerifyCSharpFix<ShouldEqualsCodeFix, ShouldEqualsAnalyzer>(oldSource, newSource);
        }

        private void VerifyCSharpDiagnosticExpressionBody(string sourceAssertion) => VerifyCSharpDiagnosticExpressionBody(sourceAssertion, 10, 13);
        private void VerifyCSharpDiagnosticExpressionBody(string sourceAssertion, int line, int column)
        {
            var source = GenerateCode.ObjectStatement(sourceAssertion);
            DiagnosticVerifier.VerifyCSharpDiagnosticUsingAllAnalyzers(source, new DiagnosticResult
            {
                Id = ShouldEqualsAnalyzer.DiagnosticId,
                Message = ShouldEqualsAnalyzer.Message,
                Locations = new DiagnosticResultLocation[]
                {
                    new DiagnosticResultLocation("Test0.cs", line, column)
                },
                Severity = DiagnosticSeverity.Info
            });
        }
    }
}
