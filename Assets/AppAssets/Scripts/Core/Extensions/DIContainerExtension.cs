using Zenject;
using UnityEngine;

public enum BindType { AsSingle, AsCached, AsTransient }

namespace AppName.Core.Extensions
{
    public static class DIContainerExtension
    {
        public static void DIBindFromInstance<T>(
            this DiContainer container, 
            T serviceName, 
            BindType bindType = BindType.AsSingle
        )
        {
            switch (bindType)
            {
                case BindType.AsSingle:
                {
                    container.Bind<T>().FromInstance(serviceName).AsSingle();
                    break;
                }
                case BindType.AsCached:
                {
                    container.Bind<T>().FromInstance(serviceName).AsCached();
                    break;
                }
                case BindType.AsTransient:
                {
                    container.Bind<T>().FromInstance(serviceName).AsTransient();
                    break;
                }
                default:
                { 
                    container.Bind<T>().FromInstance(serviceName).AsSingle();
                    break;
                }
            }
        }

        public static void DIInstantiateAndBindFromInstance<T>(
            this DiContainer container,
            T servicePrefab,
            Transform parentForServiceInstance,
            Vector3? instancePosition = null,
            Quaternion? instanceRotation = null,
            BindType bindType = BindType.AsSingle
        )
        where T : MonoBehaviour
        {
            if (!instancePosition.HasValue)
            {
                instancePosition = Vector3.zero;
            }
            if (!instanceRotation.HasValue)
            {
                instanceRotation = Quaternion.identity;
            }

            T serviceInstance =
                container.InstantiatePrefabForComponent<T>
                    (
                        servicePrefab,
                        instancePosition.Value,
                        instanceRotation.Value,
                        parentForServiceInstance
                    );

            container.DIBindFromInstance<T>(serviceInstance, bindType);
        }
    }
}