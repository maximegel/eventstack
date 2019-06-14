namespace EventStack.Common.Construction
{
    public delegate IBuildable<TBuild> BuildingSteps<TBuild>(BuilderExtensionPoint<TBuild> _ = default);
}