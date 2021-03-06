// Copyright (c) Charlie Poole, Rob Prouse and Contributors. MIT License - see LICENSE.txt

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using NUnit.Compatibility;
using NUnit.Framework.Internal;

namespace NUnit.Framework.Constraints
{
    /// <summary>
    /// DictionaryContainsKeyConstraint is used to test whether a dictionary
    /// contains an expected object as a key.
    /// </summary>
    public class DictionaryContainsKeyConstraint : CollectionItemsEqualConstraint
    {
        private const string ComparerMemberObsoletionMessage = "This member has been deprecated and will be removed in a future release. "
            + "To test using a comparer which the dictionary is not based on, use a collection constraint on the set of keys.";

        private const string ContainsMethodName = "Contains";
        private bool _isDeprecatedMode = false;

        /// <summary>
        /// Construct a DictionaryContainsKeyConstraint
        /// </summary>
        /// <param name="expected"></param>
        public DictionaryContainsKeyConstraint(object expected)
            : base(expected)
        {
            Expected = expected;
        }

        /// <summary>
        /// The display name of this Constraint for use by ToString().
        /// The default value is the name of the constraint with
        /// trailing "Constraint" removed. Derived classes may set
        /// this to another name in their constructors.
        /// </summary>
        public override string DisplayName { get { return "ContainsKey"; } }

        /// <summary>
        /// The Description of what this constraint tests, for
        /// use in messages and in the ConstraintResult.
        /// </summary>
        public override string Description
        {
            get { return "dictionary containing key " + MsgUtils.FormatValue(Expected); }
        }

        /// <summary>
        /// Gets the expected object
        /// </summary>
        protected object Expected { get; }

        /// <summary>
        /// Returns a new DictionaryContainsKeyValuePairConstraint checking for the
        /// presence of a particular key-value-pair in the dictionary.
        /// </summary>
        public DictionaryContainsKeyValuePairConstraint WithValue(object expectedValue)
        {
            var builder = this.Builder;
            if (builder == null)
            {
                builder = new ConstraintBuilder();
                builder.Append(this);
            }

            var constraint = new DictionaryContainsKeyValuePairConstraint(Expected, expectedValue);
            builder.Append(constraint);
            return constraint;
        }

        /// <summary>
        /// Flag the constraint to ignore case and return self.
        /// </summary>
        [Obsolete(ComparerMemberObsoletionMessage)]
        public new CollectionItemsEqualConstraint IgnoreCase
        {
            get
            {
                _isDeprecatedMode = true;
                return base.IgnoreCase;
            }
        }

        private bool Matches(object actual)
        {
            if (_isDeprecatedMode)
            {
                var dictionary = ConstraintUtils.RequireActual<IDictionary>(actual, nameof(actual));
                foreach (var obj in dictionary.Keys)
                    if (ItemsEqual(obj, Expected))
                        return true;

                return false;
            }

            var method = GetContainsKeyMethod(actual);
            if (method != null)
                return (bool)method.Invoke(actual, new[] { Expected });

            throw new ArgumentException($"The {TypeHelper.GetDisplayName(actual.GetType())} value must have a ContainsKey or Contains(TKey) method.");
        }

        /// <summary>
        /// Test whether the constraint is satisfied by a given value
        /// </summary>
        /// <param name="actual">The value to be tested</param>
        public override ConstraintResult ApplyTo<TActual>(TActual actual)
        {
            return new ConstraintResult(this, actual, Matches(actual));
        }

        /// <summary>
        /// Test whether the expected key is contained in the dictionary
        /// </summary>
        protected override bool Matches(IEnumerable collection)
        {
            return Matches(collection);
        }

        #region Shadowing CollectionItemsEqualConstraint Methods

        /// <summary>
        /// Flag the constraint to use the supplied predicate function
        /// </summary>
        /// <param name="comparison">The comparison function to use.</param>
        [Obsolete(ComparerMemberObsoletionMessage)]
        public DictionaryContainsKeyConstraint Using<TCollectionType, TMemberType>(Func<TCollectionType, TMemberType, bool> comparison)
        {
            // reverse the order of the arguments to match expectations of PredicateEqualityComparer
            Func<TMemberType, TCollectionType, bool> invertedComparison = (actual, expected) => comparison.Invoke(expected, actual);

            _isDeprecatedMode = true;
            base.Using(EqualityAdapter.For(invertedComparison));
            return this;
        }

        /// <summary>
        /// Flag the constraint to use the supplied Comparison object.
        /// </summary>
        /// <param name="comparison">The Comparison object to use.</param>
        [Obsolete(ComparerMemberObsoletionMessage)]
        public new CollectionItemsEqualConstraint Using<T>(Comparison<T> comparison)
        {
            _isDeprecatedMode = true;
            return base.Using(comparison);
        }


        /// <summary>
        /// Flag the constraint to use the supplied IComparer object.
        /// </summary>
        /// <param name="comparer">The IComparer object to use.</param>
        [Obsolete(ComparerMemberObsoletionMessage)]
        public new CollectionItemsEqualConstraint Using(IComparer comparer)
        {
            _isDeprecatedMode = true;
            return base.Using(comparer);
        }

        /// <summary>
        /// Flag the constraint to use the supplied IComparer object.
        /// </summary>
        /// <param name="comparer">The IComparer object to use.</param>
        [Obsolete(ComparerMemberObsoletionMessage)]
        public new CollectionItemsEqualConstraint Using<T>(IComparer<T> comparer)
        {
            _isDeprecatedMode = true;
            return base.Using(comparer);
        }

        /// <summary>
        /// Flag the constraint to use the supplied IEqualityComparer object.
        /// </summary>
        /// <param name="comparer">The IComparer object to use.</param>
        [Obsolete(ComparerMemberObsoletionMessage)]
        public new CollectionItemsEqualConstraint Using(IEqualityComparer comparer)
        {
            _isDeprecatedMode = true;
            return base.Using(comparer);
        }

        /// <summary>
        /// Flag the constraint to use the supplied IEqualityComparer object.
        /// </summary>
        /// <param name="comparer">The IComparer object to use.</param>
        [Obsolete(ComparerMemberObsoletionMessage)]
        public new CollectionItemsEqualConstraint Using<T>(IEqualityComparer<T> comparer)
        {
            _isDeprecatedMode = true;
            return base.Using(comparer);
        }

        /// <summary>
        /// Flag the constraint to use the supplied boolean-returning delegate.
        /// </summary>
        /// <param name="comparer">The supplied boolean-returning delegate to use.</param>
        [Obsolete(ComparerMemberObsoletionMessage)]
        public new CollectionItemsEqualConstraint Using<T>(Func<T, T, bool> comparer)
        {
            _isDeprecatedMode = true;
            return base.Using(comparer);
        }

        #endregion

        private static MethodInfo GetContainsKeyMethod(object keyedItemContainer)
        {
            if (keyedItemContainer == null) throw new ArgumentNullException(nameof(keyedItemContainer));
            var instanceType = keyedItemContainer.GetType();

            var method = FindContainsKeyMethod(instanceType)
                         ?? instanceType
                            .GetInterfaces()
                            .Concat(GetBaseTypes(instanceType))
                            .Select(FindContainsKeyMethod)
                            .FirstOrDefault(m => m != null);

            return method;
        }

        private static MethodInfo FindContainsKeyMethod(Type type)
        {
            var methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public);
            var method = methods.FirstOrDefault(m =>
                m.ReturnType == typeof(bool)
                && (m.Name == "ContainsKey" || (m.Name == nameof(IDictionary.Contains) && m.DeclaringType == typeof(IDictionary)))
                && !m.IsGenericMethod
                && m.GetParameters().Length == 1);

            if (method == null && type.GetTypeInfo().IsGenericType)
            {
                var definition = type.GetGenericTypeDefinition();
                var tKeyGenericArg = definition.GetGenericArguments().FirstOrDefault(typeArg => typeArg.Name == "TKey");

                if (tKeyGenericArg != null)
                {
                    method = definition
                             .GetMethods(BindingFlags.Instance | BindingFlags.Public)
                             .FirstOrDefault(m => m.ReturnType == typeof(bool) &&
                                                  m.Name == ContainsMethodName &&
                                                  !m.IsGenericMethod &&
                                                  m.GetParameters().Length == 1 &&
                                                  m.GetParameters()[0].ParameterType == tKeyGenericArg);

                    if (method != null)
                    {
                        method = methods.Single(m => m.MetadataToken == method.MetadataToken);
                    }
                }
            }

            return method;
        }

        private static IEnumerable<Type> GetBaseTypes(Type type)
        {
            for (; ; )
            {
                type = type.GetTypeInfo().BaseType;
                if (type == null) break;
                yield return type;
            }
        }
    }
}
