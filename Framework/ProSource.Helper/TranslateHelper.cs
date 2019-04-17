using Common.Contract;
using Common.Contract.Enums;
using Common.Contract.Translation.Request;
using Common.Contract.Translation.Response;
using Dispatcher;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ProSource.Helpers
{
    public static class TranslateHelper
    {
        public static Cache cache;

        public static string Translate(string keyword, int languageId)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return keyword;
            }

            if (cache == null)
                return keyword;

            if (!cache.KeywordsDict.ContainsKeyword(keyword))
            {
                if (!cache.NonKeywordDict.ContainsKeyword(keyword))
                {
                    AddLog(keyword);
                    cache.NonKeywordDict.Add(CustomToLower(keyword), null);
                }
            }

            if (languageId == (int)LanguageInfo.tr)
                return keyword;

            var translateList = GetTranslateDictByLanguageId(languageId);

            string translatedText = null;
            if (translateList.TryGetValue(CustomToLower(keyword), out translatedText))
                return translatedText;
            else
                return keyword;
        }

        public static bool ContainsKeyword(this Dictionary<string, string> dict, string keyword)
        {
            return dict.ContainsKey(CustomToLower(keyword));
        }

        private static void AddLog(string keyword)
        {
            DispatcherEngine.Current.Execute<AddKeywordLogRequest, AddKeywordLogResponse>(new AddKeywordLogRequest
            { Text = CustomToLower(keyword) });
        }

        public static Dictionary<string, string> GetTranslateDictByLanguageId(int languageId)
        {
            if (cache.TranslateDict.ContainsKey(languageId))
                return cache.TranslateDict[languageId];
            return new Dictionary<string, string>();
        }

        public static string GetClientSideCache(int languageId)
        {
            if (languageId == (int)LanguageInfo.tr)
                return null;

            if (cache.ClientSideCache == null)
                return null;

            if (cache.ClientSideCache.ContainsKey(languageId))
                return cache.ClientSideCache[languageId];
            else
            {
                FillClientSideCache(languageId);
                return cache.ClientSideCache[languageId];
            }
        }

        private static string CustomToLower(string keyword)
        {
            var tempkeyword = string.Empty;
            if (string.IsNullOrWhiteSpace(keyword))
                return tempkeyword;
            for (int i = 0; i < keyword.Length; i++)
            {
                var tempChar = keyword[i];

                switch (tempChar)
                {
                    case 'İ':
                        tempkeyword += 'i';
                        break;
                    case 'I':
                        tempkeyword += 'ı';
                        break;
                    case 'Ö':
                        tempkeyword += 'ö';
                        break;
                    case 'Ş':
                        tempkeyword += 'ş';
                        break;
                    case 'Ü':
                        tempkeyword += 'ü';
                        break;
                    case 'Ğ':
                        tempkeyword += 'ğ';
                        break;
                    default:
                        tempkeyword += char.ToLower(tempChar);
                        break;
                }
            }

            return tempkeyword;
        }

        public static void FillClientSideCache(int languageId)
        {
            if (languageId == (int)LanguageInfo.tr)
                return;

            var sb = new StringBuilder();

            var dict = GetTranslateDictByLanguageId(languageId);

            foreach (var item in dict)
            {
                sb.Append($"page.Resource[\"{item.Key}\"] = \"{item.Value}\";");
            }

            var clientDict = new Dictionary<int, string>();
            clientDict.Add(languageId, sb.ToString());
            cache.ClientSideCache = clientDict;
        }

        public static void Refresh()
        {
            var newCache = new Cache();

            newCache.Fill();
            cache = newCache;
        }

        #region Translate static cache class
        public class Cache
        {
            private static Dictionary<int, string> _ClientSideCache;

            private static Dictionary<int, Dictionary<string, string>> _TranslateDict;

            public Dictionary<int, string> ClientSideCache
            {
                get => _ClientSideCache;
                set => _ClientSideCache = value;
            }
            public Dictionary<int, Dictionary<string, string>> TranslateDict
            {
                get => _TranslateDict;
                set => _TranslateDict = value;
            }

            private static Dictionary<string, string> _NonKeywordDict;

            public Dictionary<string, string> NonKeywordDict
            {
                get => _NonKeywordDict;
                set => _NonKeywordDict = value;
            }

            private static Dictionary<string, string> _KeywordsDict;

            public Dictionary<string, string> KeywordsDict
            {
                get => _KeywordsDict;
                set => _KeywordsDict = value;
            }

            private static string _Version;

            public string Version
            {
                get => _Version;
                set => _Version = value;
            }

            private static bool _IsVersionChanged;
            public bool IsVersionChanged
            {
                get => _IsVersionChanged;
                set => _IsVersionChanged = value;
            }

            public void Fill()
            {
                var request = new FillMultiLanguageCacheRequest();
                var data = DispatcherEngine.Current.Execute<FillMultiLanguageCacheRequest, FillMultiLanguageCacheResponse>(request);

                if (!data.HasError())
                {
                    TranslateDict = data.translations;
                    NonKeywordDict = data.nonKeywords;
                    KeywordsDict = data.keywords;
                    Version = data.Version;
                }
                else
                {
                    TranslateDict = new Dictionary<int, Dictionary<string, string>>();
                    NonKeywordDict = new Dictionary<string, string>();
                    KeywordsDict = new Dictionary<string, string>();
                }

            }

        }

        #endregion

    }
}
