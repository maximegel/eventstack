using System;
using System.Threading.Tasks;
using RailSharp;
using RailSharp.Internal.Option;

namespace EventStack.Infrastructure.EventSourcing.Internal
{
    internal static class OptionExtensions
    {
        // TODO(maximegelinas): Move to RailSharp.
        public static Option<TDestination> FlatMap<T, TDestination>(
            this Option<T> option,
            Func<T, Option<TDestination>> mapper) =>
            option is Some<T> some ? mapper(some) : Option.None;

        // TODO(maximegelinas): Move to RailSharp.
        public static async Task<Option<TDestination>> FlatMapAsync<T, TDestination>(
            this Option<T> option,
            Func<T, Task<Option<TDestination>>> mapper) =>
            option is Some<T> some ? await mapper(some) : Option.None;

        // TODO(maximegelinas): Move to RailSharp.
        public static async Task<Option<TDestination>> MapAsync<T, TDestination>(
            this Option<T> option,
            Func<T, Task<TDestination>> mapper) =>
            option is Some<T> some ? Option.Some(await mapper(some)) : Option.None;
    }
}