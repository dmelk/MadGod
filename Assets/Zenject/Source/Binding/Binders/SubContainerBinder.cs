using System;
using ModestTree;

namespace Zenject
{
    public class SubContainerBinder
    {
        readonly BindInfo _bindInfo;
        readonly BindFinalizerWrapper _finalizerWrapper;
        readonly object _subIdentifier;

        public SubContainerBinder(
            BindInfo bindInfo,
            BindFinalizerWrapper finalizerWrapper,
            object subIdentifier)
        {
            _bindInfo = bindInfo;
            _finalizerWrapper = finalizerWrapper;
            _subIdentifier = subIdentifier;

            // Reset in case the user ends the binding here
            finalizerWrapper.SubFinalizer = null;
        }

        protected IBindingFinalizer SubFinalizer
        {
            set
            {
                _finalizerWrapper.SubFinalizer = value;
            }
        }

        public ScopeBinder ByInstaller<TInstaller>()
            where TInstaller : InstallerBase
        {
            return ByInstaller(typeof(TInstaller));
        }

        public ScopeBinder ByInstaller(Type installerType)
        {
            Assert.That(installerType.DerivesFrom<InstallerBase>(),
                "Invalid installer type given during bind command.  Expected type '{0}' to derive from 'Installer<>'", installerType.Name());

            SubFinalizer = new SubContainerInstallerBindingFinalizer(
                _bindInfo, installerType, _subIdentifier);

            return new ScopeBinder(_bindInfo);
        }

        public ScopeBinder ByMethod(Action<DiContainer> installerMethod)
        {
            SubFinalizer = new SubContainerMethodBindingFinalizer(
                _bindInfo, installerMethod, _subIdentifier);

            return new ScopeBinder(_bindInfo);
        }

#if !NOT_UNITY3D

        public GameObjectNameGroupNameScopeBinder ByPrefab(UnityEngine.Object prefab)
        {
            BindingUtil.AssertIsValidPrefab(prefab);

            var gameObjectInfo = new GameObjectCreationParameters();

            SubFinalizer = new SubContainerPrefabBindingFinalizer(
                _bindInfo, gameObjectInfo, prefab, _subIdentifier);

            return new GameObjectNameGroupNameScopeBinder(_bindInfo, gameObjectInfo);
        }

        public GameObjectNameGroupNameScopeBinder ByPrefabResource(string resourcePath)
        {
            BindingUtil.AssertIsValidResourcePath(resourcePath);

            var gameObjectInfo = new GameObjectCreationParameters();

            SubFinalizer = new SubContainerPrefabResourceBindingFinalizer(
                _bindInfo, gameObjectInfo, resourcePath, _subIdentifier);

            return new GameObjectNameGroupNameScopeBinder(_bindInfo, gameObjectInfo);
        }
#endif
    }
}
