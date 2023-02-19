﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Ray.BiliBiliTool.Infrastructure.Cookie
{
    public class CookieInfo
    {
        public CookieInfo(string cookieStr, Func<string, string> nameBuilder = null, Func<string, string> valueBuilder = null) : this(cookieStr?.Split(';'), nameBuilder, valueBuilder)
        {
        }

        public CookieInfo(IEnumerable<string> cookieItemList, Func<string, string> nameBuilder = null, Func<string, string> valueBuilder = null)
        {
            CookieItemDictionary = BuildCookieItemDictionaryByCookieItemList(cookieItemList, nameBuilder, valueBuilder);

            CookieStr = string.Join("; ", CookieItemDictionary.Select(kv => $"{kv.Key}={kv.Value}"));
        }

        public string CookieStr { get; set; }

        public Dictionary<string, string> CookieItemDictionary { get; private set; }

        public virtual void Check()
        {
            if (string.IsNullOrWhiteSpace(CookieStr)) throw new Exception("Cookie字符串为空");
        }

        public static Dictionary<string, string> BuildCookieItemDictionaryByCookieItemList(IEnumerable<string> cookieItemList, Func<string, string> nameBuilder = null, Func<string, string> valueBuilder = null)
        {
            var re = new Dictionary<string, string>();
            foreach (var item in cookieItemList ?? new List<string>())
            {
                var index = item.IndexOf('=');
                if (index == -1) continue;

                var name = item[..index].Trim();
                if (nameBuilder != null) name = nameBuilder(name);

                var value = item[(index+1)..].Trim();
                if (valueBuilder != null) value = valueBuilder(value);

                re.TryAdd(name, value);
            }
            return re;
        }
    }
}
