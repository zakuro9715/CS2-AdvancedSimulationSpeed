/* Copyright 2024 zakuro <z@kuro.red> (https://x.com/zakuro9715)
 * 
 * This file is part of AdvancedSimulationSpeed.
 * 
 * AdvancedSimulationSpeed is free software: you can redistribute it and/or modify it under the
 * terms of the GNU General Public License as published by the Free Software Foundation, either
 * version 3 of the License, or (at your option) any later version.
 * 
 * AdvancedSimulationSpeed is distributed in the hope that it will be useful, but WITHOUT ANY
 * WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR
 * PURPOSE. See the GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License along with
 * AdvancedSimulationSpeed. If not, see <https://www.gnu.org/licenses/>.
 */

using Colossal;
using Colossal.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
namespace AdvancedSimulationSpeed.Locales
{
    public class LocaleLoadException : Exception {
        public LocaleLoadException(): base("Locale loading error") { }
        public LocaleLoadException(string message) : base(message) { }
    }

    public class  LocaleNotFoundException: LocaleLoadException
    {
        public LocaleNotFoundException(string name): base($"Locale resource `{name}` not found") { }
    }

    public class LocaleLoader
    {
        private readonly Assembly _assembly = Assembly.GetAssembly(typeof(AdvancedSimulationSpeed.Mod));
       public  Dictionary<string, LocaleDictionarySource> Locales { get; private set; }
        public LocaleLoader() {
            _assembly = Assembly.GetAssembly(typeof(Mod));
        }

        private IEnumerable<string> getLocaleResourceNames() =>
            _assembly.GetManifestResourceNames().Where((name) => name.Contains("Locales") && name.EndsWith(".json"));

        public Dictionary<string, LocaleDictionarySource> LoadAll()
        {
            Locales = new Dictionary<string, LocaleDictionarySource>();
            foreach (var localeResourceName in getLocaleResourceNames())
            {
                var localeName = Path.GetFileNameWithoutExtension(localeResourceName);
                localeName = localeName.Substring(localeName.LastIndexOf('.') + 1);

                using var resourceStream = _assembly.GetManifestResourceStream(localeResourceName) ?? throw new LocaleNotFoundException(localeResourceName);
                using var reader = new StreamReader(resourceStream, Encoding.UTF8);
    JSON.MakeInto<Dictionary<string, string>>(JSON.Load(reader.ReadToEnd()), out var dictionary);
                Locales.Add(localeName, new LocaleDictionarySource(localeName, dictionary));
            }
            return Locales;
        }
    }

    public class LocaleDictionarySource : IDictionarySource
    {
        private readonly Dictionary<string, string> _dictionary;

        public LocaleDictionarySource(string localeId, Dictionary<string, string> dictionary)
        {
            LocaleId = localeId;
            _dictionary = dictionary;
        }

        public string LocaleId { get; }

        public IEnumerable<KeyValuePair<string, string>> ReadEntries(IList<IDictionaryEntryError> errors, Dictionary<string, int> indexCounts)
        {
            return _dictionary;
        }

        public void Unload() { }
    }
}
