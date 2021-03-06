﻿// Copyright (c) Charlie Poole, Rob Prouse and Contributors. MIT License - see LICENSE.txt

using System;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

namespace NUnit.Framework.Attributes
{
    public class TestDummy : Test
    {
        public TestDummy() : base("TestDummy") { }

        #region Overrides

        public string TestKind
        {
            get { return "dummy-test"; }
        }

        public override bool HasChildren
        {
            get
            {
                return false;
            }
        }

        public override System.Collections.Generic.IList<ITest> Tests
        {
            get
            {
                return new ITest[0];
            }
        }

        public override TNode AddToXml(TNode parentNode, bool recursive)
        {
            throw new NotImplementedException();
        }

        public Internal.Commands.TestCommand MakeTestCommand()
        {
            throw new NotImplementedException();
        }

        public override TestResult MakeTestResult()
        {
            throw new NotImplementedException();
        }

        public override string XmlElementName
        {
            get { throw new NotImplementedException(); }
        }

        public override object[] Arguments
        {
            get
            {
                return new object[0];
            }
        }

        #endregion
    }
}
