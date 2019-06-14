namespace EventStack.Common.Construction
{
    public interface IBuildable<out T>
    {
        T Build();
    }
}