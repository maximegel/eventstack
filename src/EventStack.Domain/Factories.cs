using System;

namespace EventStack.Domain
{
    public static class Factories
    {
        public static T NonPublicParamlessCtor<T>() => (T) Activator.CreateInstance(typeof(T), true);

        public static T ParamlessCtor<T>()
            where T : new() => new T();
    }
}