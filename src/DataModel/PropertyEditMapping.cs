public class PropertyEditMapping<TInput>
{
    public Func<TInput, object> Accessor;
    public Func<TInput, object> ValueGenerator;

    public PropertyEditMapping(Func<TInput, object> accessor, Func<TInput, object> valueGenerator)
    {
        Accessor = accessor;
        ValueGenerator = valueGenerator;
    }
}