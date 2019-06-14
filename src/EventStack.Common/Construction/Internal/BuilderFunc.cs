using System;

namespace EventStack.Common.Construction.Internal
{
    internal class BuilderFunc<T> : IBuildable<T>
    {
        private readonly Func<T> _func;

        public BuilderFunc(Func<T> func) => _func = func;

        /// <inheritdoc />
        public T Build() => _func();
    }
}