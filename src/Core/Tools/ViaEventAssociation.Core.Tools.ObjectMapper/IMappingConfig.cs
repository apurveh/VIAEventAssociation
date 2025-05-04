namespace ViaEventAssociation.Core.Tools.ObjectMapper;

public interface IMappingConfig<in TInput, out TOutput>
    where TOutput : class
    where TInput : class 
{
    TOutput Map(TInput input);
}