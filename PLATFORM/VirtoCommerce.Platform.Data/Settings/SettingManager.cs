﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Data.Model;
using VirtoCommerce.Platform.Data.Repositories;
using VirtoCommerce.Platform.Data.Settings.Converters;

namespace VirtoCommerce.Platform.Data.Settings
{
    public class SettingsManager : ISettingsManager
    {
        private readonly IModuleManifestProvider _manifestProvider;
        private readonly Func<IPlatformRepository> _repositoryFactory;
        private readonly CacheManager _cacheManager;
        private readonly CacheKey _cacheKey = CacheKey.Create(CacheGroups.Settings, "AllSettings");

        public SettingsManager(IModuleManifestProvider manifestProvider, Func<IPlatformRepository> repositoryFactory, CacheManager cacheManager)
        {
            _manifestProvider = manifestProvider;
            _repositoryFactory = repositoryFactory;
            _cacheManager = cacheManager;
        }

        #region ISettingsManager Members

        public ModuleDescriptor[] GetModules()
        {
            var retVal = GetModuleManifestsWithSettings()
                .Select(x => x.ToModel())
                .ToArray();

            return retVal;
        }

		public SettingEntry[] GetObjectSettings(string objectType, string objectId)
		{
			if (objectType == null)
				throw new ArgumentNullException("objectType");
			if (objectId == null)
				throw new ArgumentNullException("objectId");

			var retVal = new List<SettingEntry>();
			using(var repository = _repositoryFactory())
			{
				var settings = repository.Settings.Include(s => s.SettingValues)
												  .Where(x => x.ObjectId == objectId && x.ObjectType == objectType).OrderBy(x=>x.Name).ToList();
				retVal.AddRange(settings.Select(x => x.ToModel()));
			}
			return retVal.ToArray();
		}

		public void RemoveObjectSettings(string objectType, string objectId)
		{
			if (objectType == null)
				throw new ArgumentNullException("objectType");
			if (objectId == null)
				throw new ArgumentNullException("objectId");
			using (var repository = _repositoryFactory())
			{
				var settings = repository.Settings.Include(s => s.SettingValues)
												  .Where(x => x.ObjectId == objectId && x.ObjectType == objectType).ToList();
				foreach(var setting in settings)
				{
					repository.Remove(setting);
				}
				repository.UnitOfWork.Commit();
			}

		}
        public SettingEntry[] GetModuleSettings(string moduleId)
        {
            var result = new List<SettingEntry>();

            var manifest = GetModuleManifestsWithSettings().FirstOrDefault(m => m.Id == moduleId);

            if (manifest != null && manifest.Settings != null && manifest.Settings.Any())
            {
                var settingEntities = GetAllEntities();

                foreach (var group in manifest.Settings)
                {
                    if (group.Settings != null)
                    {
                        foreach (var setting in group.Settings)
                        {
                            var settingEntity = settingEntities.FirstOrDefault(x => x.Name == setting.Name && x.ObjectId == null);
                            var settingDescriptor = setting.ToModel(settingEntity, group.Name);
                            result.Add(settingDescriptor);
                        }
                    }
                }
            }

            return result.OrderBy(x=>x.Name).ToArray();
        }

        public void SaveSettings(SettingEntry[] settings)
        {
            if (settings != null)
            {
				var settingKeys = settings.Select(x => String.Join("-", x.Name, x.ObjectType, x.ObjectId)).Distinct().ToArray();
           
                using (var repository = _repositoryFactory())
				using(var changeTracker = new ObservableChangeTracker())
                {
					var alreadyExistSettings =  repository.Settings.Include(s => s.SettingValues)
																   .Where(x => settingKeys.Contains(x.Name + "-" + x.ObjectType + "-" + x.ObjectId)).ToList();

					changeTracker.AddAction = x=>  repository.Add(x);
					//Need for real remove object from nested collection (because EF default remove references only)
					changeTracker.RemoveAction = x => repository.Remove(x);

					var target = new { Settings = new ObservableCollection<SettingEntity>(alreadyExistSettings) };
					var source = new { Settings = new ObservableCollection<SettingEntity>(settings.Select(x => x.ToEntity())) };
                  
					changeTracker.Attach(target);
                    var settingComparer = AnonymousComparer.Create((SettingEntity x) => String.Join("-", x.Name, x.ObjectType, x.ObjectId));
					source.Settings.Patch(target.Settings, settingComparer, (sourceSetting, targetSetting) => sourceSetting.Patch(targetSetting));

                    repository.UnitOfWork.Commit();
                }

                ClearCache();
            }
        }

        public T[] GetArray<T>(string name, T[] defaultValue)
        {
            var result = defaultValue;

            var repositorySetting = GetAllEntities()
                .FirstOrDefault(s => string.Equals(s.Name, name, StringComparison.OrdinalIgnoreCase));

            if (repositorySetting != null)
            {
                result = repositorySetting.SettingValues
                    .Select(v => (T)v.RawValue())
                    .ToArray();
            }
            else
            {
                var manifestSetting = LoadSettingFromManifest(name);

                if (manifestSetting != null)
                {
                    if (manifestSetting.ArrayValues != null)
                    {
                        result = manifestSetting.ArrayValues
                            .Select(v => (T)manifestSetting.RawValue(v))
                            .ToArray();
                    }
                    else if (manifestSetting.DefaultValue != null)
                    {
                        result = new[] { (T)manifestSetting.RawDefaultValue() };
                    }
                }
            }

            return result;
        }

        public T GetValue<T>(string name, T defaultValue)
        {
            var result = defaultValue;

            var values = GetArray(name, new[] { defaultValue });

            if (values.Any())
            {
                result = values.First();
            }

            return result;
        }

        public void SetValue<T>(string name, T value)
        {
            var setting = name.ToModel(value);
            SaveSettings(new[] { setting });
        }

        #endregion


        private IEnumerable<ModuleManifest> GetModuleManifestsWithSettings()
        {
            return _manifestProvider.GetModuleManifests().Values
                .Where(m => m.Settings != null && m.Settings.Any());
        }

        private ModuleSetting LoadSettingFromManifest(string name)
        {
            return GetAllManifestSettings().FirstOrDefault(s => s.Name == name);
        }

        private IEnumerable<ModuleSetting> GetAllManifestSettings()
        {
            return GetModuleManifestsWithSettings()
                .SelectMany(m => m.Settings)
                .SelectMany(g => g.Settings);
        }

        private List<SettingEntity> GetAllEntities()
        {
            var result = _cacheManager.Get(_cacheKey, LoadAllEntities);
            return result;
        }

        private List<SettingEntity> LoadAllEntities()
        {
            using (var repository = _repositoryFactory())
            {
                return repository.Settings
                    .Include(s => s.SettingValues)
                    .ToList();
            }
        }

        private void ClearCache()
        {
            _cacheManager.Remove(_cacheKey);
        }
    }
}
