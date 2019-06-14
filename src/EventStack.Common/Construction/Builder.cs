using System;
using EventStack.Common.Construction.Internal;

namespace EventStack.Common.Construction
{
    public static class Builder
    {
        public static IBuildable<T> For<T>(Func<T> func) => new BuilderFunc<T>(func);
    }
}