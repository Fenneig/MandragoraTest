namespace Mandragora.Services
{
    public interface IServiceLocator<T>
    {
        TP Register<TP>(TP newService) where TP : T;
        void UnRegister<TP>(TP service) where TP : T;
        TP Get<TP>() where TP : T;
    }
}