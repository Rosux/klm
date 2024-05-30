using System.Linq.Expressions;

public class PropertyEditMapping<TInput>
{
    public Expression<Func<TInput, object>> Accessor;
    public Func<TInput, object> ValueGenerator;

    public PropertyEditMapping(Expression<Func<TInput, object>> accessor, Func<TInput, object> valueGenerator)
    {
        Accessor = accessor;
        ValueGenerator = valueGenerator;
    }
}