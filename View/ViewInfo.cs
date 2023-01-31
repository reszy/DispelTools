using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace View
{
    internal interface IViewInfo
    {
        ViewConstructor? PrepareConstructor();
    }

    internal class ViewConstructor
    {
        private object[] constructorParams;
        private System.Reflection.ConstructorInfo constructor;

        public ViewConstructor(ConstructorInfo constructor, object[] constructorParams)
        {
            this.constructorParams = constructorParams;
            this.constructor = constructor;
        }

        public INestedView Construct()
        {
            return (INestedView)constructor.Invoke(constructorParams);
        }
    }
    internal class ViewInfo<T> : IViewInfo where T : INestedView
    {
        private readonly Type viewType;
        private readonly Type[] constructorParams;

        public ViewInfo(params Type[] param)
        {
            viewType = typeof(T);
            constructorParams = param;
        }
        public ViewConstructor? PrepareConstructor()
        {
            try
            {
                if (constructorParams.Length > 0)
                {
                    ConstructorInfo? constructor = viewType.GetConstructors().Where(ctorInfo => DoesConstructorMatch(ctorInfo)).FirstOrDefault();
                    var paramsInstances = constructorParams.Select(cparam => cparam.GetConstructor(Array.Empty<Type>())!.Invoke(Array.Empty<object>())).ToArray();
                    return (constructor is null || paramsInstances is null || paramsInstances.Length != constructorParams.Length) ? null : new ViewConstructor(constructor, paramsInstances);
                }
                else
                {
                    var constructor = viewType.GetConstructor(constructorParams);
                    return constructor is null ? null : new ViewConstructor(constructor, Array.Empty<object>());
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public bool DoesConstructorMatch(ConstructorInfo constructor)
        {
            var realParams = constructor.GetParameters();
            if(realParams.Length == constructorParams.Length) 
            { 
                for(int i =0; i<realParams.Length; i++)
                {
                    if(!realParams[i].ParameterType.IsAssignableFrom(constructorParams[i]))
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }
    }
}
