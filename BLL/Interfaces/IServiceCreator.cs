using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    /*
     * В данном случае для упрощения примера я не буду использовать контейнеры внедрения зависимостей, 
     * а вместо этого воспользуюсь абстрактной фабрикой, которую будет представлять интерфейс IServiceCreator. 
     * Хотя естественнно можно также использовать для внедрения зависимостей DI-контейнеры типа Ninject.
    */
    public interface IServiceCreator
    {
        IUserService CreateUserService(string connection);
    }
}
